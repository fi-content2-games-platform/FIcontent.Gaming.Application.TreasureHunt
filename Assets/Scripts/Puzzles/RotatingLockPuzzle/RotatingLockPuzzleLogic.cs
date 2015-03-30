/* Copyright (c) 2015 ETH Zurich
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
using System.Collections.Generic;

public class RotatingLockPuzzleLogic : PuzzleLogic {

    public List<float> solution = new List<float> {180.0f, 90.0f, 0.0f};

    public List<GameObject> cylinders;

    private bool solvedOuter;
    private bool solvedInner;
    private bool solvedMiddle;

    override protected void Reset()
    {
        solvedOuter = false;
        solvedInner = false;
        solvedMiddle = false;
  
        foreach (GameObject cylinder in cylinders)
        {
            cylinder.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    override protected bool CheckIfSolved()
    {
		if (GameStateManager.GetCurrentState() is CameraState && puzzle.isCurrentlyInView)
        {
            Vector3 gravity = Sensors.Gravity();
            Vector3 projectedGravity = gravity.ProjectOnPlane(Vector3.forward);
            projectedGravity.Normalize();
            
            float angle = Vector3.Angle(-Vector3.up, projectedGravity);
            float sign = Mathf.Sign(Vector3.Dot(projectedGravity, Vector3.right));
            angle = sign * angle;
            angle = angle - 360.0f * Mathf.Floor(angle / 360.0f);
            
            if (!solvedOuter)
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject cylinder = cylinders[i];
					cylinder.transform.localEulerAngles = new Vector3(0, 0, angle);
                }
                
                float goalAngle = solution[0];
                if (Mathf.Abs(goalAngle - angle) < 4.0f)
                {
                    solvedOuter = true;
                    GameObject outer = cylinders[0];
					outer.transform.localEulerAngles = new Vector3(0, 0, goalAngle);
					SoundManager.Instance.PlaySound(SoundManager.safeDialClickSound);
                }
            }
            else if (!solvedMiddle)
            {
                for (int i = 1; i < 3; i++)
                {
                    GameObject cylinder = cylinders[i];
                    cylinder.transform.localEulerAngles = new Vector3(0, 0, angle);
                }
                
                float goalAngle = solution[1];
                if (Mathf.Abs(goalAngle - angle) < 4.0f)
                {
                    solvedMiddle = true;
                    GameObject middle = cylinders[1];
                    middle.transform.localEulerAngles = new Vector3(0, 0, goalAngle);
					SoundManager.Instance.PlaySound(SoundManager.safeDialClickSound);
                }
            }
            else if (!solvedInner)
            {
                for (int i = 2; i < 3; i++)
                {
                    GameObject cylinder = cylinders[i];
					cylinder.transform.localEulerAngles = new Vector3(0, 0, angle);
                }
                
                float goalAngle = solution[2];
                if (Mathf.Abs(goalAngle - angle) < 4.0f)
                {
                    solvedInner = true;
                    GameObject inner = cylinders[2];
					inner.transform.localEulerAngles = new Vector3(0, 0, goalAngle);
					SoundManager.Instance.PlaySound(SoundManager.safeDialClickSound);
                }
            }
        }

        return (solvedInner && solvedMiddle && solvedOuter);
    }

	override protected void OnSkip()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject cylinder = cylinders[i];
			cylinder.transform.localEulerAngles = new Vector3(0, 0, solution[i]);
        }
    }
    
    void Awake()
    {
        if (cylinders.Count != 3)
        {
            Debug.LogError("Connect cylinders in the RotatingLock puzzle.");
        }
        if (solution.Count != 3)
        {
			Debug.LogError("RotatingLock puzzle solution expects 3 values.");
        }
    }
}