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

/// <summary>
/// Epilogue game state.
/// </summary>
public class EpilogueState : GameState
{
	// Epilogue strings
	private static string epilogueTitle = "Epilogue";
	
	private static string epilogueTextNormal = "That's it. You found the treasure.\nCongratulations!\nYou are truly an expert treasure hunter!";
	private static string enteredName = "";
	
	private static string submittingString = "Submitting...";
	private static string errorString = "Can't connect to server.";
	private static string noNameString = "Please enter a name.";
	private static string successString = "Successfully entered.";

	// Deal with errors in submission
	public static bool error = false;
	private static bool noName = false;
	private static bool submitting = false;

	/// <summary>
	/// Called on entering the game state.
	/// </summary>
	/// <param name="previousState">Previous state.</param>
	override public void OnEnter(GameState previousState)
	{
		// Activate chest, position it & animate it.
		Camera cam = GameObject.FindWithTag("ChestCamera").camera;
		GUIManager.treasureChest.transform.localPosition = cam.ViewportToWorldPoint(new Vector3(0.25f, 0.25f, 10));
		GUIManager.treasureChest.transform.localEulerAngles = new Vector3(0, 180, 0);
		
		GUIManager.treasureChest.SetActive(true);
		GUIManager.treasureChest.animation.Play();
	}

	/// <summary>
	/// Called on exiting the game state.
	/// </summary>
	/// <param name="nextState">Next state.</param>
	override public void OnExit(GameState nextState)
	{
		// Deactivate chest.
		GUIManager.treasureChest.SetActive(false);
	}

	/// <summary>
	/// OnGui method.
	/// </summary>
	override public void OnDisplay()
	{
		// Draw left epilogue page.
		GUIManager.GroupLeftPage(delegate(float w, float h) {
			GUI.Label(new Rect(0, 0, 1, 1), epilogueTitle, GUIManager.boldLabelStyle);

			GUI.Label(new Rect(0, h/11, w, h), epilogueTextNormal, GUIManager.multilineLabelStyle);
		});
		
		string statusString = "";

		// Draw enter button if score not already submitted.
		if (!GameManager.HasEnteredLeaderboard() && !submitting)
		{
			GUIManager.BottomRightButton("     Enter", delegate {
				ScoreEntry entry = GameManager.GetScoreEntry();
				entry.username = enteredName;
				Leaderboard.Instance.SubmitScore(entry);
				submitting = true;
				statusString = submittingString;
			});
		}

		// Draw right epilogue page.
		GUIManager.GroupRightPage(delegate(float w, float h) {
			string walkingTime = string.Format("Time spent walking:     {0}", GameManager.GetPassedWalkingTime().ToString());
			string puzzleTime = string.Format("Time spent solving puzzles:     {0}", GameManager.GetPassedPuzzleTime().ToString());
			string hints = string.Format("Needed hints:     {0}", GameManager.GetNumberOfHintsNeeded().ToString());
			string skipped = string.Format("Skipped puzzles:     {0}", GameManager.GetNumberOfSkippedPuzzles().ToString());
			string points = string.Format("Aqcuired points:     {0}", GameManager.GetFinalScore().ToString());
			
			GUI.Label(new Rect(0, 0*h/10, 1, 1), walkingTime, GUIManager.boldLabelStyle);    
			GUI.Label(new Rect(0, 1*h/10, 1, 1), puzzleTime, GUIManager.boldLabelStyle);
			GUI.Label(new Rect(0, 2*h/10, 1, 1), hints, GUIManager.boldLabelStyle);
			GUI.Label(new Rect(0, 3*h/10, 1, 1), skipped, GUIManager.boldLabelStyle);
			GUI.Label(new Rect(0, 4*h/10, 1, 1), points, GUIManager.boldLabelStyle);
			
			GUIManager.CenteredLabel("Immortalize your name in the leaderboard", w/2, 5.5f*h/10);

			// Text field to enter name.
			enteredName = GUI.TextField(new Rect(0, 7*h/10, w, h/10), enteredName, GUIManager.drawnButtonStyle);
			
			if (GameManager.leaderboardEntry != null && GameManager.leaderboardEntry.username.CompareTo("") != 0)
			{
				enteredName = GameManager.leaderboardEntry.username;
			}
			
			if (error)
			{
				if (enteredName.CompareTo("") == 0)
				{
					noName = true;
					error = false;
				}
				else
				{
					noName = false;
					statusString = errorString;
				}
				submitting = false;
			}
			
			if (noName)
			{
				statusString = noNameString;
			}
			
			if (submitting)
			{
				statusString = submittingString;
			}
			
			if (GameManager.HasEnteredLeaderboard())
			{
				GameManager.SaveAllData();
				statusString = successString;
				submitting = false;
			}
			
			GUIManager.CenteredLabel(statusString, w/2, 8*h/10);
		});
		
		// Draw back button to index state.
		GUIManager.BackButton(delegate {
			GameStateManager.ChangeTo(new IndexState());
		});
	}
}