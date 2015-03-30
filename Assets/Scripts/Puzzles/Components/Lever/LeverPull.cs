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
using System.Collections;

public class LeverPull : MonoBehaviour
{
	private bool isDown;
	private bool moving;
	private Quaternion upRotation;
	private Quaternion downRotation;
	
	private bool activated;
	
	public bool IsMoving()
	{
		return moving;
	}
	
	void Start()
	{
		activated = true;
		isDown = false;
		moving = false;
		upRotation = Quaternion.Euler(38, 0, 0);
		downRotation = Quaternion.Euler(-38, 0, 0);
	}
	
	void Update()
	{
		if (transform.IsTouched())
		{
			if (isDown)
			{
				StartCoroutine(MoveUp());
			}
			else
			{
				StartCoroutine(MoveDown());
			}
		}
	}

	IEnumerator MoveUp()
	{
		if (activated)
		{
			moving = true;
			yield return StartCoroutine(RotateObject(transform, downRotation, upRotation, 0.25f));
			moving = false;
			isDown = false;
		}
	}

	IEnumerator MoveDown()
	{
		if (activated)
		{
			moving = true;
			yield return StartCoroutine(RotateObject(transform, upRotation, downRotation, 0.25f));
			moving = false;
			isDown = true;
		}
	}

	IEnumerator RotateObject(Transform transform, Quaternion startRotation, Quaternion endRotation, float time)
	{
		float i = 0.0f;
		float rate = 1.0f / time;
		while (i < 1.0f)
		{
			i += Time.deltaTime * rate;
			transform.localRotation = Quaternion.Slerp(startRotation, endRotation, i);
			yield return null;
		}
	}

	public void ForceUpPosition()
	{
		transform.localRotation = upRotation;
		isDown = false;
		moving = false;
	}

	public void ForceDownPosition()
	{
		transform.localRotation = downRotation;
		isDown = true;
		moving = false;
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
