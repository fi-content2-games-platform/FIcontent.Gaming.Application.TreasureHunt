/* Copyright (c) 2015 ETH Zurich, Tizian Zeltner
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
 
using UnityEngine;
using System.Collections;

public class SoundButtonAction : ButtonAction
{
    public int i;

    private SoundButtonsPuzzleLogic logic;

    void Start()
    {
        logic = GetComponentInParent<SoundButtonsPuzzleLogic>();;
    }
    
    override protected void Action()
    {
        PlaySoundButtonPuzzleSound(i);
        logic.Press(i);
    }

    override protected void OnUpdate()
    {
        if (logic.IsSolved())
        {
            buttonScript.Deactivate();
        }
        else
        {
            buttonScript.Activate();
        }
    }

    private void PlaySoundButtonPuzzleSound(int i)
    {
        switch(i)
        {
            case 1:
				SoundManager.Instance.PlaySoundWithCallback(SoundManager.soundButtonsPuzzleSound3, UnpressButton);
                break;
            case 2:
				SoundManager.Instance.PlaySoundWithCallback(SoundManager.soundButtonsPuzzleSound1, UnpressButton);
                break;
            case 3:
				SoundManager.Instance.PlaySoundWithCallback(SoundManager.soundButtonsPuzzleSound2, UnpressButton);
                break;
            default:
                break;
        }
    }
}