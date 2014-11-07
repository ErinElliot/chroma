using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorScript : MonoBehaviour {

	public Color m_color;

	// Use this for initialization
	void Awake () {
		//Color c = new Color(1.0f,0.0f,0.0f) + new Color(0.0f,1.0f,0.0f);
		//m_color = new Color (Random.value, Random.value, Random.value);
		setGameColor(Color.black);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setGameColor(Color color)
	{
		m_color = clampColor(color);
		renderer.material.color = m_color;
		string guiText;
		HashSet<Color> primaryColors = new HashSet<Color>();
		primaryColors.Add (Color.red);
		primaryColors.Add (Color.blue);
		primaryColors.Add (Color.green);

		HashSet<Color> secondaryColors = new HashSet<Color> ();
		HashSet<Color> tertiaryColors = new HashSet<Color> ();
		foreach (Color c in primaryColors)
		{
			foreach(Color d in primaryColors)
			{
				secondaryColors.Add(c + d);
			}
		}
		secondaryColors.ExceptWith (primaryColors);
		foreach (Color c in primaryColors)
		{
			foreach(Color d in primaryColors)
			{
				tertiaryColors.Add(c + d/2.0f);
			}
		}


		if (containsColor(primaryColors, color))
						guiText = "1";
		else if (containsColor(secondaryColors, color))
						guiText = "2";
		else if (containsColor(tertiaryColors, color))
						guiText = "3";
				else
						guiText = "";

		transform.FindChild ("ColorLabel").GetComponent<TextMesh> ().text = guiText;
	}

	private bool containsColor(IEnumerable<Color> colors, Color c)
	{
		foreach (Color d in colors) 
		{
			if( Mathf.Approximately (c.r, d.r) && Mathf.Approximately (c.g, d.g) && Mathf.Approximately (c.b, d.b))
				return true;
		}
		return false;
	}

	public Color getGameColor()
	{
		return m_color;
	}

	public void activateColor()
	{
		Color newColor = new Color (1.0f - m_color.r, 1.0f - m_color.g, 1.0f - m_color.b);
		setGameColor (newColor);
	}

	public void activateColorAsAdjacent(Color color)
	{
		setGameColor (new Color (color.r + m_color.r, color.g + m_color.g, color.b + m_color.b));
	}

	public void activateColorAsOuterRing(Color color)
	{
		setGameColor (new Color (color.r / 2.0f + m_color.r, color.g / 2.0f + m_color.g, color.b / 2.0f + m_color.b));
	}

	private Color clampColor(Color color)
	{
		return new Color (Mathf.Clamp (color.r, 0.0f, 1.0f),
		                  Mathf.Clamp (color.g, 0.0f, 1.0f),
		                  Mathf.Clamp (color.b, 0.0f, 1.0f));
	}
}
