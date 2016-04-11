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

/// <summary>
/// Chapter game state.
/// </summary>
public class ChapterState : GameState
{
	// Chapter number
	private static int chapterNumber;
	// Chapter title
	private static string chapterTitle;

	// Quaternion to keep track of arrow rotation for compass popup.
	private static Quaternion arrowRotation = Quaternion.Euler(0, 0, 0);

	/// <summary>
	/// Called on entering the game state.
	/// </summary>
	/// <param name="previousState">Previous state.</param>
	override public void OnEnter(GameState previousState)
	{
		// Inherit chapter number & title from index state.
		chapterNumber = IndexState.selectedChapterNumber;
		chapterTitle = IndexState.selectedChapterTitle;
	}

	/// <summary>
	/// OnGui method.
	/// </summary>
	override public void OnDisplay()
	{
		// Inherit location & puzzle information for current chapter.
		Location location = QuestManager.GetLocation(chapterNumber);
		Puzzle puzzle = QuestManager.GetPuzzle(chapterNumber);

		// Display left page for this chapter.
		GUIManager.GroupLeftPage(delegate(float w, float h) {
			GUI.Label(new Rect(0, 0, 1, 1), chapterTitle, GUIManager.boldLabelStyle);
			GUI.Label(new Rect(0, h/11, w, h), location.clueText, GUIManager.multilineLabelStyle);
		});

		// Display right page for this chapter.
		GUIManager.GroupRightPageExtended(delegate(float w, float h) {
			GUI.Label(new Rect(0, 0, w, h), location.clueText2, GUIManager.multilineLabelStyle);
		});
		
		// Draw back button to index state.
		GUIManager.BackButton(delegate {
			GameStateManager.ChangeTo(new IndexState());
		});

		// Draw button to access the camera.
		GUIManager.TopRightButton("Camera", delegate {
			GameStateManager.ChangeTo(new CameraState());
		});

		// Draw button to access the compass.
		GUIManager.BottomLeftButton("  Compass", delegate {
			hasPopover = true;
		});

		// If help function available, draw button to access it.
		if (chapterNumber > 0 && !puzzle.IsSolved())
		{
			GUIManager.BottomRightButton("Help", delegate {
				GameStateManager.ChangeTo(new HelpState());
			});
		}
	}

	/// <summary>
	/// OnGUI method to draw on the popover layer.
	/// </summary>
	override public void OnPopoverDisplay()
	{
		// Point compass arrow to North.
		Quaternion from = arrowRotation;
		Quaternion to = Sensors.North();
		arrowRotation = Quaternion.Slerp(from, to, Time.deltaTime * 2);

		// Draw compass arrow.
		GUIManager.PopoverCompass(arrowRotation.eulerAngles.z, delegate {
			hasPopover = false;
		});
	}
}