using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

	public int spinSpeed;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, spinSpeed * Time.deltaTime, 0);
	}
}
