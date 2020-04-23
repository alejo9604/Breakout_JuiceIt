using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemEffect : MonoBehaviour
{

    [SerializeField] private ParticleSystem ps;

    private Color currentColor = Color.white;

    public void SetMainColor(Color color)
    {
        this.currentColor = color;
    }

    public void Init()
    {
        if (this.ps == null)
            this.ps = this.transform.GetComponentInChildren<ParticleSystem>();

        if (ps == null) {
            Debug.LogError("No Particle System found.");
            return;
        }

        this.ps.SetMainColor(this.currentColor);
    }
}
