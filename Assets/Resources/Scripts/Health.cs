using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Health : MonoBehaviour, IHealthable {

	public abstract void Hurt (int damage);

}

