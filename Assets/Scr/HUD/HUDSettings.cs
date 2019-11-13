using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class HUDSettings : MonoBehaviour
{

    //Control var.
    private bool isHide = true;

    [Header("Color")]
    [SerializeField] private Toggle colorToggle;

    [Header("Tweening")]
    [SerializeField] private Toggle tweeningEnableToggle;
    [SerializeField] private Toggle moveYAtStartToggle;
    [SerializeField] private Toggle rotationAtStartToggle;
    [SerializeField] private Toggle scaleAtStartToggle;
    [SerializeField] private Slider delayOnBlocksDelaySldier;
    [SerializeField] private TextMeshProUGUI delayOnBlocksValue;
    [SerializeField] private Slider tweenDurationSlider;
    [SerializeField] private TextMeshProUGUI tweenDurationValue;
    [SerializeField] private Slider tweenEaseSlider;
    [SerializeField] private TextMeshProUGUI tweenEaseValue;


    private void Start()
    {
        this.gameObject.SetActive(!this.isHide);

        this.ResetHUD();

        //Add events
        EventManager.Instance.AddListener<InputToggleSettingsEvent>(this.OnToggle);
    }

    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<InputToggleSettingsEvent>(this.OnToggle);
        }
    }

    private void ResetHUD()
    {
        this.colorToggle.isOn = Settings.EFFECT_SCREEN_COLORS;

        this.tweeningEnableToggle.isOn = Settings.IS_TWEENING_ENABLE;
        this.moveYAtStartToggle.isOn = Settings.TWEENING_Y_AT_START;
        this.rotationAtStartToggle.isOn = Settings.TWEENING_ROTATION_AT_START;
        this.scaleAtStartToggle.isOn = Settings.TWEENING_SCALE_AT_START;
        this.delayOnBlocksDelaySldier.value = Settings.TWEENING_DELAY_VALUE * 10;
        this.delayOnBlocksValue.text = Settings.TWEENING_DELAY_VALUE.ToString(CultureInfo.InvariantCulture);
        this.tweenDurationSlider.value = Settings.TWEENING_ENTER_TIME * 10;
        this.tweenDurationValue.text = Settings.TWEENING_ENTER_TIME.ToString(CultureInfo.InvariantCulture);
        this.tweenEaseSlider.value = Settings.EaseToIndex(Settings.TWEENING_EASE);
        this.tweenEaseValue.text = this.tweenEaseSlider.value.ToString(CultureInfo.InvariantCulture);
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

    public void ToogleScaleStart(bool value)
    {
        Settings.TWEENING_SCALE_AT_START = value;
    }

    public void SliderDelayBlocks (float value)
    {
        value /= 10;
        Settings.TWEENING_DELAY_VALUE = value;
        this.delayOnBlocksValue.text = Settings.TWEENING_DELAY_VALUE.ToString(CultureInfo.InvariantCulture);
    }

    public void SliderTweenDuration(float value)
    {
        value /= 10;
        Settings.TWEENING_ENTER_TIME = value;
        this.tweenDurationValue.text = Settings.TWEENING_ENTER_TIME.ToString(CultureInfo.InvariantCulture);
    }

    public void SliderTweenEase(float value)
    {
        Settings.TWEENING_EASE = Settings.GetEaseByIndex((int) value);
        int newEaseNumber = Settings.EaseToIndex(Settings.TWEENING_EASE);
        this.tweenEaseValue.text = newEaseNumber.ToString(CultureInfo.InvariantCulture);

        if(newEaseNumber != value)
            this.tweenEaseSlider.value = newEaseNumber;
    }
    #endregion ButtonActions

}
