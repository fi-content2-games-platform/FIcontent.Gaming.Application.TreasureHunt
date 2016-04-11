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

public abstract class PuzzleLogic : MonoBehaviour
{
    #region Fields

    // Set automatically by QuestManager.
    protected int puzzleNumber;

    // Coresponding Puzzle object is cached here.
    protected Puzzle puzzle;

    // Used to detect auto-solving.
    protected bool isSolvedInternally;

    #endregion

    #region Mandatory To Implement

    // Implement these methods in subclasses

    // Check the puzzle data state to find out if it was solved.
    abstract protected bool CheckIfSolved();

    // Reset the puzzle data to initial state.
    abstract protected void Reset();

    #endregion

    #region Optional To Implement

    // In case the puzzle is skipped, set the data to a solved state.
	virtual protected void OnSkip()
    {
    }

    // Do cleanup after the puzzle was solved.
    virtual protected void OnSolve()
    {
    }

	// Do additional things in Update()
	virtual protected void OnUpdate()
	{
	}

    #endregion

    #region Public Methods

    // Puzzle Scripts can use this to react.
    public bool IsSolved()
    {
        return isSolvedInternally;
    }

    #endregion

    #region Internal

    void Start()
    {
        isSolvedInternally = false;
        Reset();
        puzzle = QuestManager.GetPuzzle(puzzleNumber);
    }

    void Update()
    {
        // Reset handling
        if (puzzle.NeedsReset())
        {
            isSolvedInternally = false;
            Reset();
            puzzle.ResetOver();
        }

        // Not solved in QuestManager, check if puzzle is in a solved state.
        if (!puzzle.IsSolved())
        {
            if (CheckIfSolved())
            {
                isSolvedInternally = true;
                puzzle.Solve();
                OnSolve();
            }
        }

        // Solved in QuestManager but not internally. This indicates that the puzzle was skipped.
        else if (!isSolvedInternally)
        {
            isSolvedInternally = true;
			OnSkip();
        }

		OnUpdate();
    }

    public void SetPuzzleNumber(int n)
    {
        puzzleNumber = n;
    }

    #endregion
}