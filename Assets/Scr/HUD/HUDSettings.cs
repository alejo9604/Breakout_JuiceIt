using UnityEngine;
using UnityEngine.UI;

public class HUDSettings : MonoBehaviour
{

    //Control var.
    private bool isHide = true;

    private void Start()
    {
        this.gameObject.SetActive(!this.isHide);

        //Add events
        EventManager.Instance.AddListener<InputToggleSettingsEvent>(this.OnToggle);
    }

    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<InputToggleSettingsEvent>(this.OnToggle);
        }
    }


    #region Events
    private void OnToggle(InputToggleSettingsEvent e)
    {
        this.isHide = !this.isHide;
        this.gameObject.SetActive(!this.isHide);
    }
    #endregion Events


    #region ButtonActions
    //Color
    public void ToogleScreenColor(bool value)
    {
        Settings.EFFECT_SCREEN_COLORS = value;
        EventManager.Instance.Trigger(new ChangeColorEvent
        {
            enable = value
        });
    }


    //Tweening
    public void ToogleTweeningEnable(bool value)
    {
        Settings.IS_TWEENING_ENABLE = value;
    }

    public void ToogleMoveYAtStart(bool value)
    {
        Settings.TWEENING_Y_AT_START = value;
    }

    public void ToogleRotationStart(bool value)
    {
        Settings.TWEENING_ROTATION_AT_START = value;
    }
    #endregion ButtonActions

}
