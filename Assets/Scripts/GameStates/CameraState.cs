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
/// Camera game state.
/// </summary>
public class CameraState : GameState
{
	// Number of puzzle
	private static int puzzleNumber;

	// Deal with the delayed fade in for popups.
	private static float alpha;
	private static float timeBeforePopup;

	private static bool alreadySolved;

	// Popup message strings
	private static string genericSolvedText = "Puzzle solved!\nThe next chapter is unlocked.";
	private static string firstMapPieceText = "Puzzle solved!\nYou found the first map piece\nand unlocked the next chapter.";
	private static string secondMapPieceText = "Puzzle solved!\nYou found the second map piece\nand unlocked the next chapter.";
	private static string thirdMapPieceText = "Puzzle solved!\nYou found the third map piece\nand unlocked the next chapter.";
	private static string fourthMapPieceText = "Puzzle solved!\nYou found the last map piece\nand unlocked the next chapter.";
	private static string mapPiecesPlacedText = "Map pieces placed!\nYou unlocked the final chapter.";
	private static string treasureFoundText = "You found the treasure!\nEpilogue unlocked.";

	// Message that tracking might be tricky.
	private static string trackingMessage = "Recognition may be tricky at times. Try different angles and distances if you have trouble.";

	/// <summary>
	/// Called on entering the game state.
	/// </summary>
	/// <param name="previousState">Previous state.</param>
	override public void OnEnter(GameState previousState)
	{
		GUIManager.tts.ClearSay ();
		// Activate camera.
		CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_DEFAULT);
		CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
		CameraDevice.Instance.Start();

		// Deactivate GUI background.
		GUIManager.bookBackground.enabled = false;

		// Inherit puzzle number from index state.
		puzzleNumber = IndexState.selectedChapterNumber;

		if (QuestManager.GetPuzzle(puzzleNumber).IsSolved())
		{
			alreadySolved = true;
		}
		else
		{
			alreadySolved = false;
		}
	} 

	/// <summary>
	/// Called on exiting the game state.
	/// </summary>
	/// <param name="nextState">Next state.</param>
	override public void OnExit(GameState nextState)
	{
		// Deactivate camera.
		CameraDevice.Instance.Stop();
		CameraDevice.Instance.Deinit();

		// Activate GUI background.
		GUIManager.bookBackground.enabled = true;
	}

	/// <summary>
	/// Update method.
	/// </summary>
	override public void OnUpdate()
	{
		if (QuestManager.GetPuzzle(puzzleNumber).IsSolved())
		{
			hasPopover = true;
		}
		// Delay popup by a second.
		if (hasPopover)
		{
			if (timeBeforePopup > 0)
			{
				timeBeforePopup -= Time.deltaTime;
			}
		}
		else
		{
			timeBeforePopup = 0.25f;
		}

		// Debug: Auto solve puzzle with cheat fuction.
		if (Input.GetKeyDown(KeyCode.C))
		{
			Puzzle p = QuestManager.GetPuzzle(puzzleNumber);
			p.Help();
		}
	}

	/// <summary>
	/// OnGui method.
	/// </summary>
	override public void OnDisplay()
	{
		// Inherit screen size.
		float width = GUIManager.virtualWidth;
		float height = GUIManager.virtualHeight;

		// Draw back button to prologue / chapter state.
		GUIManager.BackButton(delegate {

			if (puzzleNumber == 0)
			{
				GameStateManager.ChangeTo(new PrologueState());
			}
			else
			{
				GameStateManager.ChangeTo(new ChapterState());
			}
		});

		// Display a message if nothing is detected.
		if (!QuestManager.LookingAtAnyPuzzle())
		{
			GUIManager.CenteredLabel(trackingMessage, width/2, 7*height/8);
		}
	}

	/// <summary>
	/// OnGUI method to draw on the popover layer.
	/// </summary>
	override public void OnPopoverDisplay()
	{
		// Fade in popup.
		Color normalColor = GUI.color;
		Color myColor = GUI.color;
		
		if (timeBeforePopup < 0 || alreadySolved)
		{
			if (!Mathf.Approximately(alpha, 1) && !alreadySolved)
			{
				alpha = Mathf.MoveTowards(alpha, 1, 0.5f*Time.deltaTime);
				myColor.a = alpha;
				GUI.color = myColor;
			}


			// Set popup text.
			string text;
			if (puzzleNumber == 1)
			{
				text = firstMapPieceText;
			}
			else if (puzzleNumber == 2)
			{
				text = secondMapPieceText;
			}
			else if (puzzleNumber == 3)
			{
				text = thirdMapPieceText;
			}
			else if (puzzleNumber == 4)
			{
				text = fourthMapPieceText;
			}
			else if (puzzleNumber == 5)
			{
				text = mapPiecesPlacedText;
			}
			else if (puzzleNumber == 6)
			{
				text = treasureFoundText;
			}
			else
			{
				text = genericSolvedText;
			}

			// Draw popup with one button.
			GUIManager.PopoverOneButton(text, "Return to Journal Index", delegate {
				GameStateManager.ChangeTo(new IndexState());
				hasPopover = false;
			});

			// Display the map pieces if necessary.
			if (puzzleNumber < 5)
			{
				if (puzzleNumber > 0)
				{
					GUIManager.FirstMapPiece();
				}
				if (puzzleNumber > 1)
				{
					GUIManager.SecondMapPiece();
				}
				if (puzzleNumber > 2)
				{
					GUIManager.ThirdMapPiece();
				}
				if (puzzleNumber > 3)
				{
					GUIManager.FourthMapPiece();
				}
			}
		}
		else
		{
			alpha = 0;
			myColor.a = alpha;
			GUI.color = myColor;
		}
		
		GUI.color = normalColor;
	}
}