using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using System.IO;

namespace UnityEngine
{
		public class TTS
		{
				private string tts_base_url = "http://fiware.tts.mivoq.it/";
				private string input_locale = "en";
				private Gender voice_gender = Gender.Neutral;
				private int voice_age = 35;
				private int voice_variant = 1;
				private string voice_name = null;

				public TTS ()
				{
				}

				public TTS (string url)
				{
						this.tts_base_url = url;
				}

				public enum Gender
				{
					Female,
					Male,
					Neutral
				};

				private static string GenderToString(Gender g)
				{
					switch (g) {
					case Gender.Female:
						return "female";
					case Gender.Male:
						return "male";
					case Gender.Neutral:
						return "neutral";
					}
					return null;
				}

				private WWW GetWWWClip (string text, string locale, string styleid)
				{
					if (this.tts_base_url == null)
						return null;
					return new WWW (this.tts_base_url
				    	            + "say?input[type]=TEXT&output[type]=AUDIO&output[format]=WAVE_FILE"
				        	        + "&input[locale]=" + WWW.EscapeURL(locale)
				 	          	    + "&input[content]=" + WWW.EscapeURL (text)
				            	    + "&voice[gender]=" + this.GenderToString(this.voice_gender)
				        			+ "&voice[age]=" + this.voice_age
					                + "&voice[name]=" + WWW.EscapeURL (this.voice_name)
					                + "&voice[variant]=" + this.voice_variant
					                + "&utterance[style]=" + WWW.EscapeURL(styleid)
					                + "&FAKEEXT=.wav"
					                );
				}
		
				public AudioClip GetAudioClip (string text, string locale, string styleid, bool threeD)
				{
					WWW r = this.GetWWWClip (text, locale, styleid);
					if (r == null) {
						return null;
					}
					return r.GetAudioClip (threeD);
				}

				public AudioClip GetAudioClip (string text, string styleid, bool threeD)
				{
					return this.GetAudioClip (text, this.input_locale, styleid, threeD);
				}

				private AudioSource internalaudio = null;
				public void SetAudioSource(AudioSource audio)
				{
					this.internalaudio = audio;
				}

				/* AudioClip queue */
				private Queue clipqueue = new Queue ();
				public void SayAdd(string text, string styleid, bool threeD){
					this.SayAdd (text, this.input_locale, styleid, threeD);
				}

				public void SayAdd(string text, string locale, string styleid, bool threeD){
					this.clipqueue.Enqueue (this.GetAudioClip (text, locale, styleid, false));
				}

				public void ClearSay() {
					this.clipqueue.Clear ();
					this.internalaudio.Stop ();
					this.internalaudio.clip = null;
				}

				/* Process the queue */
				private bool played = false;
				public void OnUpdate(){
					// play only if there is a source
					if (internalaudio == null)
						return;
					// update the clip if needed
					if (this.internalaudio.clip == null) {
						if(this.clipqueue.Count == 0)
							return;
						this.internalaudio.clip = (AudioClip) this.clipqueue.Dequeue();
						this.played = false;
					}
					// play something if there is something to play
					if (this.internalaudio.clip != null && this.internalaudio.clip.isReadyToPlay) {
						if ( !this.played && !this.internalaudio.isPlaying) {
											this.internalaudio.Play ();
											this.played = true;
						}
						else if( this.played && !this.internalaudio.isPlaying ) {
							if(this.clipqueue.Count == 0)
								return;
							this.internalaudio.clip = (AudioClip) this.clipqueue.Dequeue();
							this.played = false;
						}
					}
				}




				private void Start ()
				{
				}

				public void SetUrl (string url)
				{
						this.tts_base_url = url;
				}
				public string GetUrl ()
				{
					return this.tts_base_url;
				}
				public void SetDefaultLocale (string locale)
				{
					this.input_locale = locale;
				}
				public string GetDefaultLocale ()
				{
					return this.input_locale;
				}
				public void SetVoiceGender(Gender gender) {
					this.voice_gender = gender;
				}
				public Gender GetVoiceGender() {
					return this.voice_gender;
				}
				public void SetVoiceVariant(int variant) {
					if(variant >= 1)
						this.voice_variant = variant;
				}
				public int GetVoiceVariant() {
					return this.voice_variant;
				}
				public void SetVoiceAge(int age) {
					if(age >= 0)
						this.voice_age = age;
				}
				public int GetVoiceAge() {
					return this.voice_age;
				}







				/*public IEnumerator GetLocales ()
				{
						this.Start ();
						if (this.locales != null) {
								return this.locales.Keys.GetEnumerator ();
						} else
								return new string[0].GetEnumerator ();
				}
	
				public IEnumerator GetVoices (string locale)
				{
						this.Start ();
						if ((this.locales != null) && (this.locales.ContainsKey (locale))) {
								return ((ArrayList)this.locales [locale]).GetEnumerator ();
						} else
								return new string[0].GetEnumerator ();
				}
		
				public IEnumerator GetVoices ()
				{
						this.Start ();
						if (this.voices != null) {
								return this.voices.Keys.GetEnumerator ();
						} else {
								return new string[0].GetEnumerator ();
						}
				}

		/*
				private void Say (AudioSource audio, string text, string emotionid)
				{
						WWW r = this.GetWWWClip (text, emotionid);
						if (!string.IsNullOrEmpty (r.error))
								Debug.Log (r.error);
						audio.PlayOneShot (r.audioClip);
				}

				private Hashtable ParseVoices (string text)
				{
						//Debug.Log (text);
						if (string.IsNullOrEmpty (text))
								return null;
						string line;
						StringReader strReader = new StringReader (text);
						Hashtable tmp = new Hashtable ();
						Hashtable tmp2 = new Hashtable ();
						line = strReader.ReadLine ();
						while (!string.IsNullOrEmpty(line)) {
								string [] tokens = line.Split ();
								if (tokens.Length < 3) {
										Debug.Log (line);
								} else {
										Hashtable voice = new Hashtable{{"id", tokens[0]},{"locale", tokens[1]},{"gender", tokens[2]}};
										if (!tmp2.ContainsKey (tokens [1])) {
												ArrayList a = new ArrayList ();
												tmp2.Add (tokens [1], a);
										}
										((ArrayList)(tmp2 [(string)tokens [1]])).Add (voice);
										tmp.Add (tokens [0], voice);
								}
								line = strReader.ReadLine ();
						}
						this.voices = tmp;
						this.locales = tmp2;
						return tmp;
				}

				private void PopulateVoices ()
				{
						WWW r = new WWW (this.tts_base_url + "voices");
						float initTime = Time.realtimeSinceStartup;
						float timeout = 10.0f;
						while (!r.isDone) {
								if (timeout < 0.0f)
										return;
								if (Time.realtimeSinceStartup >= initTime + timeout)
										break;
						}
						if (!string.IsNullOrEmpty (r.error))
								Debug.Log (r.error);
						else
								ParseVoices (r.text);
				}
		
		XDocument speakxml = new XDocument ();
						XNamespace ns = @"http://www.w3.org/2001/10/synthesis";
						XElement root = new XElement (ns + "speak");
						root.SetAttributeValue (XNamespace.Xml + "lang", this.voice ["locale"]);
						root.SetAttributeValue ("version", "1.0");
						XElement paragraph = new XElement (ns + "p");
						XElement prosody = new XElement (ns + "prosody");
						Hashtable pa = null;
						if (!string.IsNullOrEmpty (emotionid) && emotions.ContainsKey (emotionid)) {
								pa = (Hashtable)emotions [emotionid];
						}
						if (pa != null) {
								foreach (DictionaryEntry entry in pa) {
										prosody.SetAttributeValue ((string)entry.Key, (string)entry.Value);
								}
								prosody.Add (text);
								paragraph.Add (prosody);
						} else {
								paragraph.Add (text);
						}
						root.Add (paragraph);
						speakxml.Add (root);
						//Debug.Log (speakxml.ToString ());*/
		}
}

