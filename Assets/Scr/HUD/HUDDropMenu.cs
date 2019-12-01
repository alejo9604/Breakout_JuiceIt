using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDDropMenu : HUDItem
{
    [Space(20)]
    [SerializeField] private RectTransform tittleRect;
    [SerializeField] private GameObject options;

    private bool isCollapse = true;
    private Vector2 initSizedelta;
    private float titleSize = 40;

    public Action<HUDDropMenu> OnDropMenuGroupChange;

    public float GetHeight()
    {
        return this.isCollapse ? this.titleSize : this.initSizedelta.y;
    }

    public void SetIsCollapse(bool value)
    {
        this.isCollapse = value;
        this.SetOptions();
    }

    public void ToggleCollapse()
    {
        this.isCollapse = !this.isCollapse;
        this.SetOptions();
    }

    private void SetOptions()
    {
        this.options.SetActive(!this.isCollapse);
    }

    public void SetPosition(float yPos)
    {
        this.rectTransform.anchoredPosition = new Vector2(this.rectTransform.anchoredPosition.x, yPos);
    }

    public override void Init()
    {
        base.Init();

        this.initSizedelta = this.rectTransform.sizeDelta;

        if (this.tittleRect == null)
            this.tittleRect = this.transform.GetChild(0).GetComponent<RectTransform>();
        this.titleSize = this.tittleRect.sizeDelta.y;

        this.SetIsCollapse(true);
    }


    protected override void OnTriggerClick()
    {
        if (!this.CanClick())
            return;

        base.OnTriggerClick();
        this.ToggleCollapse();

        if (this.OnDropMenuGroupChange != null)
            this.OnDropMenuGroupChange(this);
    }

}
