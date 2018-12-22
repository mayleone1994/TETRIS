using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public int rows, lines;
	public GameObject[] piece;
	public GameObject grid;
	public Text textScore;
	public AudioClip line;
	[HideInInspector]
	public int score;
	[HideInInspector]
	public bool isPink = true;
	[HideInInspector]
	public GameOver over;
	private List<GameObject> pieces = new List<GameObject>();
	private GameObject pieceClone;
	private int rand;

	void Start () {
		GenerateGrid ();
		score = 0;
		over = GameObject.FindObjectOfType<GameOver> ();
		rand = Random.Range (0, piece.Length);
		Invoke ("GeneratePiece", 1);
	
	}

	void GenerateGrid(){

		for (int i = 0; i <= lines; i++) {
			for(int j = 0; j < rows; j++){
				var g = Instantiate(grid, new Vector2(j, i), Quaternion.identity) as GameObject;
				g.transform.parent = transform;
			}
		}

	}

	void Update(){

		textScore.text = score.ToString ();
	}

	public void RandomPiece(){

		rand = Random.Range (0, piece.Length);
		pieceClone = Instantiate (piece [rand], new Vector2(-5, 10.2f), Quaternion.identity) as GameObject;
		Destroy(pieceClone.gameObject.GetComponent<Move>());
	}

	void GeneratePiece(){

		if (over.CheckOver ())
			Instantiate (piece [rand], transform.position, Quaternion.identity);
		else
			over.TheOver ();
		Destroy (pieceClone);
	}

	public void CheckGrid(){

		int l = 0;
		while (l < lines){
			pieces.Clear ();
			pieces = GameObject.FindGameObjectsWithTag ("Piece").ToList();
			if (pieces.Count(p => p.transform.position.x >= 0 && p.transform.position.x < rows && p.transform.position.y == l)== rows){
				AudioSource.PlayClipAtPoint(line, Camera.main.transform.position);
				foreach(var piece in pieces){
					if (piece.transform.position.y == l)
					Destroy(piece);
				}
				FallPieces(l);
				score+= 100;
				l = 0;
			} else {
				l++;
			}
		}
		GeneratePiece ();
	}


	public void FallPieces(int l){

		if (pieces.Count > 0) {
			foreach(var piece in pieces){
				if (piece.transform.position.y >= l)
					piece.transform.position -= new Vector3(0, 1, 0);
			}

		} else
			return;
	}

}
