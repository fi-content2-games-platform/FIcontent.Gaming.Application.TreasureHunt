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
/// Game state. Allows game to be built like a FSM.
/// </summary>
public class GameState
{
	#region Fields

	/// <summary>
	/// GUI supports popovers. GUI behind it will be disabled.
	/// </summary>
	protected static bool hasPopover;

	#endregion

	#region Getters
	
	/// <summary>
	/// Returns whether a popover is currently active.
	/// </summary>
	/// <returns><c>true</c> if this instance has popover; otherwise, <c>false</c>.</returns>
	public bool HasPopover()
	{
		return hasPopover;
	}

	#endregion

	#region Optional To Implement
	
	/// <summary>
	/// Update method.
	/// </summary>
	virtual public void OnUpdate()
	{
	}
	
	/// <summary>
	/// OnGui method.
	/// </summary>
	virtual public void OnDisplay()
	{
	}

	/// <summary>
	/// OnGUI method to draw on the popover layer.
	/// </summary>
	virtual public void OnPopoverDisplay()
	{
	}
	
	/// <summary>
	/// Called on entering the game state.
	/// </summary>
	/// <param name="previousState">Previous state.</param>
    virtual public void OnEnter(GameState previousState)
	{
	}
	
	/// <summary>
	/// Called on exiting the game state.
	/// </summary>
	/// <param name="nextState">Next state.</param>
	virtual public void OnExit(GameState nextState)
	{
	}

	#endregion
}