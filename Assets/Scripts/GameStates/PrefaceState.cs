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
/// Preface game state.
/// </summary>
public class PrefaceState : GameState
{
	// Preface is drawn differently the second time.
	private static bool firstTime;

	// Preface strings
	private static string title = "Preface";
	
	private static string storyLeft = 
		"Hello there!\n" +
		"What is it? Have you never seen an enchanted journal? Well, I guess our kind is quite rare these days.\n" +
		"Things were very different back when there was more magic in this world. But that was a long time ago, back when I was still with my former master.\n" +
		"He was quite the adventurer and he showed me many things.\n" +
		"I even know where he's hidden all his riches in the end...";
	
	private static string storyRight = 
		"Let's make a game out of it.\n" +
		"I'll give you hints on where to go and what to do. If you can find the treasure, you can keep it. I certainly have no use for it...\n\n" +
		"You'll need to collect four pieces of a treasure map which are hidden in the city. Each one is protected by some contraption or puzzle.\n\n" +
		"Humans usually can't see these kinds of mechanisms but as long as you travel with me, my magical aura surrounds you and helps you see.";

	/// <summary>
	/// Called on entering the game state.
	/// </summary>
	/// <param name="previousState">Previous state.</param>
	override public void OnEnter(GameState previousState)
	{
		// Depending on last state, determine if this is the first time.
		if (previousState is MenuState)
		{
			firstTime = true;
		}
		else if (previousState is IndexState)
		{
			firstTime = false;
		}
		GUIManager.tts.ClearSay ();
		GUIManager.tts.SayAdd (title, "happy", false);
		GUIManager.tts.SayAdd (storyLeft, "neutral", false);
		GUIManager.tts.SayAdd (storyRight, "neutral", false);
	}

	/// <summary>
	/// OnGui method.
	/// </summary>
	override public void OnDisplay()
	{
		// Left page.
		GUIManager.DrawGroupContents dgcLeft = delegate(float w, float h) {
			GUI.Label(new Rect(0, 0, 1, 1), title, GUIManager.boldLabelStyle);
			GUI.Label(new Rect(0, h/11, w, h), storyLeft, GUIManager.multilineLabelStyle);
		};

		// Right page.
		GUIManager.DrawGroupContents dgcRight = delegate(float w, float h) {
			GUI.Label(new Rect(0, 0, w, h), storyRight, GUIManager.multilineLabelStyle);
		};

		if (firstTime)
		{
			GUIManager.GroupLeftPageExtended(dgcLeft);
			GUIManager.GroupRightPage(dgcRight);
		}
		else
		{
			GUIManager.GroupLeftPage(dgcLeft);
			GUIManager.GroupRightPageExtended(dgcRight);
		}
		
		if (firstTime)
		{
			GUIManager.TopRightButton("Continue", delegate {
				GameStateManager.ChangeTo(new IndexState());
			});
		}
		else
		{
			GUIManager.BackButton(delegate {
				GameStateManager.ChangeTo(new IndexState());
			});
		}
	}
}