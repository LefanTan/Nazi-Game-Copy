using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Enemy{
	
	public class EnemyKnifeState : StateBase {

		public string tag = "Slash";

		public override string Tag {
			get {
				return tag;
			}
		}

		StateDesignEnemy enemyState;

		public EnemyKnifeState(StateDesignEnemy enemyState){
			this.enemyState = enemyState;
		}
			
		public override void UpdateState (){
			Look ();
		}
			
		public override void ToShootState (){
			enemyState.currentState = enemyState.shootState;
			enemyState.enemy.SpeedChange (-enemyState.speedChange);
		}
			
		public override void ToKnifeState (){
			Debug.Log ("Cant transition to the same state");
		}

		public override void ToDeathState (){
			enemyState.currentState = enemyState.deathState;
		}

		void Look(){
			Vector2 frontPosition = enemyState.transform.up * .5f;
			RaycastHit2D rayhit = Physics2D.Raycast (frontPosition + (Vector2)enemyState.transform.position, enemyState.transform.up);
			//Debug.DrawRay (frontPosition + (Vector2)enemyState.transform.position, enemyState.transform.up);
			if (rayhit.collider != null && rayhit.collider.transform == enemyState.target) {
				if (Vector2.Distance (enemyState.target.position, enemyState.transform.position) <= enemyState.sightRange)
					Attack ();
				else if(Vector2.Distance (enemyState.target.position, enemyState.transform.position) >= enemyState.knifeRange)
					ToShootState ();
			}
		}

		void Attack(){
			enemyState.currentAttackComponent.FireTrigger (true);
		}

	}
}