using UnityEngine;
using System.Collections;

public class ColorScript : MonoBehaviour {

	public Color m_color;

	// Use this for initialization
	void Awake () {
		//Color c = new Color(1.0f,0.0f,0.0f) + new Color(0.0f,1.0f,0.0f);
		renderer.material.color = new Color (Random.value, Random.value, Random.value);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
