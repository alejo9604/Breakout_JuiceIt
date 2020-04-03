using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDDropGroup : MonoBehaviour
{

    private HUDDropMenu[] dropsMenusItems;

    [Range(0, 15)]
    public float spacing = 5;

    //Control var.
    private bool isFirstTime = true;

    void Start()
    {
        this.Init();

        EventManager.Instance.AddListener<InputToggleSettingsEvent>(this.CheckFirstTime);
    }

    private void OnDestroy()
    {
        if(EventManager.HasInstance())
            EventManager.Instance.RemoveListener<InputToggleSettingsEvent>(this.CheckFirstTime);
    }

    [ContextMenu("Init")]
    private void Init()
    {
        this.dropsMenusItems = this.GetComponentsInChildren<HUDDropMenu>();
        for (int i = 0; i < this.dropsMenusItems.Length; i++) {
            this.dropsMenusItems[i].Init();
            this.dropsMenusItems[i].OnDropMenuGroupChange = this.OnDropMenuItemChanged;
        }

    }

    private void CheckFirstTime(InputToggleSettingsEvent e)
    {
        if (!this.isFirstTime)
            return;

        this.isFirstTime = false;

        this.CollapseItems(true, true);
        this.UpdateDropMenuItems();
    }


    private void OnDropMenuItemChanged(HUDDropMenu itemChanged)
    {
        for (int i = 0; i < this.dropsMenusItems.Length; i++) {
            if (this.dropsMenusItems[i] != itemChanged)
                this.dropsMenusItems[i].SetIsCollapse(true);
        }

        this.UpdateDropMenuItems();
    } 

    private void UpdateDropMenuItems()
    {
        float pos = 0;
        for (int i = 0; i < this.dropsMenusItems.Length; i++) {
            this.dropsMenusItems[i].SetPosition(pos);
            pos -= this.dropsMenusItems[i].GetHeight() - this.spacing;
        }
    }

    private void CollapseItems(bool collapse, bool avoidFirstOne = false)
    {
        for (int i = 0; i < this.dropsMenusItems.Length; i++) {
            if (avoidFirstOne && i == 0) {
                this.dropsMenusItems[i].SetIsCollapse(!collapse);
                continue;
            }
            this.dropsMenusItems[i].SetIsCollapse(collapse);
        }
    }


    #region Util
    [ContextMenu("CollapseAll")]
    private void CollapseAll() { this.CollapseItems(true); this.UpdateDropMenuItems(); }

    [ContextMenu("ExpandAll")]
    private void ExpandAll() { this.CollapseItems(false); this.UpdateDropMenuItems(); }
    #endregion Util

}
