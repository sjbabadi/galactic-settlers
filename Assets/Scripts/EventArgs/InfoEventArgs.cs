using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///  Generic version of EventArgs to pass along information when posting events.
/// </summary>
/// <typeparam name="T"></typeparam>
public class InfoEventArgs<T> : EventArgs
{
    public T info;

    public InfoEventArgs()
    {
        info = default(T);
    }

    public InfoEventArgs(T info)
    {
        this.info = info;
    }
}