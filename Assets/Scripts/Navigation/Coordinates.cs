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
using System;
using System.Xml.Serialization;

/*
 * Coordinates represents geographical coordinates with latitude and longitude.
 * 
 * Calculations taken from:
 * http://www.movable-type.co.uk/scripts/latlong.html
 * 
 */
[Serializable]
public class Coordinates
{
	[XmlElement]
	public float latitude;
	[XmlElement]
	public float longitude;
	
	// Initializes a new instance of the with the default location: 0° 0' 0'' N 0° 0' 0'' E.
	public Coordinates()
	{
		this.latitude = 0.0f;
		this.longitude = 0.0f;
	}
	
	// Initializes a new instance from floating point latitude and longitude.
	public Coordinates(float latitude, float longitude)
	{
		this.latitude = latitude;
		this.longitude = longitude;
	}
	
	// Initializes a new instance from latitude and longitude in degrees, minutes, seconds and direction.
	public Coordinates(int latDeg, int latMin, float latSec, string latDir,
	                   int lonDeg, int lonMin, float lonSec, string lonDir)
	{
		float latSign = 0;
		if (latDir.Equals("N"))
		{
			latSign = 1;
		}
		else if (latDir.Equals("S"))
		{
			latSign = -1;
		}
		float lonSign = 0;
		if (lonDir.Equals("E"))
		{
			lonSign = 1;
		}
		else if (lonDir.Equals("W"))
		{
			lonSign = -1;
		}
		
		this.latitude = latSign * latDeg + (latMin / 60.0f) + (latSec / 3600.0f);
		this.longitude = lonSign * lonDeg + (lonMin / 60.0f) + (lonSec / 3600.0f);
	}
	
	// Calculates the great-circle distance between two points with the haversine formula. Returns meters.
	public Distance DistanceTo(Coordinates other)
	{
		if (float.IsNaN(longitude) || float.IsNaN(latitude) || float.IsNaN(other.longitude) || float.IsNaN(other.latitude))
		{
			return new Distance(float.NaN);
		}
		float R = 6371000;   // earth radius in meters
		float dLat = (other.latitude - this.latitude) * Mathf.Deg2Rad;
		float dLon = (other.longitude - this.longitude) * Mathf.Deg2Rad;
		float lat1 = this.latitude * Mathf.Deg2Rad;
		float lat2 = other.latitude * Mathf.Deg2Rad;
		
		float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) + 
			Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2) * Mathf.Cos(lat1) * Mathf.Cos(lat2);
		float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
		return new Distance(R * c);
	}
	
	// Calculates the bearing of two points: The dirction (relative to true north) in which the destination lies.
	public float BearingTo(Coordinates destination)
	{
		if (float.IsNaN(longitude) || float.IsNaN(latitude) || float.IsNaN(destination.longitude) || float.IsNaN(destination.latitude))
		{
			return float.NaN;
		}
		float lat1 = this.latitude * Mathf.Deg2Rad;
		float lat2 = destination.latitude * Mathf.Deg2Rad;
		float lon1 = this.longitude * Mathf.Deg2Rad;
		float lon2 = destination.longitude * Mathf.Deg2Rad;
		
		float y = Mathf.Sin(lon2 - lon1) * Mathf.Cos(lat2);
		float x = Mathf.Cos(lat1) * Mathf.Sin(lat2) - Mathf.Sin(lat1) * Mathf.Cos(lat2) * Mathf.Cos(lon2 - lon1);
		float brng = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
		return brng - 360.0f * Mathf.Floor(brng / 360.0f);
	}
	
	// Returns formatted string version of the coordinates. e.g. "47° 41' 8.3'' N 8° 39' 29.5'' E"
	public override string ToString()
	{
		string latDir = "";
		if (this.latitude >= 0)
		{
			latDir = "N";
		}
		else
		{
			latDir = "S";
		}
		
		float latDegrees = Mathf.Abs(this.latitude);
		float latMinutes = (latDegrees - Mathf.Floor(latDegrees)) * 60.0f;
		float latSeconds = (latMinutes - Mathf.Floor(latMinutes)) * 60.0f;
		float latTenths = (latSeconds - Mathf.Floor(latSeconds)) * 10.0f;
		
		latDegrees = Mathf.Floor(latDegrees);
		latMinutes = Mathf.Floor(latMinutes);
		latSeconds = Mathf.Floor(latSeconds);
		latTenths = Mathf.Floor(latTenths);
		
		string lonDir = "";
		if (this.longitude >= 0)
		{
			lonDir = "E";
		}
		else
		{
			lonDir = "W";
		}
		
		float lonDegrees = Mathf.Abs(longitude);
		float lonMinutes = (lonDegrees - Mathf.Floor(lonDegrees)) * 60.0f;
		float lonSeconds = (lonMinutes - Mathf.Floor(lonMinutes)) * 60.0f;
		float lonTenths = (lonSeconds - Mathf.Floor(lonSeconds)) * 10.0f;
		
		lonDegrees = Mathf.Floor(lonDegrees);
		lonMinutes = Mathf.Floor(lonMinutes);
		lonSeconds = Mathf.Floor(lonSeconds);
		lonTenths = Mathf.Floor(lonTenths);
		
		return string.Format("{0}° {1}' {2}.{3}'' {4} {5}° {6}' {7}.{8}'' {9}",
		                     latDegrees, latMinutes, latSeconds, latTenths, latDir,
		                     lonDegrees, lonMinutes, lonSeconds, lonTenths, lonDir);
	}
	
	public static bool operator ==(Coordinates c1, Coordinates c2)
	{
		if (object.ReferenceEquals(c1, c2))
		{
			return true;
		}
		
		if ((object)c1 == null || (object)c2 == null)
		{
			return false;
		}
		
		return c1.longitude == c2.longitude && c1.latitude == c2.latitude;
	}
	
	public static bool operator !=(Coordinates c1, Coordinates c2)
	{
		return !(c1 == c2);
	}
	
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		
		Coordinates c = obj as Coordinates;
		if ((object)c == null)
		{
			return false;
		}
		
		return longitude == c.longitude && latitude == c.latitude;
	}
	
	public override int GetHashCode()
	{
		return latitude.GetHashCode() ^ longitude.GetHashCode();
	}
}