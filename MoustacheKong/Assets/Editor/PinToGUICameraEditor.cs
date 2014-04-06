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

using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(PinToGUICamera))]
public class PinToGUICameraEditor : Editor {
	public override void OnInspectorGUI () {
		PinToGUICamera pin = (PinToGUICamera)target;
		
		EditorGUIUtility.LookLikeInspector();
		EditorGUI.indentLevel = 0;

		CameraOrigin so = pin.origin;
		
		int soSelect = (
			so == CameraOrigin.LeftTop?0:
			so == CameraOrigin.CenterTop?1:
			so == CameraOrigin.RightTop?2:
			so == CameraOrigin.LeftMiddle?3:
			so == CameraOrigin.CenterMiddle?4:
			so == CameraOrigin.RightMiddle?5:
			so == CameraOrigin.LeftBottom?6:
			so == CameraOrigin.CenterBottom?7:
			so == CameraOrigin.RightBottom?8
		:0);
		string[] grid = {"LT", "CT", "RT", "LM", "CM", "RM", "LB", "CB", "RB"};
		CameraOrigin[] somap = {
			CameraOrigin.LeftTop,
			CameraOrigin.CenterTop,
			CameraOrigin.RightTop,
			CameraOrigin.LeftMiddle,
			CameraOrigin.CenterMiddle,
			CameraOrigin.RightMiddle,
			CameraOrigin.LeftBottom,
			CameraOrigin.CenterBottom,
			CameraOrigin.RightBottom
		};
		GUILayout.Label("Origin");
		GUILayout.Space(10);
		GUILayout.BeginHorizontal();
		GUILayout.Space(20);
		soSelect = GUILayout.SelectionGrid(soSelect, grid, 3,GUILayout.Width(100),GUILayout.Height(100));
		GUILayout.EndHorizontal();
		GUILayout.Space(10);
		
		if (somap[soSelect] != so) {
			GUI.changed = true;
			so = somap[soSelect];
		}
		
		if (GUI.changed)
		{
			Undo.RegisterUndo(pin, "PinToGUICamera Change");
			pin.origin = so;
			pin.FixTransform();
		}
	}
}
