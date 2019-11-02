using UnityEngine;

public class BreakoutElement : MonoBehaviour
{
    //Components
    protected SpriteRenderer render;

    //Control vars
    protected Vector3 initPosition;
    protected Vector3 initScale;

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

}
