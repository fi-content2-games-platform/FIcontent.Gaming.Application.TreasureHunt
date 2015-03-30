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

public abstract class ButtonAction : MonoBehaviour
{
    #region Fields

    protected ButtonPress buttonScript;
    private bool performedAction;

    #endregion

    #region Mandatory To Implement

    // Action that is performed when the button is pressed.
    abstract protected void Action();

    #endregion

    #region Optional To Implement

    // Additional things that can happen in Update()
    virtual protected void OnUpdate()
    {
    }

    #endregion

    #region Internal

    void Awake()
    {
        buttonScript = GetComponentInChildren<ButtonPress>();
        if (buttonScript == null)
        {
            Debug.Log("ButtonPress component missing.");
        }
        performedAction = false;
    }

    void Update()
    {
        if (buttonScript.isPressed)
        {
            if (performedAction == false)
            {
                performedAction = true;
                Action();
            }
        }
        else
        {
            performedAction = false;
        }
        
        OnUpdate();
    }

	protected void UnpressButton()
	{
		buttonScript.isPressed = false;
	}

    #endregion
}