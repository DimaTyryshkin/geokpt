using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SiberianWellness.Validation
{

    /// <summary>
    /// Not null violation represents data for objects that do not have their required (NotNull) fields  
    /// assigned.
    /// </summary>
    public class ValidationProblem
    {  
        public ValidationProblem(FieldInfo fieldInfo, Object sourceMB, string header)
        {
            FieldInfo = fieldInfo;
            SourceObject = sourceMB;
            Header = header;
        }
 
        public FieldInfo FieldInfo { get;   } 
        public Object SourceObject { get;   }
        public string Header { get;   }
 
        public string FullName
        {
            get
            {
                if (SourceObject is MonoBehaviour mb)
                {
                    return GameObjectFullName(mb.gameObject);
                }
                else
                {
                    return SourceObject.name;
                }
            }
        }

        public static string GameObjectFullName(GameObject go)
        {
            Transform currentParent = go.transform.parent;
            string    fullName      = go.name;
            while (currentParent != null)
            {
                fullName      = currentParent.gameObject.name + "/" + fullName;
                currentParent = currentParent.transform.parent;
            }

            return fullName;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current 
        /// <see cref="ValidationProblem"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current 
        /// <see cref="ValidationProblem"/>.</returns>
        public override string ToString()
        {
            return $"[{Header}: Field={FieldInfo.Name}, FullName={FullName}]";
        }
    }
}
