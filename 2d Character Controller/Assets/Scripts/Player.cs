using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour {

float jumpHeight = 4;
float timeToJumpApex = 30f;
float accelerationTimeAirborne = 0.2f;
float accelerationTimeGrounded = 0.05f;
float moveSpeed = 6f;

float gravity;
float jumpVelocity;
float velocityXSmoothing;

//Once jump is pressed, decrease the effect of gravity until
//either jump is released, or the apex of the jump is reached.
bool jumpHeld = true;

Vector3 velocity;
Controller2D controller;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();

		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity)*timeToJumpApex;
	}

	void Update(){
		if (controller.collisions.above || controller.collisions.below){
			velocity.y = 0;
		}
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		
		if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below){
			velocity.y = jumpVelocity;
		}
		float targetVelocityX = input.x * moveSpeed;
		//Attain max horizontal velocity faster when airborne.
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,  controller.collisions.below?accelerationTimeGrounded:accelerationTimeAirborne);;
		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}
