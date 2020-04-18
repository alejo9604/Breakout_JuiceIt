using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParticleColor : MonoBehaviour
{
    public ParticleSystemEffect PS;

    public Color color;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            this.Test();
    }


    private void Test()
    {
        /*
        ParticleSystem.MainModule settings = this.PS.main;
        psColor = settings.startColor;

        Color minColor = this.color;
        minColor.a = psColor.colorMin.a;
        psColor.colorMin = minColor;

        Color maxColor= this.color;
        maxColor.a = psColor.colorMax.a;
        psColor.colorMax = maxColor;

        settings.startColor = psColor;
        */

        this.PS.SetMainColor(this.color);
        this.PS.OnInit();
    }
}
