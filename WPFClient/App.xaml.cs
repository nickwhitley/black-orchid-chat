﻿using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static HubConnection _connection;
        protected override void OnStartup(StartupEventArgs e)
        {
            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(@"https://3c55-2601-548-4100-c1f0-409b-9c14-504a-eb44.ngrok.io/chat")
                .WithAutomaticReconnect()
                .Build();

            connection.StartAsync();

            _connection = connection;

            base.OnStartup(e);
        }
    }
}
