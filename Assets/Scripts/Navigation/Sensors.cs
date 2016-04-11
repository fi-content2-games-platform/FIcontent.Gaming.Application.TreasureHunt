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

// Provides access to the relevant device sensors.
public class Sensors : MonoBehaviour
{
	public static int updateIntervalGPS = 5;

	private static Coordinates location;
	private static float altitude;
	private static float horizonatalAccuracy;
	private static double timestamp;
    
    void Start()
    {
        // Enable sensors.
        Input.compass.enabled = true;
        Input.gyro.enabled = true;

        // Init GPS readings with NaN.
        location = new Coordinates(float.NaN, float.NaN);
        altitude = float.NaN;
        horizonatalAccuracy = float.NaN;
		timestamp = double.NaN;

		InvokeRepeating("UpdateLocation", 0, updateIntervalGPS);
    }

	public static Quaternion Attitude()
	{
		Quaternion q = Input.gyro.attitude;
		return new Quaternion(q.x, q.y, -q.z, q.w);
	}

	public static Quaternion North()
	{
		return Quaternion.Euler(0, 0, -(Input.compass.magneticHeading + MagneticDeclination.Zurich));
	}

	public static Quaternion Bearing(float bearing)
	{
		return Quaternion.Euler(0, 0, -(Input.compass.magneticHeading + MagneticDeclination.Zurich - bearing));
	}

	public static Vector3 Gravity()
	{
		return Input.gyro.gravity;
	}

	public static Coordinates Location()
	{
		return location;
	}

	public static float Altitude()
	{
		return altitude;
	}

	public static float HorizontalAccuracy()
	{
		return horizonatalAccuracy;
	}

	public static double Timestamp()
	{
		return timestamp;
	}

    void UpdateLocation()
    {
        StartCoroutine(Sensors.UpdateLocationCoroutine());
    }
    
    // Updates the GPS readings. (http://docs.unity3d.com/Documentation/ScriptReference/LocationService.Start.html)
	public static IEnumerator UpdateLocationCoroutine()
	{
		if (!Input.location.isEnabledByUser)
		{
			Debug.Log("GPS isn't enabled by user.");    // TODO Error message
			location = new Coordinates(float.NaN, float.NaN);
			altitude = float.NaN;
			horizonatalAccuracy = float.NaN;
			timestamp = double.NaN;
			
			yield break;
		}
		
		if (Input.location.status == LocationServiceStatus.Stopped)
		{
			Input.location.Start();
		}
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1);
			maxWait--;
		}
		
		if (maxWait < 1)
		{
			Debug.Log("GPS timeout.");
			yield break;
		}
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			Debug.Log("GPS failed.");
			yield break;
		}
		else
		{
			location = new Coordinates(Input.location.lastData.latitude, Input.location.lastData.longitude);
			altitude = Input.location.lastData.altitude;
			horizonatalAccuracy = Input.location.lastData.horizontalAccuracy;
			timestamp = Input.location.lastData.timestamp;
		}
	}
}