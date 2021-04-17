using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
	public string name;
	public float fadeTime;
	public AudioClip clip;

	public List<AudioSource> sources;

	[Range(0f, 1f)]
	public float volume = 0.7f;
	//[Range(0.5f, 1.5f)]
	//public float pitch = 1f;

	//[Range(0f, 0.5f)]
	//public float randomVolume = 0.1f;
	//[Range(0f, 0.5f)]
	//public float randomPitch = 0.1f;

	public bool loop = false;

	public IEnumerator AudioActionStop(AudioSource source)
	{
		float initialValue = 1f;
		float target = 0f;
		float duration = fadeTime;
		float timer = 0f;

		while (timer < duration)
		{
			source.volume = Mathf.Lerp(initialValue, target, timer / duration);
			timer += Time.deltaTime;
			yield return null;
		}

		source.Stop();
	}

	public IEnumerator AudioActionPause(AudioSource source)
	{
		float initialValue = 1f;
		float target = 0f;
		float duration = fadeTime / 2;
		float timer = 0f;

		while (timer < duration)
		{
			source.volume = Mathf.Lerp(initialValue, target, timer / duration);
			timer += Time.deltaTime;
			yield return null;
		}

		source.Pause();
	}

	public IEnumerator AudioActionPlay(AudioSource source)
	{
		source.Play();

		float initialValue = 0f;
		float target = 1f;
		float duration = fadeTime;
		float timer = 0f;

		while (timer < duration)
		{
			source.volume = Mathf.Lerp(initialValue, target, timer / duration);
			timer += Time.deltaTime;
			yield return null;
		}
	}

	public IEnumerator AudioActionUnpause(AudioSource source)
	{
		source.UnPause();

		float initialValue = 0f;
		float target = 1f;
		float duration = fadeTime / 2;
		float timer = 0f;

		while (timer < duration)
		{
			source.volume = Mathf.Lerp(initialValue, target, timer / duration);
			timer += Time.deltaTime;
			yield return null;
		}
	}
}

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public string startingMusic;

	[SerializeField]
	Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			if (instance != this)
			{
				Destroy(this.gameObject);
			}
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
	}

	void Start()
	{
		foreach(Sound sound in sounds)
        {
			foreach (AudioSource source in sound.sources)
            {
				source.clip = sound.clip;
            }
        }

		PlaySound(startingMusic);
	}

	public void PlaySound(string _name)
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			if (sounds[i].name == _name)
			{
				foreach (AudioSource source in sounds[i].sources)
                {
					StartCoroutine(sounds[i].AudioActionPlay(source));
				}
				return;
			}
		}

		// no sound with _name
		Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
	}

	public void PauseSound(string _name)
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			if (sounds[i].name == _name)
			{
				foreach (AudioSource source in sounds[i].sources)
				{
					StartCoroutine(sounds[i].AudioActionPause(source));
				}
				return;
			}
		}

		// no sound with _name
		Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
	}

	public void PauseSound()
	{
		foreach (AudioSource source in sounds[0].sources)
		{
			StartCoroutine(sounds[0].AudioActionPause(source));
		}
	}

	public void UnpauseSound(string _name)
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			if (sounds[i].name == _name)
			{
				foreach (AudioSource source in sounds[i].sources)
				{
					StartCoroutine(sounds[i].AudioActionUnpause(source));
				}
				return;
			}
		}

		// no sound with _name
		Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
	}

	public void UnpauseSound()
	{
		foreach (AudioSource source in sounds[0].sources)
		{
			StartCoroutine(sounds[0].AudioActionUnpause(source));
		}
	}

	public void StopAllSoundButFirst()
	{
		for (int i = 1; i < sounds.Length; i++) //Skips first sound, ambience
		{
			foreach (AudioSource source in sounds[i].sources)
			{
				StartCoroutine(sounds[i].AudioActionStop(source));
			}
		}
	}

	public void StopSound(string _name)
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			if (sounds[i].name == _name)
			{
				foreach (AudioSource source in sounds[i].sources)
				{
					StartCoroutine(sounds[i].AudioActionStop(source));
				}
				return;
			}
		}

		// no sound with _name
		Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
	}
}
