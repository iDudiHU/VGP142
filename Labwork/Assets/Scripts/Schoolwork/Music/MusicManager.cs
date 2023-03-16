using Schoolwork.Helpers;
using UnityEngine;

namespace Schoolwork.Music
{
	[DefaultExecutionOrder(-1)]
	public class MusicManager : PersistentSingleton<MusicManager>
	{
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private SoundtrackScriptableObject soundtrack;

		protected override void Awake()
		{
			base.Awake();
		}

		private void Start()
		{
			Debug.Log("Music Manager exist");
			soundtrack = Resources.Load<SoundtrackScriptableObject>("/ScriptableObjects/Soundtrack");
			if (soundtrack != null && soundtrack.tracks.Length > 0)
			{
				audioSource.clip = soundtrack.tracks[0];
				audioSource.Play();
			}
		}
	}
}
