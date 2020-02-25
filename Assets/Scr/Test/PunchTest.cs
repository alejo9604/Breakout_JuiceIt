using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PunchTest : MonoBehaviour
{

    public Transform target;
    public Vector3 initScale;
    Coroutine courutine;
    Tween tween;
    
    public float punch;
    public float duration = 0.8f;
    public int vibrato = 10;
    public float elas = 1;
    

    // Start is called before the first frame update
    void Start()
    {
        this.initScale = this.target.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            this.EndTween();
            if (this.courutine != null)
                StopCoroutine(this.courutine);
            this.target.localScale = this.initScale;
            this.courutine = StartCoroutine(Test());
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            this.EndTween();
            if (this.courutine != null)
                StopCoroutine(this.courutine);
            this.target.localScale = this.initScale;
            this.tween = this.target.DOPunchScale(Vector3.one * this.punch, duration, vibrato, elas);
        }
    }

    private void EndTween()
    {
        if(this.tween != null && this.tween.IsActive() && this.tween.IsPlaying()){
            this.tween.Complete();
            this.tween.Kill();
            this.tween = null;
        }
    }

    public float speed = 1;
    float t = 0;
    float x = 1;
    IEnumerator Test()
    {
        t = 0;
        x = 1;
        while (Mathf.Abs(x) > 0.001f) {
            t += Time.deltaTime* speed;
            x = this.EvaluatePunch(t);
            this.target.localScale = this.initScale + Vector3.one * x;
            yield return null;
        }
    }

    [Space(20)]
    public float amplitud = 1;
    public float period = 1;
    public float fast = 1;

     
    private float EvaluatePunch(float t)
    {
        return amplitud * (float) Math.Pow(Math.E, -(fast*t)) * Mathf.Cos(2 * Mathf.PI * period * t);

    }
}
