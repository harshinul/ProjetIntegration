using UnityEngine;

public class AttackNode : Node
{
    private PlayerAttackScript attackScript;

    public AttackNode(PlayerAttackScript attackScript, Conditions[] conditions, BehaviorTree BT) : base(conditions, BT)
    {
        this.attackScript = attackScript;
    }

    public override void ExecuteAction()
    {
        base.ExecuteAction();
        attackScript.TriggerLightAttack();
    }

    public override void Tick(float deltaTime)
    {
        if (!attackScript.IsAttacking)
        {
            FinishAction(true);
        }
    }
    public override void FinishAction(bool result)
    {
        base.FinishAction(result);
    }
}
