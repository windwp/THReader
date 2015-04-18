using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace TungHoanhReader.Services
{
    class PrimaryTileHelper
    {
        internal void UpdateBadge(int value)
        {
            var type = Windows.UI.Notifications.BadgeTemplateType.BadgeNumber;
            var badge = BadgeUpdateManager.GetTemplateContent(type);

            var xml = badge.SelectSingleNode("/badge") as XmlElement;
            xml.SetAttribute("value", value.ToString());

            var updater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            updater.Update(new BadgeNotification(badge));
        }
    }
}
