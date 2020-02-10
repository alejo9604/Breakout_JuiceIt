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

        this.isFirstTime = true;

        if (this.dropsMenusItems.Length > 0)
            this.dropsMenusItems[0].SetIsCollapse(false);
        for (int i = 1; i < this.dropsMenusItems.Length; i++) {
            this.dropsMenusItems[i].SetIsCollapse(true);
        }
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
    
}
