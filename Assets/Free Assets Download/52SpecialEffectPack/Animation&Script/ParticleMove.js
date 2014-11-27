
public var speed : float = 1.1f;

function Update () {
	transform.Translate(Vector3.back * speed);
}