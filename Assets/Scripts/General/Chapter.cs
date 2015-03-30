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
/// Defines a chapter of the game.
/// </summary>
public class Chapter : MonoBehaviour
{
    #region Fields

    //Internal Fields

	/// <summary>
	/// The chapter number.
	/// </summary>
    [HideInInspector]
    public int chapterNumber;

	// Whether chapter is unlocked, i.e. puzzle of last location solved.
    private bool isUnlocked;
	// Whether chapter is read, i.e. opened once.
    private bool isRead;

    #endregion

    #region Public Methods
	
	/// <summary>
	/// Unlock the chapter so that it can be accessed from the index.
	/// </summary>
    public void Unlock()
    {
        isUnlocked = true;
    }
	
	/// <summary>
	/// Mark the chapter as read. This results in a graphical change in the index.
	/// </summary>
    public void Read()
    {
        isRead = true;
    }

    #endregion

    #region Getters
	
	/// <summary>
	/// Returns whether the chapters is unlocked and can be accessed.
	/// </summary>
	/// <returns><c>true</c> if this chapter is unlocked; otherwise, <c>false</c>.</returns>
    public bool IsUnlocked()
    {
        return isUnlocked;
    }
	
	/// <summary>
	/// Returns whether the chapter is read.
	/// </summary>
	/// <returns><c>true</c> if this chapter is read; otherwise, <c>false</c>.</returns>
    public bool IsRead()
    {
        return isRead;
    }

    #endregion

    #region Serialization
	
	/// <summary>
	/// Serialize the internal data to an object to save it.
	/// </summary>
	/// <returns>ChapterData object containing the data.</returns>
    public ChapterData SerializeToData()
    {
        ChapterData cd = new ChapterData();
        cd.isUnlocked = isUnlocked;
        cd.isRead = isRead;
        return cd;
    }

	/// <summary>
	/// Deserialize the internal data from an object.
	/// </summary>
	/// <param name="cd">ChapterData object.</param>
    public void DeserializeFromData(ChapterData cd)
    {
        isUnlocked = cd.isUnlocked;
        isRead = cd.isRead;
    }

    #endregion
}