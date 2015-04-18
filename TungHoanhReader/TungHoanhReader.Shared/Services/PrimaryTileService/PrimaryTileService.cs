
namespace TungHoanhReader.Services
{
    public class PrimaryTileService: IPrimaryTileService
    {
        private PrimaryTileHelper Helper;
        public PrimaryTileService()
        {
            this.Helper = new PrimaryTileHelper();
        }

        public void UpdateBadge(int value)
        {
            this.Helper.UpdateBadge(value);
        }
    }
}
