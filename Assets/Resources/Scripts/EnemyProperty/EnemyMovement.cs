using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyMovement : MonoBehaviour, ICharacterable {

	public float speed = 10f;

	Transform target;
	Vector2[] path;
	int targetIndex;

	PathFinding pathFinding;
	DiContainer container;

	[Inject]
	private void Init(PathFinding pathFinding){
		this.pathFinding = pathFinding;
	}

	void OnEnable(){
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		StartCoroutine (RefreshPath ());
	}

	IEnumerator RefreshPath(){
		Vector2 targetPositionOld = (Vector2)target.position;

		while (true) {
			if (targetPositionOld != (Vector2)target.position) {
				targetPositionOld = (Vector2)target.position;

				path = pathFinding.RequestPath (transform.position, target.position);
				StopCoroutine ("FollowPath");
				StartCoroutine ("FollowPath");
			}

			yield return new WaitForSeconds (.25f);
		}
	}

	IEnumerator FollowPath(){
		if (path.Length > 0) {
			targetIndex = 0;
			Vector2 currentWaypoint = path [0];

			while (true) {
				if ((Vector2)transform.position == currentWaypoint) {
					targetIndex++;
					if (targetIndex >= path.Length) {
						yield break;
					}
					currentWaypoint = path [targetIndex];
				}

				Vector3 directionFromPlayerToEnemy = (target.transform.position - transform.position).normalized;
				transform.position = Vector2.MoveTowards (transform.position, currentWaypoint, speed * Time.deltaTime);

				Vector2 frontPosition = transform.up * .5f;
				RaycastHit2D rayhit = Physics2D.Raycast (frontPosition + (Vector2)transform.position, transform.up);

				if (rayhit.collider != null && rayhit.collider.CompareTag("Player")) {
					//look at player
					transform.rotation = Quaternion.LookRotation (Vector3.forward, target.position - transform.position);
				} else {
					//look at path
					transform.rotation = Quaternion.LookRotation (Vector3.forward, currentWaypoint - (Vector2)transform.position);
				}

				yield return null;
			}
		}
	}

	public void SpeedChange(float increase){
		speed += increase;
	}

}
