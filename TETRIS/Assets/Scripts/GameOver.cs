using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	public Text text;
	public AudioClip over;
	public AudioSource music;
	private bool gameOver;
	
	public bool CheckOver(){
		var allPieces = GameObject.FindGameObjectsWithTag ("Piece");
		foreach (var a in allPieces) {
			if (a.transform.position.y > 15)
				return false;
		}
		return true;
	} 

	void Update(){
		if (gameOver && Input.GetKeyDown (KeyCode.Return))
			Application.LoadLevel (0);
	}

	public void TheOver(){
		music.Stop ();
		AudioSource.PlayClipAtPoint (over, Camera.main.transform.position);
		text.gameObject.SetActive (true);
		gameOver = true;

	}
}
