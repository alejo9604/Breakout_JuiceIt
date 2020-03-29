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
        this.wobble?.EnableWobble(Settings.BOUNCY_LINES_ENABLED);

        EventManager.Instance.AddListener<InputToggleBouncyLinesEvent>(this.OnInputToggleBouncyLines);
    }


    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<InputToggleBouncyLinesEvent>(this.OnInputToggleBouncyLines);
        }
    }


    public override void ChangeColor(Color color)
    {
        base.ChangeColor(color);
        this.wobble?.ChangeColor(color);
    }


    public override void OnCollision(Vector2 position, Vector2 velocity)
    {
        base.OnCollision(position, velocity);
        this.wobble?.OnImpact(position, velocity);
    }

    #region Events
    private void OnInputToggleBouncyLines(InputToggleBouncyLinesEvent e)
    {
        this.wobble?.EnableWobble(e.enabled);
    }
    #endregion Events
}
