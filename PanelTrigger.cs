using UnityEngine;

public class PanelTrigger : MonoBehaviour
{
    [Header("Select ONE sound for this panel")]
    public bool playSuccessOnEnable;       // Audio 1
    public bool playFailOnEnable;          // Audio 2
    public bool playLevelCompleteOnEnable; // Audio 3

    void OnEnable()
    {
        // The exact millisecond this panel is turned ON, play the matching sound
        if (playSuccessOnEnable)
        {
            PanelSoundPlayer.PlaySuccessSound();
        }
        else if (playFailOnEnable)
        {
            PanelSoundPlayer.PlayFailSound();
        }
        else if (playLevelCompleteOnEnable)
        {
            PanelSoundPlayer.PlayLevelCompleteSound();
        }
    }

    // Manual triggers (just in case you still want to link buttons directly)
    public void TriggerSuccessSound()       => PanelSoundPlayer.PlaySuccessSound();
    public void TriggerFailSound()          => PanelSoundPlayer.PlayFailSound();
    public void TriggerLevelCompleteSound() => PanelSoundPlayer.PlayLevelCompleteSound();
}