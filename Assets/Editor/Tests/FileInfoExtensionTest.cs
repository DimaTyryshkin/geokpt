using System.IO;
using NUnit.Framework;
using SiberianWellness.Common;
using UnityEditor.Compilation;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
	public class FileInfoExtensionTest
	{ 
		[Test]
		public void Test()
		{
			Test1("file", ".txt");
			Test1("dir/file", ".jpg");
			Test1("file", ".xml");
			Test1("file", "._test_");
			
			Test1("file", ".xml.zip");
			Test1("dir/file", ".xml.xml");
			
			Test1("file", "..xml.xml");
			
			Test1("", ".xml"); 
			Test1("", ".xml.zip"); 
			Test1("", "..xml.zip");
			
			Test2("file", "file");
			Test2("dir/file", "file");
			Test2(".file", ".file");
			Test2(".file.x", ".file.x");
			Test2("dir/.file.x", ".file.x");
		}

		void Test1(string fileName, string extension)
		{
			Test2(fileName + extension, extension);

		}
		
		void Test2(string fullName, string extension)
		{
			FileInfo f1 = new FileInfo(fullName );
			Assert.AreEqual(extension, f1.GetFullExtension());
		}
	}
}