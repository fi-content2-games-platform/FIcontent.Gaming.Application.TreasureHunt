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

/// <summary>
/// Vector3 extensions.
/// </summary>
public static class Vector3Extensions
{
	/// <summary>
	/// Projects a vector on a plane with the specified normal vector.
	/// </summary>
	/// <returns>Vector projected onto plane.</returns>
	/// <param name="v">Vector to project.</param>
	/// <param name="planeNormal">Normal vector of the plane</param>
    public static Vector3 ProjectOnPlane(this Vector3 v, Vector3 planeNormal)
    {
        return v - (Vector3.Dot(v, planeNormal) * planeNormal);
    }
}