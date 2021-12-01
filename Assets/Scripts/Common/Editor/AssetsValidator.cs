using System;
using System.Collections;
using SiberianWellness.Common;
using SiberianWellness.Validation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace SiberianWellness.Validation
{
	/// <summary>
	/// Поиск проблем во всех файлах проекта. Только в редакторе
	/// </summary>
	public static class AssetsValidator
	{
		public static IEnumerator PrintNullInAllScenes(FieldValidator[] validators)
		{
			var scope = ObjectValidator.StartSearch(validators, true);

			string[] scenes = new string[]
			{
				"Menu",
				"Persistent",
				"LocationPersistent",
				"Home",
				"Home_LM",
				"MatchThree",
			};
			
			

			foreach (var sceneName in scenes)
			{
				var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/" + sceneName +".unity");
				Assert.IsNotNull(sceneAsset, $"Не найдена сцена с именем '{sceneName}'");
				
				float progress = (float) scenes.IndexOf(sceneName) / scenes.Length;
				EditorUtility.DisplayProgressBar("Поиск Null на всех сценах", "", progress);

				var pathToScene = AssetDatabase.GetAssetPath(sceneAsset);
				Debug.Log($"<b>Scene {sceneAsset.name}</b>");
				EditorSceneManager.OpenScene(pathToScene);

				//На всякий случай ждем немного 
				yield return null;
				yield return null;
				yield return null;
				yield return null;
				PrintNullInScene();
			}

			((IDisposable)scope).Dispose();

			EditorUtility.ClearProgressBar();
		}

		public  static void PrintNullInScriptableObject() 
		{
			string[] assetsGuid = AssetDatabase.FindAssets("t:ScriptableObject");
			foreach (string guid in assetsGuid)
			{
				string pathToObj = AssetDatabase.GUIDToAssetPath(guid);

				if (pathToObj.Contains("CandyMatch3Kit")) //TODO remove
				{
					continue;
				}

				ScriptableObject obj = (ScriptableObject) AssetDatabase.LoadAssetAtPath(pathToObj, typeof(ScriptableObject));

				PrintNullOnScriptableObject(obj, pathToObj);
			}
		}

		public static void PrintNullInPrefabs()
		{
			string[] assetsGuid = AssetDatabase.FindAssets("t:GameObject");

			foreach (string guid in assetsGuid)
			{
				string pathToGameObject = AssetDatabase.GUIDToAssetPath(guid);

				if (pathToGameObject.Contains("CandyMatch3Kit")) //TODO remove
				{
					continue;
				}

				GameObject gameObject = (GameObject) AssetDatabase.LoadAssetAtPath(pathToGameObject, typeof(GameObject));

				PrintNullOnGameObject(gameObject, pathToGameObject, gameObject);
			}
		}
 
		public static void PrintNullInScene()
		{
			var scene                = SceneManager.GetSceneAt(0);  
			var rootSceneGameObjects = scene.GetRootGameObjects();

			foreach (GameObject rootGameObjectInScene in rootSceneGameObjects)
			{
				PrintNullOnGameObject(rootGameObjectInScene, "Scene "+ SceneManager.GetActiveScene().name);
			}
		}

		static void PrintNullOnGameObject(GameObject gameObject, string pathToAsset, GameObject ownerForLog = null)
		{
			ObjectValidator.ClearProblems();
			ObjectValidator.ValidateGo(gameObject);
			if (ObjectValidator.Problems != null && ObjectValidator.Problems.Count > 0)
			{
				foreach (ValidationProblem violation in ObjectValidator.Problems)
				{
					Debug.LogError(violation + "\nPath: " + pathToAsset, ownerForLog ?? gameObject);
				}
			}

			foreach (Transform child in gameObject.transform)
			{
				PrintNullOnGameObject(child.gameObject, pathToAsset, ownerForLog);
			}
		}

		static void PrintNullOnScriptableObject(ScriptableObject scriptableObject, string pathToAsset)
		{
			ObjectValidator.ClearProblems();
			ObjectValidator.ValidateObject(scriptableObject);
			if (ObjectValidator.Problems.Count > 0)
			{
				foreach (ValidationProblem violation in ObjectValidator.Problems)
				{
					Debug.LogError(violation + "\nPath: " + pathToAsset, scriptableObject);
				}
			}
		}
	}
}