using UnityEngine;

public class Selector : Node
{
    Node[] children;
    int index = 0;

    public Selector(Node[] Children, Conditions[] condition, BehaviorTree BT) : base(condition, BT)
    {
        this.children = Children;
        foreach (Node child in children)
        {
            child.SetParent(this);
        }
    }
    public override void EvaluateAction()
    {
        base.EvaluateAction();
        children[index].EvaluateAction();
    }
    public override void FinishAction(bool result)
    {
        base.FinishAction(result);
        if (result)
        {
            base.FinishAction(true);
        }
        else if (index == children.Length - 1)
        {
            base.FinishAction(false);
        }
        else
        {
            index++;
            children[index].EvaluateAction();
        }
    }
}