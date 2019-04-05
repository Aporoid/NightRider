using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
	PlayerController playerCont = new PlayerController();

	[SerializeField]
	private Text winText;

    private bool endGame = false;

    private void OnTriggerEnter2D(Collider2D collision)
	{
		playerCont.FullStop();
        endGame = true;
        TestEnding();
	}

    private void TestEnding()
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
}
