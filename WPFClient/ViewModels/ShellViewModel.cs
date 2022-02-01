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
    internal class ShellViewModel : Conductor<Screen>.Collection.OneActive
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IServerConnection serverConnection;
        private readonly LoginViewModel loginViewModel;
        private readonly ChatPageViewModel chatPageViewModel;

        public ShellViewModel(IEventAggregator eventAggregator,
                              IServerConnection serverConnection,
                              LoginViewModel loginViewModel,
                              ChatPageViewModel chatPageViewModel)
        {
            this.eventAggregator = eventAggregator;
            this.serverConnection = serverConnection;
            this.loginViewModel = loginViewModel;
            this.chatPageViewModel = chatPageViewModel;

            Items.AddRange(new Screen[] { loginViewModel, chatPageViewModel });
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            eventAggregator.SubscribeOnPublishedThread(this);
            ActivateItemAsync(loginViewModel);
            return base.OnActivateAsync(cancellationToken);
        }
    }
}
