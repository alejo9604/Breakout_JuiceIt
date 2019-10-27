using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float clampXPos = 5f;

    [Header("Color")]
    [SerializeField]
    private Color color;

    //Components
    private SpriteRenderer render;

    //Control var
    private Camera mainCamera;
    private Vector3 targetPos;

    private void Start() {
        this.targetPos = this.transform.position;

        //TODO: Optimize this
        this.mainCamera = Camera.main;
        this.render = GetComponent<SpriteRenderer>();

        //Add events
        EventManager.Instance.AddListener<ChangeColorEvent>(this.OnChangeColor);
    }

    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<ChangeColorEvent>(this.OnChangeColor);
        }
    }

    void Update()
    {
        //Movement
        this.targetPos.x = mainCamera.ScreenToWorldPoint( Input.mousePosition).x;
        this.targetPos.x = Mathf.Clamp(this.targetPos.x, -clampXPos, clampXPos);
        this.transform.position = Vector3.Lerp(this.transform.position, this.targetPos, Time.deltaTime * speed);
    }


    #region Events
    private void OnChangeColor(ChangeColorEvent e)
    {
        this.render.color = Settings.EFFECT_SCREEN_COLORS ? this.color : Color.white;
    }
    #endregion Events


    private void OnDrawGizmosSelected() {
        Vector3 center = this.transform.position;
        center.x = 0;
        Gizmos.DrawWireCube(center, new Vector3(clampXPos*2, this.transform.localScale.y));
    }
}
