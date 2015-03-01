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
/// Help game state.
/// </summary>
public class HelpState : GameState
{
	// Chapter number
	private static int chapterNumber;
	// Chapter title
	private static string chapterTitle;

	// Deal with two different popovers
	private static bool helpRequestPromptShowing = false;
	private static bool noHelpPromptShowing = false;

	// No GPS string
	private static string noSignal = "Waiting for GPS signal...";

	// Quaternion to keep track of arrow rotation for direction help arrow.
	private static Quaternion arrowRotation = Quaternion.Euler(0, 0, 0);

	/// <summary>
	/// Called on entering the game state.
	/// </summary>
	/// <param name="previousState">Previous state.</param>
	override public void OnEnter(GameState previousState)
	{
		// Inherit chapter number & title from index state.
		chapterNumber = IndexState.selectedChapterNumber;
		chapterTitle = IndexState.selectedChapterTitle + " Help";
		GUIManager.tts.ClearSay ();
		GUIManager.tts.SayAdd (chapterTitle, "happy", false);
		Puzzle puzzle = QuestManager.GetPuzzle(chapterNumber);
		if (puzzle.hasHelpFunction) {
			if (puzzle.IsTier1HelpActive ()) {
				GUIManager.tts.SayAdd (puzzle.tier1HelpText, "neutral", false);
			}
			if(puzzle.IsTier2HelpActive()){
				GUIManager.tts.SayAdd (puzzle.tier2HelpText, "neutral", false);
			}
		}
	}

	/// <summary>
	/// OnGui method.
	/// </summary>
	override public void OnDisplay()
	{
		// Inherit location & puzzle information for current chapter.
		Location location = QuestManager.GetLocation(chapterNumber);
		Puzzle puzzle = QuestManager.GetPuzzle(chapterNumber);

		// Inherit screen size.
		float width = GUIManager.virtualWidth;
		float height = GUIManager.virtualHeight;
		
		float halfWidth = 0.5f * width;
		
		GUI.Label(new Rect(0.05f*halfWidth, 0.15f*height, 1, 1), chapterTitle, GUIManager.boldLabelStyle);
		
		if (location.IsDistanceHelpActive() || location.IsDirectionHelpActive())
		{
			// Calculate distance & direction
			Coordinates here = Sensors.Location();
			Coordinates there = QuestManager.GetLocation(chapterNumber).GetCoordinates();
			float bearing = here.BearingTo(there);
			Distance distance = here.DistanceTo(there);
			
			if (location.IsDistanceHelpActive())
			{
				// Display distance
				string distanceString = "";
				
				if (!distance.IsValid())  // No GPS
				{
					distanceString = noSignal;
				}
				else
				{
					distanceString = string.Format("{0} away.", distance.ToString());
				}
				Rect rect = GUILayoutUtility.GetRect(new GUIContent(distanceString), GUIManager.headerStyle);
				rect.x = width/4 - rect.width/2;
				rect.y = 0.7f*height;
				GUI.Label(rect, distanceString, GUIManager.headerStyle);
			}
			
			if (location.IsDirectionHelpActive())
			{
				if (distance.IsValid())
				{
					// Display help arrow
					Quaternion from = arrowRotation;
					Quaternion to = Sensors.Bearing(bearing);
					arrowRotation = Quaternion.Slerp(from, to, 2*Time.deltaTime);
					GUIManager.DrawArrowLeft(arrowRotation.eulerAngles.z);
				}
			}
		}
		
		if (puzzle.hasHelpFunction)
		{
			string tier1HelpText = puzzle.tier1HelpText;
			string tier2HelpText = puzzle.tier2HelpText;
			
			// Right page
			GUIManager.GroupRightPageExtended(delegate(float w, float h) {
				if (puzzle.IsTier1HelpActive())
				{
					GUI.Label(new Rect(0, h/11, w, h), tier1HelpText, GUIManager.multilineLabelStyle);
				}
				
				if (puzzle.IsTier2HelpActive())
				{
					GUI.Label(new Rect(0, 6*h/11, w, h), tier2HelpText, GUIManager.multilineLabelStyle);
				}
			});
		}

		// Draw back button to chapter state.
		GUIManager.BackButton(delegate {
			GameStateManager.ChangeTo(new ChapterState());
		});

		// If help available, draw buttons to access both help functions.
		if (!location.IsDirectionHelpActive())
		{
			GUIManager.BottomLeftButtonLarge("     Help with finding location", delegate {
				if (puzzle.IsSeen())
				{
					noHelpPromptShowing = true;
					hasPopover = true;
				}
				else
				{
					helpRequestPromptShowing = true;
					hasPopover = true;
				}
			});
		}
		
		if (!puzzle.IsSolved() && puzzle.hasHelpFunction)
		{
			GUIManager.BottomRightButtonLarge("     Help with solving puzzle", delegate {
				if (puzzle.IsSeen())
				{
					helpRequestPromptShowing = true;
					hasPopover = true;
				}
				else
				{
					noHelpPromptShowing = true;
					hasPopover = true;
				}
			});
		}
	}

	/// <summary>
	/// OnGUI method to draw on the popover layer.
	/// </summary>
	override public void OnPopoverDisplay()
	{
		// Inherit location & puzzle information for current chapter.
		Puzzle puzzle = QuestManager.GetPuzzle(chapterNumber);
		Location location = QuestManager.GetLocation(chapterNumber);

		if (noHelpPromptShowing)
		{
			string text = "";
			if (puzzle.IsSeen())
			{
				text = "You already found the puzzle.\nI can't help you find it anymore.";
				
			}
			else
			{
				text = "I won't give you any hints on\nhow to solve the puzzle\nuntil you find it.";
			}
			
			GUIManager.PopoverOneButton(text, "OK", delegate {
				noHelpPromptShowing = false;
				hasPopover = false;
			});
		}

		if (helpRequestPromptShowing)
		{
			string text = "";
			GUIManager.OnClick onClickLeft;
			GUIManager.OnClick onClickRight = delegate {
				helpRequestPromptShowing = false;
				hasPopover = false;
			};
			
			if (!puzzle.IsSeen())
			{
				if (!location.IsDistanceHelpActive())
				{
					text = "Would you like to know how far\nyou have to go?\nThis results in a point deduction.";
				}
				else if (!location.IsDirectionHelpActive())
				{
					text = "Would you like to know in which\ndirection you have to go?\nThis results in a point deduction.";
				}
				
				onClickLeft = delegate {
					helpRequestPromptShowing = false;
					hasPopover = false;
					location.Help();
				};
			}
			else
			{
				if (!puzzle.IsTier1HelpActive())
				{
					text = "Would you like a hint?\nThis results in a point deduction.";
				}
				else if (!puzzle.IsTier2HelpActive())
				{
					text = "Would you like another hint?\nThis results in a point deduction.";
				}
				else
				{
					text = "Would you like to skip\nthe puzzle?\nThis will have severe consequences...";
				}
				
				onClickLeft = delegate {
					helpRequestPromptShowing = false;
					hasPopover = false;
					puzzle.Help();
					if (puzzle.IsSolved())
					{
						GameStateManager.ChangeTo(new CameraState());
					}
				};
			}
			
			GUIManager.PopoverTwoButtons(text, "Yes", onClickLeft, "No", onClickRight);
		}
	}
}