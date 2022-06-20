using Picsum;
using Picsum.Services;
using Platform.Contracts;

PicsumStreamService picsumSvc = new();

IObserver<IPhoto> observer = picsumSvc.GetObserver(    
    (IPhoto photo) => Console.WriteLine($"{photo.Author} - {photo.Url}"),
    (Exception error) => throw new Exception("error", error),
    () => Console.WriteLine("Stream is complete")
);

await picsumSvc.Stream(observer);