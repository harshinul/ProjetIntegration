using UnityEngine;

public class Sequence : Node
{
    Node[] children;
    int index = 0;
    public Sequence(Node[] Children, Conditions[] condition, BehaviorTree BT) : base(condition, BT)
    {
        this.children = Children;
        foreach (Node n in children)
        {
            n.SetParent(this);
        }
    }
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        SequenceContinue(0);
    }
    public void SequenceContinue(int index)
    {
        children[index].ExecuteAction();
    }
    public override void FinishAction(bool result)
    {
        if (!result)
        {
            base.FinishAction(false);
        }
        else if (index == children.Length - 1)
        {
            base.FinishAction(true);
        }
        else
        {
            index++;
            SequenceContinue(index);
        }
    }
}
