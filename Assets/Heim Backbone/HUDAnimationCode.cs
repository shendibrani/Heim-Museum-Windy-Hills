using UnityEngine;
using System.Collections;

public class HUDAnimationCode : MonoBehaviour
{

    private Texture2D[] hudAnimationArray = new Texture2D[40];
    private Texture2D HUDtop;
    private string temp;
    private int arrayPosition;

    void OnGUI()
    {
        AnimateHudTop("HUD/Hudcrop_");
    }

    public void AnimateHudTop(string imageName)
    {
        AssignHUDTextures(imageName);

        if (arrayPosition < hudAnimationArray.Length)
        {
            arrayPosition++;
        }

        // Make sure the arrayposition does not exceed the array.length
        if (arrayPosition > hudAnimationArray.Length - 2)
        {
            arrayPosition = hudAnimationArray.Length - 1;
        }

        // Assign the texture of the selected arrayposition to the visionAnimation
        HUDtop = hudAnimationArray[arrayPosition];

        GUI.DrawTexture(new Rect(((Screen.width / 2) - (1150 / 2) - 20), -10, 1150, 150), HUDtop);
    }

    private void AssignHUDTextures(string imageName)
    {
        for (int i = 0; i < hudAnimationArray.Length; i++)
        {
            hudAnimationArray[i] = (Texture2D)Resources.Load(imageName + i.ToString("D5"), typeof(Texture2D));
        }
    }
}