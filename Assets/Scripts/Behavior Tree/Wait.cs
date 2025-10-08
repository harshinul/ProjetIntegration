using UnityEngine;

public class Wait : Node
{
    float secondsToWait;
    float timer;

    public Wait(float secondsToWait, Conditions[] condition, BehaviorTree BT) : base(condition, BT)
    {
        this.secondsToWait = secondsToWait;
    }

    public override void ExecuteAction()
    {
        timer = 0;
        base.ExecuteAction();
    }
    public override void Tick(float deltaTime)
    {
        //if (interrupted)
        //    return;

        timer += deltaTime;
        if (timer >= secondsToWait)
        {
            FinishAction(true);
        }
    }
}
