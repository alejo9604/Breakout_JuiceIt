using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{

    private const float DELAY_INPUT_TIME = 0.15f;

    //Events
    private InputToggleSettingsEvent inputToggleSettingsEvent = new InputToggleSettingsEvent();

    //Control var.
    private float delayTime = DELAY_INPUT_TIME;
    
    private void ResetDelay() { this.delayTime = DELAY_INPUT_TIME;  }

    void Update()
    {
        if (this.delayTime > 0) {
            this.delayTime -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            this.TriggerInput(this.inputToggleSettingsEvent);
    }

    private void TriggerInput(GameEvent e)
    {
        this.ResetDelay();
        EventManager.Instance.Trigger(e);
    }
}
