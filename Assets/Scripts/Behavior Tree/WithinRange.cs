using UnityEngine;

public class WithinRange : Conditions
{
    GameObject target;
    Transform self;
    float range;
    float distanceBetweenTarget;

    public WithinRange(GameObject target, Transform self, float range,bool reverseCondition = false)
    {
        this.target = target;
        this.self = self;
        this.range = range;
        this.reverseCondition = reverseCondition;
    }

    public override bool Evaluate()
    {
        distanceBetweenTarget = (target.transform.position - self.position).magnitude;

        if(distanceBetweenTarget <= range)
        {
            return CheckForReverse(true);
        }
        else
        {
            return CheckForReverse(false);
        }
    }
}
