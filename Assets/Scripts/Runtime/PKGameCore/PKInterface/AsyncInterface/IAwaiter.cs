using System.Runtime.CompilerServices;

namespace Runtime.PKGameCore.PKInterface.AsyncInterface
{
    public interface IAwaiter : INotifyCompletion
    {
        bool IsCompleted { get; }
        void GetResult();
    }

    public interface IAwaiter<TRes> : INotifyCompletion
    {
        bool IsCompleted { get; }
        TRes GetResult();
        void SetResult(TRes res);
    }
}