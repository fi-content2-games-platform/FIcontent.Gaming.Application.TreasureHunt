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

/// <summary>
/// Transform extensions.
/// </summary>
public static class TransformExtensions
{
    private static float maxPickingDistance = 2000;
	
	/// <summary>
	/// Returns wheter the game object was touched. Uses the VuforiaCamera Tag.
	/// </summary>
	/// <returns><c>true</c> if is t is touched; otherwise, <c>false</c>.</returns>
	/// <param name="t">Transform.</param>
    public static bool IsTouched(this Transform t)
    {
        if (GameStateManager.GetCurrentState() is CameraState)
        {
            // Use a raycast to do touch input.
            foreach (Touch touch in Input.touches)
            {
                GameObject vuforiaCam = GameObject.FindWithTag("VuforiaCamera");
                Ray ray = vuforiaCam.camera.ScreenPointToRay(touch.position);
                if (touch.phase == TouchPhase.Began)
                {
                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(ray, out hit, maxPickingDistance))
                    {
                        if (hit.transform.Equals(t))
                        {
                            return true;
                        }
                    }
                }
            }

			if (Input.GetMouseButtonDown(0))
			{
				GameObject vuforiaCam = GameObject.FindWithTag("VuforiaCamera");
				Ray ray = vuforiaCam.camera.ScreenPointToRay(Input.mousePosition);

				RaycastHit hit = new RaycastHit();
				if (Physics.Raycast(ray, out hit, maxPickingDistance))
				{
					if (hit.transform.Equals(t))
					{
						return true;
					}
				}
				
			}
        }
        return false;
    }
}