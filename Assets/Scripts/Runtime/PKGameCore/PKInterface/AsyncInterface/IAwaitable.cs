namespace Runtime.PKGameCore.PKInterface.AsyncInterface
{
    public interface IAwaitable<out TAwaiter> where TAwaiter : IAwaiter
    {
        TAwaiter GetAwaiter();
    }

    public interface IAwaitable<out TAwaiter, out TRes> where TAwaiter : IAwaiter<TRes>
    {
        TAwaiter GetAwaiter();
    }
}