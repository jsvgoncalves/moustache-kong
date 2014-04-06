using UnityEngine;
using System.Collections;

public enum CameraOrigin {
	Left			= 1 << 0,
	Center			= 1 << 1,
	Right			= 1 << 2,
	
	Bottom			= 1 << 4,
	Middle			= 1 << 5,
	Top				= 1 << 6,
	
	LeftBottom 		= Left | Bottom,
	LeftMiddle 		= Left | Middle,
	LeftTop 		= Left | Top,
	
	CenterBottom	= Center | Bottom,
	CenterMiddle	= Center | Middle,
	CenterTop		= Center | Top,
	
	RightBottom		= Right | Bottom,
	RightMiddle		= Right | Middle,
	RightTop		= Right | Top
}

[ExecuteInEditMode]
public class PinToGUICamera : MonoBehaviour {
	
	public CameraOrigin origin = CameraOrigin.CenterMiddle;
	
	public void FixTransform() {
		//Find Camera
		Transform t = transform.parent;
		Camera c = null;
		do {
			c = t.GetComponent<Camera>();
			t = t.parent;
		} while (c == null && t != null);
		if (c == null) return;
		
		float x = 0;
		float y = 0;
		if ((origin & CameraOrigin.Left) == CameraOrigin.Left){
			if (c.orthographic) {
				x = -c.orthographicSize*c.aspect/c.transform.localScale.x;
			}
		} else if ((origin & CameraOrigin.Center) == CameraOrigin.Center){
			//
		} else if ((origin & CameraOrigin.Right) == CameraOrigin.Right) {
			if (c.orthographic) {
				x = c.orthographicSize*c.aspect/c.transform.localScale.x;
			}
		}
		
		if ((origin & CameraOrigin.Bottom) == CameraOrigin.Bottom) {
			if (c.orthographic) {
				y = -c.orthographicSize/c.transform.localScale.y;
			}
			
		} else if ((origin & CameraOrigin.Middle) == CameraOrigin.Middle) {
			//
		} else if ((origin & CameraOrigin.Top) == CameraOrigin.Top) {
			if (c.orthographic) {
				y = c.orthographicSize/c.transform.localScale.y;
			}
		}
		transform.localPosition = new Vector3(x,y,transform.localPosition.z);
	}
	
	void Start() {
		FixTransform();	
	}
	
	void OnDrawGizmos() {
		FixTransform();	
	}
}
