using Picsum;

await foreach (PicsumPhoto photo in PicsumPhoto.Stream())
    photo.Print();