using UnityEngine;
using System.Collections;

public class MyButtonPuzzleLogic : PuzzleLogic
{
    // Puzzle data
    private bool pressed;

    // Trigger to change data
    public void PressButton()
    {
        pressed = true;
    }

    // Check for completion
    protected override bool CheckIfSolved()
    {
        return pressed;
    }

    // Reset data
    protected override void Reset()
    {
        pressed = false;
    }
}