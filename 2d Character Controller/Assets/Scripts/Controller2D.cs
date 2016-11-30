using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider))]
public class Controller2D : MonoBehaviour {

	public LayerMask collisionMask;
	const float skinWidth = 0.015f;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	float horizontalRaySpacing;
	float verticalRaySpacing;

	//Note: Gravity is local to the player. So, the direction of gravity is altered
	//by rotating the player. If the player is aligned with the world axis, gravity is straight
	//down. If the player is aligned so that their feet always point towards the center of a 
	//sphere, then gravity will point towards the center of that sphere.
	//Wall running and antigravity can be simulated by having the player oriented to that surface.

	//This implementation differs from Sebastian Lagues primarily in that:
	//1. Coordinates are taken with respect to the transform, rather than the world
	//2. This is a '2.5d' implementation. Eg, 3d coordinates and geometry is used throughout,
	//but confined to a 2d plane.
	//3. A rigid body component kinematic trigger is added to the character. This allows trigger
	//collision events to be sent, in addition to the raycast collision system.

	BoxCollider collider;
	RaycastOrigins raycastOrigins;
	public CollisionInfo collisions;

	// Use this for initialization
	void Start () {
		collider = GetComponent<BoxCollider>();
		CalculateRaySpacing();
	}

	void VerticalCollision(ref Vector3 velocity){
		//diection will be -1 or +1 depending on sign of velocity.y
		float directionY = Mathf.Sign(velocity.y);
		float rayLength = Mathf.Abs(velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++){
			Vector3 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += collider.transform.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit hit;
			bool isHit = Physics.Raycast(rayOrigin, Vector3.up *directionY, out hit, rayLength, collisionMask);


			Debug.DrawRay(rayOrigin, collider.transform.up * directionY * rayLength, Color.red);
			
			//if there is a hit, move the character only enough to come
			//into contact with the 
			if(isHit){
				velocity.y = (hit.distance-skinWidth) * directionY;
				rayLength = hit.distance;

				collisions.above = directionY == 1;
				collisions.below = directionY == -1;
			}
		}
	}

		void HorizontalCollision(ref Vector3 velocity){
		//diection will be -1 or +1 depending on sign of velocity.y
		float directionX = Mathf.Sign(velocity.x);
		float rayLength = Mathf.Abs(velocity.x) + skinWidth;

		for (int i = 0; i < horizontalRayCount; i++){
			Vector3 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += collider.transform.up * (horizontalRaySpacing * i);
			RaycastHit hit;
			bool isHit = Physics.Raycast(rayOrigin, Vector3.right *directionX, out hit, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, collider.transform.right * directionX * rayLength, Color.red);
			//if there is a hit, move the character only enough to come
			//into contact with the 
			if(isHit){
				velocity.x = (hit.distance-skinWidth) * directionX;
				rayLength = hit.distance;

				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}
		}
	}
	void UpdateRaycastOrigins(){
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector3(bounds.min.x, bounds.min.y, 0);
		raycastOrigins.bottomRight = new Vector3(bounds.max.x, bounds.min.y, 0);
		raycastOrigins.topLeft = new Vector3(bounds.min.x, bounds.max.y, 0);
		raycastOrigins.topRight = new Vector3(bounds.max.x, bounds.max.y, 0);
	}

	struct RaycastOrigins{
		public Vector3 topLeft, topRight;
		public Vector3 bottomLeft, bottomRight;
	}

	public struct CollisionInfo{
		public bool above, below;
		public bool left, right;

		public void Reset(){
			above = below = false;
			left = right = false;
		}
	}

	void CalculateRaySpacing(){
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount  = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}



	public void Move(Vector3 velocity){
		UpdateRaycastOrigins();
		collisions.Reset();

		if (velocity.y != 0){
			VerticalCollision(ref velocity);
		}
		if (velocity.x != 0){
			HorizontalCollision(ref velocity);
		}
		//note that this occurs in local space. Eg, if the player is rotated 45 degrees and is falling
		//''straight down'', they will fall on a 45 degree slope.
		transform.Translate(velocity);
	}
}
