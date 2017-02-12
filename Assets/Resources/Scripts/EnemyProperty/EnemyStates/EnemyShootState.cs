using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Enemy{
	
	public class EnemyShootState : StateBase{

		public string tag = "Shoot";

		public override string Tag {
			get {
				return tag;
			}
		}

		StateDesignEnemy enemyState;


		public EnemyShootState(StateDesignEnemy enemyState){
			this.enemyState = enemyState;
		}

		public override void UpdateState (){
			Look ();
		}
			
		public override void ToShootState (){
			Debug.Log ("Cant transition to the same state");
		}
			
		public override void ToKnifeState (){
			enemyState.currentState = enemyState.knifeState;
			enemyState.enemy.SpeedChange (enemyState.speedChange);
		}

		public override void ToDeathState (){
			enemyState.currentState = enemyState.deathState;
		}

		void Look(){
			//fronPosition move the raycast upwards so it wouldn't hit itself
			Vector2 frontPosition = enemyState.transform.up * .5f;
			RaycastHit2D rayhit = Physics2D.Raycast (frontPosition + (Vector2)enemyState.transform.position, enemyState.transform.up);
			//Debug.DrawRay (frontPosition + (Vector2)enemyState.transform.position, enemyState.transform.up);
			if (rayhit.collider != null && rayhit.collider.transform == enemyState.target) {
				if (Vector2.Distance (enemyState.target.position, enemyState.transform.position) >= enemyState.knifeRange)
					Attack ();
				else
					ToKnifeState ();
			}
		}

		void Attack(){
			enemyState.currentAttackComponent.FireTrigger (true);
		}

	}
}