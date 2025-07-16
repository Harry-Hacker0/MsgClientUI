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
using MsgClientUI.Interface;
using MsgClientUI.Messages;

namespace MsgClientUI.Infrastructure
{
    public delegate void Notify();

    internal class User
    {
        public event EventHandler<MessageItem> UpdateMessages;
        int code;
        NetworkStream stream;
        TcpClient tcpClient;

        public User() { Connect(); }

        public void Connect()
        {
            try
            {
                if (tcpClient == null)
                {
                    tcpClient = new TcpClient("127.0.0.1", 5000);
                    stream = tcpClient.GetStream();
                    ReceiveMessage();
                }
                else
                {
                    if (tcpClient.Connected)
                    {
                        stream.Close();
                        tcpClient.Close();
                    }
                    else
                    {
                        tcpClient = new TcpClient("127.0.0.1", 5000);
                        stream = tcpClient.GetStream();
                        ReceiveMessage();
                    }
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
                    Console.WriteLine("----------------------RECEIVED----------------------");

                    byte[] buffer = new byte[1024];
                    int byteRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (byteRead == 0) break; //the client disconnected.

                    recMessage = Encoding.UTF8.GetString(buffer, 0, byteRead);

                    var startCoded = recMessage[0..3];
                    var everythingBetween = recMessage[3..(recMessage.Length - 3)];
                    var endCoded = recMessage[(recMessage.Length - 3)..recMessage.Length];

                   

                    Debug.WriteLine($"YOU'VE GOT MAIL: {recMessage}");
                    Debug.WriteLine($"     Start Code: {startCoded}");
                    Debug.WriteLine($"        Payload: {everythingBetween}");
                    Debug.WriteLine($"       End Code: {endCoded}");

                    switch (startCoded)
                    {
                        case "CHT":
                            Debug.WriteLine($"From Client : {recMessage}");

                            if (endCoded == "NOT")
                            {
                                continuedMessage += recMessage[3..(recMessage.Length - 3)];
                                Debug.WriteLine($"Message For Display: {continuedMessage}");
                            }

                            if (endCoded == "END")
                            {
                                continuedMessage += recMessage[3..(recMessage.Length - 3)];

                                Debug.WriteLine($"Message For Display: {continuedMessage}");

                                ChatMessage message = JsonSerializer.Deserialize<ChatMessage>(continuedMessage);

                                MessageItem displayMsg = new MessageItem(message.timestamp, message.author, message.channel, message.content);
                                OnProcessCompleted(displayMsg);
                                continuedMessage = "";
                            }

                            break;


                        case "ACT":

                            Debug.WriteLine($"From Server : {recMessage}");

                            if (endCoded == "NOT")
                            {
                                continuedMessage += recMessage[3..(recMessage.Length - 3)];
                            }

                            if (endCoded == "END")
                            {
                                continuedMessage += recMessage[3..(recMessage.Length - 3)];

                                Debug.WriteLine($"HERE: {continuedMessage}");

                                ActionMessage message = JsonSerializer.Deserialize<ActionMessage>(continuedMessage);

                                switch (message.actionType)
                                {
                                    case "IDK":
                                        if (int.TryParse(message.actionDetails, out int someInteger)) code = someInteger; // Add else statement to ping server to resend message.

                                        Debug.WriteLine($"Update Client ID: {message}");
                                        Debug.WriteLine($"Specified Code: {message.actionDetails}");
                                        Debug.WriteLine($"Code: {code}");

                                        break;
                                    default:
                                        Debug.WriteLine($"Unknown Type: {message.actionDetails}");
                                        break;
                                }

                                continuedMessage = "";
                                break;
                            }

                            break;


                        case "INF":

                            Debug.WriteLine($"From Server : {recMessage}");

                            if (endCoded == "NOT")
                            {
                                continuedMessage += recMessage[3..(recMessage.Length - 3)];
                            }

                            if (endCoded == "END")
                            {
                                continuedMessage += recMessage[3..(recMessage.Length - 3)];

                                Debug.WriteLine($"HERE: {continuedMessage}");

                                InfrastructureMessage message = JsonSerializer.Deserialize<InfrastructureMessage>(continuedMessage);

                                switch (message.infrastructureType) 
                                {
                                    case "ID_UPDATE":
                                        if (int.TryParse(message.infrastructureDetails, out int someInteger)) code = someInteger; // Add else statement to ping server to resend message.

                                        Debug.WriteLine($"Update Client ID: {message}");
                                        Debug.WriteLine($"Specified Code: {message.infrastructureDetails}");
                                        Debug.WriteLine($"Code: {code}");

                                        break;
                                    default:
                                        Debug.WriteLine($"Unknown Type: {message.infrastructureType}");
                                        break;
                                }

                                continuedMessage = "";
                                break;
                            }

                            break;


                        default:

                            Debug.WriteLine($"What Did We Get? : {recMessage}");

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                stream.Close();
                Console.WriteLine($"Client Disconnected.");
            }
        }

        protected virtual void OnProcessCompleted(MessageItem displayMsg)
        {
            Debug.WriteLine($"We Got One!");
            UpdateMessages?.Invoke(this, displayMsg);
        }

        public void SendMessage(string type,ChatMessage message)
        {
            string toBeSent = JsonSerializer.Serialize(message);
            string remaining = "";

            Debug.WriteLine($"To Be Sent Size: {toBeSent.Length}");

            while (toBeSent.Length > 1018)
            {
                Console.WriteLine($"----------- While Loop");
                remaining = toBeSent.Substring(1018);
                toBeSent = toBeSent.Substring(0, 1018);


                Debug.WriteLine($"Remainder: {remaining}");
                Debug.WriteLine($"  Payload: {toBeSent}");
                Send(type, toBeSent, "NOT");

                toBeSent = remaining;
                remaining = "";
            }

            Send(type, toBeSent, "END");
        }

        public async void Send(string type, string message, string end)
        {
            byte[] response = Encoding.UTF8.GetBytes($"{type + message + end}");

            Debug.WriteLine($"To Be Sent Bytes: {response.Length}");

            await stream.WriteAsync(response, 0, response.Length);
        }

        public int GetCode()
        {
            return code;
        }

        public void SetCode(int newCode)
        {
            code = newCode;
        }

    }
}
