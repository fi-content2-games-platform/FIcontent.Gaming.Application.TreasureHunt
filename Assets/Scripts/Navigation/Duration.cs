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
using System;
using System.Collections;
using System.Xml.Serialization;

[Serializable]
public class Duration
{
	[XmlElement]
	public float seconds;
	
	public Duration()
	{
		this.seconds = 0.0f;
	}
	
	public Duration(float seconds)
	{
		this.seconds = seconds;
	}
	
	public Duration(int minutes, float seconds)
	{
		this.seconds = 60 * minutes + seconds;
	}
	
	public Duration(int hours, int minutes, float seconds)
	{
		this.seconds = 3600 * hours + 60 * minutes + seconds;
	}
	
	public static Duration Now()
	{
		return new Duration(Time.time);
	}
	
	public static Duration FromSeconds(float seconds)
	{
		return new Duration(seconds);
	}
	
	public static Duration FromMinutes(float minutes)
	{
		return new Duration(60 * minutes);
	}
	
	public static Duration FromHours(float hours)
	{
		return new Duration(3600 * hours);
	}
	
	public int SecondsInt()
	{
		return Mathf.RoundToInt(seconds);
	}
	
	public int MinutesInt()
	{
		return Mathf.FloorToInt(seconds / 60f);
	}
	
	public int HoursInts()
	{
		return Mathf.FloorToInt(seconds / 3600f);
	}
	
	public float Seconds()
	{
		return seconds;
	}
	
	public float Minutes()
	{
		return seconds / 60f;
	}
	
	public float Hours()
	{
		return seconds / 3600f;
	}
	
	public static Duration operator +(Duration d1, Duration d2)
	{
		return new Duration(d1.seconds + d2.seconds);
	}
	
	public static Duration operator -(Duration d1, Duration d2)
	{
		return new Duration(d1.seconds - d2.seconds);
	}
	
	public static bool operator ==(Duration d1, Duration d2)
	{
		if (object.ReferenceEquals(d1, d2))
		{
			return true;
		}
		
		if ((object)d1 == null || (object)d2 == null)
		{
			return false;
		}
		
		return d1.seconds == d2.seconds;
	}
	
	public static bool operator !=(Duration d1, Duration d2)
	{
		return !(d1 == d2);
	}
	
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		
		Duration d = obj as Duration;
		if ((object)d == null)
		{
			return false;
		}
		
		return seconds == d.seconds;
	}
	
	public override string ToString()
	{
		float t = seconds;
		
		int h = (int)(t / 3600f);
		t = t - 3600f * h;
		
		int min = (int)(t / 60f);
		t = t - 60f * min;
		
		int s = (int)t;
		
		if (h == 0)
		{
			return string.Format("{0}:{1}", min.ToString(), s.ToString("00"));
		}
		else
		{
			return string.Format("{0}:{1}:{2}", h.ToString(), min.ToString("00"), s.ToString("00"));
		}
	}
	
	public override int GetHashCode()
	{
		return seconds.GetHashCode();
	}
}
