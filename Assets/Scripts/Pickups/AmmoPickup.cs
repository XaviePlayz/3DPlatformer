using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Pickup {	

	protected override void OnContactDo(){
        //FPSPlayerController.FPSPlayerInstance.AddAmmo(Random.Range(1, 3));
    }
}
