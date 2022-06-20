using Platform.Contracts;
using System.Reactive.Subjects;

namespace Picsum.Services;
public class PicsumStreamService : StreamServiceBase<IPhoto>, IStreamService<IPhoto>
{
    public async Task Stream(IObserver<IPhoto> observer)
    {
        Subject<IPhoto> stream = new();
        stream.Subscribe(observer);

        await foreach (PicsumPhoto photo in PicsumPhoto.Stream())
            stream.OnNext(photo);

        stream.OnCompleted();
    }

    public IAsyncEnumerable<IPhoto> StreamAsync() => PicsumPhoto.Stream();
}
