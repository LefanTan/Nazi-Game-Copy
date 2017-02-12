using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Enemy{

	public class StateDesignEnemy : MonoBehaviour, IStateDesign {
		
		public float speedChange;
		public float knifeRange;
		public float sightRange;
		public bool startWithShootState;
		public bool startWithKnifeState;
		public GameObject bloodSplashPrefab;
		public AudioClip bloodSplashSound;

		[HideInInspector] public EnemyShootState shootState;
		[HideInInspector] public EnemyKnifeState knifeState;
		[HideInInspector] public EnemyDeathState deathState;
		[HideInInspector] public StateBase currentState;
		[HideInInspector] public IAttackable currentAttackComponent;
		[HideInInspector] public ICharacterable enemy;
		[HideInInspector] public BoxCollider2D enemyCollider;
		[HideInInspector] public Transform target;
		[HideInInspector] public ObjectPooler bloodPooler;

		private IAttackable[] attackComponents;

		void Awake(){
			shootState = new EnemyShootState (this);
			knifeState = new EnemyKnifeState (this);
			deathState = new EnemyDeathState (this);

			enemyCollider = GetComponent<BoxCollider2D> ();
			attackComponents = GetComponents<IAttackable> ();
			currentAttackComponent = GetComponent<IAttackable> ();
			enemy = GetComponent<ICharacterable> ();
			//target = GameObject.FindGameObjectWithTag ("Player").transform;
			bloodPooler = GameObject.FindGameObjectWithTag ("BloodPooler").GetComponent<ObjectPooler>();
		}
			
		void Update(){
			ChangeAttackComponent ();
			currentState.UpdateState ();
		}

		void OnEnable(){

			if (startWithKnifeState)
				currentState = knifeState;
			else if (startWithShootState)
				currentState = shootState;
		}

		void ChangeAttackComponent(){
			if (currentState.Tag == "Death")
				return;

			if (attackComponents != null) {
				foreach (IAttackable a in attackComponents) {
					if (a.GetType ().ToString () == currentState.Tag) {
						currentAttackComponent = a;
					}
				}
			}
		}

		public void ToDeathState(){
			currentState = deathState;
			Debug.Log ("enemy died");
		}
			
	}

}