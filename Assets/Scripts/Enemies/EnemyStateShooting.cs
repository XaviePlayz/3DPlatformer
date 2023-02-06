using UnityEngine;

public class EnemyStateShooting : EnemyState {

    public EnemyStateShooting(EnemyController enemy){
        enemyRef = enemy;
    }
	
    public override void Update(){
        Vector3 enemyPlayerVector = FPSPlayerController.FPSPlayerInstance.transform.position - enemyRef.transform.position;
        float distance = enemyPlayerVector.sqrMagnitude;
        float angle = Vector3.Angle(enemyPlayerVector, enemyRef.transform.forward);

        if(distance <= enemyRef.shootingDistance && angle <= enemyRef.shootingAngle && Random.value <= enemyRef.shotProbability){
            enemyRef.Shoot(FPSCamera.CameraPosition);
        }        
    }
}
