using UnityEngine;
using DG.Tweening;

public class Brick : BreakoutElement
{

    private int id;
    private Vector3 initPosition;

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

        this.initPosition = this.transform.position;
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


    private void PlayEnterAxisYTween()
    {
        Vector3 offset = Vector3.up * (Breakout.Instance.GetCameraWorldSize().y + Breakout.Instance.GetCameraPosition().y / 2);
        this.transform.DOMove(this.initPosition, Settings.TWEENING_ENTER_TIME).From(offset + this.initPosition, true);
    }

    private void PlayEnterRotationTween()
    {
        float rotation = Random.Range(-Settings.TWEENING_ROTATION_MAX_ANGLE, Settings.TWEENING_ROTATION_MAX_ANGLE);
        if (Mathf.Abs(rotation) < Settings.TWEENING_ROTATION_MIN_ANGLE)
            rotation = Mathf.Sign(rotation) * Settings.TWEENING_ROTATION_MIN_ANGLE;

        this.transform.DORotate(Vector3.zero, Settings.TWEENING_ENTER_TIME).From(Vector3.back * rotation);
    }
}
