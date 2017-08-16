using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {
	public Player player;

	private void OnTriggerEnter2D(Collider2D collision) {
		player.AddLife();
		Destroy(this.gameObject);
	}
}
