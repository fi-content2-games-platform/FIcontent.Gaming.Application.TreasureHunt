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

public class CombinationLockDial : MonoBehaviour {

    private int digit;
    private bool turning;

	private float angle;
    private bool activated;

    void Start()
    {
        digit = 0;
		angle = 54;
        activated = true;
        turning = false;
    }

    void Update()
    {
        if (transform.IsTouched())
        {
            if (!turning)
            {
                StartCoroutine(RotateDial());
            }
        }
    }
    
    IEnumerator RotateDial()
    {
        if (activated)
        {
			digit = (digit + 1) % 10;
			SoundManager.Instance.PlaySound(SoundManager.combinationLockClickSound);
            turning = true;
            yield return StartCoroutine(RotateOnePosition(0.25f));
            turning = false;
        }
    }
    
    IEnumerator RotateOnePosition(float time)
    {
        float i = 0.0f;
        float rate = 1.0f / time;
		Quaternion startRotation = Quaternion.Euler(angle, 0, 0);
		angle = angle + 18;
		Quaternion endRotation = Quaternion.Euler(angle, 0, 0);
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, i);
            yield return null;
        }
    }

	public void SetDigit(int i)
	{
		if (i < 0 || i > 9)
		{
			Debug.Log("Combination Lock: Digit set to out of bounds value.");
		}

		digit = i;

		switch(i)
		{
			case 0:
				angle = 54;
				break;
				
			case 1:
				angle = 72;
				break;
				
			case 2:
				angle = 90;
				break;
				
			case 3:
				angle = 108;
				break;
				
			case 4:
				angle = 126;
				break;
				
			case 5:
				angle = 144;
				break;
				
			case 6:
				angle = 162;
				break;
				
			case 7:
				angle = 180;
				break;
				
			case 8:
				angle = 198;
				break;
				
			case 9:
				angle = 216;
				break;
				
			default:
				break;
		}

		transform.localRotation = Quaternion.Euler(angle, 0, 0);
	}

	public int GetDigit()
	{
		return digit;
	}
	
	public void Activate()
	{
		activated = true;
	}
	
	public void Deactivate()
	{
		activated = false;
	}
}
