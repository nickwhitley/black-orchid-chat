using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPFClient.Models;

namespace WPFClient.ViewModels
{
    public class ChatPageViewModel : Screen
    {
        private readonly IServerConnection connection;

        public ChatPageViewModel(IServerConnection connection)
        {
            this.connection = connection;
        }
    }
}
