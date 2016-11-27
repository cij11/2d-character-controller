using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour {

float gravity = -2f;
float moveSpeed = 6f;
Vector3 velocity;
Controller2D controller;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();
	}

	void Update(){
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		velocity.x = input.x * moveSpeed;
		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}
