using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : BreakoutElement
{

    private int id;

    public int GetBrickId()
    {
        return this.id;
    }

    public void SetBrickId(int id)
    {
        this.id = id;
    }

    public override void ResetElement()
    {
        base.ResetElement();

        this.gameObject.SetActive(true);
    }
}
