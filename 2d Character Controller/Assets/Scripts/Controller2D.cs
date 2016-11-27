using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider))]
public class Controller2D : MonoBehaviour {

	const float skinWidth = 0.015f;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	float horizontalRaySpacing;
	float verticalRaySpacing;

	BoxCollider collider;
	RaycastOrigins raycastOrigins;

	// Use this for initialization
	void Start () {
		collider = GetComponent<BoxCollider>();
	}

	void Update(){
		UpdateRaycastOrigins();
		CalculateRaySpacing();

		for (int i = 0; i < verticalRayCount; i++){
			Debug.DrawRay(raycastOrigins.bottomLeft + Vector3.right * verticalRaySpacing * i, Vector3.up * -2, Color.red);
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

	void CalculateRaySpacing(){
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount  = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}
}
