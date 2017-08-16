using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShurikens : MonoBehaviour {
	public Transform shurikenSpawn;
	public GameObject shurikenObj;
	public float shurikenSpeed = 20;
	public float shootDelay = 0.2f;
	
	private float accumulator = 100;

	Collider2D selfCollider2D;

	void Start() {
		selfCollider2D = GetComponent<Collider2D>();
	}

	void Update() {
		accumulator += Time.deltaTime;

		if (Input.GetButtonDown("Fire1") && accumulator > shootDelay) {
			Shoot();
			accumulator = 0.0f;
		}
	}

	void Shoot() {
		Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
		Vector2 direction = cursorInWorldPos - myPos;
		direction.Normalize();
		GameObject shurikenInst = (GameObject)Instantiate(shurikenObj, myPos, Quaternion.identity);
		shurikenInst.GetComponent<Rigidbody2D>().velocity = direction * shurikenSpeed;

		Physics2D.IgnoreCollision(selfCollider2D, shurikenInst.GetComponent<Collider2D>());
	}
}
