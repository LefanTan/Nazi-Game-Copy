using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectPooler : MonoBehaviour {

	public GameObject[] poolObjects;
	public int poolAmount = 2;
	public bool willGrow = true;

	List<GameObject> pooledObjectsList;
	DiContainer container;

	[Inject]
	void Init(DiContainer container){
		this.container = container;
	}

	//instantiate certain amount of gameobjects at start
	void Start () {
		pooledObjectsList = new List<GameObject> ();

		foreach (GameObject poolObject in poolObjects) {
			InstantiatePooledObject (poolObject);
		}
	}

	private void InstantiatePooledObject(GameObject pooledObject){

		for (int i = 0; i < poolAmount; i++) {
			GameObject obj = container.InstantiatePrefab(pooledObject);
			obj.SetActive (false);
			obj.transform.SetParent (this.transform);
			pooledObjectsList.Add (obj);
		}

	}


}
