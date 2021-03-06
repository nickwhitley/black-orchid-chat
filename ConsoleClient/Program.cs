using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Text.RegularExpressions;
using static System.Console;

namespace ConsoleClient
{
    class Program
    {
        public static HubConnection _connection = ConnectToServer(@"https://blackorchidchat.azurewebsites.net/chat");

        static void Main(string[] args)
        {
            try
            {
                _connection.StartAsync().Wait();
            }
            catch (AggregateException)
            {
                WriteLine($"The server is currently down. Please try again later.");
                Write("\r\nPress enter to exit");
                PrintConnectionStatus();
                ReadKey();
                Environment.Exit(0);
            }

            PrintConnectionStatus();


            AskForUserName();

            do
            {
                SendMessage();

            } while (_connection.State == HubConnectionState.Connected);
        }

        private static void PrintConnectionStatus()
        {
            Console.WriteLine(_connection.State);
        }

        private static HubConnection ConnectToServer(string address)
        {
            return new HubConnectionBuilder().WithUrl(address).WithAutomaticReconnect().Build();
        }

        private static void SendUserNameToServer(string userName)
        {
            _connection.InvokeCoreAsync("RetrieveUsername", args: new[] { userName });
            _connection.On("ReceiveChatMessage", (string message) =>
            {
                WriteLine($"{ message }");
            });
        }

        private static string AskForUserName()
        {
            string userInput = "";
            bool isValidName = false;

            do
            {
                Write("What would you like your user name to be?  ");
                userInput = ReadLine();

                string pattern = @"^[a-zA-Z0-9]+$";
                Regex exp = new Regex(pattern);

                if (exp.IsMatch(userInput))
                    isValidName = true;

                else
                {
                    WriteLine("Invalid, try again (no special characters)..");
                    isValidName = false;
                }
            }
            while (!isValidName);

            SendUserNameToServer(userInput);

            return userInput;
        }

        private static void SendMessage()
        {
            string message = ReadLine();
            _connection.InvokeCoreAsync("SendChatMessage", args: new[] { message });
        }

    }
}
