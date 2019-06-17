﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Breakout : MonoBehaviour
{
    //Components
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private SpriteRenderer[] walls;
    [SerializeField]
    private List<SpriteRenderer> bricks;
    [SerializeField]
    private HUDSettings settings;

    [Header("Color")]
    [SerializeField]
    private Color backcgroundColor = Color.green;
    [SerializeField]
    private Color bricksColor = Color.green;
    [SerializeField]
    private Color wallsColor = Color.green;


    //Control vars.
    private bool _useColroSettings;


    private void Start()
    {
        this.EnterTweening();
    }

    private void Update()
    {
        //Inputs
        if (Input.GetKeyDown(KeyCode.Escape) && this.settings != null)
            this.settings.ToggletHide();


        //Color
        if (this._useColroSettings != Settings.EFFECT_SCREEN_COLORS)
        {
            this._useColroSettings = Settings.EFFECT_SCREEN_COLORS;
            this.UpdateColors();
        }
    }

    [ContextMenu("Get bricks")]
    public void GetBricks()
    {
        this.bricks = new List<SpriteRenderer>();
        GameObject[] bricksObjects = GameObject.FindGameObjectsWithTag("Brick");
        for (int i = 0; i < bricksObjects.Length; i++)
        {
            this.bricks.Add(bricksObjects[i].GetComponent<SpriteRenderer>());
        }
    }



    private void UpdateColors()
    {
        if (this.mainCamera != null)
            this.mainCamera.backgroundColor = Settings.EFFECT_SCREEN_COLORS ? this.backcgroundColor : Color.black;

        if (this.walls != null)
        {
            for (int i = 0; i < this.walls.Length; i++)
            {
                this.walls[i].color = Settings.EFFECT_SCREEN_COLORS ? this.wallsColor : Color.white;
            }
        }

        if (this.bricks != null)
        {
            for (int i = 0; i < this.bricks.Count; i++)
            {
                this.bricks[i].color = Settings.EFFECT_SCREEN_COLORS ? this.bricksColor : Color.white;
            }
        }
    }




    private void EnterTweening()
    {
        Vector3 maxCamPosY = Vector3.up * (this.GetCameraWorldSize().y + this.mainCamera.transform.position.y);

        if (this.bricks != null)
        {
            for (int i = 0; i < this.bricks.Count; i++)
            {
                this.bricks[i].transform.DOMove(this.bricks[i].transform.position, 0.6f).From(maxCamPosY + this.bricks[i].transform.position, true);
            }
        }
    }



    #region Utils

    public Vector2 GetCameraWorldSize()
    {
        Vector2 size = Vector2.zero;
        size.y = this.mainCamera.orthographicSize * 2;
        size.x = size.y * Screen.width / Screen.height;

        return size;
    }

    #endregion

}
