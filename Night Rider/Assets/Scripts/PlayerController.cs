using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private Rigidbody2D rb2d;
	[SerializeField]
	private float speed = 7;
	[SerializeField]
	private float maxSpeed = 7;

	public Text speedText;
	public Text noiseText;
	public Text TimerText;
	private AudioSource audioSource;
	private float horizontalInput;
	private float timer = 20;
	public float noiseRating = 0f;

	private void Start()
	{
		StartCoroutine("Countdown");
		Time.timeScale = 1;
		noiseText.text = "Noise made: 0 dB";
		speedText.text = "Speed: 45 MPH";
		TimerText.text = "";
		audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		UpdateHorizontalInput();
		TimerText.text = "Time: " + timer.ToString() + " Sec";
	}

	private void FixedUpdate()
	{
		Move();
		Thruster();
		AlertNoise();
	}

	private void UpdateHorizontalInput()
	{
		horizontalInput = Input.GetAxisRaw("Horizontal");
	}

	private void Move()
	{
		rb2d.AddForce(Vector2.right * horizontalInput * speed);
		Vector2 clampedVelocity = rb2d.velocity;
		clampedVelocity.x = Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed);
		rb2d.velocity = clampedVelocity;
	}

	private void Thruster()
	{
		if (Input.GetButtonDown("Jump"))
		{
			speed = 11;
			maxSpeed = 11;
			noiseRating++;
			audioSource.Play();
			speedText.text = "Speed: 75 MPH";
			noiseText.text = "Noise made: " + noiseRating.ToString() + " dB";
			Debug.Log("Sound + 1");
		}
		else if (Input.GetButtonUp("Jump"))
		{
			speed = 7;
			maxSpeed = 7;
			speedText.text = "Speed: 45 MPH";
			Debug.Log("Slowing down");
		}
	}

	private IEnumerator Countdown()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);
			timer--;
		}
	}

	private void AlertNoise()
	{
		if(noiseRating == 5 || timer == 0)
		{
			speed = 0;
			maxSpeed = 0;

			Debug.Log("You got caught!");
		}
	}

	public void FullStop()
	{
		speed = 0;
		maxSpeed = 0;
	}
}
