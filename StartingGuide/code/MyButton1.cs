using UnityEngine;
using System.Collections;

public class MyButton : Button
{
    public MyButtonPuzzleLogic logic;

    protected override void Action()
    {
        logic.PressButton();
        UnpressButton();
    }
}