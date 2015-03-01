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
/// Prologue game state.
/// </summary>
public class PrologueState : GameState
{
	private static string title = "Prologue";
	// No signal string
	private static string noSignal = "Waiting for GPS signal...";

	// Quaternion to keep track of arrow rotation for direction arrow.
	private static Quaternion arrowRotation = Quaternion.Euler(0, 0, 0);
	/// <summary>
	/// Called on entering the game state.
	/// </summary>
	/// <param name="previousState">Previous state.</param>
	override public void OnEnter(GameState previousState)
	{
		GUIManager.tts.ClearSay ();
		Location location = QuestManager.GetLocation(0);
		string chapterText = location.clueText;
		GUIManager.tts.SayAdd (title, "happy", false);
		GUIManager.tts.SayAdd (chapterText, "neutral", false);
	}
	
	override public void OnDisplay()
	{
		// Inherit location information.
		Location location = QuestManager.GetLocation(0);

		// Inherit screen size.
		float width = GUIManager.virtualWidth;
		float height = GUIManager.virtualHeight;

		// Draw left prologue page.
		GUIManager.GroupLeftPage(delegate(float w, float h) {
			string chapterText = location.clueText;
			
			GUI.Label(new Rect(0, 0, 1, 1), title, GUIManager.boldLabelStyle);
			GUI.Label(new Rect(0, h/11, w, h), chapterText, GUIManager.multilineLabelStyle);
		});
		
		// Point arrow to start location
		
		// Calculate distance & direction
		Coordinates here = Sensors.Location();
		Coordinates there = QuestManager.GetLocation(0).GetCoordinates();
		float bearing = here.BearingTo(there);
		Distance distance = here.DistanceTo(there);
		
		if (distance.IsValid())
		{
			Quaternion from = arrowRotation;
			Quaternion to = Sensors.Bearing(bearing);
			arrowRotation = Quaternion.Slerp(from, to, 2*Time.deltaTime);
			GUIManager.DrawArrowRight(arrowRotation.eulerAngles.z);
		}
		
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
		rect.x = width/2 + width/4 - rect.width/2;
		rect.y = 0.7f*height;
		GUI.Label(rect, distanceString, GUIManager.headerStyle);
		
		// Draw back button to index state.
		GUIManager.BackButton(delegate {
			GameStateManager.ChangeTo(new IndexState());
		});

		// Draw button to access camera state.
		GUIManager.TopRightButton("Camera", delegate {
			GameStateManager.ChangeTo(new CameraState());
		});

		// Help / Tutorial state accessible from here
		GUIManager.BottomRightButton("Help", delegate {
			GameStateManager.ChangeTo(new TutorialState());
		});
	}
}