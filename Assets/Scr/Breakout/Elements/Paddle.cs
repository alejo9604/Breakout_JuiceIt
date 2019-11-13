using Random = UnityEngine.Random;
using UnityEngine;
using DG.Tweening;

public class Paddle : BreakoutElement
{
    [Header("Movement")]
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float clampXPos = 5f;

    [Header("Color")]
    [SerializeField]
    private Color color;


    //Control var
    private Camera mainCamera;
    private Vector3 targetPosisition;

    private Camera MainCamera()
    {
        if(this.mainCamera == null)
            this.mainCamera = Breakout.Instance.GetCamera();

        if (this.mainCamera == null)
            this.mainCamera = Camera.main;

        return this.mainCamera;
    }

    protected override void Start() {

        base.Start();

        this.targetPosisition = this.transform.position;

        //Add events
        EventManager.Instance.AddListener<InputResetLevelEvent>(this.OnInputResetLevel);
        EventManager.Instance.AddListener<ChangeColorEvent>(this.OnChangeColor);
    }

    private void OnDestroy()
    {
        if (EventManager.HasInstance()) {
            EventManager.Instance.RemoveListener<InputResetLevelEvent>(this.OnInputResetLevel);
            EventManager.Instance.RemoveListener<ChangeColorEvent>(this.OnChangeColor);
        }
    }

    void Update()
    {
        //Movement
        this.targetPosisition.x = this.MainCamera().ScreenToWorldPoint( Input.mousePosition).x;
        this.targetPosisition.x = Mathf.Clamp(this.targetPosisition.x, -clampXPos, clampXPos);
        this.targetPosisition.y = this.transform.position.y;
        this.transform.position = Vector3.Lerp(this.transform.position, this.targetPosisition, Time.deltaTime * speed);
    }


    public override void ResetElement()
    {
        base.ResetElement();

        if (!Settings.IS_TWEENING_ENABLE) {
            this.transform.position = this.initPosition;
            this.transform.eulerAngles = Vector3.zero;
        }
        else {
            this.PlayEnterTween();
        }
    }



    #region Events
    private void OnInputResetLevel(InputResetLevelEvent e)
    {
        this.ResetElement();
    }

    private void OnChangeColor(ChangeColorEvent e)
    {
        this.ChangeColor(Settings.EFFECT_SCREEN_COLORS ? this.color : Color.white);
    }
    #endregion Events


    //Tweening
    public void PlayEnterTween()
    {
        Sequence enterSequence = DOTween.Sequence();


        if (Settings.TWEENING_DELAY_VALUE > 0) {
            enterSequence.AppendCallback(() => this.gameObject.SetActive(false));
            enterSequence.AppendInterval(Random.Range(0, Settings.TWEENING_DELAY_VALUE));
            enterSequence.AppendCallback(() => this.gameObject.SetActive(true));
            enterSequence.AppendInterval(0.1f);
        }

        if (Settings.TWEENING_Y_AT_START)
            enterSequence.Join(this.PlayEnterAxisYTween());
        else
            this.transform.position = this.initPosition;

        if (Settings.TWEENING_ROTATION_AT_START)
            enterSequence.Join(this.PlayEnterRotationTween());
        else
            this.transform.eulerAngles = Vector3.zero;

        if (Settings.TWEENING_SCALE_AT_START)
            enterSequence.Join(this.PlayEnterScaleTween());
        else
            this.transform.localScale = this.initScale;

    }


    private Tween PlayEnterAxisYTween()
    {
        Vector3 offset = Vector3.up * (Breakout.Instance.GetCameraWorldSize().y + Breakout.Instance.GetCameraPosition().y / 2);
        return this.transform.DOMoveY(this.initPosition.y, Settings.TWEENING_ENTER_TIME).From(offset.y + this.initPosition.y, true).SetEase(Settings.TWEENING_EASE);
    }

    private Tween PlayEnterRotationTween()
    {
        float rotation = Random.Range(-Settings.TWEENING_ROTATION_MAX_ANGLE, Settings.TWEENING_ROTATION_MAX_ANGLE);
        if (Mathf.Abs(rotation) < Settings.TWEENING_ROTATION_MIN_ANGLE)
            rotation = Mathf.Sign(rotation) * Settings.TWEENING_ROTATION_MIN_ANGLE;

        return this.transform.DORotate(Vector3.zero, Settings.TWEENING_ENTER_TIME).From(Vector3.back * rotation).SetEase(Settings.TWEENING_EASE);
    }

    private Tween PlayEnterScaleTween()
    {
        return this.transform.DOScale(this.initScale, Settings.TWEENING_ENTER_TIME).From(this.initScale * Settings.TWEENING_SCALE_INIT_FACTOR).SetEase(Settings.TWEENING_EASE);
    }


    private void OnDrawGizmosSelected() {
        Vector3 center = this.transform.position;
        center.x = 0;
        Gizmos.DrawWireCube(center, new Vector3(clampXPos*2, this.transform.localScale.y));
    }
}
