using System.Collections;
using UnityEditor;
using UnityEngine;

namespace SiberianWellness.Common
{
    public class EditorCoroutine
    {
        /// <summary>
        /// Use only "yield return null" to skip frame in editor
        /// </summary> 
        /// <param name="routineContainer">corotine still live while <paramref name="routineContainer"/> still exist </param>
        /// <returns></returns>
        public static EditorCoroutine Start(IEnumerator routine, UnityEngine.Object routineContainer = null)
        { 
            EditorCoroutine coroutine = new EditorCoroutine(routine, routineContainer);
            coroutine.Start();
            return coroutine;
        }

        IEnumerator routine;
        Object      routineContainer;
        bool        routineContainerRequired;

        EditorCoroutine(IEnumerator routine, Object routineContainer = null)
        {
            this.routine             = routine;
            this.routineContainer    = routineContainer;
            routineContainerRequired = routineContainer;
        }

        void Start()
        {
            EditorApplication.update += Update;
        }

        public void Stop()
        {
            EditorApplication.update -= Update;
        }

        void Update()
        {
            if (!routineContainerRequired || routineContainer)
            {
                if (!routine.MoveNext())
                {
                    Stop();
                }
            }
            else
            {
                Stop();
            }
        }
    }
}