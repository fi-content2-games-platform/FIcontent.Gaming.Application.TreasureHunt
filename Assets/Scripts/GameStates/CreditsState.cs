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
/// Credits game state.
/// </summary>
public class CreditsState : GameState
{
	/// <summary>
	/// OnGui method.
	/// </summary>
	override public void OnDisplay()
	{
		// Draw back button to menu state.
		GUIManager.BackButton(delegate {
			GameStateManager.ChangeTo(new MenuState());
		});

		// Draw left page of credits.
		GUIManager.GroupLeftPageExtended(delegate(float w, float h) {
			Rect journalTitleRect = GUILayoutUtility.GetRect(new GUIContent("Credits"), GUIManager.headerStyle);
			journalTitleRect.x = (w - journalTitleRect.width)/2;
			journalTitleRect.y = 0;
			
			GUI.Label(journalTitleRect, "Credits", GUIManager.headerStyle);
			
			GUI.Label(new Rect(0, h*0.2f, 1, 1), "Tizian Zeltner", GUIManager.boldLabelStyle);
			
			GUI.Label(new Rect(0, h*0.4f, 1, 1), "Fabio Zünd", GUIManager.boldLabelStyle);
			
			GUI.Label(new Rect(0, h*0.6f, 1, 1), "Marcel Lancelle", GUIManager.boldLabelStyle);
			
			GUI.Label(new Rect(0, h*0.8f, 1, 1), "Alessia Marra", GUIManager.boldLabelStyle);
			
			GUI.Label(new Rect(100, h*0.3f, 1, 1), "Programming, story, GUI design", GUIManager.labelStyle);
			GUI.Label(new Rect(100, h*0.5f, 1, 1), "Coordinator", GUIManager.labelStyle);
			GUI.Label(new Rect(100, h*0.7f, 1, 1), "Leaderboard server-side programming", GUIManager.labelStyle);
			GUI.Label(new Rect(100, h*0.9f, 1, 1), "Icon & 3D asset design", GUIManager.labelStyle);
		});

		// Draw right page of credits.
		GUIManager.GroupRightPageExtended(delegate(float w, float h) {
			GUI.Label(new Rect(0, h*0.0f, 1, 1), "Special thanks to", GUIManager.boldLabelStyle);
			GUI.Label(new Rect(100, h*0.1f, 1, 1), "Milan Bombsch", GUIManager.labelStyle);
			GUI.Label(new Rect(100, h*0.2f, 1, 1), "Isabelle Roesch", GUIManager.labelStyle);
		});
	}
}