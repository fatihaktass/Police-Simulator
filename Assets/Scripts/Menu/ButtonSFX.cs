using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    public AudioSource[] buttonSfxs;

    public void ButtonSoundEffect(int casePoint)
    {
        switch (casePoint)
        {
            case 0:
                buttonSfxs[0].Play();
              break;
            case 1:
                buttonSfxs[1].Play();
              break;
            case 2:
                buttonSfxs[2].Play();
              break;

            default:
                buttonSfxs[0].Play();
                break;
        }
    }
}
