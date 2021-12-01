using System;
using System.IO;

using UnityEngine;

using System.Collections.Generic; 
using SiberianWellness.NotNullValidation;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SiberianWellness.Common
{ 
    [CreateAssetMenu]
    public class ObjectCollection<T> : ScriptableObject where T : Object
    {
        [Serializable]
        public class ObjectCollectionException : Exception
        {  
            public ObjectCollectionException(string collectionName, string itemAlias) : base($"В коллекции '{collectionName}' отсутствует элемент '{itemAlias}'")
            {
            }
        }
        
        
        [IsntNull]
        public T[] collection;

        public int Count
        {
            get { return collection.Length; }
        }

        public void AssertElements()
        {
            AssertWrapper.IsAllNotNull(collection);
        }

        public T GetByIndex(int index)
        {
            return collection[index];
        }

        public T GetByName(string name)
        {
            foreach (var obj in collection)
            {
                if (obj.name == name)
                    return (T) obj;
            }

            throw new ObjectCollectionException(this.name,  name);
        }

        public int GetIndex(T obj)
        {
            for (int k = 0; k < collection.Length; k++)
            {
                if (obj.name == collection[k].name)
                    return k;
            }
 
            throw new ObjectCollectionException(name, obj.name);
        } 

#if UNITY_EDITOR
        protected static void LoadInternal (MenuCommand command, bool includeSubFolder = false)
        {
            var t = command.context as ObjectCollection<T>;
            t.LoadFromCurrentDirectory(includeSubFolder);
        }

        void LoadFromCurrentDirectory(bool includeSubFolder)
        {
            var fileName = AssetDatabase.GetAssetPath(this);
            var path     = Path.GetDirectoryName(fileName);

            List<T> result = new List<T>();

            if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
            {
                var allGo = EditorExtension.LoadAllAssetsOfType<GameObject>(path, includeSubFolder);
                foreach (var obj in allGo)
                {
                    if (obj is GameObject go)
                    {
                        var c = go.GetComponent<T>();
                        if (c)
                            result.Add(c);
                    }
                }
            }
            else
            {
                //GameObject, ScriptableObject, Sprite ...
                var allAssets = EditorExtension.LoadAllAssetsOfType<T>(path, includeSubFolder);
                foreach (var obj in allAssets)
                {
                    if (obj is T inst)
                    {
                        result.Add(inst);
                    }
                }
            }

            collection = result.ToArray();
            EditorUtility.SetDirty(this);
        }

#endif
    }
}