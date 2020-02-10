using UnityEngine;
using DG.Tweening;

public class BreakoutElement : MonoBehaviour
{
    //Components
    protected SpriteRenderer render;

    //Control vars
    protected Vector3 initPosition;
    protected Vector3 initScale;

    //Glow
    protected Tween glowTween;

    protected virtual void Start()
    {
        this.render = GetComponent<SpriteRenderer>();

        this.initPosition = this.transform.position;
        this.initScale = this.transform.localScale;
    }

    public virtual void Init(){ }

    public virtual void ResetElement() { }


    public virtual void ChangeColor(Color color)
    {
        this.render.color = color;
    }

    public virtual void OnGlowColor(Color glowColor, Color initColor, float duration, Ease ease = Ease.InBack)
    {
        if (this.render == null)
            return;

        if(this.glowTween != null && (this.glowTween.IsActive() || this.glowTween.IsPlaying())) {
            this.glowTween.Complete();
            this.glowTween.Kill();
            this.glowTween = null;
        }

        this.glowTween = this.render.DOColor(initColor, duration).From(glowColor).SetEase(ease);
    }
}
