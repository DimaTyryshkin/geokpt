using System;

namespace Geo
{
	[Serializable]
	public class NotificationsConfig
	{
		public static readonly string delayFormat = @"d\:hh\:mm"; // "1:02:03" - один день, два часа, три минуты
		[Serializable]
		public class Notification
		{
			public string title;
			public string msg;
			public string delay;
			public TimeSpan Delay => TimeSpan.ParseExact(delay,delayFormat,null);
		}

		public Notification[] notifications = new []
		{
			new Notification()
			{
				title = "Коллега👷‍♀️",
				msg = "Нужна помощь с координатами?",
				delay = "1:00:00",
			},
			new Notification()
			{
				title = "Коллега👷‍♀️",
				msg   = "Могу помочь с координатами",
				delay = "5:00:00",
			}
		};
	}
	
}