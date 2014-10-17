using UnityEngine;
using System.Collections.Generic;

public class CubeGrid : MonoBehaviour {

	public GameObject cubePrefab;
	public Dictionary<Vector3Int, GameObject> m_cubes;
	// Use this for initialization
	void Start () {
		m_cubes = new Dictionary<Vector3Int, GameObject> ();
		//createXYPlane (0,0,0, 5, 5);
		//populateVolume (0, 0, 0, 10, 10, 10, 0.05f);
		createXYPlaneRandom (0,0,0,5,5,5);
		//createXYPlane2 (0, 0, 0, 5, 5, 5);
		//createXYPlane (0, 0, 0, 2, 2);

		//GameObject obj = m_cubes [new Vector3Int (0, 0, 4)];
		//obj.renderer.material.color = new Color (0.0f, 0.0f, 0.0f);
		//0 0 4 = 0 4
		//1 1 3 = 1 5
		//2 2 2 = 2 6
		//3 3 1 = 3 7
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
		for (int x = 0; x < width; x++) 
		{
			for(int y = 0; y < height; y++)
			{
				int depth = Random.Range(zBack, zFront);
				Vector3Int position = new Vector3Int(x + xCenter - width/2 - depth, y + yCenter - height/2 - depth, depth + x + y);
				GameObject cube = (GameObject)Instantiate(cubePrefab, (Vector3)position, Quaternion.identity);
				m_cubes.Add(position, cube);
			}
		}
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
		hit.transform.renderer.material.color = Color.white;
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

	
	
}
