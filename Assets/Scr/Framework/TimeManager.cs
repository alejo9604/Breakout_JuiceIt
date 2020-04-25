using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Object=UnityEngine.Object;

public class TimeManager : Singleton<TimeManager>
{
    public Coroutine Defer(Action callback)
    {
        return StartCoroutine(this.DeferEnumerator(callback));
    }

    private IEnumerator DeferEnumerator(Action callback)
    {
        yield return new WaitForEndOfFrame();

        if (callback != null) {
            callback();
        }
    }

    public Coroutine DeferFixedUpdate(Action callback)
    {
        return StartCoroutine(this.DeferFixedUpdateEnumerator(callback));
    }

    private IEnumerator DeferFixedUpdateEnumerator(Action callback)
    {
        yield return new WaitForFixedUpdate();

        if (callback != null) {
            callback();
        }
    }

    public Coroutine Delay(Action callback, float delay)
    {
        return StartCoroutine(this.DelayEnumerator(callback, delay));
    }

    private IEnumerator DelayEnumerator(Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (callback != null) {
            callback();
        }
    }

    public Coroutine DelayUntil(Action callback, Func<bool> waitCondition)
    {
        return StartCoroutine(this.DelayEnumerator(callback, waitCondition));
    }

    private IEnumerator DelayEnumerator(Action callback, Func<bool> waitCondition)
    {
        yield return new WaitUntil(waitCondition);

        if (callback != null) {
            callback();
        }
    }
}
