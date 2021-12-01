using System;
using Geo.Data;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
	public class PlayerPrefsTest
	{
		[Test]
		public void DefaultValue()
		{
			Assert.AreEqual("", PlayerPrefs.GetString("key", null)); //Внимание подстава!
			Assert.AreEqual("", PlayerPrefs.GetString("key", ""));
			Assert.AreEqual("", PlayerPrefs.GetString("key", String.Empty));
			Assert.AreEqual(" ", PlayerPrefs.GetString("key", " "));
		}
		
		[Test]
		public void PlayerPrefsImplementations()
		{
			PlayerPrefsWrapper real = new PlayerPrefsWrapper();
			PlayerPrefsTestMethod(real);
			
			FakePlayerPrefs fake = new FakePlayerPrefs();
			PlayerPrefsTestMethod(fake);
			Assert.AreEqual(4, fake.save.Count);
		}
		 
		void PlayerPrefsTestMethod(IPlayerPrefs playerPrefs)
		{  
			//Дебильные дефолтные значения
			Assert.AreEqual(null, playerPrefs.GetString("PlayerPrefsTest.key", null)); //Внимание подстава!
			Assert.AreEqual("", playerPrefs.GetString("PlayerPrefsTest.key", ""));
			Assert.AreEqual("", playerPrefs.GetString("PlayerPrefsTest.key", String.Empty));
			Assert.AreEqual(" ", playerPrefs.GetString("PlayerPrefsTest.key", " "));
			
			//Нормальные дефолтные занчения
			Assert.AreEqual("defaultValue",playerPrefs.GetString("PlayerPrefsTest.key0","defaultValue"));
			
			//Запись значений
			playerPrefs.SetString("PlayerPrefsTest.key1", "v1");
			playerPrefs.SetString("PlayerPrefsTest.key2", "v2");
			
			playerPrefs.SetString("PlayerPrefsTest.key3", "v");
			playerPrefs.SetString("PlayerPrefsTest.key4", "v");
			
			
			
			Assert.AreEqual("v1",playerPrefs.GetString("PlayerPrefsTest.key1","--"));
			Assert.AreEqual("v2",playerPrefs.GetString("PlayerPrefsTest.key2","--"));
			
			Assert.AreEqual("v",playerPrefs.GetString("PlayerPrefsTest.key3","--"));
			Assert.AreEqual("v",playerPrefs.GetString("PlayerPrefsTest.key4","--")); 
		}

		
	}
}