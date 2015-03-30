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
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
public class GameData
{
	[XmlElement]
	public Coordinates lastLocation = null;
	
	[XmlElement]
	public Distance walkedDistance = new Distance(0.0f);
	
	[XmlElement]
	public Duration passedTotalTime = new Duration(0.0f);
	
	[XmlElement]
	public Duration passedPuzzleTime = new Duration(0.0f);
	
	[XmlElement]
	public int numberHintsNeeded = 0;
	
	[XmlElement]
	public int numberPuzzlesSkipped = 0;
	
	[XmlElement]
	public bool gameStarted = false;
	
	[XmlElement]
	public bool questStarted = false;
	
	[XmlElement]
	public bool enteredLeaderboard = false;
	
	[XmlElement]
	public ScoreEntry leaderboardEntry = new ScoreEntry();
}
