using UnityEngine;

abstract public class BehaviorTree : MonoBehaviour
{
    protected Node root;

    public Node activeNode;

    abstract protected void InitializeTree();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeTree();
        EvaluateTree();
    }

    // Update is called once per frame
    void Update()
    {
        if (activeNode != null)
            activeNode.Tick(Time.deltaTime);
    }
    public void EvaluateTree()
    {
        root.ExecuteAction();
    }
    public void Interupt()
    {
        activeNode.Interrupt();
        EvaluateTree();
    }
}
