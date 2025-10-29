using UnityEngine;
using UnityEngine.AI;

public class AIBeginner : BehaviorTree
{
    [SerializeField] private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        RebuildTree();
    }

    protected override void InitializeTree()
    {
        RebuildTree();
    }

    private void RebuildTree()
    {
        if (target != null)
        {

            var agent = GetComponent<NavMeshAgent>();
            var self = transform;

            var moveTo = new Moveto(agent, target, 2f, null, this);
            var attack = new AttackNode(GetComponent<PlayerAttackScript>(),self,target,2f,null,this);

            root = new Sequence(new Node[] {moveTo,attack}, null, this);
        }
    }
}
