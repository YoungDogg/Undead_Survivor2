using UnityEngine;

public class DeadStateSMB : StateMachineBehaviour
{
    // OnStateEnter는 이 'Dead' 애니메이션 상태가 끝날 때 호출됩니다.
    // 죽는 모션이 다 보여지고 메서드가 실행되게 합니다.
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Enemy enemyScript = animator.GetComponent<Enemy>();

        if (enemyScript) { enemyScript.Dead(); }
    }
}
