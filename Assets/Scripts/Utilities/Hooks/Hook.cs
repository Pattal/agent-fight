using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public abstract class Hook<T> : MonoBehaviour where T : class
{
    [Tooltip("Transform that it will be saved on the Transform var asset")]
    public T Reference;

    [Tooltip("Transform Scritable var that will store at runtime a transform")]
    public ScriptableVariable<T> HookVariable;

    [Tooltip("Should be cleared on OnDisable event")]
    [SerializeField] private bool _clearOnDisable = true;


    private void OnEnable()
    {
        UpdateHook();
    }

    private void OnDisable()
    {
        if (_clearOnDisable)
            DisableHook(); 
    }

    private void OnDestroy()
    {
        DisableHook();
    }


    public virtual void UpdateHook() => HookVariable.Value = Reference;
    public virtual void DisableHook() => HookVariable.Value = null;
    public virtual void RemoveHook() => HookVariable.Value = null;
}
