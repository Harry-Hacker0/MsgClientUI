using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using MsgClientUI.Messages;
using MsgClientUI.Bindings;

namespace MsgClientUI.Infrastructure
{
    public delegate void Notify();

    public class ConnectionHandler
    {
        public event EventHandler<MessageItem> UpdateMessages;
        public event EventHandler<UserItem> UpdateUsers;
        private int connectionID;
        private string userName;
        NetworkStream stream;
        TcpClient tcpClient;
        bool isConnected = false;
        public ConnectionHandler() { Connect(); }

        public bool IsConnected
        {
            get { return isConnected; }
            set { isConnected = value; }
        }

        public int ConnectionID
        {
            get { return connectionID; }
            set { connectionID = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public void Connect()
        {
            try
            {
                switch (IsConnected)
                {
                    case true:
                        IsConnected = false;
                        stream.Close();
                        tcpClient.Close();
                        
                        break;
                    case false:
                        IsConnected = true;
                        tcpClient = new TcpClient("127.0.0.1", 5000);
                        stream = tcpClient.GetStream();
                        ReceiveMessage();
                        break;
                }
            }
            catch (Exception ex) { Debug.WriteLine($"Unable To Connect."); }
        }

        private async void ReceiveMessage()
        {
            string recMessage;
            string continuedMessage = "";
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int byteRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (byteRead == 0) break; //the client disconnected.

                    recMessage = Encoding.UTF8.GetString(buffer, 0, byteRead);

                    var startCode = recMessage[0..3];
                    var everythingBetween = recMessage[3..(recMessage.Length - 3)];
                    var endCode = recMessage[(recMessage.Length - 3)..recMessage.Length];
                    continuedMessage += everythingBetween;

                    Debug.WriteLine($"""
                        YOU'VE GOT MAIL: {recMessage}
                        :------------------------------:
                        Start Code: {startCode}
                        End Code: {endCode}
                        Payload: {everythingBetween}
                        :------------------------------:
                        Current Message: {continuedMessage}
                        """);


                    switch (endCode)
                    {
                        case "END":
                            break;

                        case "NOT":
                            continue;

                        default:
                            Debug.WriteLine($"Unknown: {continuedMessage}");
                            continuedMessage = "";
                            continue;
                    }


                    switch (startCode)
                    {
                        case "CHT":
                            Debug.WriteLine($"From Client : {recMessage}");

                            ChatMessage chatMessage = JsonSerializer.Deserialize<ChatMessage>(continuedMessage);
                            OnProcessCompletedMessage(new MessageItem(chatMessage));
                            break;

                        case "ACT":
                            ActionMessage actionMessage = JsonSerializer.Deserialize<ActionMessage>(continuedMessage);

                            switch (actionMessage.actionType)
                            {
                                default:
                                    Debug.WriteLine($"Unknown ACT Type: {actionMessage.actionDetails}");
                                    break;
                            }
                            break;


                        case "INF":
                            InfrastructureMessage infrastructureMessage = JsonSerializer.Deserialize<InfrastructureMessage>(continuedMessage);

                            switch (infrastructureMessage.infrastructureType) 
                            {
                                case "ID_UPDATE":
                                    if (int.TryParse(infrastructureMessage.infrastructureDetails, out int someInteger)) ConnectionID = someInteger; // Add else statement to ping server to resend message.
                                    break;

                                case "ADD_USER":
                                    OnProcessCompletedUser(new UserItem(infrastructureMessage));
                                    break;

                                default:
                                    Debug.WriteLine($"Unknown INF Type: {infrastructureMessage.infrastructureType}");
                                    break;
                            }
                            break;

                        default:
                            Debug.WriteLine($"Unknown Start Code: {recMessage}");
                            break;
                    }

                    continuedMessage = "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                isConnected = false;
                stream.Close();
                Console.WriteLine($"Client Disconnected.");
            }
        }

        // Update channel chat.
        protected virtual void OnProcessCompletedMessage(MessageItem displayMsg)
        {
            UpdateMessages?.Invoke(this, displayMsg);
        }

        // Update available channels.
        protected virtual void OnProcessCompletedUser(UserItem displayMsg)
        {
            UpdateUsers?.Invoke(this, displayMsg);
        }

        // Break up and send messages to server.
        public void SendMessage(string type,ChatMessage message)
        {
            string toBeSent = JsonSerializer.Serialize(message);
            string remaining = "";


            while (toBeSent.Length > 1018)
            {
                remaining = toBeSent.Substring(1018);
                toBeSent = toBeSent.Substring(0, 1018);

                Send(type, toBeSent, "NOT");

                toBeSent = remaining;
                remaining = "";
            }

            Send(type, toBeSent, "END");
        }

        // Await sent message.
        public async void Send(string type, string message, string end)
        {
            byte[] response = Encoding.UTF8.GetBytes($"{type + message + end}");

            Debug.WriteLine($"Sending: {message}");

            await stream.WriteAsync(response, 0, response.Length);
        }
    }
}
