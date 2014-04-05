#pragma strict

private var textDisplay : GUIText;

function OnMouseEnter() {
	guiText.material.color = Color.red; 
}
	
function OnMouseExit() {
	guiText.material.color = Color.white; 
}

function OnMouseDown() {
	Application.LoadLevel("Level1");
}