using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Breakout : MonoBehaviour
{
    //Components
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Wall[] walls;
    [SerializeField]
    private List<Brick> bricks;

    [Header("Color")]
    [SerializeField]
    private Color backcgroundColor = Color.green;
    [SerializeField]
    private Color bricksColor = Color.green;
    [SerializeField]
    private Color wallsColor = Color.green;




    private void Start()
    {
        //TODo: move this to event
        this.EnterTweening();

        //Add events
        EventManager.Instance.AddListener<ChangeColorEvent>(this.OnChangeColor);
        EventManager.Instance.AddListener<InputResetLevelEvent>(this.OnInputResetLevel);
    }

    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<ChangeColorEvent>(this.OnChangeColor);
            EventManager.Instance.RemoveListener<InputResetLevelEvent>(this.OnInputResetLevel);
        }
    }


    [ContextMenu("Get bricks")]
    public void GetBricks()
    {
        this.bricks = new List<Brick>();
        GameObject[] bricksObjects = GameObject.FindGameObjectsWithTag("Brick");
        Brick tempBrick = null;
        for (int i = 0; i < bricksObjects.Length; i++)
        {
            tempBrick = bricksObjects[i].GetComponent<Brick>();
            if (tempBrick == null)
                tempBrick = bricksObjects[i].AddComponent<Brick>();
            this.bricks.Add(tempBrick);
            this.bricks[i].SetBrickId(i);
        }
    }

    #region Events
    private void OnChangeColor(ChangeColorEvent e)
    {
        if (this.mainCamera != null)
            this.mainCamera.backgroundColor = Settings.EFFECT_SCREEN_COLORS ? this.backcgroundColor : Color.black;

        if (this.walls != null)
        {
            for (int i = 0; i < this.walls.Length; i++)
            {
                this.walls[i].ChangeColor(Settings.EFFECT_SCREEN_COLORS ? this.wallsColor : Color.white);
            }
        }

        if (this.bricks != null)
        {
            for (int i = 0; i < this.bricks.Count; i++)
            {
                this.bricks[i].ChangeColor(Settings.EFFECT_SCREEN_COLORS ? this.bricksColor: Color.white);
            }
        }
    }

    private void OnInputResetLevel(InputResetLevelEvent e)
    {
        if (this.bricks != null) {
            for (int i = 0; i < this.bricks.Count; i++) {
                this.bricks[i].ResetElement();
            }
        }
    }

    #endregion Events


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
