namespace Jenkins.Notifier.ViewModel.Commands
{
    using System;
    using System.Threading.Tasks;

    public class AwaitableDelegateCommand : AwaitableDelegateCommandParametrized<object>, IAsyncCommand
    {
        public AwaitableDelegateCommand(Func<Task> executeMethod)
            : base(o => executeMethod())
        {
        }
    }
}
