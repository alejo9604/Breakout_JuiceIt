using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PunchTest : MonoBehaviour
{

    public Transform target;

    public float punch;
    public float duration = 0.8f;
    public int vibrato = 10;
    public float elas = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            this.target.DOPunchScale(Vector3.one * this.punch, duration, vibrato, elas);
        }
    }
}
