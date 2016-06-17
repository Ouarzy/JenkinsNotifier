namespace Jenkins.Notifier.ViewModel.Commands
{
    using System.Threading.Tasks;

    public interface IAsyncCommand<in T> : IRaiseCanExecuteChanged
    {
        Task ExecuteAsync(T obj);
    }

    public interface IAsyncCommand : IAsyncCommand<object>
    {
    }
}