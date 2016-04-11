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
using System.Collections.Generic;

/// <summary>
/// Defines a puzzle.
/// </summary>
public class Puzzle : MonoBehaviour
{
    #region Fields

    /// <summary>
    /// First hint text for help function.
	/// </summary>
    [Multiline]
    public string tier1HelpText;
	/// <summary>
	/// Second hint text for help function.
	/// </summary>
    [Multiline]
    public string tier2HelpText;

	/// <summary>
	/// Whether this puzzle has help available.
	/// </summary>
	public bool hasHelpFunction = true;

    // Internal Fields

	/// <summary>
	/// The puzzle number.
	/// </summary>
    [HideInInspector]
    public int puzzleNumber;

	/// <summary>
	/// Whether puzzle is currently in view of the camera.
	/// </summary>
    [HideInInspector]
    public bool isCurrentlyInView;

	// Whether the puzzle is seen, i.e. looked at once.
	private bool isSeen;
	// Whether the puzzle is solved.
    private bool isSolved;
	// Whether the first hint is active in the help function.
    private bool isTier1HelpActive;
	// Whether the second hint is active in the help function.
    private bool isTier2HelpActive;
	// Whether this puzzle was skipped.
    private bool isSkipped;
	// Whether this puzzle needs to be reset.
    private bool needsReset;

    #endregion

    #region Public Methods
	
	/// <summary>
	/// Mark the puzzle as solved. This unlocks the next chapter and location.
	/// </summary>
    public void Solve()
    {
        if (!isSolved)
        {
            isSolved = true;
            if (puzzleNumber + 1 < QuestManager.questLength)
            {
                QuestManager.GetChapter(puzzleNumber + 1).Unlock();
                QuestManager.GetLocation(puzzleNumber + 1).Unlock();
            }
            Debug.Log(string.Format("Solved puzzle #{0}", puzzleNumber));
        }
    }

    /// <summary>
	/// Triggers the help function. Each time, more hints are unlocked.
	/// </summary>
    public void Help()
    {
        if (!isSolved)
        {
            if (!isTier1HelpActive)
            {
				GameManager.NeededHint();
                isTier1HelpActive = true;
            }
            else if (!isTier2HelpActive)
            {
				GameManager.NeededHint();
                isTier2HelpActive = true;
            }
			else if (!isSkipped)
            {
				GameManager.SkippedPuzzle();
				isSkipped = true;
                Solve();
            }
        }
    }

	/// <summary>
	/// Marks puzzle as seen.
	/// </summary>
	public void See()
	{
		isSeen = true;
	}

    /// <summary>
	/// Marks that the reset is complete. Used for the internal framework.
	/// </summary>
    public void ResetOver()
    {
        needsReset = false;
    }

    #endregion

    #region Getters

    /// <summary>
	/// Returns whether this puzzle was already solved.
	/// </summary>
    /// <returns><c>true</c> if this puzzle is solved; otherwise, <c>false</c>.</returns>
    public bool IsSolved()
    {
        return isSolved;
    }

    /// <summary>
	/// Returns whether the first hint is unlocked.
	/// </summary>
    /// <returns><c>true</c> if the first hint is unlocked; otherwise, <c>false</c>.</returns>
    public bool IsTier1HelpActive()
    {
        return isTier1HelpActive;
    }

	/// <summary>
	/// Returns whether the second hint is unlocked.
	/// </summary>
	/// <returns><c>true</c> if the second hint is unlocked; otherwise, <c>false</c>.</returns>
    public bool IsTier2HelpActive()
    {
        return isTier2HelpActive;
    }

    /// <summary>
	/// Returns whether the puzzle needs to be reset. Used for the internal framework.
	/// </summary>
    /// <returns><c>true</c>, if reset is needsed, <c>false</c> otherwise.</returns>
    public bool NeedsReset()
    {
        return needsReset;
    }

	/// <summary>
	/// Returns wheter puzzles is seen.
	/// </summary>
	/// <returns><c>true</c> if this puzzle was seen; otherwise, <c>false</c>.</returns>
	public bool IsSeen()
	{
		return isSeen;
	}

    #endregion

    #region Serialization

    /// <summary>
	/// Serialize the internal data to an object to save it.
	/// </summary>
    /// <returns>PuzzleData object containing the data.</returns>
    public PuzzleData SerializeToData()
    {
        PuzzleData pd = new PuzzleData();
		pd.isSeen = isSeen;
        pd.isSolved = isSolved;
        pd.isTier1HelpActive = isTier1HelpActive;
        pd.isTier2HelpActive = isTier2HelpActive;
        pd.needsReset = needsReset;
		pd.isSkipped = isSkipped;
        return pd;
    }

    /// <summary>
	/// Deserialize the internal data from an object.
	/// </summary>
    /// <param name="pd">PuzzleData object.</param>
    public void DeserializeFromData(PuzzleData pd)
    {
		isSeen = pd.isSeen;
        isSolved = pd.isSolved;
        isTier1HelpActive = pd.isTier1HelpActive;
        isTier2HelpActive = pd.isTier2HelpActive;
        needsReset = pd.needsReset;
		isSkipped = pd.isSkipped;
    }

    #endregion
}