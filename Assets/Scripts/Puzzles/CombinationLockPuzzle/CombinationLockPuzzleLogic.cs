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
using System.Collections.Generic;

public class CombinationLockPuzzleLogic : PuzzleLogic {

    public List<int> combination = new List<int> {3, 5, 8, 7};

	public List<GameObject> dialGameObjects;
    CombinationLockDial[] dials;

    override protected void Reset()
    {
        foreach (CombinationLockDial dial in dials)
        {
            dial.SetDigit(0);
            dial.Activate();
        }
    }

    override protected bool CheckIfSolved()
    {
        bool solved = true;

        for (int i = 0; i < dials.Length; i++)
        {
            if (dials[i].GetDigit() != combination[i])
            {
                solved = false;
            }
        }

        return solved;
    }

    override protected void OnSkip()
    {
        for (int i = 0; i < dials.Length; i++)
        {
            dials[i].SetDigit(combination[i]);
            dials[i].Deactivate();
        }
    }

    override protected void OnSolve()
    {
        for (int i = 0; i < dials.Length; i++)
        {
            dials[i].Deactivate();
        }
    }

    void Awake()
    {
		if (dialGameObjects.Count != 4)
		{
			Debug.LogError("Connect cylinders in the CombinationLock puzzle.");
		}
		dials = new CombinationLockDial[4];
		int i = 0;
		foreach (GameObject go in dialGameObjects)
		{
			CombinationLockDial dial = go.GetComponent<CombinationLockDial>();
			dials[i] = dial;
			i++;
		}
        if (combination.Count != dials.Length)
        {
            Debug.LogError("Number of Digit Dials in the Combination Lock doesn't match the combination.");
        }
    }
}