using UnityEngine;

[CreateAssetMenu(fileName = "New Soundtrack", menuName = "ScriptableObjects/Soundtrack")]
public class SoundtrackScriptableObject : ScriptableObject
{
    public AudioClip[] tracks;
}