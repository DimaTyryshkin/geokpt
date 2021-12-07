using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MaxValue
    {
        [Test]
        public void MaxValueMain()
        {
            int[] MyArray = new int[] { 34, 56, 32, 1, 9, 65 };
            Assert.AreEqual(MaxValueFunction(MyArray),5);
        }

        public int MaxValueFunction(int[] MaxArray)
        {
            int number = MaxArray[0];
            int index = 0;
            for (int i = 0; i < MaxArray.Length; i++)
            {
                if (MaxArray[i] > number)
                {
                    number = MaxArray[i];
                    index = i;
                }
            }
            return index;
        }
    }
      
}
