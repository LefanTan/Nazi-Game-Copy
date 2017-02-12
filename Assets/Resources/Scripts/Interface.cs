using UnityEngine;
using System.Collections;

//put it into movement script
public interface ICharacterable{
	void SpeedChange (float increase);
}

//put it with health scripts
public interface IHealthable{
	void Hurt (int damage);
}

//put it in attack script etc shoot, slash
public interface IAttackable{
	void AttackStuff();
	void FireTrigger(bool trigger);
}

//items that can be used
public interface IUsable{
	void Use();
}

public interface IStateDesign{
	void ToDeathState ();
}