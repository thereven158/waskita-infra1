using System;
using A3.UserInterface;
using UnityEngine;

public abstract class DelayedDisplayBehavior : DisplayBehavior
{
    public abstract override bool IsOpen { get; }
    
    public abstract void Open(Action onFinish = null);

    public abstract override void Close();

    public abstract override void Init();
}
