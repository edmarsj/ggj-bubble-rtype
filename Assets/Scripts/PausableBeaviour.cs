using System;
using UnityEngine;

public abstract class PausableBehaviour: MonoBehaviour
{
    [Header("Shared References")]
    [SerializeField] protected Shared _shared;
    [Space]
    private bool _isPaused = false;

    private void Update()
    {
        if (!_shared.IsPaused)
        {
            if (_isPaused)
            {
                OnUnpause();
            }

            DoUpdate();
        }
        else if(!_isPaused)
        {
            _isPaused = true;
            OnPause();
        }


    }

    protected virtual void OnUnpause()
    {
     
    }

    protected virtual void DoUpdate() { }
    protected virtual void OnPause() { }

}
