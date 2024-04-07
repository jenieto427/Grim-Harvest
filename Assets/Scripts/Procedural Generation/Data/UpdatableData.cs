using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ensure this using directive is only compiled in the Unity Editor
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UpdatableData : ScriptableObject
{
    public event System.Action OnValuesUpdated;
    public bool autoUpdate;

    // Use UNITY_EDITOR preprocessor directive to exclude editor-specific code from builds
#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        EditorApplication.delayCall += _OnValidate;
    }

    void _OnValidate()
    {
        if (autoUpdate)
        {
            EditorApplication.update += NotifyOfUpdatedValues;
        }
    }
#endif

    public void NotifyOfUpdatedValues()
    {
        // The update event subscription and unsubscription should also be wrapped
        // as they depend on the UnityEditor namespace
#if UNITY_EDITOR
        EditorApplication.update -= NotifyOfUpdatedValues;
#endif

        if (OnValuesUpdated != null)
        {
            OnValuesUpdated();
        }
    }
}
