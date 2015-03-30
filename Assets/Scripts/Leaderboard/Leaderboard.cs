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
using System.Collections.Generic;
using System;
using System.IO;

public class Leaderboard : MonoBehaviour
{
	private string gameID = "ARTreasureHunt";
	private string url = "http://130.206.83.3:4567/lb";
	
	private List<ScoreEntry> leaderboard = new List<ScoreEntry>();
	
	private static Leaderboard instance;
	public static Leaderboard Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject go = new GameObject("Leaderboard");
				instance = go.AddComponent<Leaderboard>();
			}
			return instance;
		}
	}
	
	public List<ScoreEntry> GetLeaderboard()
	{
		return leaderboard;
	}
	
	public void UpdateLeaderboard()
	{
		leaderboard = new List<ScoreEntry>();
		StartCoroutine(UpdateLeaderboardInternal());
	}
	
	public void SubmitScore(ScoreEntry scoreEntry)
	{
		StartCoroutine(SubmitScoreInternal(scoreEntry));
	}
	
	private IEnumerator UpdateLeaderboardInternal()
	{
		string urlGET = string.Format("{0}/{1}/{2}", url, gameID, "rankedlist");
		
		WWW www = new WWW(urlGET);
		yield return www;
		yield return new WaitForSeconds(1f);
		
		List<ScoreEntry> lb = new List<ScoreEntry>();
		
		if (www.error == null)
		{
			// Debug.Log("WWW OK: " + www.text);
			
			JSONObject json = new JSONObject(www.text);
			
			foreach (JSONObject jo in json.list)
			{
				string username = jo.GetField("playerID").str;
				string highscore = jo.GetField("highscore").str;
				string totalTime = jo.GetField("totalTime").str;
				string puzzleTime = jo.GetField("puzzleTime").str;
				string distance = jo.GetField("distance").str;
				string hints = jo.GetField("hints").str;
				string skips = jo.GetField("skips").str;
				
				lb.Add(new ScoreEntry(username, highscore, totalTime, puzzleTime, distance, hints, skips));
			}
		}
		else
		{
			lb = null;
			// Debug.Log("WWW Error: " + www.error);
		}
		
		leaderboard = lb;
	}
	
	private IEnumerator SubmitScoreInternal(ScoreEntry scoreEntry)
	{
		EpilogueState.error = false;
		
		string playerID = scoreEntry.username.Replace(" ", "%20");
		
		string urlPOST = string.Format("{0}/{1}/{2}/{3}", url, gameID, playerID, "score");
		
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Content-Type", "application/json");
		
		JSONObject highscore = new JSONObject();
		highscore.AddField("name", "highscore");
		highscore.AddField("value", scoreEntry.highscore);
		
		JSONObject totalTime = new JSONObject();
		totalTime.AddField("name", "totalTime");
		totalTime.AddField("value", scoreEntry.totalTime);
		
		JSONObject puzzleTime = new JSONObject();
		puzzleTime.AddField("name", "puzzleTime");
		puzzleTime.AddField("value", scoreEntry.puzzleTime);
		
		JSONObject distance = new JSONObject();
		distance.AddField("name", "distance");
		distance.AddField("value", scoreEntry.distance);
		
		JSONObject hints = new JSONObject();
		hints.AddField("name", "hints");
		hints.AddField("value", scoreEntry.hints);
		
		JSONObject skips = new JSONObject();
		skips.AddField("name", "skips");
		skips.AddField("value", scoreEntry.skips);
		
		JSONObject entries = new JSONObject(JSONObject.Type.ARRAY);
		entries.Add(highscore);
		entries.Add(totalTime);
		entries.Add(puzzleTime);
		entries.Add(distance);
		entries.Add(hints);
		entries.Add(skips);
		
		JSONObject data = new JSONObject();
		data.AddField("scoreEntries", entries);
		
		byte[] pData = System.Text.Encoding.UTF8.GetBytes(data.ToString());
		
		Debug.Log(data.ToString());
		
		WWW www = new WWW(urlPOST, pData, headers);
		yield return www;
		yield return new WaitForSeconds(1f);
		
		if (www.error == null)
		{
			Debug.Log("WWW OK: " + www.text);
			GameManager.EnterLeaderboard();
			GameManager.leaderboardEntry = scoreEntry;
		}
		else
		{
			Debug.Log("WWW Error: " + www.error);
			EpilogueState.error = true;
		}
	}
}