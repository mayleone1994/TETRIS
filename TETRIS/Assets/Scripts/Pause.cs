using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	private GameManager manager;
	private bool isPause;

	void Start () {
	
		manager = GameObject.FindObjectOfType<GameManager> ();
	}

	void Update () {
	
		if (Input.GetKeyDown (KeyCode.Return)) 
			StartCoroutine("Wait");
	}

	IEnumerator Wait(){
		yield return new WaitForSeconds (0.2f);
		isPause = !isPause;
		Pausing ();
	}

	void Pausing(){
		var piece = GameObject.FindObjectOfType<Move>();
		if (isPause) {
			piece.canMove = false;
			piece.CancelInvoke("FallPiece");
			manager.over.gameObject.SetActive (false);
			manager.enabled = false;
		}
		
		if (!isPause) {
			piece.canMove = true;
			manager.enabled = true;
			manager.over.gameObject.SetActive (true);
			piece.InvokeRepeating("FallPiece", 0.3f, 0.3f);
		}
	}
}
