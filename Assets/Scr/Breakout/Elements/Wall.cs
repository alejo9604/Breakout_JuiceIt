using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : BreakoutElement
{
    private WobbleLine wobble;

    protected override void Start()
    {
        base.Start();
        this.wobble = this.GetComponentInChildren<WobbleLine>();
    }


    public override void ChangeColor(Color color)
    {
        base.ChangeColor(color);
        this.wobble?.ChangeColor(color);
    }
}
