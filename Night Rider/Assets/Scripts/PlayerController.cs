using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private Rigidbody2D rb2d;
	[SerializeField]
	private float speed = 7;
	[SerializeField]
	private float maxSpeed = 7;

	private float horizontalInput;
	private float timer;
	private float noiseRating = 0f;

	private void Update()
	{
		UpdateHorizontalInput();
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
		//transform.position += transform.forward * Time.deltaTime * speed;
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
			timer = 0;
			noiseRating++;
			Debug.Log("Going fast");
			Debug.Log("Sound + 1");
		}
		else if (Input.GetButtonUp("Jump"))
		{
			timer += Time.deltaTime;
			speed = 7;
			maxSpeed = 7;
			Debug.Log("Slowing down");
			if(timer > 1)
			{
				timer -= 1;
				noiseRating++;
				Debug.Log("Sound + 1");
			}
		}
	}

	private void AlertNoise()
	{
		if(noiseRating == 3)
		{
			speed = 0;
			maxSpeed = 0;
			Debug.Log("You got caught!");
		}
	}
}
