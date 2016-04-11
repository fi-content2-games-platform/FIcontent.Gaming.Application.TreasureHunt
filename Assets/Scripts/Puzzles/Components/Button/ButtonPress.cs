/* Copyright (c) 2015  Tizian Zeltner, ETH Zurich
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

public class ButtonPress : MonoBehaviour
{
	public bool isPressed;

    private bool isMoving;
    private Vector3 unpressedPos;
    private Vector3 pressedPos;

    private bool activated;
    
    void Start()
    {
        activated = true;
		isMoving = false;
        unpressedPos = new Vector3(0.0f, 0.0f, 0.0f);
        pressedPos = new Vector3(0.0f, 0.0f, -0.04f);
    }

    void Update()
    {
        if (transform.IsTouched())
        {
			if (!isMoving)
            {
                StartCoroutine(MoveButton());
            }
        }
    }

    IEnumerator MoveButton()
    {
        if (activated)
        {
			isMoving = true;
            yield return StartCoroutine(MoveObject(transform, unpressedPos, pressedPos, 0.25f));

			yield return StartCoroutine(WaitInPressedState());

            yield return StartCoroutine(MoveObject(transform, pressedPos, unpressedPos, 0.25f));
			isMoving = false;
        }
    }

    IEnumerator MoveObject(Transform transform, Vector3 startPos, Vector3 endPos, float time)
    {
        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localPosition = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }

	IEnumerator WaitInPressedState()
	{
		isPressed = true;
		while (isPressed)
		{
			yield return null;
		}
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