using UnityEngine;

public abstract class Conditions
{
    protected bool reverseCondition;
    abstract public bool Evaluate();

    public bool CheckForReverse(bool result)
    {
        if (reverseCondition)
            return !result;
        return result;
    }
}