using UnityEngine;

public class GoToTest : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] UnityEngine.AI.NavMeshAgent agent;
    void Start()
    {
        
    }

    void Update()
    {
        agent.SetDestination(target.position);
    }
}
