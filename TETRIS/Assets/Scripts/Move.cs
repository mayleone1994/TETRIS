using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	public enum PieceType {None, d360, I, d90};
	public PieceType pieceType;
	public AudioClip rot, endFallPiece, cancelRot;
	[HideInInspector]
	public bool canMove = true;
	private GameManager manager;
	private Transform[] child;
	
	
	void Start () {
		manager = GameObject.FindObjectOfType<GameManager> ();
		manager.RandomPiece ();
		child = GetComponentsInChildren<Transform> ();
		foreach (Transform t in child)
			t.GetComponent<SpriteRenderer> ().material.color = manager.isPink ? Color.magenta : Color.yellow;
		manager.isPink = !manager.isPink;
		
		InvokeRepeating ("FallPiece", 0.3f, 0.3f);
	}
	
	
	void Update () {
		if (canMove)
			CheckKey ();
	}
	
	
	void FallPiece(){
		transform.position -= new Vector3 (0, 1, 0);
		if (!CheckTransform (Vector2.down)) {
			AudioSource.PlayClipAtPoint(endFallPiece, Camera.main.transform.position);
			transform.position += new Vector3 (0, 1, 0);
			foreach (var t in child) {
				t.parent = null;
				t.position = new Vector3(Mathf.Round(t.position.x),Mathf.Round(t.position.y), Mathf.Round(t.position.z));
				t.gameObject.layer = LayerMask.NameToLayer("Grid");
			}
			manager.score+= 10;
			manager.CheckGrid ();
			Destroy(gameObject.GetComponent<Move>());
		}
	}
	
	
	void FallDown(){
		CancelInvoke ("FallPiece");
		canMove = false;
		while (CheckTransform(Vector2.down)){
			transform.position-= new Vector3(0, 1, 0);
		}
		transform.position+= new Vector3(0, 1, 0);
		FallPiece ();
	}

	
	void CheckKey(){
		if (Input.GetKeyDown (KeyCode.RightArrow)) 
			transform.position += new Vector3 (1, 0, 0);
		if (!CheckTransform (Vector2.right))
			transform.position -= new Vector3 (1, 0, 0);
		
		if (Input.GetKeyDown (KeyCode.LeftArrow)) 
			transform.position -= new Vector3 (1, 0, 0);
		if (!CheckTransform (Vector2.left))
			transform.position += new Vector3 (1, 0, 0);
		
		if (Input.GetKeyDown (KeyCode.DownArrow))
			FallPiece ();
		
		if (Input.GetKeyDown (KeyCode.Space))
			FallDown ();
		
		
		if (Input.GetKeyDown (KeyCode.UpArrow) && pieceType != PieceType.None) {
			if (pieceType == PieceType.d360)
				Rotate360();
			if (pieceType == PieceType.I)
				RotateI();
			if (pieceType == PieceType.d90)
				Rotate90();
		}
	}
	
	
	bool CheckTransform(Vector2 dir, float dis = 0.5f){
		foreach (Transform t in child) {
			RaycastHit2D hit = Physics2D.Raycast(t.transform.position, dir, dis, LayerMask.GetMask("Grid"));
			if (hit.collider != null || t.position.x >= manager.rows || t.position.x <=-1 || t.position.y <= -1)
				return false;
		}
		
		return true;
	}
	
	void Rotate360(){
		transform.Rotate (0, 0, -90);
		if (!CheckTransform (Vector2.right) || !CheckTransform (Vector2.left) || !CheckTransform (Vector2.down) ||
		    !CheckTransform (Vector2.up)) {
			transform.Rotate (0, 0, 90);
			AudioSource.PlayClipAtPoint(cancelRot, Camera.main.transform.position);
		}
		else 
			AudioSource.PlayClipAtPoint(rot, Camera.main.transform.position);
	}
	
	void RotateI(){
		
		if (transform.rotation.z >= 0) {
			transform.rotation = Quaternion.Euler (0, 0, -90);
			if (!CheckTransform (Vector2.right, 2) || !CheckTransform (Vector2.left, 2)){
				AudioSource.PlayClipAtPoint(cancelRot, Camera.main.transform.position);
				transform.rotation = Quaternion.Euler (0, 0, 0);
				return;
			}
			AudioSource.PlayClipAtPoint(rot, Camera.main.transform.position);
		} else if (transform.rotation.z < 0) {
			transform.rotation = Quaternion.Euler (0, 0, 0);
			if (!CheckTransform (Vector2.up, 2) || !CheckTransform (Vector2.down, 2)){
				AudioSource.PlayClipAtPoint(cancelRot, Camera.main.transform.position);
				transform.rotation = Quaternion.Euler (0, 0, -90);
				return;
			}
			AudioSource.PlayClipAtPoint(rot, Camera.main.transform.position);
		}
	}
	
	void Rotate90(){
		if (transform.rotation.z >= 0) {
			transform.rotation = Quaternion.Euler (0, 0, -90);
			if (!CheckTransform (Vector2.down) || !CheckTransform (Vector2.up)){
				AudioSource.PlayClipAtPoint(cancelRot, Camera.main.transform.position);
				transform.rotation = Quaternion.Euler (0, 0, 0);
				return;
			}
			AudioSource.PlayClipAtPoint(rot, Camera.main.transform.position);
		} else if (transform.rotation.z < 0) {
			transform.rotation = Quaternion.Euler (0, 0, 0);
			if (!CheckTransform (Vector2.right) || !CheckTransform (Vector2.left, 1)){
				AudioSource.PlayClipAtPoint(cancelRot, Camera.main.transform.position);
				transform.rotation = Quaternion.Euler (0, 0, -90);
				return;
			}
			AudioSource.PlayClipAtPoint(rot, Camera.main.transform.position);
		}
	}
	
}
