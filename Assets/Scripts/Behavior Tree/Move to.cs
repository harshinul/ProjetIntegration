using UnityEngine;
using UnityEngine.AI;

public class Moveto : Node
{
    private Transform target;
    private float stoppingDistance;
    private NavMeshAgent agent;

    public Moveto(NavMeshAgent agent, Transform target, float stoppingDistance, Conditions[] conditions, BehaviorTree BT) : base(conditions, BT)
    {
        this.agent = agent;
        this.target = target;
        this.stoppingDistance = stoppingDistance;
    }

    public override void ExecuteAction()
    {
        base.ExecuteAction();
        agent.SetDestination(target.position);
    }

    public override void Tick(float deltaTime)
    {
        if ((agent.transform.position - target.position).magnitude < stoppingDistance)
        {
            FinishAction(true);
        }
        else
        {
            if (!agent.SetDestination(target.position))
            {
                FinishAction(false);
            }
        }
    }

    public override void FinishAction(bool result)
    {
        agent.SetDestination(agent.transform.position);
        base.FinishAction(result);
    }
}
