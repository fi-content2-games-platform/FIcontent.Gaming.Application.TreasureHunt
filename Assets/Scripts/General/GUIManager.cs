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
/// GUI manager.
/// Contains help functions to draw stuff.
/// </summary>
public class GUIManager : MonoBehaviour
{
	#region Fields
	
	// GUIStyles
	public static GUIStyle titleStyle;
	public static GUIStyle headerStyle;
	public static GUIStyle labelStyle;
	public static GUIStyle centeredLabelStyle;
	public static GUIStyle largeCenteredLabelStyle;
	public static GUIStyle boldLabelStyle;
	public static GUIStyle multilineLabelStyle;
	public static GUIStyle boldMultilineLabelStyle;
	public static GUIStyle debugLabelStyle;
	public static GUIStyle noteGroup;
	
	public static GUIStyle rightGreenButtonStyle;
	public static GUIStyle rightRedButtonStyle;
	public static GUIStyle leftRedButtonStyle;
	public static GUIStyle drawnButtonStyle;
	
	// Virtual screen sizes.
	public static float virtualWidth;
	public static float virtualHeight;
	
	// Font
	private static Font noteFont;
	
	// Background texture
	public static GUITexture bookBackground;
	
	// 3D models
	public static GameObject treasureChest;
	
	// Arrow texture
	private static Texture2D arrowTexture;
	private static Texture2D compassArrowTexture;
	
	// Map pieces
	private static Texture2D mapPiece1;
	private static Texture2D mapPiece2;
	private static Texture2D mapPiece3;
	private static Texture2D mapPiece4;
	
	// GUI shadows
	private static Texture2D leftShadow;
	private static Texture2D rightShadow;
	private static Texture2D noteShadow;
	
	private static Texture2D mapPiece1Shadow;
	private static Texture2D mapPiece2Shadow;
	private static Texture2D mapPiece3Shadow;
	private static Texture2D mapPiece4Shadow;

	// GUI Styles initialized only once.
	public static bool init;
	
	#endregion
	
	#region MonoBehaviour Methods

	/// <summary>
	/// MonoBehaviour Start method.
	/// </summary>
	void Start()
	{
		// Setup screen sizes
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		virtualWidth = 1920.0f;
		virtualHeight = 1104.0f;
		
		// Instantiate 3D models.
		treasureChest = Instantiate(Resources.Load("Prefabs/TreasureChest")) as GameObject;
		treasureChest.name = "TreasureChest";
		treasureChest.SetActive(false);
		
		// Load font.
		noteFont = Resources.Load("Fonts/NoteFont") as Font;
		
		// Instantiate background texture.
		bookBackground = (new GameObject("GUIBookBackground")).AddComponent<GUITexture>();
		bookBackground.guiTexture.texture = Resources.Load ("Images/Backgrounds/BookBackground") as Texture2D;
		bookBackground.guiTexture.pixelInset = new Rect(-virtualWidth/2, -virtualHeight/2, virtualWidth, virtualHeight);
		bookBackground.transform.localPosition = new Vector3(0.5f, 0.5f, 0);
		bookBackground.transform.localScale = new Vector3(0, 0, 1);
		bookBackground.gameObject.layer = 8;
		
		// Load arrow texture.
		arrowTexture = Resources.Load("Images/Arrow") as Texture2D;
		compassArrowTexture = Resources.Load("Images/CompassArrow") as Texture2D;
		
		// Load map pieces.
		mapPiece1 = Resources.Load("Images/Map/MapPiece1") as Texture2D;
		mapPiece2 = Resources.Load("Images/Map/MapPiece2") as Texture2D;
		mapPiece3 = Resources.Load("Images/Map/MapPiece3") as Texture2D;
		mapPiece4 = Resources.Load("Images/Map/MapPiece4") as Texture2D;
		
		// Load shadows.
		leftShadow = Resources.Load("Images/Buttons/LeftRibbonShadow") as Texture2D;
		rightShadow = Resources.Load("Images/Buttons/RightRibbonShadow") as Texture2D;
		noteShadow = Resources.Load("Images/Backgrounds/NoteShadow") as Texture2D;
		
		mapPiece1Shadow = Resources.Load("Images/Map/MapPiece1Shadow") as Texture2D;
		mapPiece2Shadow = Resources.Load("Images/Map/MapPiece2Shadow") as Texture2D;
		mapPiece3Shadow = Resources.Load("Images/Map/MapPiece3Shadow") as Texture2D;
		mapPiece4Shadow = Resources.Load("Images/Map/MapPiece4Shadow") as Texture2D;
	}
	
	#endregion
	
	#region Public methods called from GameStateManager

	/// <summary>
	/// OnUpdate method called by GameStateManager.
	/// </summary>
	public static void OnUpdate()
	{
		// Draw the background texture according to the current screensize
		bookBackground.guiTexture.pixelInset = new Rect(-Screen.width/2, -Screen.height/2, Screen.width, Screen.height);
	}

	/// <summary>
	/// OnDisplay method called by GameStateManager.
	/// </summary>
	public static void OnDisplay()
	{
		// Scale the whole GUI according to the screen size
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / virtualWidth, Screen.height / virtualHeight, 1.0f));
	}
	
	#endregion
	
	#region GUI helper functions
	
	// Delegate for Buttons
	public delegate void OnClick();
	
	// Delegate for groups
	public delegate void DrawGroupContents(float width, float height);
	
	// Draw labels

	/// <summary>
	/// Draw a centered label around x, y coordinates with specified GUI style.
	/// </summary>
	/// <param name="str">String.</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="style">Style.</param>
	public static void CenteredLabel(string str, float x, float y, GUIStyle style)
	{
		Rect rect = GUILayoutUtility.GetRect(new GUIContent(str), style);
		rect.x = x - rect.width/2;
		rect.y = y;
		GUI.Label(rect, str, style);
	}

	/// <summary>
	/// Draw a centered label around x, y coordinates with default GUI style.
	/// </summary>
	/// <param name="str">String.</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public static void CenteredLabel(string str, float x, float y)
	{
		CenteredLabel(str, x, y, centeredLabelStyle);
	}

	/// <summary>
	/// Draws a page title.
	/// </summary>
	/// <param name="title">Title text.</param>
	public static void PageTitle(string title)
	{
		Rect titleRect = GUILayoutUtility.GetRect(new GUIContent(title), GUIManager.headerStyle);
		titleRect.x = virtualWidth/4 - titleRect.width/2;
		titleRect.y = virtualHeight/8;
		GUI.Label(titleRect, title, GUIManager.headerStyle);
	}
	
	// Draw arrow

	/// <summary>
	/// Draws arrow on left page.
	/// </summary>
	/// <param name="angle">Angle.</param>
	public static void DrawArrowLeft(float angle)
	{
		float arrowWidth = 3*arrowTexture.width/4;
		float arrowHeight = 3*arrowTexture.height/4;
		float arrowX = virtualWidth/4 - arrowWidth/2;
		float arrowY = (virtualHeight - arrowHeight) / 2;
		
		Vector2 pivot = new Vector2(Screen.width/4, Screen.height/2);
		Rect arrowRect = new Rect(arrowX, arrowY, arrowWidth, arrowHeight);
		
		DrawArrow(arrowRect, pivot, angle);
	}

	/// <summary>
	/// Draws arrow on right page.
	/// </summary>
	/// <param name="angle">Angle.</param>
	public static void DrawArrowRight(float angle)
	{
		float arrowWidth = 3*arrowTexture.width/4;
		float arrowHeight = 3*arrowTexture.height/4;
		float arrowX = virtualWidth/2 + virtualWidth/4 - arrowWidth/2;
		float arrowY = (virtualHeight - arrowHeight) / 2;
		
		Vector2 pivot = new Vector2(3*Screen.width/4, Screen.height/2);
		Rect arrowRect = new Rect(arrowX, arrowY, arrowWidth, arrowHeight);
		
		DrawArrow(arrowRect, pivot, angle);
	}

	// Internal draw arrow function.
	private static void DrawArrow(Rect rect, Vector2 pivot, float angle)
	{
		RotatedTexture(rect, arrowTexture, angle, pivot);
	}

	// Internal draw compass arrow function.
	private static void DrawCompassArrow(Rect rect, Vector2 pivot, float angle)
	{
		RotatedTexture(rect, compassArrowTexture, angle, pivot);
	}
	
	// Draw groups

	/// <summary>
	/// Draw a popover with one button.
	/// </summary>
	/// <param name="text">Popover text.</param>
	/// <param name="buttonText">Button text.</param>
	/// <param name="buttonClick">Button OnClick delegate.</param>
	public static void PopoverOneButton(string text, string buttonText, OnClick buttonClick)
	{
		GroupNote(delegate(float width, float height) {
			Rect textRect = GUILayoutUtility.GetRect(new GUIContent(text), GUIManager.headerStyle);
			textRect.x = width/2.0f - textRect.width/2.0f;
			textRect.y = height/6.0f;
			GUI.Label(textRect, text, GUIManager.headerStyle);
			
			Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(buttonText), GUIManager.drawnButtonStyle);
			buttonRect.width = width/2;
			buttonRect.height = height/8;
			buttonRect.x = (width - buttonRect.width)/2.0f;
			buttonRect.y = height * 0.7f;
			
			DrawnButton(buttonRect, buttonText, buttonClick);
		});
	}
	/// <summary>
	/// Draw a popover with two buttons.
	/// </summary>
	/// <param name="text">Popover text.</param>
	/// <param name="leftButtonText">Left button text.</param>
	/// <param name="leftButtonClick">Left button OnClick delegate.</param>
	/// <param name="rightButtonText">Right button text.</param>
	/// <param name="rightButtonClick">Right button OnClick delegate.</param>
	public static void PopoverTwoButtons(string text, string leftButtonText, OnClick leftButtonClick, string rightButtonText, OnClick rightButtonClick)
	{
		GroupNote(delegate(float width, float height) {
			Rect textRect = GUILayoutUtility.GetRect(new GUIContent(text), GUIManager.headerStyle);
			textRect.x = width/2.0f - textRect.width/2.0f;
			textRect.y = height/6.0f;
			GUI.Label(textRect, text, GUIManager.headerStyle);
			
			DrawnButton(new Rect(0.2f*width, 0.7f*height, width/4, height/8), leftButtonText, leftButtonClick);
			DrawnButton(new Rect(0.8f*width -width/4, 0.7f*height, width/4, height/8), rightButtonText, rightButtonClick);
		});
	}

	/// <summary>
	/// Draw a compass popover.
	/// </summary>
	/// <param name="angle">Angle.</param>
	/// <param name="buttonClick">Button OnClick delegate.</param>
	public static void PopoverCompass(float angle, OnClick buttonClick)
	{
		float noteWidth = virtualWidth * 0.5f;
		float noteHeight = virtualHeight * 0.9f;
		float noteX = virtualWidth * 0.25f;
		float noteY = virtualHeight * 0.05f;
		
		Rect note = new Rect(noteX, noteY, noteWidth, noteHeight);
		Rect shadow = new Rect(noteX+10, noteY+10, noteWidth, noteHeight);
		
		GUI.DrawTexture(shadow, GUIManager.noteShadow);
		GUI.BeginGroup(note, GUIManager.noteGroup);

		Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent("Back"), GUIManager.drawnButtonStyle);
		buttonRect.width = noteWidth/2;
		buttonRect.height = noteHeight/8;
		buttonRect.x = (noteWidth - buttonRect.width)/2.0f;
		buttonRect.y = noteHeight * 0.75f;
		DrawnButton(buttonRect, "Back", buttonClick);
		
		GUI.EndGroup();
		
		float arrowWidth = 3*arrowTexture.width/4;
		float arrowHeight = 3*arrowTexture.height/4;
		float arrowX = (virtualWidth - arrowWidth) / 2;
		float arrowY = (virtualHeight - arrowHeight) / 2 - virtualHeight/10;
		
		Vector2 pivot = new Vector2(Screen.width/2, Screen.height/2 - Screen.height/10);
		Rect arrowRect = new Rect(arrowX, arrowY, arrowWidth, arrowHeight);
		
		DrawCompassArrow(arrowRect, pivot, angle);
	}

	// Internal draw group note popover.
	private static void GroupNote(DrawGroupContents dgc)
	{
		float noteWidth = virtualWidth * 0.8f;
		float noteHeight = virtualHeight * 0.8f;
		float noteX = virtualWidth * 0.1f;
		float noteY = virtualHeight * 0.1f;
		
		Rect note = new Rect(noteX, noteY, noteWidth, noteHeight);
		Rect shadow = new Rect(noteX+10, noteY+10, noteWidth, noteHeight);
		
		GUI.DrawTexture(shadow, GUIManager.noteShadow);
		GUI.BeginGroup(note, GUIManager.noteGroup);
		dgc(note.width, note.height);
		GUI.EndGroup();
	}

	/// <summary>
	/// Draws a left page.
	/// </summary>
	/// <param name="dgc">DrawGroupContents delegate.</param>
	public static void GroupLeftPage(DrawGroupContents dgc)
	{
		float halfWidth = virtualWidth/2;
		Rect g = new Rect(0.05f*halfWidth, 0.15f*virtualHeight, 0.9f*halfWidth, 0.8f*virtualHeight);
		Group(g, dgc);
	}

	/// <summary>
	/// Draws a slightly bigger left page.
	/// </summary>
	/// <param name="dgc">DrawGroupContents delegate.</param>
	public static void GroupLeftPageExtended(DrawGroupContents dgc)
	{
		float halfWidth = virtualWidth/2;
		Rect g = new Rect(0.05f*halfWidth, 0.05f*virtualHeight, 0.9f*halfWidth, 0.9f*virtualHeight);
		Group(g, dgc);
	}

	/// <summary>
	/// Draws a right page.
	/// </summary>
	/// <param name="dgc">DrawGroupContents delegate.</param>
	public static void GroupRightPage(DrawGroupContents dgc)
	{
		float halfWidth = virtualWidth/2;
		Rect g = new Rect(1.05f*halfWidth, 0.15f*virtualHeight, 0.9f*halfWidth, 0.8f*virtualHeight);
		Group(g, dgc);
	}

	/// <summary>
	/// Draws a slightly bigger right page.
	/// </summary>
	/// <param name="dgc">DrawGroupContents delegate.</param>
	public static void GroupRightPageExtended(DrawGroupContents dgc)
	{
		float halfWidth = virtualWidth/2;
		Rect g = new Rect(1.05f*halfWidth, 0.05f*virtualHeight, 0.9f*halfWidth, 0.9f*virtualHeight);
		Group(g, dgc);
	}

	/// <summary>
	/// Draw a leaderboard page. (Different size)
	/// </summary>
	/// <param name="dgc">DrawGroupContents delegate.</param>
	public static void GroupLeaderboard(DrawGroupContents dgc)
	{
		float halfWidth = virtualWidth/2;
		Rect g = new Rect(1.05f*halfWidth, 0.05f*virtualHeight, 0.95f*halfWidth, 0.9f*virtualHeight);
		Group(g, dgc);
	}

	/// <summary>
	/// Draw group.
	/// </summary>
	/// <param name="groupRect">Group rect.</param>
	/// <param name="dgc">DrawGroupContents delegate.</param>
	public static void Group(Rect groupRect, DrawGroupContents dgc)
	{
		GUI.BeginGroup(groupRect);
		dgc(groupRect.width, groupRect.height);
		GUI.EndGroup();
	}

	/// <summary>
	/// Draw a rotated texture.
	/// </summary>
	/// <param name="rect">Rect.</param>
	/// <param name="texture">Texture.</param>
	/// <param name="angle">Angle.</param>
	/// <param name="pivot">Pivot point.</param>
	public static void RotatedTexture(Rect rect, Texture texture, float angle, Vector2 pivot)
	{
		GUIUtility.RotateAroundPivot(angle, pivot);
		GUI.DrawTexture(rect, texture, ScaleMode.ScaleToFit, true, 0);
		GUIUtility.RotateAroundPivot(-angle, pivot);
	}
	
	// Draw buttons

	/// <summary>
	/// Draw a back button.
	/// </summary>
	/// <param name="click">Button OnClick delegate.</param>
	public static void BackButton(OnClick click)
	{
		TopLeftButton("     Back", click);
	}

	/// <summary>
	/// Draw a button on the top left of the screen.
	/// </summary>
	/// <param name="text">Button text.</param>
	/// <param name="click">Button OnClick delegate.</param>
	public static void TopLeftButton(string text, OnClick click)
	{
		float w = virtualWidth/7.5f;
		float h = virtualHeight/11;
		float x = 0;
		float y = virtualHeight/22;
		
		Rect button = new Rect(x, y, w, h);
		Rect shadow = new Rect(x, y+10, w+10, h);
		if (GUI.enabled)
		{
			GUI.DrawTexture(shadow, leftShadow);
		}
		
		DrawButton(button, text, GUIManager.leftRedButtonStyle, click);
	}

	/// <summary>
	/// Draw a button on the top right of the screen.
	/// </summary>
	/// <param name="text">Button text.</param>
	/// <param name="click">Button OnClick delegate.</param>
	public static void TopRightButton(string text, OnClick click)
	{
		float w = virtualWidth/7.5f;
		float h = virtualHeight/11;
		float x = virtualWidth - w;
		float y = virtualHeight/22;
		
		Rect button = new Rect(x, y, w, h);
		Rect shadow = new Rect(x+10, y+10, w, h);
		
		if (GUI.enabled)
		{
			GUI.DrawTexture(shadow, rightShadow);
		}
		
		DrawButton(button, text, GUIManager.rightRedButtonStyle, click);
	}

	/// <summary>
	/// Draw a button on the bottom left of the screen.
	/// </summary>
	/// <param name="text">Button text.</param>
	/// <param name="click">Button OnClick delegate.</param>
	public static void BottomLeftButton(string text, OnClick click)
	{
		float w = virtualWidth/7.5f;
		float h = virtualHeight/11;
		float x = 0;
		float y = virtualHeight - h - virtualHeight/22;
		
		Rect button = new Rect(x, y, w, h);
		Rect shadow = new Rect(x, y+10, w+10, h);
		
		if (GUI.enabled)
		{
			GUI.DrawTexture(shadow, leftShadow);
		}
		
		DrawButton(button, text, GUIManager.leftRedButtonStyle, click);
	}

	/// <summary>
	/// Draw a button on the bottom right of the screen.
	/// </summary>
	/// <param name="text">Button text.</param>
	/// <param name="click">Button OnClick delegate.</param>
	public static void BottomRightButton(string text, OnClick click)
	{
		float w = virtualWidth/7.5f;
		float h = virtualHeight/11;
		float x = virtualWidth - w;
		float y = virtualHeight - h - virtualHeight/22;
		
		Rect button = new Rect(x, y, w, h);
		Rect shadow = new Rect(x+10, y+10, w, h);
		
		if (GUI.enabled)
		{
			GUI.DrawTexture(shadow, rightShadow);
		}
		
		DrawButton(button, text, GUIManager.rightRedButtonStyle, click);
	}

	/// <summary>
	/// Draw a bigger button on the bottom left of the screen.
	/// </summary>
	/// <param name="text">Button text.</param>
	/// <param name="click">Button OnClick delegate.</param>
	public static void BottomLeftButtonLarge(string text, OnClick click)
	{
		float w = virtualWidth/2.5f;
		float h = virtualHeight/11;
		float x = 0;
		float y = virtualHeight - h - virtualHeight/22;
		
		Rect button = new Rect(x, y, w, h);
		Rect shadow = new Rect(x, y+10, w+10, h);
		
		if (GUI.enabled)
		{
			GUI.DrawTexture(shadow, leftShadow);
		}
		
		DrawButton(button, text, GUIManager.leftRedButtonStyle, click);
	}

	/// <summary>
	/// Draw a bigger button on the bottom right of the screen.
	/// </summary>
	/// <param name="text">Button text.</param>
	/// <param name="click">Button OnClick delegate.</param>
	public static void BottomRightButtonLarge(string text, OnClick click)
	{
		float w = virtualWidth/2.5f;
		float h = virtualHeight/11;
		float x = virtualWidth - w;
		float y = virtualHeight - h - virtualHeight/22;
		
		Rect button = new Rect(x, y, w, h);
		Rect shadow = new Rect(x+10, y+10, w, h);
		
		if (GUI.enabled)
		{
			GUI.DrawTexture(shadow, rightShadow);
		}
		
		DrawButton(button, text, GUIManager.rightRedButtonStyle, click);
	}

	/// <summary>
	/// Draw a button in the popover note style.
	/// </summary>
	/// <param name="rect">Rect.</param>
	/// <param name="text">Button text.</param>
	/// <param name="click">Button OnClick delegate.</param>
	public static void DrawnButton(Rect rect, string text, OnClick click)
	{
		DrawButton(rect, text, GUIManager.drawnButtonStyle, click);
	}

	/// <summary>
	/// Draws a menu button.
	/// </summary>
	/// <param name="i">Position in menu.</param>
	/// <param name="text">Button text.</param>
	/// <param name="click">Button OnClick delegate.</param>
	public static void MenuButton(int i, string text, OnClick click)
	{
		float dy = (virtualHeight - 4f*150f)/5f;
		float x = virtualWidth - 800;
		float y = (i-1) * 150 + i * dy;
		float w = 800;
		float h = 150;
		
		Rect button = new Rect(x, y, w, h);
		Rect shadow = new Rect(x+10, y+10, w, h);
		
		if (GUI.enabled)
		{
			GUI.DrawTexture(shadow, rightShadow);
		}
		
		DrawButton(button, text, GUIManager.rightRedButtonStyle, click);
	}
	
	/// <summary>
	/// Draws a chapter button.
	/// </summary>
	/// <param name="i">Position in index.</param>
	/// <param name="text">Button text.</param>
	/// <param name="read">If set to <c>true</c> display as read.</param>
	/// <param name="click">Button OnClick delegate.</param>
	public static void ChapterButton(int i, string text, bool read, OnClick click)
	{
		GUIStyle buttonStyle;
		if (read)
		{
			buttonStyle = GUIManager.rightRedButtonStyle;
		}
		else
		{
			buttonStyle = GUIManager.rightGreenButtonStyle;
		}
		float w = 800;
		float h = 80;
		
		float offsetY = 75;
		float dy = (virtualHeight - 9*h - 2*offsetY)/8.0f;
		
		float x = virtualWidth - 800;
		float y = offsetY + i *(h+dy);
		
		
		Rect button = new Rect(x, y, w, h);
		Rect shadow = new Rect(x+10, y+10, w, h);
		
		if (GUI.enabled)
		{
			GUI.DrawTexture(shadow, rightShadow);
		}
		
		DrawButton(button, text, buttonStyle, click);
	}

	// Internal draw button function.
	private static void DrawButton(Rect rect, string text, GUIStyle style, OnClick click)
	{
		if (GUI.Button(rect, text, style))
		{
			SoundManager.Instance.PlaySound(SoundManager.pageSound);
			click();
		}
	}
	
	// Draw map pieces

	/// <summary>
	/// Draws map piece #1.
	/// </summary>
	public static void FirstMapPiece()
	{
		float imageWidth = mapPiece1.width*0.4f;
		float imageHeight = mapPiece1.height*0.4f;
		
		float noteX = virtualWidth * 0.1f;
		float noteY = virtualHeight * 0.1f;
		
		float imageX = noteX - 80;
		float imageY = noteY - 80;
		
		Rect mapPiece = new Rect(imageX, imageY, imageWidth, imageHeight);
		Rect mapPieceShadow = new Rect(imageX+10, imageY+10, imageWidth, imageHeight);
		
		GUIManager.RotatedTexture(mapPieceShadow, mapPiece1Shadow, -2.0f, new Vector2(noteX, noteY));
		GUIManager.RotatedTexture(mapPiece, mapPiece1, -2.0f, new Vector2(noteX, noteY));
	}

	/// <summary>
	/// Draws map piece #2.
	/// </summary>
	public static void SecondMapPiece()
	{
		float imageWidth = mapPiece1.width*0.4f;
		float imageHeight = mapPiece1.height*0.4f;
		
		float noteWidth = virtualWidth * 0.8f;
		float noteX = virtualWidth * 0.1f;
		float noteY = virtualHeight * 0.1f;
		
		float imageX = noteX + noteWidth - imageWidth + 80;
		float imageY = noteY - 80;
		
		Rect mapPiece = new Rect(imageX, imageY, imageWidth, imageHeight);
		Rect mapPieceShadow = new Rect(imageX+10, imageY+10, imageWidth, imageHeight);
		
		GUIManager.RotatedTexture(mapPieceShadow, mapPiece2Shadow, 2.0f, new Vector2(noteX + noteWidth, noteY));
		GUIManager.RotatedTexture(mapPiece, mapPiece2, 2.0f, new Vector2(noteX + noteWidth, noteY));
	}

	/// <summary>
	/// Draws map piece #3.
	/// </summary>
	public static void ThirdMapPiece()
	{
		float imageWidth = mapPiece1.width*0.4f;
		float imageHeight = mapPiece1.height*0.4f;
		
		float noteHeight = virtualHeight * 0.8f;
		float noteX = virtualWidth * 0.1f;
		float noteY = virtualHeight * 0.1f;
		
		float imageX = noteX - 80;
		float imageY = noteY + noteHeight - imageHeight + 80;
		
		Rect mapPiece = new Rect(imageX, imageY, imageWidth, imageHeight);
		Rect mapPieceShadow = new Rect(imageX+10, imageY+10, imageWidth, imageHeight);
		
		GUIManager.RotatedTexture(mapPieceShadow, mapPiece3Shadow, -3.0f, new Vector2(noteX, noteY + noteHeight));
		GUIManager.RotatedTexture(mapPiece, mapPiece3, -3.0f, new Vector2(noteX, noteY + noteHeight));
	}

	/// <summary>
	/// Draws map piece #4.
	/// </summary>
	public static void FourthMapPiece()
	{
		float imageWidth = mapPiece1.width*0.4f;
		float imageHeight = mapPiece1.height*0.4f;
		
		float noteWidth = virtualWidth * 0.8f;
		float noteHeight = virtualHeight * 0.8f;
		float noteX = virtualWidth * 0.1f;
		float noteY = virtualHeight * 0.1f;
		
		float imageX = noteX + noteWidth - imageWidth + 80;
		float imageY = noteY + noteHeight - imageHeight + 80;
		
		Rect mapPiece = new Rect(imageX, imageY, imageWidth, imageHeight);
		Rect mapPieceShadow = new Rect(imageX+10, imageY+10, imageWidth, imageHeight);
		
		GUIManager.RotatedTexture(mapPieceShadow, mapPiece4Shadow, 2.0f, new Vector2(noteX + noteWidth, noteY + noteHeight));
		GUIManager.RotatedTexture(mapPiece, mapPiece4, 2.0f, new Vector2(noteX + noteWidth, noteY + noteHeight));
	}
	
	#endregion
	
	#region GUIStyle setup code

	/// <summary>
	/// Initialize all GUI Styles.
	/// </summary>
	public static void Init()
	{
		GUI.skin.font = noteFont;
		
		titleStyle = new GUIStyle();
		titleStyle.fontSize = 150;
		titleStyle.fontStyle = FontStyle.Bold;
		titleStyle.normal.textColor = Color.black;
		titleStyle.alignment = TextAnchor.MiddleCenter;
		
		headerStyle = new GUIStyle();
		headerStyle.fontSize = 100;
		headerStyle.fontStyle = FontStyle.Bold;
		headerStyle.normal.textColor = Color.black;
		headerStyle.alignment = TextAnchor.MiddleCenter;
		
		labelStyle = new GUIStyle();
		labelStyle.fontSize = 50;
		labelStyle.normal.textColor = Color.black;
		
		centeredLabelStyle = new GUIStyle();
		centeredLabelStyle.fontSize = 50;
		centeredLabelStyle.normal.textColor = Color.black;
		centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
		
		boldLabelStyle = new GUIStyle();
		boldLabelStyle.fontSize = 60;
		boldLabelStyle.fontStyle = FontStyle.Bold;
		boldLabelStyle.normal.textColor = Color.black;
		
		multilineLabelStyle = new GUIStyle();
		multilineLabelStyle.fontSize = 50;
		multilineLabelStyle.normal.textColor = Color.black;
		multilineLabelStyle.wordWrap = true;
		
		boldMultilineLabelStyle = new GUIStyle();
		boldMultilineLabelStyle.fontSize = 50;
		boldMultilineLabelStyle.normal.textColor = Color.black;
		boldMultilineLabelStyle.wordWrap = true;
		boldMultilineLabelStyle.fontStyle = FontStyle.Bold;
		
		debugLabelStyle = new GUIStyle();
		debugLabelStyle.fontSize = 20;
		debugLabelStyle.normal.textColor = Color.white;
		
		largeCenteredLabelStyle = new GUIStyle();
		largeCenteredLabelStyle.fontSize = 80;
		largeCenteredLabelStyle.normal.textColor = Color.black;
		largeCenteredLabelStyle.wordWrap = true;
		largeCenteredLabelStyle.alignment = TextAnchor.MiddleCenter;
		
		noteGroup = new GUIStyle();
		noteGroup.normal.background = Resources.Load("Images/Backgrounds/Note") as Texture2D;
		
		rightRedButtonStyle = new GUIStyle("Button");
		rightRedButtonStyle.alignment = TextAnchor.MiddleCenter;
		rightRedButtonStyle.fontSize = 50;
		rightRedButtonStyle.fontStyle = FontStyle.Bold;
		rightRedButtonStyle.normal.textColor = Color.white;
		rightRedButtonStyle.normal.background = Resources.Load("Images/Buttons/RightRedRibbon") as Texture2D;
		rightRedButtonStyle.active.background = Resources.Load("Images/Buttons/RightRedRibbonHighlight") as Texture2D;
		rightRedButtonStyle.hover.background = Resources.Load("Images/Buttons/RightRedRibbonHighlight") as Texture2D;
		rightRedButtonStyle.border.left = 0;
		rightRedButtonStyle.border.right = 0;
		rightRedButtonStyle.border.top = 0;
		rightRedButtonStyle.border.bottom = 0;
		
		leftRedButtonStyle = new GUIStyle("Button");
		leftRedButtonStyle.alignment = TextAnchor.MiddleCenter;
		leftRedButtonStyle.fontSize = 50;
		leftRedButtonStyle.fontStyle = FontStyle.Bold;
		leftRedButtonStyle.normal.textColor = Color.white;
		leftRedButtonStyle.alignment = TextAnchor.MiddleLeft;
		leftRedButtonStyle.normal.background = Resources.Load("Images/Buttons/LeftRedRibbon") as Texture2D;
		leftRedButtonStyle.active.background = Resources.Load("Images/Buttons/LeftRedRibbonHighlight") as Texture2D;
		leftRedButtonStyle.hover.background = Resources.Load("Images/Buttons/LeftRedRibbonHighlight") as Texture2D;
		leftRedButtonStyle.border.left = 0;
		leftRedButtonStyle.border.right = 0;
		leftRedButtonStyle.border.top = 0;
		leftRedButtonStyle.border.bottom = 0;
		
		rightGreenButtonStyle = new GUIStyle("Button");
		rightGreenButtonStyle.alignment = TextAnchor.MiddleCenter;
		rightGreenButtonStyle.fontSize = 50;
		rightGreenButtonStyle.fontStyle = FontStyle.Bold;
		rightGreenButtonStyle.normal.textColor = Color.white;
		rightGreenButtonStyle.normal.background = Resources.Load("Images/Buttons/RightGreenRibbon") as Texture2D;
		rightGreenButtonStyle.active.background = Resources.Load("Images/Buttons/RightGreenRibbonHighlight") as Texture2D;
		rightGreenButtonStyle.hover.background = Resources.Load("Images/Buttons/RightGreenRibbonHighlight") as Texture2D;
		rightGreenButtonStyle.border.left = 0;
		rightGreenButtonStyle.border.right = 0;
		rightGreenButtonStyle.border.top = 0;
		rightGreenButtonStyle.border.bottom = 0;
		
		drawnButtonStyle = new GUIStyle("Button");
		drawnButtonStyle.alignment = TextAnchor.MiddleCenter;
		drawnButtonStyle.fontSize = 50;
		drawnButtonStyle.fontStyle = FontStyle.Bold;
		drawnButtonStyle.normal.textColor = Color.black;
		drawnButtonStyle.normal.background = Resources.Load("Images/Buttons/DrawnButton") as Texture2D;
		drawnButtonStyle.active.background = Resources.Load("Images/Buttons/DrawnButtonHighlight") as Texture2D;
		drawnButtonStyle.hover.background = Resources.Load("Images/Buttons/DrawnButtonHighlight") as Texture2D;
		drawnButtonStyle.border.left = 0;
		drawnButtonStyle.border.right = 0;
		drawnButtonStyle.border.top = 0;
		drawnButtonStyle.border.bottom = 0;
	}
	
	#endregion
}