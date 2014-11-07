using UnityEngine;
using System.Collections.Generic;

public class CubeGrid : MonoBehaviour {

	public GameObject cubePrefab;
	public Dictionary<Vector3Int, GameObject> m_cubes;
	public Vector3Int m_minPos;
	public Vector3Int m_maxPos;
	public Vector3Int m_minHexDepthPos;
	public Vector3Int m_maxHexDepthPos;
	public bool m_hexPerspective;
	// Use this for initialization
	void Start () {
		m_cubes = new Dictionary<Vector3Int, GameObject> ();
		m_hexPerspective = true;
		//createXYPlane (0,0,0, 5, 5);
		//populateVolume (0, 0, 0, 10, 10, 10, 0.05f);
		//createXYPlaneRandom (0,0,0,15,5,5);
		//createXYPlane2 (0, 0, 0, 5, 5, 5);
		createXYPlane (0, 0, 0, 8, 8);

		//GameObject obj = m_cubes [new Vector3Int (0, 0, 4)];
		//obj.renderer.material.color = new Color (0.0f, 0.0f, 0.0f);
		//0 0 4 = 0 4
		//1 1 3 = 1 5
		//2 2 2 = 2 6
		//3 3 1 = 3 7
		m_cubes [new Vector3Int (0, 0, 8)].GetComponent<ColorScript> ().setGameColor (Color.red);
		m_cubes [new Vector3Int (0, 2, 10)].GetComponent<ColorScript> ().setGameColor (new Color(0.0f, 1.0f, 0.5f));
		setMinMax ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
			clickOnCube();
	}

	void createXYPlane(int xCenter, int yCenter, int zFront, int width, int height)
	{
		for (int x = 0; x < width; x++) 
		{
			for(int y = 0; y < height; y++)
			{
				Vector3Int position = new Vector3Int(x + xCenter - width/2, y + yCenter - height/2, zFront + x + y);
				GameObject cube = (GameObject)Instantiate(cubePrefab, (Vector3)position, Quaternion.identity);
				m_cubes.Add(position, cube);
			}
		}
	}
	void createXYPlaneRandom(int xCenter, int yCenter, int zBack, int zFront, int width, int height)
	{
		int xCenterOffset = 0;
		int yCenterOffset = 0;
		int zCenterOffset = 0;
		for (int x = 0; x < width; x++) 
		{
			for(int y = 0; y < height; y++)
			{
				int depth = Random.Range(zBack, zFront);
				Vector3Int position = new Vector3Int(x + xCenter - width/2 - depth, y + yCenter - height/2 - depth, depth + x + y);
				GameObject cube = (GameObject)Instantiate(cubePrefab, (Vector3)position, Quaternion.identity);
				m_cubes.Add(position, cube);
				if(x == width/2 && y == height/2)
				{
					xCenterOffset = position.x;
					yCenterOffset = position.y;
					zCenterOffset = position.z;
				}
			}
		}
		Dictionary<Vector3Int,GameObject> newCubes = new Dictionary<Vector3Int, GameObject> ();
		foreach (KeyValuePair<Vector3Int, GameObject> cube in m_cubes) 
		{
			newCubes.Add(new Vector3Int(cube.Key.x - xCenterOffset, cube.Key.y - yCenterOffset, cube.Key.z - zCenterOffset),
			             cube.Value);
			cube.Value.transform.Translate(new Vector3(-xCenterOffset, -yCenterOffset, -zCenterOffset));
		}
		m_cubes = newCubes;
	}

	void createXYPlane2(int xCenter, int yCenter, int zBack, int zFront, int width, int height)
	{
		for (int x = 0; x < width; x++) 
		{
			for(int y = 0; y < height; y++)
			{
				int depth = zBack;
				//Vector3Int position = new Vector3Int(x + xCenter - width/2 - depth, (y + yCenter - height/2 - depth) * 2, depth + (y));
				Vector3Int position = new Vector3Int(2*x - y, x - 2*y, depth);
				GameObject cube = (GameObject)Instantiate(cubePrefab, (Vector3)position, Quaternion.identity);
				m_cubes.Add(position, cube);
			}
		}
	}


	void populateVolume(int xMin, int yMin, int zMin, int xMax, int yMax, int zMax, float percent)
	{
		int volume = (xMax - xMin) * (yMax - yMin) * (zMax - zMin);
		float cubeCount = (int)(volume * percent);
		while (cubeCount > 0) 
		{
			Vector3Int position = new Vector3Int(Random.Range(xMin, xMax),
			                                     Random.Range(yMin, yMax),
			                                     Random.Range(zMin, zMax));
			if(!m_cubes.ContainsKey(position))
			{
				GameObject cube = (GameObject)Instantiate(cubePrefab, (Vector3)position, Quaternion.identity);
				m_cubes.Add(position, cube);
				cubeCount--;
			}
		}
	}

	void clickOnCube()
	{
		Camera mainCamera = Camera.main;
		
		// We need to actually hit an object
		RaycastHit hit;
		Ray rayToMouse = mainCamera.ScreenPointToRay(Input.mousePosition);
		if (!Physics.Raycast(rayToMouse, out hit, 100))
			return;
		// We need to hit a rigidbody that is not kinematic
		//if (!hit.rigidbody || hit.rigidbody.isKinematic)
		//	return;
		//make sure it's a chesspiece
		//if(hit.transform.tag != "ChessPiece")
		//	return;
		ColorScript colorScript = hit.transform.GetComponent<ColorScript> ();
		if (colorScript == null)
			return;
		//hit.transform.renderer.material.color = Color.white;
		Vector3Int pos = (Vector3Int)hit.transform.position;

		List<GameObject> cubes;
		if(m_hexPerspective)
		 cubes = getClosestAdjacentHexagons (pos.x, pos.y, pos.z);
		else
			cubes = getClosestAdjacentSquares(pos.x, pos.y, pos.z);

		Color clickedColor = colorScript.getGameColor ();
		colorScript.activateColor();
		foreach (GameObject cube in cubes) 
		{
			cube.GetComponent<ColorScript>().activateColorAsAdjacent(clickedColor);
			//cube.GetComponent<ColorScript>().setGameColor(Color.red + Color.blue);
		}

		List<GameObject> outerCubes = getClosestOuterHexagonRing (pos.x, pos.y, pos.z);
		foreach (GameObject cube in outerCubes)
		{
			cube.GetComponent<ColorScript>().activateColorAsOuterRing(clickedColor);
		}

		//getClosestHexagon (pos.x, pos.y, pos.z).renderer.material.color = Color.white;
		// material.color = Color.white

		/*
		if(networkedGame)
		{
			applyForceToPiece(hit.transform.networkView.viewID, rayToMouse.direction * 1000 * (1.0f/Time.timeScale), hit.point);
			if(Network.isClient)
			{
				networkView.RPC("applyForceToPiece", RPCMode.Server, hit.transform.networkView.viewID, rayToMouse.direction * 1000 * (1.0f/Time.timeScale), hit.point);
			}
		}
		else
		{
			applyForceToPiece(hit.transform.networkView.viewID, rayToMouse.direction * 1000 * (1.0f/Time.timeScale), hit.point);
		}*/
		
		//lastPieceLaunched = hit.transform.gameObject;
		//checkSlowMotion = true;
	}

	void setMinMax()
	{
		bool firstTime = true;
		foreach (KeyValuePair<Vector3Int, GameObject> cube in m_cubes)
		{
			Vector3Int pos = cube.Key;
			if(firstTime)
			{
				m_minPos = pos;
				m_maxPos = pos;
				m_minHexDepthPos = pos;
				m_maxHexDepthPos = pos;
				firstTime = false;
				continue;
			}

			if(pos.x < m_minPos.x)
				m_minPos.x = pos.x;
			if(pos.y < m_minPos.y)
				m_minPos.y = pos.y;
			if(pos.z < m_minPos.z)
				m_minPos.z = pos.z;
			if(pos.x > m_maxPos.x)
				m_maxPos.x = pos.x;
			if(pos.y > m_maxPos.y)
				m_maxPos.y = pos.y;
			if(pos.z > m_maxPos.z)
				m_maxPos.z = pos.z;

			int minHexDepth = getHexagonDepth(m_minHexDepthPos.x, m_minHexDepthPos.y, m_minHexDepthPos.z);
			int maxHexDepth = getHexagonDepth(m_maxHexDepthPos.x, m_maxHexDepthPos.y, m_maxHexDepthPos.z);
			int hexDepth = getHexagonDepth(pos.x, pos.y, pos.z);
			if(hexDepth < minHexDepth)
				m_minHexDepthPos = pos;
			if(hexDepth > maxHexDepth)
				m_maxHexDepthPos = pos;
		}
	}

	GameObject getClosestHexagon(int x, int y, int z)
	{
		int hexDepth = getHexagonDepth (x, y, z);
		//w = (x - x1) + (y - y1) - (z - z1)
		//m_minHexDepth = 

		GameObject closestHexagon = null;
		//int depthOffset = hexDepth - m_minHexDepth;
		//for (int i = 0; i <= m_maxHexDepth + m_minHexDepth; i++)
		//{
		//TODO: CALCULATE
		for(int i = -150; i <= 150; i++)
		{
			GameObject cube;
			Vector3Int pos = new Vector3Int(x + i, y + i, z - i);
			//Vector3Int pos = new Vector3Int(x - depthOffset + i, y - depthOffset + i, z - depthOffset - i);
			if(m_cubes.TryGetValue(pos, out cube))
			{
				closestHexagon = cube;
			}
		}
		return closestHexagon;
	}
	//min
	//-12, -9, 9
	//-30

	//click
	//1, 3, -2
	//6

	//0, 3, -3
	//6

	//-36
	//-36, -33, -39
	//-36, -33, 33

	//

	//-12, -9, 9
	//
	//-12, -12, 12

	//w = (u - u1) + (v - v1)
	//u = x*sqrt(2)
	//v = y*sqrt(2)
	//w = (x*sqrt(2) - x1) + (y*sqrt(2) - y1) = x + y - z
	//


	List<GameObject> getClosestAdjacentHexagons(int x, int y, int z)
	{
		List<GameObject> cubes = new List<GameObject> ();
		GameObject cube1 = getClosestHexagon(x + 1, y, z + 1);
		GameObject cube2 = getClosestHexagon(x, y + 1, z + 1);
		GameObject cube3 = getClosestHexagon(x - 1, y + 1, z);
		GameObject cube4 = getClosestHexagon(x - 1, y, z - 1);
		GameObject cube5 = getClosestHexagon(x, y - 1, z - 1);
		GameObject cube6 = getClosestHexagon(x + 1, y - 1, z);
		if(cube1)
			cubes.Add (cube1);
		if(cube2)
			cubes.Add (cube2);
		if(cube3)
			cubes.Add (cube3);
		if(cube4)
			cubes.Add (cube4);
		if(cube5)
			cubes.Add (cube5);
		if(cube6)
			cubes.Add (cube6);
		return cubes;
	}

	//w = -(x - x1) -(y - y1) + (z - z1)
	int getHexagonDepth(int x, int y, int z)
	{
		return x + y - z;
	}

	GameObject getClosestSquare(int x, int y, int z)
	{
		GameObject closestSquare = null;
		for (int i = m_minPos.x; i <= m_maxPos.x; i++)
		{
			GameObject cube;
			Vector3Int pos = new Vector3Int(i, y, z);
			if(m_cubes.TryGetValue(pos, out cube))
			{
				closestSquare = cube;
			}
		}
		return closestSquare;
	}

	List<GameObject> getClosestAdjacentSquares(int x, int y, int z)
	{
		List<GameObject> cubes = new List<GameObject> ();
		GameObject cube1 = getClosestSquare(x, y, z + 1);
		GameObject cube2 = getClosestSquare(x, y + 1, z);
		GameObject cube3 = getClosestSquare(x, y, z - 1);
		GameObject cube4 = getClosestSquare(x, y - 1, z);
		if(cube1)
			cubes.Add (cube1);
		if(cube2)
			cubes.Add (cube2);
		if(cube3)
			cubes.Add (cube3);
		if(cube4)
			cubes.Add (cube4);
		return cubes;
	}

	List<GameObject> getClosestOuterHexagonRing(int x, int y, int z)
	{
		List<GameObject> cubes = new List<GameObject>();
		List<GameObject> adjacentCubes = getClosestAdjacentHexagons (x, y, z);
		GameObject centerCube = getClosestHexagon (x, y, z);
		foreach(GameObject cube in adjacentCubes)
		{
			Vector3Int cubePos = (Vector3Int) cube.transform.position;
			List<GameObject> adjacentCubes2 = getClosestAdjacentHexagons(cubePos.x, cubePos.y, cubePos.z);
			foreach(GameObject cube2 in adjacentCubes2)
			{
				if(!adjacentCubes.Contains(cube2) && !cubes.Contains(cube2) && cube2 != centerCube)
				{
					cubes.Add(cube2);
				}
			}
		}

		return cubes;
	}

}
