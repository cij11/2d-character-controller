using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

Controller2D controller;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();
	}
}
