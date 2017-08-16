using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillOnTouch : MonoBehaviour {
	private void OnCollisionEnter2D(Collision2D collision) {
		CheckForKills(collision.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		CheckForKills(collider.gameObject);
	}

	void CheckForKills(GameObject gameObject) {
		if (gameObject.CompareTag("Player")) {
			SceneManager.LoadScene("GameOverScene");
		}

		if (!gameObject.CompareTag("Obstacle")) {
			Destroy(gameObject);
		}
	}
}
