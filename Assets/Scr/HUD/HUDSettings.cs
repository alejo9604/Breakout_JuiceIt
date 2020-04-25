using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class HUDSettings : MonoBehaviour
{

    [SerializeField] private bool activeOnStart = false;
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

    [Header("Strech&Squeeze")]
    [SerializeField] private Toggle paddleStrechToggle;
    [SerializeField] private Toggle ballExtraScaleOnHit;
    [SerializeField] private Toggle ballRotationAndStrech;
    [SerializeField] private Toggle ballStrechOnHit;
    [SerializeField] private Toggle ballGlowOnHit;
    [SerializeField] private Slider ballGravitySlider;
    [SerializeField] private TextMeshProUGUI ballGravityValue;
    [SerializeField] private Toggle blocksJellyOnHit;
    [SerializeField] private Toggle bouncyLinesOnHit;

    [Header("Sound")]
    [SerializeField] private Toggle wallSoundToggle;
    [SerializeField] private Toggle brickSoundToggle;
    [SerializeField] private Toggle paddleSoundToggle;
    [SerializeField] private Toggle musicToggle;

    [Header("Particles")]
    [SerializeField] private Toggle ballCollisionParticleToggle;

    private void Start()
    {
        if(!activeOnStart)
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
        this.SetToggle(this.colorToggle, Settings.EFFECT_SCREEN_COLORS);

        this.SetToggle(this.tweeningEnableToggle, Settings.IS_TWEENING_ENABLE);
        this.SetToggle(this.moveYAtStartToggle, Settings.TWEENING_Y_AT_START);
        this.SetToggle(this.rotationAtStartToggle, Settings.TWEENING_ROTATION_AT_START);
        this.SetToggle(this.scaleAtStartToggle, Settings.TWEENING_SCALE_AT_START);

        this.SetSlider(this.delayOnBlocksDelaySldier, this.delayOnBlocksValue, 
            Settings.TWEENING_DELAY_VALUE * 10, Settings.TWEENING_DELAY_VALUE.ToString(CultureInfo.InvariantCulture));
        this.SetSlider(this.tweenDurationSlider, this.tweenDurationValue,
            Settings.TWEENING_ENTER_TIME * 10, Settings.TWEENING_ENTER_TIME.ToString(CultureInfo.InvariantCulture));
        this.SetSlider(this.tweenEaseSlider, this.tweenEaseValue,
            Settings.EaseToIndex(Settings.TWEENING_EASE), this.tweenEaseSlider.value.ToString(CultureInfo.InvariantCulture));

        this.SetToggle(this.paddleStrechToggle, Settings.PADDLE_STRECH);
        this.SetToggle(this.ballExtraScaleOnHit, Settings.BALL_EXTRA_SCALE_ON_HIT);
        this.SetToggle(this.ballRotationAndStrech, Settings.BALL_ROTATION_AND_STRECH);
        this.SetToggle(this.ballStrechOnHit, Settings.BALL_STRECH_ON_HIT);
        this.SetToggle(this.ballGlowOnHit, Settings.BALL_GLOW_ON_HIT);
        this.SetSlider(this.ballGravitySlider, this.ballGravityValue, Settings.BALL_GRAVITY, Settings.BALL_GRAVITY.ToString());
        this.SetToggle(this.blocksJellyOnHit, Settings.BLOCK_JELLY);
        this.SetToggle(this.bouncyLinesOnHit, Settings.BOUNCY_LINES_ENABLED);

        this.SetToggle(this.wallSoundToggle, Settings.SOUND_WALL);
        this.SetToggle(this.brickSoundToggle, Settings.SOUND_BRICK);
        this.SetToggle(this.paddleSoundToggle, Settings.SOUND_PADDLE);
        this.SetToggle(this.musicToggle, Settings.SOUND_MUSIC);
        
        this.SetToggle(this.ballCollisionParticleToggle, Settings.BALL_COLLISION_PARTICLE);
    }

    private void SetToggle(Toggle toggleElement, bool value)
    {
        if (toggleElement != null)
            toggleElement.isOn = value;
    }

    private void SetSlider(Slider sliderElement, TextMeshProUGUI sliderText, float value, string text)
    {
        if (sliderElement != null)
            sliderElement.value = value;
        if (sliderText != null)
            sliderText.text = text;
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


    #region Tweening
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
    #endregion Tweening

    #region Strech
    public void TogglePaddleStrech(bool value)
    {
        Settings.PADDLE_STRECH = value;
    }

    public void ToggleBallExtraScaleOnHit(bool value)
    {
        Settings.BALL_EXTRA_SCALE_ON_HIT = value;
    }

    public void ToggleBallRotationAndStrech(bool value)
    {
        Settings.BALL_ROTATION_AND_STRECH = value;
    }

    public void ToggleBallStretchOnHit(bool value)
    {
        Settings.BALL_STRECH_ON_HIT = value;
    }

    public void ToggleBallGlowOnHit(bool value)
    {
        Settings.BALL_GLOW_ON_HIT = value;
    }

    public void SliderBallGravity(float value)
    {
        Settings.BALL_GRAVITY = value;
        this.ballGravityValue.text = Settings.BALL_GRAVITY.ToString();
    }

    public void ToggleBlocksJellyOnHit(bool value)
    {
        Settings.BLOCK_JELLY = value;
    }

    public void ToggleBouncyLinesOnHit(bool value)
    {
        Settings.BOUNCY_LINES_ENABLED = value;
        EventManager.Instance.Trigger(new InputToggleBouncyLinesEvent
        {
            enabled = Settings.BOUNCY_LINES_ENABLED
        });
    }
    #endregion Strech

    #region Sound
    public void ToggleWallSoundOnHit(bool value)
    {
        Settings.SOUND_WALL = value;
    }

    public void ToggleBrickSoundOnHit(bool value)
    {
        Settings.SOUND_BRICK = value;
    }

    public void TogglePaddleSoundOnHit(bool value)
    {
        Settings.SOUND_PADDLE = value;
    }

    public void ToggleMusic(bool value)
    {
        Settings.SOUND_MUSIC = value;
        AudioManager.Instance.ToggleMusic(Settings.SOUND_MUSIC);
    }
    #endregion Sound

    #region Particles
    public void ToggleBallCollisionParticle(bool value)
    {
        Settings.BALL_COLLISION_PARTICLE = value;
    }
    #endregion Sound

    #endregion ButtonActions

}
