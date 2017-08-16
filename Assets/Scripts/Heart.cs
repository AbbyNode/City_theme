using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {
	private void OnTriggerEnter2D(Collider2D collision) {
		GameObject.Find("Player").GetComponent<Player>().AddLife();
		Destroy(this.gameObject);
	}
}
