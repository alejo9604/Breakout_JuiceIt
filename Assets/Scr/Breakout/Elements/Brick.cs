using UnityEngine;
using DG.Tweening;

public class Brick : BreakoutElement
{

    private int id;

    public int GetBrickId()
    {
        return this.id;
    }

    public void SetBrickId(int id)
    {
        this.id = id;
    }

    public override void Init()
    {
        base.Init();
    }

    public override void ResetElement()
    {
        base.ResetElement();

        this.gameObject.SetActive(true);

        if (!Settings.IS_TWEENING_ENABLE) {
            this.transform.position = this.initPosition;
            this.transform.eulerAngles = Vector3.zero; 
        }
        else {
            this.PlayEnterTween();
        }
    }

    public override void OnCollision()
    {
        base.OnCollision();
        this.gameObject.SetActive(false);
    }


    //Tweening
    public void PlayEnterTween()
    {
        Sequence enterSequence = DOTween.Sequence();

        if (Settings.TWEENING_DELAY_VALUE > 0) {
            enterSequence.AppendCallback(() => this.gameObject.SetActive(false));
            enterSequence.AppendInterval(Random.Range(0, Settings.TWEENING_DELAY_VALUE));
            enterSequence.AppendCallback(() => this.gameObject.SetActive(true));
            enterSequence.AppendInterval(0.05f);
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
        return this.transform.DOMove(this.initPosition, Settings.TWEENING_ENTER_TIME).From(offset + this.initPosition, true).SetEase(Settings.TWEENING_EASE);
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
}
