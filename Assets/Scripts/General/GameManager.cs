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
using System;
using System.IO;

/// <summary>
/// Game manager.
/// </summary>
public class GameManager : MonoBehaviour
{
	#region Fields

	// Public fields

	/// <summary>
	/// The current version of the game.
	/// </summary>
	public static string version = "Version 1.0";

	/// <summary>
	/// Returns wheter there is a connection to the internet.
	/// </summary>
	public static bool connectedToInternet = false;

	// Internal fields
	
	// Filename for storage of game data.
	private static string gameDataFileName = "game.xml";

	// Deal with distance calculation.
	private static Coordinates lastLocation;
	private static Distance walkedDistance;

	// Deal with time measurement.
	private static Duration currentTime;
	private static Duration lastTime;
	private static Duration passedTotalTime;
	private static Duration passedPuzzleTime;

	// Keep track of #(hints) & #(skipped puzzles).
	private static int numberHintsNeeded;
	private static int numberPuzzlesSkipped;

	// Started game & it's running.
	private static bool gameRunning;
	// Started game & it isn't running.
	private static bool gameStarted;
	// Quest started (after prologe is complete).
	private static bool questStarted;

	// Entered a name into the leaderboard.
	private static bool enteredLeaderboard; 

	// What is entered into leaderboard.
	public static ScoreEntry leaderboardEntry;

	// A not so unique user id as a temporary username.
	private static string notSoUUID;

	// Highscore field in leaderboard will be filled with increasing timestamps.
	private static long timestamp;
	
	#endregion
	
	#region MonoBehaviour Methods

	/// <summary>
	/// MonoBehaviour Awake method.
	/// </summary>
	void Awake()
	{
		// Initialize all data.
		GameData gameData = new GameData();
		DeserializeFromData(gameData);
		
		// Try to load any saved game from the file.
		var g = new Serializer<GameData>(gameDataFileName);
		try
		{
			GameData gd = g.Load();
			DeserializeFromData(gd);
		} 
		catch {}
		
		notSoUUID = DateTime.Now.ToString("yyy-MM-dd HH:mm:ss");
		timestamp = 0;
		
		// Auto save
		InvokeRepeating("AutoSave", 0, 5);
		
		StartCoroutine(CheckConnection());
	}

	/// <summary>
	/// MonoBehaviour Update method.
	/// </summary>
	void Update()
	{
		if (gameRunning)
		{
			// Update walked distance
			Coordinates newLocation = Sensors.Location();
			if (lastLocation != null)
			{
				if (newLocation != lastLocation)
				{
					Distance deltaDistance = lastLocation.DistanceTo(newLocation);
					if (deltaDistance.IsValid())
					{
						walkedDistance += deltaDistance;
					}
				}
			}
			lastLocation = newLocation;
			
			// Update passed time
			Duration deltaTime = currentTime - lastTime;
			passedTotalTime += deltaTime;
			if (QuestManager.LookingAtAnyPuzzle())
			{
				passedPuzzleTime += deltaTime;
			}
			lastTime = currentTime;
			currentTime = Duration.Now();
		}
	}

	/// <summary>
	/// MonoBehaviour OnApplicationQuit method.
	/// </summary>
	void OnApplicationQuit()
	{
		SaveAllData();
	}

	/// <summary>
	/// MonoBehaviour OnApplicationPause method.
	/// </summary>
	void OnApplicationPause()
	{
		SaveAllData();
	}
	
	#endregion
	
	#region Public Methods

	/// <summary>
	/// Resets all internal data for a new game. Clears saved files.
	/// </summary>
	public static void Reset()
	{
		var g = new Serializer<GameData>(gameDataFileName);
		g.Delete();
		
		GameData gameData = new GameData();
		DeserializeFromData(gameData);
	}

	/// <summary>
	/// Start a new game: Clear saved game, reset the quest.
	/// </summary>
	public static void StartGame()
	{
		Reset();
		gameStarted = true;
		QuestManager.Reset();
	}
	
	/// <summary>
	/// Start the quest: Start timer & distance measurement.
	/// </summary>
	public static void StartQuest()
	{
		questStarted = true;
		gameRunning = true;
		currentTime = Duration.Now();
		lastTime = Duration.Now();
	}
	
	/// <summary>
	/// Pause the game.
	/// </summary>
	public static void PauseGame()
	{
		gameRunning = false;
	}

	/// <summary>
	/// Mark the leaderboard as used. You can only enter a name once.
	/// </summary>
	public static void EnterLeaderboard()
	{
		enteredLeaderboard = true;
	}
	
	/// <summary>
	/// Increments the number of hints needed.
	/// </summary>
	public static void NeededHint()
	{
		numberHintsNeeded++;
	}
	
	/// <summary>
	/// Increments the number of skipped puzzles.
	/// </summary>
	public static void SkippedPuzzle()
	{
		numberPuzzlesSkipped++;
	}
	
	/// <summary>
	/// Save data for the GameManager.
	/// </summary>
	public static void SaveGameData()
	{
		var g = new Serializer<GameData>(gameDataFileName);
		
		GameData gameData = SerializeToData();
		g.Save(gameData);
	}
	
	/// <summary>
	/// Save all data for the whole game.
	/// </summary>
	public static void SaveAllData()
	{
		GameManager.SaveGameData();
		QuestManager.SaveQuestData();
	}
	
	/// <summary>
	/// Returns a ScoreEntry filled with current values.
	/// </summary>
	/// <returns>The score entry.</returns>
	public static ScoreEntry GetScoreEntry()
	{
		return new ScoreEntry(notSoUUID,
		                      timestamp.ToString(),
		                      passedTotalTime.SecondsInt().ToString(),
		                      passedPuzzleTime.SecondsInt().ToString(),
		                      walkedDistance.MetersInt().ToString(),
		                      numberHintsNeeded.ToString(),
		                      numberPuzzlesSkipped.ToString());
	}
	
	#endregion

	#region Private Methods

	// Check for an internet connection by pinging google.com.
	// http://answers.unity3d.com/questions/41408/is-it-possible-to-check-internet-conneciton-for-ww.html
	IEnumerator CheckConnection()
	{
		WWW www = new WWW("http://www.google.com");
		yield return www;
		
		if(www.error != null)
		{
			connectedToInternet = false;
			yield return new WaitForSeconds(2);// trying again after 2 sec
			StartCoroutine(CheckConnection());
		}
		else
		{
			connectedToInternet = true;
			yield return new WaitForSeconds(5);// recheck if the internet still exists after 5 sec
			StartCoroutine(CheckConnection());
		}
	}
	
	// Auto save function called every 5 seconds.
	private void AutoSave()
	{
		GameManager.SaveAllData();
	}

	#endregion
	
	#region Getters
	
	/// <summary>
	/// Returns the walked distance.
	/// </summary>
	/// <returns>The walked distance.</returns>
	public static Distance GetWalkedDistance()
	{
		return walkedDistance;
	}
	
	/// <summary>
	/// Returns the passed total time.
	/// </summary>
	/// <returns>The passed total time.</returns>
	public static Duration GetPassedTotalTime()
	{
		return passedTotalTime;
	}
	
	/// <summary>
	/// Returns the time passed with solving puzzles.
	/// </summary>
	/// <returns>The passed puzzle time.</returns>
	public static Duration GetPassedPuzzleTime()
	{
		return passedPuzzleTime;
	}
	
	/// <summary>
	/// Returns the time passed with walking around.
	/// </summary>
	/// <returns>The passed walking time.</returns>
	public static Duration GetPassedWalkingTime()
	{
		return passedTotalTime - passedPuzzleTime;
	}
	
	/// <summary>
	/// Returns number of hints needed.
	/// </summary>
	/// <returns>The number of hints needed.</returns>
	public static int GetNumberOfHintsNeeded()
	{
		return numberHintsNeeded;
	}
	
	/// <summary>
	/// Returns number of skipped puzzles.
	/// </summary>
	/// <returns>The number of skipped puzzles.</returns>
	public static int GetNumberOfSkippedPuzzles()
	{
		return numberPuzzlesSkipped;
	}
	
	/// <summary>
	/// Calculates the final score in a separate class.
	/// </summary>
	/// <returns>The final score.</returns>
	public static int GetFinalScore()
	{
		return Score.FinalScore((passedTotalTime - passedPuzzleTime).SecondsInt(), passedPuzzleTime.SecondsInt(), numberHintsNeeded, numberPuzzlesSkipped);
	}
	
	/// <summary>
	/// Returns whether a game was started.
	/// </summary>
	/// <returns><c>true</c>, if game was started, <c>false</c> otherwise.</returns>
	public static bool GameStarted()
	{
		return gameStarted;
	}
	
	/// <summary>
	/// Returns whether the quest was started.
	/// </summary>
	/// <returns><c>true</c>, if quest was started, <c>false</c> otherwise.</returns>
	public static bool QuestStarted()
	{
		return questStarted;
	}
	
	/// <summary>
	/// Returns whether the player already entered his name into the leaderboard.
	/// </summary>
	/// <returns><c>true</c> if player has entered leaderboard; otherwise, <c>false</c>.</returns>
	public static bool HasEnteredLeaderboard()
	{
		return enteredLeaderboard;
	}
	
	#endregion
	
	#region Serialization

	/// <summary>
	/// Serialize the internal data to an object to save it.
	/// </summary>
	/// <returns>GameData object containing the data.</returns>
	public static GameData SerializeToData()
	{
		GameData gd = new GameData();
		gd.lastLocation = lastLocation;
		gd.walkedDistance = walkedDistance;
		gd.passedTotalTime = passedTotalTime;
		gd.passedPuzzleTime = passedPuzzleTime;
		gd.gameStarted = gameStarted;
		gd.questStarted = questStarted;
		gd.enteredLeaderboard = enteredLeaderboard;
		gd.numberHintsNeeded = numberHintsNeeded;
		gd.numberPuzzlesSkipped = numberPuzzlesSkipped;
		gd.leaderboardEntry = leaderboardEntry;
		return gd;
	}
	
	/// <summary>
	/// Deserialize the internal data from an object.
	/// </summary>
	/// <param name="gd">GameData object.</param>
	public static void DeserializeFromData(GameData gd)
	{
		lastLocation = gd.lastLocation;
		walkedDistance = gd.walkedDistance;
		passedTotalTime = gd.passedTotalTime;
		passedPuzzleTime = gd.passedPuzzleTime;
		gameStarted = gd.gameStarted;
		questStarted = gd.questStarted;
		enteredLeaderboard = gd.enteredLeaderboard;
		numberHintsNeeded = gd.numberHintsNeeded;
		numberPuzzlesSkipped = gd.numberPuzzlesSkipped;
		leaderboardEntry = gd.leaderboardEntry;
	}
	
	#endregion
}