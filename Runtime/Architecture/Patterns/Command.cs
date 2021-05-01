using System.Threading.Tasks;

namespace Kalendra.Commons.Runtime.Architecture.Patterns
{
    #region Sync
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
    
    public interface ICommand<in T>
    {
        void Execute(T arg);
        void Undo(T arg);
    }
    #endregion

    #region Async
    public interface ICommandAsync
    {
        Task Execute();
        Task Undo();
    }

    public interface ICommandAsync<in T>
    {
        Task Execute(T arg);
        Task Undo(T arg);
    }
    #endregion
}