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
/// Menu game state.
/// </summary>
public class MenuState : GameState
{
	// Menu strings
	private static string title = "AR\nTreasure\nHunt";

	private static string popupText = "Would you like to start a new game?\nYour current progress will be lost.";

	/// <summary>
	/// OnGui method.
	/// </summary>
	override public void OnDisplay()
	{
		// Inherit screen size.
		float width = GUIManager.virtualWidth;
		float height = GUIManager.virtualHeight;
		float halfWidth = width/2;

		// Draw title.
		GUIManager.CenteredLabel(title, halfWidth/2, height/8, GUIManager.titleStyle);

		// Draw version number.
		GUIManager.CenteredLabel(GameManager.version, halfWidth/2, 7*height/8);

		// Draw resume game button if a game was started.
		if (GameManager.GameStarted())
		{
			GUIManager.MenuButton(1, "Resume Game", delegate {
				GameStateManager.ChangeTo(new IndexState());
				if (GameManager.QuestStarted())
				{
					GameManager.StartQuest();
				}
			});
		}

		// Draw new game button.
		GUIManager.MenuButton(2, "New Game", delegate {
			if (!GameManager.GameStarted())
			{
				GameStateManager.ChangeTo(new PrefaceState());
				GameManager.StartGame();
			}
			else
			{
				hasPopover = true;
			}
		});

		// Draw leaderboard button.
		GUIManager.MenuButton(3, "Leaderboard", delegate {
			GameStateManager.ChangeTo(new LeaderboardState());
		});

		// Draw credits button.
		GUIManager.MenuButton(4, "Credits", delegate {
			GameStateManager.ChangeTo(new CreditsState());
		});
	}

	/// <summary>
	/// OnGUI method to draw on the popover layer.
	/// </summary>
	override public void OnPopoverDisplay()
	{
		// "Do you really want to start a new game?" prompt.

		GUIManager.OnClick leftClick = delegate {
			GameStateManager.ChangeTo(new PrefaceState());
			GameManager.StartGame();
			hasPopover = false;
		};
		
		GUIManager.OnClick rightClick = delegate {
			hasPopover = false;
		};
		
		GUIManager.PopoverTwoButtons(popupText, "Yes", leftClick, "No", rightClick);
	}
}