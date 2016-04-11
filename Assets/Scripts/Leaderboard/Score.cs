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

public static class Score
{
	private static float wPT = 0.5f;
	private static float wWT = 0.5f;
	private static float wH = 0.5f;
	
	private static float minPT = 120;
	private static float maxPT = 1500;
	
	private static float minWT = 900;
	private static float maxWT = 7200;
	
	private static int minH = 20;
	private static int maxH = 0;
	
	public static int FinalScore(int walkingTime, int puzzleTime, int hints, int skips)
	{
		float score = 1000 * (FWT(walkingTime) * wWT + FPT(puzzleTime) * wPT - FH(hints) * wH);
		score = score * Mathf.Pow(2, -skips);
		if (score < 0) 
		{
			return 0;
		}
		else
		{
			return Mathf.RoundToInt(score);
		}
	}
	
	private static float FPT(float time)
	{
		if (time < minPT) return 1;
		if (time > maxPT) return 0;
		return A(minPT, maxPT) * time + B(minPT, maxPT);
	}
	
	private static float FWT(float time)
	{
		if (time < minWT) return 1;
		if (time > maxWT) return 0;
		return A(minWT, maxWT) * time + B(minWT, maxWT);
	}
	
	private static float FH(int hints)
	{
		if (hints > minH) return 1;
		if (hints < maxH) return 0;
		return A(minH, maxH) * hints + B(minH, maxH);
	}
	
	private static float A(float min, float max)
	{
		return -1.0f/(max-min);
	}
	
	private static float B(float min, float max)
	{
		return max/(max-min);
	}
}