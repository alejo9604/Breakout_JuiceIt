using UnityEngine;
using UnityEngine.UI;

public class HUDSettings : MonoBehaviour
{

    //Control var.
    private bool isHide = true;

    private void Start()
    {
        this.gameObject.SetActive(!this.isHide);
    }


    public void ToggletHide()
    {
        this.isHide = !this.isHide;
        this.gameObject.SetActive(!this.isHide);
    }


    public void ToogleScreenColor(bool value)
    {
        Settings.EFFECT_SCREEN_COLORS = value;
    }

}
