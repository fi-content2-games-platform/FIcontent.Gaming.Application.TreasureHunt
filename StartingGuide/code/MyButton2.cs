using UnityEngine;
using System.Collections;

public class MyButton : Button
{
    public MyButtonPuzzleLogic logic;
    public AudioClip buttonSound;

    protected override void Action()
    {
        logic.PressButton();
        UnpressButton();

        Camera.main.audio.PlayOneShot(buttonSound);
    }
}