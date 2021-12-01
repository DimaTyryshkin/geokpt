using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SiberianWellness.Common
{
    public static class AssetDatabaseWrapper
    {
#if UNITY_EDITOR
        public static void CreateFolder(string path)
        {
            AssetDatabase.DeleteAsset(path);

            var p = path.Split('/');

            string curPath = "Assets";

            for (int i = 1; i < p.Length; i++)
            {
                var nextPath = curPath + "/" + p[i];

                if (!Directory.Exists(nextPath))
                {
                    AssetDatabase.CreateFolder(curPath, p[i]);
                }

                curPath = nextPath;
            }
        }
#endif
    }
}