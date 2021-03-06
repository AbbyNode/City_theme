﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {
	public int initialLives = 1;

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	public float moveSpeed = 6;

	int lives;

	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	Controller2D controller;
	Animator animator;
	SpriteRenderer spriteRenderer;

	Vector2 directionalInput;
	bool wallSliding;
	int wallDirX;

	TextMesh livesUI;
	SpriteRenderer redTint;

	void Start() {
		controller = GetComponent<Controller2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		lives = initialLives;

		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

		livesUI = GameObject.Find("PlayerLives").GetComponent<TextMesh>();
		redTint = GameObject.Find("RedTint").GetComponent<SpriteRenderer>();
	}

	void Update() {
		CalculateVelocity();
		HandleWallSliding();

		Vector3 velocityDelta = velocity * Time.deltaTime;
		controller.Move(velocityDelta, directionalInput);

		animator.SetBool("Grounded", controller.collisions.below);
		animator.SetBool("Running", (velocityDelta.x < -0.1 || velocityDelta.x > 0.1));
		animator.SetBool("Jumping", (velocityDelta.y > 0));
		animator.SetBool("Falling", (velocityDelta.y < -0.1));

		bool touchingWall = (controller.collisions.left || controller.collisions.right);
		animator.SetBool("TouchingWall", touchingWall);

		if (velocity.x < 0) {
			spriteRenderer.flipX = true;
		} else if (velocity.x > 0) {
			spriteRenderer.flipX = false;
		}

		if (controller.collisions.above || controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			} else {
				velocity.y = 0;
			}
		}
	}

	public void SetDirectionalInput(Vector2 input) {
		directionalInput = input;
	}

	public void OnJumpInputDown() {
		if (wallSliding) {
			if (wallDirX == directionalInput.x) {
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			} else if (directionalInput.x == 0) {
				velocity.x = -wallDirX * wallJumpOff.x;
				velocity.y = wallJumpOff.y;
			} else {
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
		}
		if (controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) { // not jumping against max slope
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			} else {
				velocity.y = maxJumpVelocity;
			}
		}
	}

	public void OnJumpInputUp() {
		if (velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
	}

	public void OnDuckInputDown() {
		animator.SetBool("IsDucking", true);
	}

	public void OnDuckInputUp() {
		animator.SetBool("IsDucking", false);
	}


	void HandleWallSliding() {
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (directionalInput.x != wallDirX && directionalInput.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				} else {
					timeToWallUnstick = wallStickTime;
				}
			} else {
				timeToWallUnstick = wallStickTime;
			}
		}
	}

	void CalculateVelocity() {
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}

	void Hit() {
		animator.SetBool("IsHit", true);
		lives--;
		if (this.CompareTag("Player")) {
			livesUI.text = "Lives: " + lives;
			redTint.color = new Color(1, 1, 1, 1);
		}

		StartCoroutine(HitTimer(0.8f));
	}

	public void AddLife() {
		lives++;
		livesUI.text = "Lives: " + lives;
	}

	public int GetLives() {
		return this.lives;
	}

	public IEnumerator HitTimer(float seconds) {
		yield return new WaitForSeconds(seconds);

		animator.SetBool("IsHit", false);

		if (this.CompareTag("Player")) {
			redTint.color = new Color(1, 1, 1, 0);
		}

		if (lives <= 0) {
			if (this.CompareTag("Player")) {
				SceneManager.LoadScene("GameOverScene");
			} else if (this.CompareTag("Boss")) {
				SceneManager.LoadScene("WinScene");
			} else {
				Destroy(this.gameObject);
			}
		}
	}


	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Shuriken")) {
			Hit();
		}
	}
}
