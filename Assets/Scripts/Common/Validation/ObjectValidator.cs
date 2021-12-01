using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection; 
using SiberianWellness.Common;
using UnityEngine;

using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SiberianWellness.Validation
{
	/// <summary>
	/// Проходит по дереву объектов и находит проблемы при пимоши валидаторов.
	/// Используется для поиска проблем в ассетах как в редакторе, так и в рантайме.
	/// </summary>
	public class ObjectValidator
	{
		public class SearchScope : IDisposable
		{
			bool printStats;
			 
			public SearchScope(bool printStats)
			{
				this.printStats = printStats;
				allScanedObject = new HashSet<object>();
				erroringFields  = new List<ValidationProblem>();
			}

			void IDisposable.Dispose()
			{
				searchScope = null;

				if (printStats)
					PrintStats();
				
				FinishSearch();
			}
		}

		static        SearchScope            searchScope;
		static        List<ValidationProblem> erroringFields;
		static        HashSet<System.Object> allScanedObject;
		public static List<ValidationProblem> Problems => erroringFields;

		static FieldValidator[] fieldsValidators;
        
		static int      maxActualDepth      = 0;
		static int      totalDepthSum       = 0;
		static int      totalUnityObjects          = 0;
		static int      totalObject    = 0;
		static int      maxDepthInIteration = 0;
		static TimeSpan totalTime           = new TimeSpan();


		public static SearchScope StartSearch(FieldValidator fieldValidator, bool printStats)
		{
			return StartSearch(new[] {fieldValidator},printStats);
		}

		public static SearchScope StartSearch(FieldValidator[] fieldsValidatorsForSearchProblems, bool printStats)
		{
			AssertWrapper.IsAllNotNull(fieldsValidatorsForSearchProblems);

			if (searchScope != null)
				throw new InvalidOperationException("Last search should be finished before start new search");

			fieldsValidators  = new List<FieldValidator>(fieldsValidatorsForSearchProblems).ToArray();
			searchScope = new SearchScope(printStats);
			return searchScope;
		}

		public static void ClearProblems()
		{
			if (erroringFields != null)
				erroringFields.Clear();
		}

		static void FinishSearch()
		{ 
			erroringFields  = null;
			allScanedObject = null;
			fieldsValidators      = null;

			totalObject = 0;
			maxActualDepth   = 0;
			totalUnityObjects       = 0;
			totalDepthSum    = 0;
			totalTime        = new TimeSpan();
		}


		static void PrintStats()
		{
			Debug.Log("TotalObjectCount = " + totalObject);
			Debug.Log("MaxActualDepth = " + maxActualDepth);
			Debug.Log("AverageActualDepth = " + ((float) totalDepthSum) / totalUnityObjects);
			Debug.Log("TotalMilliseconds = " + totalTime.TotalMilliseconds); 

		}

		public static void ValidateGo(GameObject sourceObject)
		{
			MonoBehaviour[] monobehaviours = sourceObject.GetComponents<MonoBehaviour>();
			for (int i = 0; i < monobehaviours.Length; i++)
			{
				try
				{
					ValidateObject(monobehaviours[i]);
				}

				catch (System.ArgumentNullException e)
				{
					//Эта ветка перехватывает случаи Missing(Mono Script)
					Debug.LogError(e.Message + " object=" + ValidationProblem.GameObjectFullName(sourceObject), sourceObject);
				}
			}
		}

		public static void ValidateObject(Object obj)
		{
			if(searchScope == null)
				throw new InvalidOperationException("search should start with a call " + nameof(StartSearch));

			Stopwatch w = new Stopwatch();
			w.Start();
			if (!obj) 
			{
				throw new ArgumentNullException("MonoBehaviour is null. It likely references a script that's been deleted.");
			}
  
			totalUnityObjects++;
			maxDepthInIteration = 0;
			MakeRecursion(obj,  obj,null,0);
			totalDepthSum += maxDepthInIteration;

			w.Stop();
			totalTime += w.Elapsed; //TODO Посчитать сумму со всей сцены и вывести
		}

		static bool NeedInCheck(Type type)
		{
			var assemblyName = type.Assembly.FullName;
			if (assemblyName.Contains("UnityEngine"))
				return false; //Отличное место для проверки Image.sprite

			if (assemblyName.Contains("mscorlib"))
				return false;

			return true;
		}

		static void GetFieldsIncludeInherited(Type type, List<FieldInfo> result)
		{
			var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

			result.AddRange(fields);
			if (NeedInCheck(type.BaseType))
				GetFieldsIncludeInherited(type.BaseType, result);
		}

		static void MakeRecursion(System.Object obj, Object root, FieldInfo field, int depth)
		{
			if (IsNull(obj))
				return;
			
			var type = obj.GetType();
			
			if (type.IsClass)
			{
				if (allScanedObject.Contains(obj))
					return;

				allScanedObject.Add(obj);
			}
 
			if(!NeedInCheck(type))
				return;
			
			 
			totalObject++;  
			 
			if (depth > maxDepthInIteration)
				maxDepthInIteration = depth;

			if (depth > maxActualDepth)
				maxActualDepth = depth;

			

			List<FieldInfo> fields= new List<FieldInfo>();
			GetFieldsIncludeInherited(obj.GetType(), fields);
			 
			foreach (FieldInfo f in fields)
			{ 
#if UNITY_EDITOR
				//Поля не сереализуемые в резакторе скипаем если игра не запущена. Они будут заполнены только в рантайме.
				if (!f.IsPublic && EditorApplication.isPlaying == false)
				{
					SerializeField sf = f.GetCustomAttribute<SerializeField>();
					if (sf == null)
						continue;
				}
#endif

				object fieldObject = f.GetValue(obj);

				foreach (var v in fieldsValidators)
					v.FindProblems(fieldObject, fieldObject?.GetType(), f, root);
				
				if(NeedInRecursion(fieldObject))
					MakeRecursion(fieldObject, root, f, depth++);

				if (fieldObject is ICollection collection) //Нельзя ходить по IEnumareble, так как будем залазить в символы строки=(
				{
					foreach (var item in collection)
					{
						foreach (var v in fieldsValidators)
							v.FindProblems(item, item?.GetType(), f, root);

						if(NeedInRecursion(item))
							MakeRecursion(item, root, f, depth++);
					}
				}
			}
		}

		static bool NeedInRecursion(System.Object obj)
		{
			if (obj is ScriptableObject so)
			{
				bool existInAssets = true;
#if UNITY_EDITOR
			existInAssets = !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(so));
#endif
				return !existInAssets;
			}
			
			//не заходим никуда больлше, все остльные либо не нужны, либо будут проверены и так 
			if (obj is UnityEngine.Object)
				return false;

			return true;
		}

		static bool IsNull(object obj)
		{ 
			if (obj == null || obj.Equals(null))
			{
				return true;
			}
			else
			{  
				return false;
			}
		} 
	}
}