using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Enemy;

public class EnemyHealth : Health {

	public Vector2 barOffset = new Vector2 (10f, -20f);
	public Texture barTexture;
	public int health = 100;
	public Vector2 barSize = new Vector2 (45, 3);

	IStateDesign enemy;
	float barFull;
	float originalBarSizeX;
	int healthOriginal;

	[Inject]
	public void Init(IStateDesign enemy){
		this.enemy = enemy;
	}

	void Awake(){
		healthOriginal = health;
		originalBarSizeX = barSize.x;
	}

	void Update(){
		if (health <= 0) {
			enemy.ToDeathState ();
		}
			
		Debug.Log ("The IStateDesign enemy object: " + enemy);
	}

	void OnEnable(){
		health = healthOriginal;
		barFull = health;
		barSize.x = originalBarSizeX;
	}

	public override void Hurt (int damage){	
		health -= damage;
		barSize.x -= ((float)damage / barFull) * originalBarSizeX;
	}
		
}