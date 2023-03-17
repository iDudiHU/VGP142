using Schoolwork.Helpers;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Schoolwork.Music
{
	[DefaultExecutionOrder(-1)]
	public class MusicManager : Singelton<MusicManager>
	{
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private SoundtrackScriptableObject soundtrack;
		[SerializeField] private AudioMixer GameMixer;
		[SerializeField] private AudioMixer SFXMixer;

		protected override void Awake()
		{
			base.Awake();
		}
		private void Setup()
		{
			soundtrack = Resources.Load<SoundtrackScriptableObject>("ScriptableObjects/Soundtrack");
			if (!audioSource)
				audioSource = gameObject.AddComponent<AudioSource>();
			if (!GameMixer)
			{
				GameMixer = Resources.Load<AudioMixer>("Mixer/GameAudio");
				audioSource.outputAudioMixerGroup = GameMixer.FindMatchingGroups("Music")[0];
			}
			if (!SFXMixer)
				SFXMixer = Resources.Load<AudioMixer>("Mixer/SFX");
			if (soundtrack != null && soundtrack.tracks.Length > 0)
				audioSource.clip = soundtrack.tracks[0];
		}

		public void Play()
		{
			if (audioSource == null || soundtrack == null)
				Setup();
			audioSource.Play();
		}
		public void SwitchToIdle()
		{
			if (audioSource == null || soundtrack == null)
				StartCoroutine(FadeOutAndIn(soundtrack.tracks[1]));
		}
		public void SwitchToCombat()
		{
			if (audioSource == null || soundtrack == null)
				StartCoroutine(FadeOutAndIn(soundtrack.tracks[2]));
		}

		private IEnumerator FadeOutAndIn(AudioClip newTrack, float fadeTime = 1.0f)
		{
			float startingVolume = audioSource.volume;
			float targetVolume = 0.0f;
			while (audioSource.volume > 0)
			{
				audioSource.volume -= startingVolume * Time.deltaTime / fadeTime;
				yield return null;
			}

			audioSource.Stop();
			audioSource.clip = newTrack;
			audioSource.Play();

			targetVolume = startingVolume;
			while (audioSource.volume < targetVolume)
			{
				audioSource.volume += targetVolume * Time.deltaTime / fadeTime;
				yield return null;
			}
			audioSource.volume = startingVolume;
		}

	}
}
