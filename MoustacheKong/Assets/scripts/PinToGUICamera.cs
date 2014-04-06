/*
 Copyright (C) 2013 Kelvin Nishikawa - Critical Bacon, Inc.
 
 Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 and associated documentation files (the "Software"), to deal in the Software without restriction,
 including without limitation the rights to use, copy, modify, merge, publish, distribute,
 sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
 is furnished to do so, subject to the following conditions:
 
    The above copyright notice and this permission notice shall be included
    in all copies or substantial portions of the Software.
    
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

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
