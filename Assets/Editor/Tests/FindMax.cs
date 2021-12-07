using System.Collections;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class FindMax
    {
        [Test]
        public void FindMaxTest()
        {
            int[] testData1 = new int[]{1,55,23,87,5,6};
            int[] testData2 = new int[]{1,55,23,87,53,644};
            int[] testData3 = new int[]{1,52,234,2,111,123,34,12,2222,5,23,87,5,6};
            int[] testData4 = new int[]{1,11,2223,8456,52346,6765};

            Assert.AreEqual(FindMaxIndexTest(testData1),3);
            Assert.AreEqual(FindMaxIndexTest(testData2),5);
            Assert.AreEqual(FindMaxIndexTest(testData3),8);
            Assert.AreEqual(FindMaxIndexTest(testData4),4);
        }


        public int FindMaxIndexTest(int[] data)
        {
            int maxVal = data[0];
            int maxIndex = 0;
            for (int i=0;i<data.Length;i++) { maxVal = (maxVal<data[i])?data[i]:maxVal; maxIndex = (maxVal==data[i])?i:maxIndex;}
            Debug.Log(String.Format("Find max index test: (max value is: {0} and max index is {1})",maxVal.ToString(),maxIndex.ToString()));
            return maxIndex;
        }
    }
}
