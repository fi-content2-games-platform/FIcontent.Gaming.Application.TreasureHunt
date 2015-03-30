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
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Leaderboard game state.
/// </summary>
public class LeaderboardState : GameState
{
	// How many scores are displayed on the right page.
	private static int nScores = 7;

	// Leaderboard strings
	private static string title = "Leaderboard";

	private static string playerNotFoundString = "Complete the game to get a score.";
	
	private static string updateString = "Updating...";
	private static string noInternetString = "Can't connect to server.";

	// Internal leaderboard list
	private static List<string> leaderboard;

	// Rank of the player.
	private static int playerRank;

	// Deal with errors while updating leaderboard.
	private static bool error;
	private static bool updating;
	private static bool success;

	/// <summary>
	/// Called on entering the game state.
	/// </summary>
	/// <param name="previousState">Previous state.</param>
	override public void OnEnter(GameState previousState)
	{
		// Set internal lb to null
		leaderboard = null;
		// Start update
		Leaderboard.Instance.UpdateLeaderboard();
	}

	// Help function that does most of the sorting & formatting work.
	private List<string> CalculateLeaderboard()
	{
		string playerEntryString = "";
		if (GameManager.HasEnteredLeaderboard())
		{
			ScoreEntry pse = GameManager.leaderboardEntry;
			
			string nameStr = pse.username;
			string totalTimeStr = pse.totalTime;
			string puzzleTimeStr = pse.puzzleTime;
			string hintsStr = pse.hints;
			string skipsStr = pse.skips;
			
			int totalTime = int.Parse(totalTimeStr);
			int puzzleTime = int.Parse(puzzleTimeStr);
			int walkingTime = totalTime - puzzleTime;
			int hints = int.Parse(hintsStr);
			int skips = int.Parse(skipsStr);
			
			int score = Score.FinalScore(walkingTime, puzzleTime, hints, skips);
			playerEntryString = string.Format("{0}    -    {1} Points\nWalking: {2}  Solving: {3}  Hints: {4}  Skips: {5}", nameStr, score, Duration.FromSeconds(walkingTime).ToString(), Duration.FromSeconds(puzzleTime).ToString(), hints, skips);
		}
		
		List<ScoreEntry> scoreEntries = Leaderboard.Instance.GetLeaderboard();
		
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (ScoreEntry se in scoreEntries)
		{
			string nameStr = se.username;
			string totalTimeStr = se.totalTime;
			string puzzleTimeStr = se.puzzleTime;
			string hintsStr = se.hints;
			string skipsStr = se.skips;
			
			int totalTime = int.Parse(totalTimeStr);
			int puzzleTime = int.Parse(puzzleTimeStr);
			int walkingTime = totalTime - puzzleTime;
			int hints = int.Parse(hintsStr);
			int skips = int.Parse(skipsStr);
			
			int score = Score.FinalScore(walkingTime, puzzleTime, hints, skips);
			string entryString = string.Format("{0}    -    {1} Points\nWalking: {2}  Solving: {3}  Hints: {4}  Skips: {5}", nameStr, score, Duration.FromSeconds(walkingTime).ToString(), Duration.FromSeconds(puzzleTime).ToString(), hints, skips);
			
			dictionary.Add(entryString, score);
		}
		
		List<string> lb = new List<string>();
		
		List<KeyValuePair<string, int>> temp = dictionary.ToList();
		temp.Sort((firstPair, nextPair) =>
		          {
			return -1 * firstPair.Value.CompareTo(nextPair.Value);
		}
		);
		Dictionary<string, int> sortedDictionary = new Dictionary<string, int>();
		foreach (var item in temp)
		{
			sortedDictionary.Add(item.Key, item.Value);
		}
		
		int rank = 1;
		foreach (var item in sortedDictionary)
		{
			string entryString = item.Key;
			
			if (playerEntryString.CompareTo(entryString) == 0)
			{
				playerRank = rank;
			}
			string finalStr = string.Format("{0}. {1}", rank, entryString);
			lb.Add(finalStr);
			rank++;
		}
		
		return lb;
	}

	/// <summary>
	/// Update method.
	/// </summary>
	override public void OnUpdate()
	{
		// Set error/success/updating based on return value form Leaderboard class.
		List<ScoreEntry> scoreEntries = Leaderboard.Instance.GetLeaderboard();
		if (scoreEntries == null)
		{
			error = true;
			success = false;
			updating = false;
		}
		else if (scoreEntries.Count == 0)
		{
			updating = true;
			error = false;
			success = false;
		}
		else if (leaderboard == null)
		{
			leaderboard = CalculateLeaderboard();
			success = true;
			error = false;
			updating = false;
		}
	}

	/// <summary>
	/// OnGui method.
	/// </summary>
	override public void OnDisplay()
	{
		// Inherit screen size
		float width = GUIManager.virtualWidth;
		float height = GUIManager.virtualHeight;
		float halfWidth = width/2;

		// Draw leaderboard title.
		Rect titleRect = GUILayoutUtility.GetRect(new GUIContent(title), GUIManager.headerStyle);
		titleRect.x = width/4 - titleRect.width/2;
		titleRect.y = height/8;
		GUI.Label(titleRect, title, GUIManager.headerStyle);

		// Draw left page of leaderboard.
		if (GameManager.HasEnteredLeaderboard())
		{
			if (success)
			{
				ScoreEntry playerEntry = GameManager.leaderboardEntry;
				
				GUIManager.CenteredLabel(playerEntry.username, halfWidth/2, height/4, GUIManager.largeCenteredLabelStyle);
				
				string rankString = string.Format("Your Rank: {0}", playerRank);
				
				GUIManager.CenteredLabel(rankString, halfWidth/2, height/4 + 150, GUIManager.largeCenteredLabelStyle);
				
				string points = string.Format("Acquired points:     {0}", GameManager.GetFinalScore().ToString());
				string walkingTime = string.Format("Time spent walking:     {0}", GameManager.GetPassedWalkingTime().ToString());
				string puzzleTime = string.Format("Time spent solving puzzles:     {0}", GameManager.GetPassedPuzzleTime().ToString());
				string hints = string.Format("Needed hints:     {0}", GameManager.GetNumberOfHintsNeeded().ToString());
				string skipped = string.Format("Skipped puzzles:     {0}", GameManager.GetNumberOfSkippedPuzzles().ToString());
				
				GUI.Label(new Rect(80, 8*height/15, 1, 1), points, GUIManager.boldLabelStyle);    
				GUI.Label(new Rect(80, 9*height/15, 1, 1), walkingTime, GUIManager.boldLabelStyle);
				GUI.Label(new Rect(80, 10*height/15, 1, 1), puzzleTime, GUIManager.boldLabelStyle);
				GUI.Label(new Rect(80, 11*height/15, 1, 1), hints, GUIManager.boldLabelStyle);
				GUI.Label(new Rect(80, 12*height/15, 1, 1), skipped, GUIManager.boldLabelStyle);
			}
		}
		else
		{
			GUIManager.CenteredLabel(playerNotFoundString, halfWidth/2, height/3, GUIManager.largeCenteredLabelStyle);
		}

		// Diplay updating or error messages
		if (updating)
		{
			GUIManager.CenteredLabel(updateString, 3*width/4, height/3, GUIManager.largeCenteredLabelStyle);
		}
		else if (error)
		{
			GUIManager.CenteredLabel(noInternetString, 3*width/4, height/3, GUIManager.largeCenteredLabelStyle);
		}
		else if (success && leaderboard != null)	// On success, list leaderboard on right page.
		{
			GUIManager.GroupLeaderboard(delegate(float w, float h) {
				int min = Mathf.Min(nScores, leaderboard.Count);
				for (int i = 0; i < min; i++)
				{
					string lbEntry = leaderboard[i];

					// Best player is displayed in bold.
					if (i == 0)
					{
						GUI.Label(new Rect(0, i*140, w, h), lbEntry, GUIManager.boldMultilineLabelStyle);
					}
					else
					{
						GUI.Label(new Rect(0, i*140, w, h), lbEntry, GUIManager.multilineLabelStyle);
					}
				}
			});
		}

		// Draw back button to menu state.
		GUIManager.BackButton(delegate {
			GameStateManager.ChangeTo(new MenuState());
		});
	}
}