using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
namespace dotnet_store.Services;
public class ImageProcessingService
{
    public async Task ResizeAndSaveAsync(Stream inputStream, string outputPath, int width = 1200, int height = 500)
    {
        using var image = await Image.LoadAsync(inputStream);
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Crop,
            Size = new Size(width, height),
            Position = AnchorPositionMode.Center
        }));

        await image.SaveAsync(outputPath);
    }
}