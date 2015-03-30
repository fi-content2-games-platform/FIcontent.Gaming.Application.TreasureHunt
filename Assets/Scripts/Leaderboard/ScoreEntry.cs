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
using System;
using System.Collections;
using System.Xml.Serialization;

[Serializable]
public class ScoreEntry
{
	[XmlElement]
    public string username;

	[XmlElement]
	public string highscore;

	[XmlElement]
	public string totalTime;

	[XmlElement]
	public string puzzleTime;

	[XmlElement]
	public string distance;

	[XmlElement]
	public string hints;

	[XmlElement]
	public string skips;

	public ScoreEntry()
	{
		this.username = "";
		this.highscore = "";
		this.totalTime = "";
		this.puzzleTime = "";
		this.distance = "";
		this.hints = "";
		this.skips = "";
	}

	public ScoreEntry(string username)
	{
		this.username = username;
	}
    
	public ScoreEntry(string username, string highscore, string totalTime, string puzzleTime, string distance, string hints, string skips)
    {
        this.username = username;
		this.highscore = highscore;
		this.totalTime = totalTime;
		this.puzzleTime = puzzleTime;
		this.distance = distance;
		this.hints = hints;
		this.skips = skips;
    }
}