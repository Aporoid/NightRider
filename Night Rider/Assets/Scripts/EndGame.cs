using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
	PlayerController playerCont = new PlayerController();

	[SerializeField]
	private Text winText;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		playerCont.FullStop();
		winText.text = "You crossed the border safely!";
	}
}
