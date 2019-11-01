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
    private Vector3 initPosition;


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

        this.initPosition = this.transform.position;
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
        this.transform.position = Vector3.Lerp(this.transform.position, this.targetPosisition, Time.deltaTime * speed);
    }


    public override void ResetElement()
    {
        base.ResetElement();

        this.PlayEnterTween();
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


    //Tweeing
    //Tweening
    public void PlayEnterTween()
    {
        if (Settings.TWEENING_Y_AT_START)
            this.PlayEnterAxisYTween();
        else
            this.transform.position = this.initPosition;

        if (Settings.TWEENING_ROTATION_AT_START)
            this.PlayEnterRotationTween();
        else
            this.transform.eulerAngles = Vector3.zero;

    }


    //Move this to a Sequence
    private void PlayEnterAxisYTween()
    {
        Vector3 offset = Vector3.up * (Breakout.Instance.GetCameraWorldSize().y + Breakout.Instance.GetCameraPosition().y / 2);
        this.transform.DOMoveY(this.initPosition.y, Settings.TWEENING_ENTER_TIME).From(offset.y + this.initPosition.y, true);
    }

    private void PlayEnterRotationTween()
    {
        float rotation = Random.Range(-Settings.TWEENING_ROTATION_MAX_ANGLE, Settings.TWEENING_ROTATION_MAX_ANGLE);
        if (Mathf.Abs(rotation) < Settings.TWEENING_ROTATION_MIN_ANGLE)
            rotation = Mathf.Sign(rotation) * Settings.TWEENING_ROTATION_MIN_ANGLE;

        this.transform.DORotate(Vector3.zero, Settings.TWEENING_ENTER_TIME).From(Vector3.back * rotation);
    }


    private void OnDrawGizmosSelected() {
        Vector3 center = this.transform.position;
        center.x = 0;
        Gizmos.DrawWireCube(center, new Vector3(clampXPos*2, this.transform.localScale.y));
    }
}
