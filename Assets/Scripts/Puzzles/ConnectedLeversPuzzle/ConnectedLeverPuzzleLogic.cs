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
using System.Collections.Generic;

public class ConnectedLeverPuzzleLogic : PuzzleLogic
{
	public int numberLevers = 2;
	private bool[] toggled;
	
	public void Toggle(int i)
	{
		if (i >= 0 && i < numberLevers)
		{
			toggled[i] = !toggled[i];
		}
	}
	
	public bool IsToggled(int i)
	{
		if (i >= 0 && i < numberLevers)
		{
			return toggled[i];
		}
		return false;
	}
	
	override protected void Reset()
	{
		toggled = new bool[numberLevers];

		LeverPull[] levers = GetComponentsInChildren<LeverPull>();
		for(int i = 0; i < levers.Length; i++)
		{
			levers[i].ForceUpPosition();
		}
	}
	
	override protected bool CheckIfSolved()
	{
		bool solved = true;
		
		for (int i = 0; i < numberLevers; i++)
		{
			if (!toggled[i])
			{
				solved = false;
			}
		}
		
		return solved;
	}
	
	override protected void OnSkip()
	{
		for (int i = 0; i < numberLevers; i++)
		{
			toggled[i] = true;
		}
	}
}