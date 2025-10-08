using System;
using UnityEngine;

public abstract class Node
{
    protected Node parent;
    protected BehaviorTree BT;

    protected Conditions[] conditions;
    protected bool interrupted;

    public Node(Conditions[] conditions, BehaviorTree BT)
    {
        this.BT = BT;
        this.conditions = conditions;
    }
    public Node()
    {
    }
    public void SetParent(Node parent)
    {
        this.parent = parent;
    }
    public bool EvaluateCondition()
    {
        if (conditions == null)
            return true;

        foreach (Conditions c in conditions)
        {
            if (!c.Evaluate())
                return false;
        }
        return true;
    }
    virtual public void ExecuteAction()
    {
        if (EvaluateCondition())
        {
            FinishAction(false);
        }
        BT.activeNode = this;
    }

    virtual public void Tick(float deltaTime)
    {

    }
    virtual public void FinishAction(bool result)
    {
        if (!interrupted && parent != null)
            parent.FinishAction(result);
    }
    virtual public void Interrupt()
    {
        interrupted = true;
        FinishAction(false);
    }


}
