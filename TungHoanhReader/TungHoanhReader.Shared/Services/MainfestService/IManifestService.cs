using Windows.UI;

namespace TungHoanhReader.Services
{
    public interface IManifestService 
    {
        string SplashImage { get; }
        Color SplashBackgroundColor { get; }
    }
}