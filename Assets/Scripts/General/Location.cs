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
using System.Collections.Generic;

/// <summary>
/// Defines a location in the game.
/// </summary>
public class Location : MonoBehaviour
{
	#region Fields
	
	// Editor Fields

	/// <summary>
	/// The coordinates of the location.
	/// </summary>
	public Coordinates coordinates;

	/// <summary>
	/// First page hint to find location from the last location.
	/// </summary>
	[Multiline]
	public string clueText;

	/// <summary>
	/// Second page hint to find location from the last location.
	/// </summary>
	[Multiline]
	public string clueText2;
	
	// Internal Fields

	/// <summary>
	/// The location number.
	/// </summary>
	[HideInInspector]
	public int locationNumber;

	// Useful to know which image target belongs to which location. 
	private List<string> imageTargetNames;
	// Used to deactivate components at locations which aren't unlocked.
	private List<Transform> puzzleComponents;

	// Whether location is unlocked, i.e. puzzle of last location solved.
	private bool isUnlocked;
	// Wheter location is visited, i.e. puzzle at location looked at.
	private bool isVisited;

	// Whether distance is displayed in help.
	private bool isDistanceHelpActive;
	// Whether direction is displayed in help.
	private bool isDirectionHelpActive;
	
	#endregion
	
	#region MonoBehaviour Methods

	/// <summary>
	/// MonoBehaviour Awake method.
	/// </summary>
	void Awake()
	{
		// Setup the two lists from the editor hierarchy.
		imageTargetNames = new List<string>();
		puzzleComponents = new List<Transform>();
		
		ImageTargetBehaviour[] itbs = GetComponentsInChildren<ImageTargetBehaviour>();
		
		// Look through all image targets...
		foreach (ImageTargetBehaviour itb in itbs)
		{
			// Add image target name to list.
			imageTargetNames.Add(itb.TrackableName);
			
			// All children of image targets belong to the puzzle components.
			Transform imageTargetTransform = itb.gameObject.transform;
			foreach (Transform puzzleComponent in imageTargetTransform)
			{
				puzzleComponents.Add(puzzleComponent);
			}
		}
		
		// Activate or deactivate puzzle components based on wheter this location is unlocked.
		foreach (Transform t in puzzleComponents)
		{
			t.gameObject.SetActive(isUnlocked);
		}
	}

	/// <summary>
	/// MonoBehaviour Update method.
	/// </summary>
	void Update()
	{
		// Only keep attached puzzle components active as long as location is unlocked.
		foreach (Transform t in puzzleComponents)
		{
			t.gameObject.SetActive(isUnlocked);
		}
	}
	
	#endregion
	
	#region Public Methods

	/// <summary>
	/// Unlocks the location.
	/// </summary>
	public void Unlock()
	{
		isUnlocked = true;
	}

	/// <summary>
	/// Marks location as visited.
	/// </summary>
	public void Visit()
	{
		if (!isVisited)
		{
			isVisited = true;
		}
	}

	/// <summary>
	/// Triggers the help function. Each time, more hints are unlocked.
	/// </summary>
	public void Help()
	{
		if (!isDirectionHelpActive)
		{
			GameManager.NeededHint();
			if (!isDistanceHelpActive)
			{
				isDistanceHelpActive = true;
			}
			else
			{
				isDirectionHelpActive = true;
			}
		}     
	}
	
	#endregion
	
	#region Getters

	/// <summary>
	/// Returns whether this location was visited at some point.
	/// </summary>
	/// <returns><c>true</c> if this location was visited; otherwise, <c>false</c>.</returns>
	public bool IsVisited()
	{
		return isVisited;
	}
	
	/// <summary>
	/// Returns the coordinates of the location.
	/// </summary>
	/// <returns>The coordinates.</returns>
	public Coordinates GetCoordinates()
	{
		return coordinates;
	}
	
	/// <summary>
	/// Returns a list of all image target names of this location.
	/// </summary>
	/// <returns>The image target names.</returns>
	public List<string> GetImageTargetNames()
	{
		return imageTargetNames;
	}
	
	/// <summary>
	/// Returns whether the distance help was unlocked. (Distance to the location.)
	/// </summary>
	/// <returns><c>true</c> if distance help active; otherwise, <c>false</c>.</returns>
	public bool IsDistanceHelpActive()
	{
		return isDistanceHelpActive;
	}
	
	/// <summary>
	/// Returns whether the direction help was unlocked. (Arrow pointing in the direction of the location.)
	/// </summary>
	/// <returns><c>true</c> if direction help active; otherwise, <c>false</c>.</returns>
	public bool IsDirectionHelpActive()
	{
		return isDirectionHelpActive;
	}
	
	#endregion
	
	#region Serialization

	/// <summary>
	/// Serialize the internal data to an object to save it.
	/// </summary>
	/// <returns>LocationData object.</returns>
	public LocationData SerializeToData()
	{
		LocationData ld = new LocationData();
		ld.isUnlocked = isUnlocked;
		ld.isVisited = isVisited;
		ld.isDirectionHelpActive = isDirectionHelpActive;
		ld.isDistanceHelpActive = isDistanceHelpActive;
		return ld;
	}

	/// <summary>
	/// Deserialize the internal data from an object.
	/// </summary>
	/// <param name="ld">LocationData object  containing the data.</param>
	public void DeserializeFromData(LocationData ld)
	{
		isUnlocked = ld.isUnlocked;
		isVisited = ld.isVisited;
		isDirectionHelpActive = ld.isDirectionHelpActive;
		isDistanceHelpActive = ld.isDistanceHelpActive;
	}
	
	#endregion
}