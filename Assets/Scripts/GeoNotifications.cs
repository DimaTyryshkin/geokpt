using Assets.SimpleAndroidNotifications;
using UnityEngine;
using UnityEngine.Assertions;

namespace Geo
{
	public class GeoNotifications
	{
		public void Send(NotificationsConfig config)
		{
			Assert.IsNull(config);
			NotificationManager.CancelAll();
			
			foreach (NotificationsConfig.Notification notification in config.notifications)
			{ 
				var notificationParams = new NotificationParams
				{
					Id             = UnityEngine.Random.Range(0, int.MaxValue),
					Delay          = notification.Delay,
					Title          = notification.title,
					Message        = notification.msg,
					Ticker         = "",
					Sound          = false,
					Vibrate        = false,
					Light          = false,
					SmallIcon      = NotificationIcon.Bell,
					SmallIconColor = new Color(0.3882353f, 0.7803922f, 0.4745098f), // Цвет иконки приложения
					LargeIcon      = "app_icon"
				};
				NotificationManager.SendCustom(notificationParams);
			}
		}
	}
}