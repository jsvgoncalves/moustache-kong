#pragma strict

var target : Transform;
var distance : float;
var distY : float = -1.5;
var distX : float = 5;
function Update(){
 
    transform.position.z = target.position.z -distance;
    transform.position.y = target.position.y - distY;
    transform.position.x = target.position.x - distX;
 
}