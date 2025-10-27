using UnityEngine;
using UnityEngine.AI;

public class Moveto : Node
{
    private Transform target;
    private float stoppingDistance;
    private NavMeshAgent agent;
    private const float EPS = 0.05f;

    public Moveto(NavMeshAgent agent, Transform target, float stoppingDistance, Conditions[] conditions, BehaviorTree BT) : base(conditions, BT)
    {
        this.agent = agent;
        this.target = target;
        this.stoppingDistance = stoppingDistance;
    }

    public override void ExecuteAction()
    {
        base.ExecuteAction();
        if (agent && target)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
    }

    public override void Tick(float deltaTime)
    {
        if (!agent || !target)
        {
            FinishAction(false);
            return;
        }
        agent.isStopped = false;
        agent.SetDestination(target.position);

        bool arrived =
            !agent.pathPending &&
            agent.remainingDistance <= (Mathf.Max(stoppingDistance, agent.stoppingDistance) + EPS) &&
            (!agent.hasPath || agent.velocity.sqrMagnitude <= 0.01f);

        if (arrived)
        {
            agent.ResetPath();
            FinishAction(true);
            return;
        }
    }

    public override void FinishAction(bool result)
    {
        agent.SetDestination(agent.transform.position);
        base.FinishAction(result);
    }
}
