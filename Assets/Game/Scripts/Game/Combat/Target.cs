using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Action<Target> DestroyEvent;

    private void OnDestroy() {
        DestroyEvent?.Invoke(this);
    }
}
