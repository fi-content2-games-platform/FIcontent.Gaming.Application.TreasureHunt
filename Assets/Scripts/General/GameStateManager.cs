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
/// Game state manager.
/// Basically a FSM for the game states.
/// </summary>
public class GameStateManager : MonoBehaviour
{
	// Currently active game state.
	private static GameState currentState;

	/// <summary>
	/// Returns the currently active game state.
	/// </summary>
	/// <returns>The current state.</returns>
	public static GameState GetCurrentState()
	{
		return currentState;
	}

	/// <summary>
	/// Changes the current game state.
	/// </summary>
	/// <param name="nextState">Next state.</param>
	public static void ChangeTo(GameState nextState)
	{
		GameState previousState = currentState;
		previousState.OnExit(nextState);
		currentState = nextState;
		nextState.OnEnter(previousState);
	}

	/// <summary>
	/// MonoBehaviour Start method.
	/// </summary>
	void Start()
	{
		currentState = new MenuState();
		currentState.OnEnter(null);
	}

	/// <summary>
	/// MonoBehaviour Update method.
	/// </summary>
	void Update()
	{
		GUIManager.OnUpdate();
		currentState.OnUpdate();
	}

	/// <summary>
	/// MonoBehaviour OnGUI method.
	/// </summary>
	void OnGUI()
	{
		if (!GUIManager.init)
		{
			GUIManager.Init();
		}

		if (currentState.HasPopover())
		{
			GUI.enabled = false;
		}

		GUIManager.OnDisplay();
		currentState.OnDisplay();

		if (currentState.HasPopover())
		{
			GUI.enabled = true;
			currentState.OnPopoverDisplay();
		}
	}
}