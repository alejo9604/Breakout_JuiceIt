using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleSystemExtensions
{

    public static void SetMainColor(this ParticleSystem target, Color color)
    {
        ParticleSystem.MainModule mainModule = target.main;
        ParticleSystem.MinMaxGradient mainColor = mainModule.startColor;

        switch (mainColor.mode) {
            case ParticleSystemGradientMode.Color:
                mainColor.color = OverrideColorPreserveAlpha(color, mainColor.color);
                break;

            case ParticleSystemGradientMode.TwoColors:
                mainColor.colorMin= OverrideColorPreserveAlpha(color, mainColor.colorMin);
                mainColor.colorMax = OverrideColorPreserveAlpha(color, mainColor.colorMax);
                break;

            default:
                return;
        }

        mainModule.startColor = mainColor;
    }
    

    private static Color OverrideColorPreserveAlpha(Color color, Color targetColor)
    {
        Color newColor = color;
        newColor.a = targetColor.a;
        return newColor;
    }
}
