using UnityEngine;

public class AttackNode : Node
{
    private readonly Transform self;
    private readonly Transform target;
    private readonly PlayerAttackScript attackScript;

    private float nextAttackTime = 0f;
    private readonly float cooldown = 0.3f;
    private readonly float attackRange;

    public AttackNode(PlayerAttackScript attackScript, Transform self, Transform target, float attackRange,Conditions[] conditions, BehaviorTree bt): base(conditions, bt)
    {
        this.attackScript = attackScript;
        this.self = self;
        this.target = target;
        this.attackRange = attackRange;
    }

    public override void Tick(float dt)
    {
        if (!attackScript || !self || !target)
        {
            FinishAction(false);
            return;
        }

        float d = Vector3.Distance(self.position, target.position);
        if (d > attackRange + 0.1f)
        {
            FinishAction(false);
            return;
        }

        if (!attackScript.IsAttacking && Time.time >= nextAttackTime)
        {
            attackScript.TriggerLightAttack();
            nextAttackTime = Time.time + cooldown;
        }

    }
}
