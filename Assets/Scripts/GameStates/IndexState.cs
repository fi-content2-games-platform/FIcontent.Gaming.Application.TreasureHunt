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
/// Index game state.
/// </summary>
public class IndexState : GameState
{
	/// <summary>
	/// The last selected chapter number.
	/// </summary>
	public static int selectedChapterNumber;

	/// <summary>
	/// The last selected chapter title.
	/// </summary>
	public static string selectedChapterTitle;

	// Index strings
	private static string journalTitle = "Journal Index";
	private static string timeTitle = "Passed time:";
	private static string hintsTitle = "Number of hints needed:";
	private static string skippedTitle = "Number of skipped puzzles:";
	
	private static string noInternetString = "This game doesn't require an internet\n connection but it is highly\n recommended for better GPS tracking.";

	/// <summary>
	/// Called on entering the game state.
	/// </summary>
	/// <param name="previousState">Previous state.</param>
	override public void OnEnter(GameState previousState)
	{
		// Show popover if not connected to internet.
		if (!GameManager.connectedToInternet && previousState is MenuState)
		{
			hasPopover = true;
		}
	}

	/// <summary>
	/// Update method.
	/// </summary>
	override public void OnUpdate()
	{
		// Debug: Auto unlock all chapters with cheat function.
		if (Input.GetKeyDown(KeyCode.C))
		{
			for (int i = 0; i < QuestManager.questLength; i++)
			{
				QuestManager.GetChapter(i).Unlock();
				QuestManager.GetPuzzle(i).See();
			}
		}
	}

	/// <summary>
	/// OnGui method.
	/// </summary>
	override public void OnDisplay()
	{
		// Draw exit button to menu state.
		GUIManager.TopLeftButton("     Exit", delegate {
			GameStateManager.ChangeTo(new MenuState());
			GameManager.PauseGame();
		});

		// Draw left index page.
		GUIManager.GroupLeftPage(delegate(float w, float h) {
			Rect journalTitleRect = GUILayoutUtility.GetRect(new GUIContent(journalTitle), GUIManager.headerStyle);
			journalTitleRect.x = (w - journalTitleRect.width)/2;
			journalTitleRect.y = 0;
			
			GUI.Label(journalTitleRect, journalTitle, GUIManager.headerStyle);
			
			GUI.Label(new Rect(0, h*0.2f, 1, 1), timeTitle, GUIManager.boldLabelStyle);
			
			GUI.Label(new Rect(0, h*0.4f, 1, 1), hintsTitle, GUIManager.boldLabelStyle);
			
			GUI.Label(new Rect(0, h*0.6f, 1, 1), skippedTitle, GUIManager.boldLabelStyle);
			
			string timeValue = GameManager.GetPassedTotalTime().ToString();
			string hintsValue = GameManager.GetNumberOfHintsNeeded().ToString();
			string skippedValue = GameManager.GetNumberOfSkippedPuzzles().ToString();
			
			GUIManager.CenteredLabel(timeValue, w/2, 0.3f*h);
			GUIManager.CenteredLabel(hintsValue, w/2, 0.5f*h);
			GUIManager.CenteredLabel(skippedValue, w/2, 0.7f*h);
		});

		// Draw buttons to access chapters.
		for (int i = 0; i < QuestManager.questLength; i++)
		{
			Chapter chapter = QuestManager.GetChapter(i);
			if (chapter.IsUnlocked())
			{
				string chapterTitle = "Chapter ?";
				if (i == 0)
				{
					chapterTitle = "Prologue";
				}
				else if (i == 1)
				{
					chapterTitle = "Chapter I";
				}
				else if (i == 2)
				{
					chapterTitle = "Chapter II";
				}
				else if (i == 3)
				{
					chapterTitle = "Chapter III";
				}
				else if (i == 4)
				{
					chapterTitle = "Chapter IV";
				}
				else if (i == 5)
				{
					chapterTitle = "Chapter V";
				}
				else if (i == 6)
				{
					chapterTitle = "Chapter VI";
				}

				GUIManager.ChapterButton(i+1, chapterTitle, chapter.IsRead(), delegate {
					selectedChapterNumber = i;
					selectedChapterTitle = chapterTitle;
					chapter.Read();
					
					if (i == 0)
					{
						GameStateManager.ChangeTo(new PrologueState());
					}
					else
					{
						GameStateManager.ChangeTo(new ChapterState());
					}
				});
			}
		}

		// Add button for preface.
		GUIManager.ChapterButton(0, "Preface", true, delegate {
			GameStateManager.ChangeTo(new PrefaceState());
		});

		// Add button for epilogue if unlocked.
		if (QuestManager.GetPuzzle(QuestManager.questLength-1).IsSolved())
		{
			GUIManager.ChapterButton(QuestManager.questLength+1, "Epilogue", GameManager.HasEnteredLeaderboard(), delegate {
				GameStateManager.ChangeTo(new EpilogueState());
			});
		}
	}

	/// <summary>
	/// OnGUI method to draw on the popover layer.
	/// </summary>
	override public void OnPopoverDisplay()
	{
		// Show "no internet" popover.
		GUIManager.PopoverOneButton(noInternetString, "Continue", delegate {
			hasPopover = false;
		});
	}
}