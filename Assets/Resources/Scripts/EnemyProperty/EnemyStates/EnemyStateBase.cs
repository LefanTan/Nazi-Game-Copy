using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase {

	public abstract string Tag{ get;}

	public abstract void UpdateState ();

	public abstract void ToShootState ();

	public abstract void ToKnifeState ();

	public abstract void ToDeathState ();

}
