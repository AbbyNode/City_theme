using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenShooter : MonoBehaviour {
	public Transform shurikenSpawn;
	public GameObject shurikenObj;
	public float shurikenSpeed;
	public float shootDelay;

	Collider2D collider;
	Animator animator;

	void Start() {
		collider = GetComponent<Collider2D>();
		animator = GetComponent<Animator>();
	}

	void Update() {
		if (Input.GetButton("Fire1")) {
			Shoot();
			//			GameObject shurienInst = Instantiate(shuriken, shurikenSpawn.position, shurikenSpawn.rotation);
			//		Physics2D.IgnoreCollision(collider, shurienInst.GetComponent<Collider2D>());
		}
	}

	void Shoot() {
		Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
		Vector2 direction = cursorInWorldPos - myPos;
		direction.Normalize();
		GameObject shurikeInstn = (GameObject)Instantiate(shurikenObj, myPos, Quaternion.identity);
		shurikeInstn.GetComponent<Rigidbody2D>().velocity = direction * shurikenSpeed;

		Debug.Log("ok");
	}
}
