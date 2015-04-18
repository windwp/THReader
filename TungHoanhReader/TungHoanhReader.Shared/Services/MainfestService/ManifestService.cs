using Windows.UI;

namespace TungHoanhReader.Services
{
    public class ManifestService : IManifestService
    {
        public ManifestService()
        {
            this.Helper = new ManifestHelper();
        }

        public string SplashImage
        {
            get { return this.Helper.App.SplashImage; }
        }

        public Color SplashBackgroundColor
        {
            get { return this.Helper.App.SplashBackgroundColor; }
        }

        public ManifestHelper Helper { get; set; }
    }
}
