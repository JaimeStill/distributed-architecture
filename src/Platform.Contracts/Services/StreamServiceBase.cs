using System.Reactive;

namespace Platform.Contracts;
public abstract class StreamServiceBase<T>
{
    public IObserver<T> GetObserver(
        Action<T> next,
        Action<Exception> error = null,
        Action complete = null
    )
    {
        if (error is null && complete is null)
            return Observer.Create(next);
        else if (error is null)
            return Observer.Create(next, complete);
        else if (complete is null)
            return Observer.Create(next, error);
        else
            return Observer.Create(next, error, complete);
    }
}