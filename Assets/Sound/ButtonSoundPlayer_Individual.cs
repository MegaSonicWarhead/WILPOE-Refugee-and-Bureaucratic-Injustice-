using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundPlayer_Individual : MonoBehaviour
{
    [System.Serializable]
    public class ButtonSound
    {
        public Button button;        // The UI button
        public AudioClip sound;      // The sound for this specific button
    }

    [Header("Assign individual button sounds")]
    public List<ButtonSound> buttonSounds = new List<ButtonSound>();

    [Header("Audio Source")]
    public AudioSource audioSource;

    private void Start()
    {
        if (audioSource == null)
            Debug.LogWarning("[ButtonSoundPlayer] AudioSource is missing!");

        foreach (var pair in buttonSounds)
        {
            if (pair.button != null)
            {
                pair.button.onClick.AddListener(() =>
                {
                    PlaySound(pair.sound);
                });
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
