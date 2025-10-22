using UnityEngine;
using UnityEngine.AI;

public class AIBeginner : BehaviorTree
{
    GameObject target;
    Transform self;

    protected override void InitializeTree()
    {
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();


        Moveto moveto = new Moveto(navMeshAgent, target.transform, 1, null, this);
        AttackNode attackNode = new AttackNode(this.GetComponent<PlayerAttackScript>(), new Conditions[] { new WithinRange(target, self, 2, false) }, this);

        root = new Sequence(new Node[] { moveto, attackNode },null, this);
    }

}
