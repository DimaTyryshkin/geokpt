using System;
using Geo;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
	public class TimeSpanParseTest
	{
		[Test]
		public void ParseNotificationDelayFormat()
		{
			TimeSpan t1 = TimeSpan.ParseExact("1:02:03", NotificationsConfig.delayFormat, null);
			TimeSpan t2 = new TimeSpan(1, 2, 3, 0);
			Assert.AreEqual(t1, t2);
			
			t1 = TimeSpan.ParseExact("1:00:00", NotificationsConfig.delayFormat, null);
			t2 = new TimeSpan(1, 0, 0, 0);
			Assert.AreEqual(t1, t2);
			
			t1 = TimeSpan.ParseExact("5:00:00", NotificationsConfig.delayFormat, null);
			t2 = new TimeSpan(5, 0, 0, 0);
			Assert.AreEqual(t1, t2);
		}
	}
}