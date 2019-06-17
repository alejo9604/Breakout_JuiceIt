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
    private bool _useColroSettings;

    private void Start() {
        this.targetPos = this.transform.position;
        this.mainCamera = Camera.main;

        this.render = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //Movement
        this.targetPos.x = mainCamera.ScreenToWorldPoint( Input.mousePosition).x;
        this.targetPos.x = Mathf.Clamp(this.targetPos.x, -clampXPos, clampXPos);
        this.transform.position = Vector3.Lerp(this.transform.position, this.targetPos, Time.deltaTime * speed);

        //Color
        if (this._useColroSettings != Settings.EFFECT_SCREEN_COLORS)
        {
            this.render.color = Settings.EFFECT_SCREEN_COLORS ? this.color : Color.white;
            this._useColroSettings = Settings.EFFECT_SCREEN_COLORS;
        }

    }



    private void OnDrawGizmosSelected() {
        Vector3 center = this.transform.position;
        center.x = 0;
        Gizmos.DrawWireCube(center, new Vector3(clampXPos*2, this.transform.localScale.y));
    }
}
