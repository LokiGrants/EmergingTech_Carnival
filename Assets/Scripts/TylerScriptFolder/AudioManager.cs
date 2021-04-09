using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public float speed;
	private Rigidbody rb;
	private int count;

	public AudioSource musicSource;
	public AudioSource sfxSource;
	public AudioClip winMusic;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		count = 0;
		SetCountText();
	}

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		rb.AddForce(movement * speed);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Pick Up"))
		{
			other.gameObject.SetActive(false);
			count = count + 1;
			SetCountText();

			sfxSource.pitch = Random.Range(0.9f, 1.9f);
			sfxSource.Play();
		}
	}

	void SetCountText()
	{
		if (count >= 12)
		{
			musicSource.clip = winMusic;
			musicSource.Play();
		}
	}

}
