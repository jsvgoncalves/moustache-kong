var uvAnimationTileX = 4;
var uvAnimationTileY = 1;
var framesPerSecond = 10.0;
var jumpTexture: Texture2D;
var stopTexture: Texture2D;
var runTexture: Texture2D;

private var controller: CharacterController;
private var player : GameObject;
private var running = false;
 
function Start() {
	player = GameObject.FindGameObjectWithTag ("Player");
	controller = player.GetComponent (typeof(CharacterController));
}

function Update () {
 
 	if (!player.GetComponent("HeroScript").canJump) {
 		renderer.material.mainTexture = jumpTexture;
 		renderer.material.mainTextureOffset = Vector2(0, 0);
 		renderer.material.mainTextureScale = Vector2(1, 1);
 		running = false;
 	}
 	else if (player.GetComponent("HeroScript").isStop) {
 		renderer.material.mainTexture = stopTexture;
 		renderer.material.mainTextureOffset = Vector2(0, 0);
 		renderer.material.mainTextureScale = Vector2(1, 1);
 		running = false;
 	}
 	else {
 		// Calculate index
		var index : int = Time.time * framesPerSecond;
		// repeat when exhausting all frames
		index = index % (uvAnimationTileX * uvAnimationTileY);
 		
		// Size of every tile
		var size = Vector2 (1.0 / uvAnimationTileX, 1.0 / uvAnimationTileY);
	 
		// split into horizontal and vertical index
		var uIndex = index % uvAnimationTileX;
		var vIndex = index / uvAnimationTileX;
	 
		// build offset
		// v coordinate is the bottom of the image in opengl so we need to invert.
		var offset = Vector2 (uIndex * size.x, 1.0 - size.y - vIndex * size.y);
	 	
	 	if (!running) {
 			renderer.material.mainTexture = runTexture;
 			running = true;
 		}
	 	
		renderer.material.SetTextureOffset ("_MainTex", offset);
		renderer.material.SetTextureScale ("_MainTex", size);
		
	}
}