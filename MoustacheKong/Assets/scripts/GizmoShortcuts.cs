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
using UnityEditor;
using System.Collections;

public class GizmoShortcuts {
 
	[MenuItem ("Tools/Select Main Camera #.")]
    static void SelectMainCamera () {
		Selection.activeTransform = Camera.mainCamera.transform;
    }
	
	[MenuItem ("Tools/Select Parent #+")]
    static void SelectParent () {
		if (Selection.activeTransform != null)
			Selection.activeTransform = Selection.activeTransform.parent;
    }
	
    [MenuItem ("Tools/Gizmo/Front View #1")]
    static void FrontView () {
		SceneView sv = GetSceneView();
		if (sv == null) return;
		sv.orthographic = true;
		sv.LookAtDirect(sv.pivot, Quaternion.LookRotation(Vector3.forward));
    }
 
 
    [MenuItem ("Tools/Gizmo/Side View #3")]
    static void SideView () {
		SceneView sv = GetSceneView();
		if (sv == null) return;
       	sv.orthographic = true;
       	sv.LookAtDirect(sv.pivot, Quaternion.LookRotation(Vector3.right));
    }
 
    [MenuItem ("Tools/Gizmo/Top View #7")]
    static void TopView () {
		SceneView sv = GetSceneView();
		if (sv == null) return;
       	sv.orthographic = true;
       	sv.LookAtDirect(sv.pivot, Quaternion.LookRotation(Vector3.down));
    }
 
    [MenuItem ("Tools/Gizmo/Perspective View #5")]
    static void PerspectiveView () {
		SceneView sv = GetSceneView();
		if (sv == null) return;
       	sv.orthographic = !GetSceneView().orthographic;
       	sv.LookAtDirect(sv.pivot, Quaternion.LookRotation(Vector3.forward + Vector3.right + Vector3.down));
    }
	
	[MenuItem ("Tools/Gizmo/Main Camera View #0")]
    static void MainCameraView () {
		SceneView sv = GetSceneView();
		if (sv == null) return;
		Camera cam = Camera.mainCamera;
		sv.orthographic = cam.orthographic;
       	sv.AlignViewToObject(cam.transform);
    }
 
    static SceneView GetSceneView() {
       SceneView activeSceneView = null;
 
       if (SceneView.lastActiveSceneView != null) {
         activeSceneView = SceneView.lastActiveSceneView;
       }
//       else if (SceneView.sceneViews.Count != 0){
//         activeSceneView = (SceneView.sceneViews[0] as SceneView);
//       }
 
       return activeSceneView;
    }
}