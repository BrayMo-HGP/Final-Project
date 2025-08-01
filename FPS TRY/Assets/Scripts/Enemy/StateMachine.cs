using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseState activeState;
    private Enemy enemy;
    private PatrolState patrolState;

    public BaseState ActiveState => activeState; // ✅ Makes activeState publicly readable

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        patrolState = new PatrolState(enemy, this);
        ChangeState(patrolState);
    }

    private void Update()
    {
        activeState?.Perform();
    }

    public void ChangeState(BaseState newState)
    {
        activeState?.Exit();
        activeState = newState;
        activeState?.Enter();
    }

    public void Initialise()
    {
        enemy = GetComponent<Enemy>();
        ChangeState(new PatrolState(enemy, this));
    }
}
