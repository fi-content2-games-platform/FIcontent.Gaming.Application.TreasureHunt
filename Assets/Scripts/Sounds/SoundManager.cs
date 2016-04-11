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

public class SoundManager : MonoBehaviour
{
    public static AudioClip pageSound;
    public static AudioClip slideSound;
    public static AudioClip combinationLockClickSound;
	public static AudioClip buttonSound;
	public static AudioClip safeDialClickSound;

	public static AudioClip soundButtonsPuzzleTemplate;
	public static AudioClip soundButtonsPuzzleSound1;
	public static AudioClip soundButtonsPuzzleSound2;
	public static AudioClip soundButtonsPuzzleSound3;

	private static SoundManager instance;
	public static SoundManager Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject go = new GameObject("SoundManager");
				instance = go.AddComponent<SoundManager>();
			}
			return instance;
		}
	}

    void Start()
    {
        pageSound = Resources.Load("Sounds/PageSound") as AudioClip;
        slideSound = Resources.Load("Sounds/SlideSound") as AudioClip;
		combinationLockClickSound = Resources.Load("Sounds/CombinationLockClickSound") as AudioClip;
		safeDialClickSound = Resources.Load("Sounds/SafeDialClickSound") as AudioClip;
		buttonSound = Resources.Load("Sounds/ButtonSound") as AudioClip;

        soundButtonsPuzzleTemplate = Resources.Load("Sounds/SoundButtonsPuzzleTemplate") as AudioClip;
        soundButtonsPuzzleSound1 = Resources.Load("Sounds/SoundButtonsPuzzleSound1") as AudioClip;
        soundButtonsPuzzleSound2 = Resources.Load("Sounds/SoundButtonsPuzzleSound2") as AudioClip;
        soundButtonsPuzzleSound3 = Resources.Load("Sounds/SoundButtonsPuzzleSound3") as AudioClip;
    }

    public void PlaySound(AudioClip sound)
    {
        Camera.main.audio.PlayOneShot(sound);
    }

	// http://answers.unity3d.com/questions/227593/invoke-a-function-when-an-audio-clip-is-done.html

	public delegate void AudioCallback();

	public void PlaySoundWithCallback(AudioClip sound, AudioCallback callback)
	{
		Camera.main.audio.PlayOneShot(sound);
		StartCoroutine(DelayedCallback(sound.length, callback));
	}

	private IEnumerator DelayedCallback(float time, AudioCallback callback)
	{
		yield return new WaitForSeconds(time);
		callback();
	}
}
