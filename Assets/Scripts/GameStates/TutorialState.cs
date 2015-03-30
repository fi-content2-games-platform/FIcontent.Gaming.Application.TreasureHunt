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

/// <summary>
/// Tutorial game state.
/// </summary>
public class TutorialState : GameState
{
	// Tutorial strings
	private static string title = "Help";
	
	private static string tutLeft = 
		"This first puzzle is located on the front side of the building directly above the entrance.\n\n" +
		"Stand inside the yard with the gate close behind you and point the camera at the facade.";
	
	private static string tutRight = 
		"The game should be played by day with nice weather and good lighting conditions for the image recognition to work best.\n\n" +
		"Back or front light might be really bad for the recognition.\n\n" +
		"Try different angles or distances to your target if you have trouble. You can also try to lower the device and aim at the target again.";

	/// <summary>
	/// OnGui method.
	/// </summary>
	override public void OnDisplay()
	{
		// Left page.
		GUIManager.DrawGroupContents dgcLeft = delegate(float w, float h) {
			GUI.Label(new Rect(0, 0, 1, 1), title, GUIManager.boldLabelStyle);
			GUI.Label(new Rect(0, h/11, w, h), tutLeft, GUIManager.multilineLabelStyle);
		};
		
		// Right page.
		GUIManager.DrawGroupContents dgcRight = delegate(float w, float h) {
			GUI.Label(new Rect(0, 0, w, h), tutRight, GUIManager.multilineLabelStyle);
		};

		GUIManager.GroupLeftPage(dgcLeft);
		GUIManager.GroupRightPageExtended(dgcRight);
		
		GUIManager.BackButton(delegate {
			GameStateManager.ChangeTo(new PrologueState());
		});
	}
}