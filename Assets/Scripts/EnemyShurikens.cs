using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShurikens : MonoBehaviour {
	public Transform shurikenSpawn;
	public GameObject shurikenObj;
	public float shurikenSpeed = 20;
	public float shootDelay = 0.2f;

	private float accumulator = 100;

	SpriteRenderer spriteRenderer;
	Collider2D selfCollider2D;
	Player player;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		selfCollider2D = GetComponent<Collider2D>();
		player = GetComponent<Player>();
	}

	void Update() {
		accumulator += Time.deltaTime;

		if (accumulator > shootDelay) {
			if (player.GetLives() > 0) {
				Shoot();
				accumulator = 0.0f;
			}
		}
	}

	void Shoot() {
		Vector2 playerPos = GameObject.Find("Player").GetComponent<Transform>().position;
		Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
		Vector2 direction = playerPos - myPos;
		direction.Normalize();
		direction.y += 0.2f;
		GameObject shurikenInst = (GameObject)Instantiate(shurikenObj, myPos, Quaternion.identity);
		shurikenInst.GetComponent<Rigidbody2D>().velocity = direction * shurikenSpeed;

		if (direction.x < 0) {
			spriteRenderer.flipX = true;
		} else if (direction.x > 0) {
			spriteRenderer.flipX = false;
		}

		Physics2D.IgnoreCollision(selfCollider2D, shurikenInst.GetComponent<Collider2D>());
	}
}
