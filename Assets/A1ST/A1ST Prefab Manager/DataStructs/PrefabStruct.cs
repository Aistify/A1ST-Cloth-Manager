#if UNITY_EDITOR

using System;
using UnityEngine;

namespace A1STPrefab
{
    [Serializable]
    public class PrefabStruct
    {
        public string prefabName;
        public GameObject prefab;
    }
}
#endif
