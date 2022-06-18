using Picsum;
using System.Reactive.Subjects;

Subject<PicsumPhoto> stream = new();

var sub = stream.Subscribe(
    photo => photo.Print(),
    error => throw new Exception("error", error),
    () => Console.WriteLine("Stream is complete")
);

await foreach (PicsumPhoto photo in PicsumPhoto.Stream())
    stream.OnNext(photo);

stream.OnCompleted();