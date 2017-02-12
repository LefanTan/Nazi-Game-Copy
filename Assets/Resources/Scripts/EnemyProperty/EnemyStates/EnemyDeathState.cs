using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy{
	
	public class EnemyDeathState : StateBase{
		
		private string tag = "Death";

		public override string Tag {
			get {
				return tag;
			}
		}


		StateDesignEnemy enemyState;

		public EnemyDeathState(StateDesignEnemy enemyState){
			this.enemyState = enemyState;
		}

		public override void UpdateState (){
		}

		public override void ToShootState (){

		}

		public override void ToKnifeState (){

		}

		public override void ToDeathState (){

		}
			
	}
}