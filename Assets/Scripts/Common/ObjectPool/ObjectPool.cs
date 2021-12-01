using System.Collections;
using System.Collections.Generic;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Assertions;

namespace SiberianWellness
{
    public static class MonoBehaviourPoolExtantion
    {
        public static void ReturnToPool(this GameObject thisGo)
        {
            thisGo.GetComponent<PooledObject>().ReturnToPool();
        }
        
    }

    public class ObjectPool : MonoBehaviour
    {
        [IsntNull]
        public GameObject prefab;
        
        public int initialSize;

        private readonly Stack<GameObject> instances = new Stack<GameObject>();

        /// <summary>
        /// Unity's Awake method.
        /// </summary>
        void Awake()
        {
            IsntNull.Assert(this);
        }

        /// <summary>
        /// Initializes the object pool.
        /// </summary>
        public void Initialize()
        {
            for (var i = 0; i < initialSize; i++)
            {
                var obj = CreateInstance();
                obj.SetActive(false);
                instances.Push(obj);
            }
        }

        /// <summary>
        /// Returns a new object from the pool.
        /// </summary>
        /// <returns>A new object from the pool.</returns>
        public GameObject GetObject()
        {
            var obj = instances.Count > 0 ? instances.Pop() : CreateInstance();
            obj.SetActive(true);
            return obj;
        }

        /// <summary>
        /// Returns the specified game object to the pool where it came from.
        /// </summary>
        /// <param name="obj">The object to return to its origin pool.</param>
        public void ReturnObject(GameObject obj)
        {
            var pooledObject = obj.GetComponent<PooledObject>();
            Assert.IsNotNull(pooledObject);
            Assert.IsTrue(pooledObject.pool == this);

            obj.SetActive(false);
            if (!instances.Contains(obj))
            {
                instances.Push(obj);
            }
        }
        
        public void ReturnObjectAfterTime(GameObject obj, float time)
        {
            var coroutine = WaitAndReturnObject(obj, time);
            StartCoroutine(coroutine);
        }
        
        private IEnumerator WaitAndReturnObject(GameObject obj, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            ReturnObject(obj);
        }

        /// <summary>
        /// Resets the object pool to its initial state.
        /// </summary>
        public void Reset()
        {
            var objectsToReturn = new List<GameObject>();
            foreach (var instance in transform.GetComponentsInChildren<PooledObject>())
            {
                if (instance.gameObject.activeSelf)
                {
                    objectsToReturn.Add(instance.gameObject);
                }
            }
            foreach (var instance in objectsToReturn)
            {
                ReturnObject(instance);
            }
        }

        /// <summary>
        /// Creates a new instance of the pooled object type.
        /// </summary>
        /// <returns>A new instance of the pooled object type.</returns>
        private GameObject CreateInstance()
        {
            var obj = Instantiate(prefab);
            var pooledObject = obj.AddComponent<PooledObject>();
            pooledObject.pool = this;
            obj.transform.SetParent(transform);
            return obj;
        }
    }
    
    public class PooledObject : MonoBehaviour
    {
        public ObjectPool pool;
        public GameObject go;//TODO переделать более оптимально

        public void ReturnToPool()
        {
            pool.ReturnObject(gameObject);
        }
    }
}

