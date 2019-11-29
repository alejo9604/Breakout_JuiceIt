using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDItem : MonoBehaviour
{
    protected const float CLICKED_COOLDOWN = 0.2f;

    protected RectTransform rectTransform;
    [SerializeField] private bool useCooldown = true;
    [SerializeField] protected Button m_Button;

    //Control var.
    private float cooldownTimer = 0;

    private void SetCooldown()
    {
        this.cooldownTimer = this.useCooldown ? Time.time + CLICKED_COOLDOWN : 0;
    }

    public bool CanClick()
    {
        return Time.time > this.cooldownTimer;
    }

    public virtual void SetInteractable(bool value)
    {
        if (this.m_Button != null)
            this.m_Button.interactable = value;
    }

    public virtual void Init()
    {
        this.rectTransform = this.GetComponent<RectTransform>();

        if (this.m_Button == null) 
            this.m_Button = this.GetComponent<Button>();
        
        if (this.m_Button != null) {
            this.m_Button.onClick.AddListener(this.OnTriggerClick);
        }
    }

    protected virtual void Update()
    {
    }

    public virtual void Render()
    {
    }


    protected virtual void OnTriggerClick()
    {
        this.SetCooldown();
    }

}
