using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Enemy;

public class EnemyInstaller : MonoInstaller {

	public override void InstallBindings (){
		Container.Bind<IStateDesign> ().FromSiblingComponent ();
	}


}
