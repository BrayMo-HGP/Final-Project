using UnityEngine;

public class PatrolState : BaseState
{
    private int waypointIndex;
    public float waitTimer;

    public PatrolState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        waypointIndex = 0;

        if (enemy.path != null && enemy.path.waypoints.Count > 0)
        {
            enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
        }
    }

    public override void Perform()
    {
        PatrolCycle();
        if (enemy.CanSeePlayer())
        {
          stateMachine.ChangeState(new AttackState(enemy, stateMachine));
        }
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime; // ✅ Fixed this line
            if (waitTimer > 3)
            {
                if (waypointIndex < enemy.path.waypoints.Count - 1)
                    waypointIndex++;
                else
                    waypointIndex = 0;
                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
                waitTimer = 0;
            }
        }
    }

    public override void Exit()
    {
        // Optional cleanup here if needed
    }

    private void PatrolCycle()
    {
        if (enemy.path == null || enemy.path.waypoints.Count == 0) return;

        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance < 0.2f)
        {
            waypointIndex = (waypointIndex + 1) % enemy.path.waypoints.Count;
            enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
        }
    }
}
