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

public class TileMovement : MonoBehaviour
{
	public int tileNumber;

    private bool isMoving;
	private bool activated;

	private SlidingPuzzleLogic logic;
	
	void Start()
	{
		logic = GetComponentInParent<SlidingPuzzleLogic>();
		activated = true;
		isMoving = false;
	}
    
    void Update()
    {
		if (activated && !isMoving)
		{
			if (transform.IsTouched())
			{
				int moveNumber = logic.MoveTile(tileNumber);

				if (moveNumber != 0)
				{
					SoundManager.Instance.PlaySound(SoundManager.slideSound);
				}

				if (moveNumber == 1)
				{
					StartCoroutine(MoveRight());
				}
				else if (moveNumber == -1)
				{
					StartCoroutine(MoveLeft());
				}
				else if (moveNumber == 2)
				{
					StartCoroutine(MoveDown());
				}
				else if (moveNumber == -2)
				{
					StartCoroutine(MoveUp());
				}
			}
        }
    }

	IEnumerator MoveRight()
	{
		isMoving = true;
		Vector3 start = transform.localPosition;
		Vector3 end = new Vector3(start.x - logic.delta, start.y, start.z);
		yield return StartCoroutine(MoveObject(transform, start, end, 0.25f));
		isMoving = false;
	}

	IEnumerator MoveLeft()
	{
		isMoving = true;
		Vector3 start = transform.localPosition;
		Vector3 end = new Vector3(start.x + logic.delta, start.y, start.z);
		yield return StartCoroutine(MoveObject(transform, start, end, 0.25f));
		isMoving = false;
	}

	IEnumerator MoveDown()
	{
		isMoving = true;
		Vector3 start = transform.localPosition;
		Vector3 end = new Vector3(start.x, start.y - logic.delta, start.z);
		yield return StartCoroutine(MoveObject(transform, start, end, 0.25f));
		isMoving = false;
	}

	IEnumerator MoveUp()
	{
		isMoving = true;
		Vector3 start = transform.localPosition;
		Vector3 end = new Vector3(start.x, start.y + logic.delta, start.z);
		yield return StartCoroutine(MoveObject(transform, start, end, 0.25f));
		isMoving = false;
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

	public void Activate()
	{
		activated = true;
	}
	
	public void Deactivate()
	{
		activated = false;
	}
}