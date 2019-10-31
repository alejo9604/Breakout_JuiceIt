using UnityEngine;

public class BreakoutElement : MonoBehaviour
{
    //Components
    protected SpriteRenderer render;

    protected virtual void Start()
    {
        this.render = GetComponent<SpriteRenderer>();
    }

    public virtual void Init(){ }

    public virtual void ResetElement() { }


    public virtual void ChangeColor(Color color)
    {
        this.render.color = color;
    }

}
