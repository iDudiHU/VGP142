using Schoolwork.Helpers;
using UnityEngine;
using UnityEngine.Audio;

namespace Schoolwork.Music
{
	[DefaultExecutionOrder(-1)]
	public class MusicManager : Singelton<MusicManager>
	{
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private SoundtrackScriptableObject soundtrack;
		[SerializeField] private AudioMixer audioMixer;

		protected override void Awake()
		{
			base.Awake();
		}
		private void Setup()
		{
			soundtrack = Resources.Load<SoundtrackScriptableObject>("ScriptableObjects/Soundtrack");
			if (!audioSource)
			{
				audioSource = gameObject.AddComponent<AudioSource>();
			}
			if (!audioMixer)
			{
				audioMixer = Resources.Load<AudioMixer>("Mixer/BackgroundMusic");
				audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
			}
			if (soundtrack != null && soundtrack.tracks.Length > 0)
			{
				audioSource.clip = soundtrack.tracks[0];
			}
		}

		public void Play()
		{
			if (audioSource == null || soundtrack == null)
			{
				Setup();
			}
			audioSource.Play();
		}
	}
}
