namespace Platform.Contracts;
public interface IStreamService<T>
{
    IObserver<T> GetObserver(
        Action<T> next,
        Action<Exception> error = null,
        Action complete = null
    );

    Task Stream(IObserver<T> observer);

    IAsyncEnumerable<T> StreamAsync();
}