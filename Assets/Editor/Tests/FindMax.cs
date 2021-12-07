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
        public void FindMaxData()
        {
            int[] testData1 = new int[]{1,55,23,87,5,6};
            int[] testData2 = new int[]{1,55,23,87,53,644};
            int[] testData3 = new int[]{1,52,234,2,111,123,34,12,2222,5,23,87,5,6};
            int[] testData4 = new int[]{1,11,2223,8456,52346,6765};

            Assert.AreEqual(FindMaxIndex(testData1),3);
        }


        public int FindMaxIndex(int[] data)
        {
            int maxVal = data[0];
            int maxIndex = 0;
            for (int i=0;i<data.Length;i++) { maxVal = (maxVal<data[i])?data[i]:maxVal; maxIndex = (maxVal==data[i])?i:maxIndex;}
            return maxIndex;
        }
        
    }
}
