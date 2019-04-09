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
	[SerializeField]
	private Text winText;

	public Text speedText;
	public Text noiseText;
	public Text TimerText;

	private AudioSource audioSource;
	private float horizontalInput;
    private bool engineRev = false;
	private float timer = 20;
	public float noiseRating = 0f;
	private bool stoppingTime = false;
    private bool keepRunning = true;
	private bool endGame = false;

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

        if(noiseRating < 5)
        {
            StartCoroutine("Thruster");
        }
        else if (noiseRating >= 5)
        {
            AlertNoise();
        }
	}

	private void FixedUpdate()
	{
		Move();
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

	private IEnumerator Thruster()
	{
        if (Input.GetButtonDown("Jump"))
        {
            engineRev = true;
            audioSource.Play();
        }
        if (Input.GetButtonUp("Jump"))
        {
            engineRev = false;
        }
        while (engineRev == true)
        {
			speed = 11;
			maxSpeed = 11;
            noiseRating += 0.05f;
			speedText.text = "Speed: 75 MPH";
			noiseText.text = "Noise made: " + noiseRating.ToString() + " dB";
            yield return new WaitForSeconds(1);
            if(engineRev == false)
            {
    			speed = 7;
    			maxSpeed = 7;
    			speedText.text = "Speed: 45 MPH";
    			Debug.Log("Slowing down");
            }
        }
	}

	private IEnumerator Countdown()
	{
		while (keepRunning)
		{
			yield return new WaitForSeconds(1);
			timer--;
            if (timer <= 0)
            {
                keepRunning = false;
            }
		}
	}
	private void OnTriggerEnter2D(Collider2D collider)
	{
		StartCoroutine("WaitTime");
		endGame = true;
		Ending();
	}

	private IEnumerator WaitTime()
	{
		while (stoppingTime)
		{
			yield return new WaitForSeconds(1);
			stoppingTime = true;
			FreezeYAxis();
		}
	}
	private void Ending()
	{
		if (endGame == true)
		{
			winText.text = "You crossed the border safely! " + "\n" + "Press any key to quit.";
			if (Input.anyKey)
			{
				Application.Quit();
			}
		}
	}

	private void FreezeYAxis()
	{
		rb2d.constraints = RigidbodyConstraints2D.FreezePositionY;
		rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
	}

	private void AlertNoise()
	{
		if(noiseRating == 5f || keepRunning == false)
		{
			FreezeYAxis();
			winText.text = "You got captured! " + "\n" + "Press any key to surrender willingly...";
			if (Input.anyKey)
			{
				Application.Quit();
			}
		}
	}

}
