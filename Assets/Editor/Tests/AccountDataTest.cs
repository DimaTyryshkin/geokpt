using Geo.Data;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
	public class AccountDataTest
	{
		[Test]
		public void PrivacyPolicyData()
		{
			AccountData data = new AccountData();
			Assert.IsTrue(data.privacyPolicy.NeedAccept);
			data.privacyPolicy.OnAccept();
			Assert.IsFalse(data.privacyPolicy.NeedAccept);
		}
	}
}