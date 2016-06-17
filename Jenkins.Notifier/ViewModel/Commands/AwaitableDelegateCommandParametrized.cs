namespace Jenkins.Notifier.ViewModel.Commands
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    public class AwaitableDelegateCommandParametrized<T> : IAsyncCommand<T>, ICommand
    {
        private readonly Func<T, Task> executeMethod;
        private readonly RelayCommand<T> underlyingCommand;
        private bool isExecuting;

        protected AwaitableDelegateCommandParametrized(Func<T, Task> executeMethod)
            : this(executeMethod, _ => true)
        {
        }

        private AwaitableDelegateCommandParametrized(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.underlyingCommand = new RelayCommand<T>(x => { }, canExecuteMethod);
        }

        public async Task ExecuteAsync(T obj)
        {
            try
            {
                this.isExecuting = true;
                this.RaiseCanExecuteChanged();
                await this.executeMethod(obj);
            }
            finally
            {
                this.isExecuting = false;
                this.RaiseCanExecuteChanged();
            }
        }

        public ICommand Command
        {
            get { return this; }
        }

        public bool CanExecute(object parameter)
        {
            return !this.isExecuting && this.underlyingCommand.CanExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { this.underlyingCommand.CanExecuteChanged += value; }
            remove { this.underlyingCommand.CanExecuteChanged -= value; }
        }

        public async void Execute(object parameter)
        {
            await this.ExecuteAsync((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            this.underlyingCommand.RaiseCanExecuteChanged();
        }
    }
}