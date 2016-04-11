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
using System;
using System.Collections;
using System.Xml.Serialization;

[Serializable]
public class Distance
{
	[XmlElement]
	public float meters;
	
	public Distance()
	{
		this.meters = 0.0f;
	}
	
	public Distance(float meters)
	{
		this.meters = meters;
	}
	
	public static Distance FromMeters(float meters)
	{
		return new Distance(meters);
	}
	
	public static Distance FromKilometers(float kilometers)
	{
		return new Distance(1000 * kilometers);
	}
	
	public bool IsValid()
	{
		return !float.IsNaN(meters);
	}
	
	public int MetersInt()
	{
		return Mathf.RoundToInt(meters);
	}
	
	public int KilometersInt()
	{
		return Mathf.FloorToInt(meters / 1000f);
	}
	
	public float Meters()
	{
		return meters;
	}
	
	public float Kilometers()
	{
		return meters / 1000f;
	}
	
	public static Distance operator +(Distance d1, Distance d2)
	{
		return new Distance(d1.meters + d2.meters);
	}
	
	public static Distance operator -(Distance d1, Distance d2)
	{
		return new Distance(d1.meters - d2.meters);
	}
	
	public static bool operator ==(Distance d1, Distance d2)
	{
		if (object.ReferenceEquals(d1, d2))
		{
			return true;
		}
		
		if ((object)d1 == null || (object)d2 == null)
		{
			return false;
		}
		
		return d1.meters == d2.meters;
	}
	
	public static bool operator !=(Distance d1, Distance d2)
	{
		return !(d1 == d2);
	}
	/// <summary>
	/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Distance"/>.
	/// </summary>
	/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Distance"/>.</param>
	/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current <see cref="Distance"/>; otherwise, <c>false</c>.</returns>
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		
		Distance d = obj as Distance;
		if ((object)d == null)
		{
			return false;
		}
		
		return meters == d.meters;
	}
	
	public override string ToString()
	{
		if (meters > 1000)
		{
			return string.Format("{0} km", Math.Round(meters / 1000, 2));
		}
		else
		{
			return string.Format("{0} m", Math.Round(meters));
		}
	}
	
	public override int GetHashCode()
	{
		return meters.GetHashCode();
	}
}