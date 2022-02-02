using Caliburn.Micro;
using System.Threading;
using System.Threading.Tasks;
using WPFClient.Models;

namespace WPFClient.ViewModels
{
    internal class ShellViewModel : Conductor<Screen>.Collection.OneActive, IHandle<ValidUserNameEntered>    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IServerConnection _serverConnection;
        private readonly LoginViewModel _loginViewModel;
        private readonly ChatPageViewModel _chatPageViewModel;

        public ShellViewModel(IEventAggregator eventAggregator,
                              IServerConnection serverConnection,
                              LoginViewModel loginViewModel,
                              ChatPageViewModel chatPageViewModel)
        {
            _eventAggregator = eventAggregator;
            _serverConnection = serverConnection;
            _loginViewModel = loginViewModel;
            _chatPageViewModel = chatPageViewModel;

            Items.AddRange(new Screen[] { loginViewModel, chatPageViewModel });
        }

        public Task HandleAsync(ValidUserNameEntered message, CancellationToken cancellationToken)
        {
            ChangeActiveItemAsync(_chatPageViewModel, true);
            return Task.CompletedTask;
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            _eventAggregator.SubscribeOnPublishedThread(this);
            ActivateItemAsync(_loginViewModel);
            return base.OnActivateAsync(cancellationToken);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _eventAggregator.Unsubscribe(this);
            return base.OnDeactivateAsync(close, cancellationToken);
        }
    }
}
