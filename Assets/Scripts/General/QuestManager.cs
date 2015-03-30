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
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

/// <summary>
/// Quest manager.
/// Keeps track of chapters, locations & puzzles.
/// </summary>
public class QuestManager : MonoBehaviour
{
	#region Fields
	
	// Editor Fields
	
	/// <summary>
	/// Add GameObject for each chapter. Should contain at least one ImageTarget with the puzzle(s) attached.
	/// </summary>
	public List<GameObject> chapterGameObjects;

	// Internal Fields

	// Strings
	private static string locationsFileName = "locations.xml";
	private static string chaptersFileName = "chapters.xml";
	private static string puzzlesFileName = "puzzles.xml";
	
	// Length of the quest == #chapters == #locations == #puzzles (set to # of GameObjects added to chapterGameObjects)
	public static int questLength;
	
	// Lists to cache the components of the locationGameObjects.
	private static List<Location> locations;
	private static List<Puzzle> puzzles;
	private static List<Chapter> chapters;
	
	#endregion
	
	#region MonoBehaviour Methods

	/// <summary>
	/// MonoBehaviour Awake method.
	/// </summary>
	void Awake()
	{
		// Set the quest length.
		questLength = chapterGameObjects.Count;
		if (questLength == 0)
		{
			Debug.LogError("Add some location GameObjects to the QuestManager.");
		}
		
		// Setup game objects from the editor components
		locations = new List<Location>(questLength);
		puzzles = new List<Puzzle>(questLength);
		chapters = new List<Chapter>(questLength);
		
		for (int i = 0; i < questLength; i++)
		{
			GameObject chapterGameObject = chapterGameObjects[i];
			chapterGameObject.SetActive(true);
			
			PuzzleLogic[] puzzleLogicList = chapterGameObject.GetComponentsInChildren<PuzzleLogic>(true);
			if (puzzleLogicList.Length != 0)
			{
				PuzzleLogic puzzleLogic = puzzleLogicList[0];
				puzzleLogic.SetPuzzleNumber(i);
			}
			
			Location location = chapterGameObject.GetComponent<Location>();
			Puzzle puzzle = chapterGameObject.GetComponent<Puzzle>();
			Chapter chapter = chapterGameObject.GetComponent<Chapter>();
			
			if (location == null || puzzle == null || chapter == null)
			{
				Debug.LogError(string.Format("Location GameObject #{0} doesn't have a location, puzzle and clue attached.", i));
			}
			
			location.locationNumber = i;
			locations.Add(location);
			
			puzzle.puzzleNumber = i;
			puzzles.Add(puzzle);
			
			chapter.chapterNumber = i;
			chapters.Add(chapter);
		}
		
		// At the start: Location 0 and Chapter 0 are unlocked.
		locations[0].Unlock();
		chapters[0].Unlock();
		
		// Try to load saved game.
		var l = new Serializer<List<LocationData>>(locationsFileName);
		var c = new Serializer<List<ChapterData>>(chaptersFileName);
		var p = new Serializer<List<PuzzleData>>(puzzlesFileName);
		
		try {
			List<LocationData> lds = l.Load();
			if (locations.Count != lds.Count)
			{
				Debug.LogError("Inconsistency while loading saved location data.");
			}
			for (int i = 0; i < questLength; i++)
			{
				locations[i].DeserializeFromData(lds[i]);
			}
			
			List<ChapterData> cds = c.Load();
			if (locations.Count != cds.Count)
			{
				Debug.LogError("Inconsistency while loading saved chapter data.");
			}
			for (int i = 0; i < questLength; i++)
			{
				chapters[i].DeserializeFromData(cds[i]);
			}
			
			List<PuzzleData> pds = p.Load();
			if (locations.Count != pds.Count)
			{
				Debug.LogError("Inconsistency while loading saved puzzle data.");
			}
			for (int i = 0; i < questLength; i++)
			{
				puzzles[i].DeserializeFromData(pds[i]);
			}
		}
		catch
		{
			
		}
	}

	/// <summary>
	/// MonoBehaviour Update method.
	/// </summary>
	void Update()
	{
		// Quest starts if first puzzle is solved.
		if (puzzles[0].IsSolved() && !GameManager.QuestStarted())
		{
			GameManager.StartQuest();
		}
		
		// Game over if last puzzle is solved.
		if (puzzles[questLength-1].IsSolved())
		{
			GameManager.PauseGame();
		}
		
		// Find out, which puzzle the player is looking at.
		
		foreach (Puzzle p in puzzles)
		{
			p.isCurrentlyInView = false;
		}
		if (GameStateManager.GetCurrentState() is CameraState)
		{
			// Add all active image targets to list.
			List<string> recognizedTargets = new List<string>();
			
			IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetActiveTrackableBehaviours();  
			foreach (TrackableBehaviour tb in tbs)
			{
				recognizedTargets.Add(tb.TrackableName);
			}
			
			// Campare image target names of locations with list.
			foreach (string recognized in recognizedTargets)
			{
				for (int i = 0; i < locations.Count; i++)
				{
					foreach (string target in locations[i].GetImageTargetNames())
					{
						if (string.Compare(recognized, target) == 0)
						{
							puzzles[i].isCurrentlyInView = true;
							if (i == 0)
							{
								puzzles[i].See();
							}
							else if (puzzles[i-1].IsSolved())
							{
								puzzles[i].See();
							}
						}
					}
				}
			}
		}
	}
	
	#endregion
	
	#region Public Methods

	/// <summary>
	/// Returns whether player is looking at any of the puzzles currently.
	/// </summary>
	/// <returns><c>true</c>, if player is looking at a puzzle, <c>false</c> otherwise.</returns>
	public static bool LookingAtAnyPuzzle()
	{
		bool success = false;
		foreach (Puzzle p in puzzles)
		{
			if (p.isCurrentlyInView)
			{
				success = true;
			}
		}
		return success;
	}
	
	/// <summary>
	/// Returns locations by index.
	/// </summary>
	/// <returns>The location.</returns>
	/// <param name="locationNumber">Location number.</param>
	public static Location GetLocation(int locationNumber)
	{
		if (0 <= locationNumber && locationNumber < questLength)
		{
			return locations[locationNumber];
		}
		else
		{
			Debug.LogWarning("Accessed location out of bounds.");
			return null;
		}
	}
	
	/// <summary>
	/// Returns puzzles by index.
	/// </summary>
	/// <returns>The puzzle.</returns>
	/// <param name="puzzleNumber">Puzzle number.</param>
	public static Puzzle GetPuzzle(int puzzleNumber)
	{
		if (0 <= puzzleNumber && puzzleNumber < questLength)
		{
			return puzzles[puzzleNumber];
		}
		else
		{
			Debug.LogWarning("Accessed puzzle out of bounds.");
			return null;
		}
	}
	
	/// <summary>
	/// Returns chapters by index.
	/// </summary>
	/// <returns>The chapter.</returns>
	/// <param name="chapterNumber">Chapter number.</param>
	public static Chapter GetChapter(int chapterNumber)
	{
		if (0 <= chapterNumber && chapterNumber < questLength)
		{
			return chapters[chapterNumber];
		}
		else
		{
			Debug.LogWarning("Accessed clue out of bounds.");
			return null;
		}
	}
	
	/// <summary>
	/// Resets all internal data for a new game. Clears saved files.
	/// </summary>
	public static void Reset()
	{
		var l = new Serializer<List<LocationData>>(locationsFileName);
		var c = new Serializer<List<ChapterData>>(chaptersFileName);
		var p = new Serializer<List<PuzzleData>>(puzzlesFileName);
		
		l.Delete();
		c.Delete();
		p.Delete();
		
		LocationData locationData = new LocationData();
		ChapterData chapterData = new ChapterData();
		PuzzleData puzzleData = new PuzzleData();
		
		for (int i = 0; i < questLength; i++)
		{
			locations[i].DeserializeFromData(locationData);
			
			chapters[i].DeserializeFromData(chapterData);
			
			puzzles[i].DeserializeFromData(puzzleData);
		}
		
		// Unlock first chapter and location.
		locations[0].Unlock();
		chapters[0].Unlock();
	}
	
	/// <summary>
	/// Save all internal quest data to files.
	/// </summary>
	public static void SaveQuestData()
	{
		var l = new Serializer<List<LocationData>>(locationsFileName);
		var c = new Serializer<List<ChapterData>>(chaptersFileName);
		var p = new Serializer<List<PuzzleData>>(puzzlesFileName);
		
		List<LocationData> locationDataList = new List<LocationData>();
		foreach (Location location in locations)
		{
			locationDataList.Add(location.SerializeToData());
		}
		
		l.Save(locationDataList);
		
		List<ChapterData> chapterDataList = new List<ChapterData>();
		foreach (Chapter chapter in chapters)
		{
			chapterDataList.Add(chapter.SerializeToData());
		}
		
		c.Save(chapterDataList);
		
		List<PuzzleData> puzzleDataList = new List<PuzzleData>();
		foreach (Puzzle puzzle in puzzles)
		{
			puzzleDataList.Add(puzzle.SerializeToData());
		}
		
		p.Save(puzzleDataList);
	}
	
	#endregion
}