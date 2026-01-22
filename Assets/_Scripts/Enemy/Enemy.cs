using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyStateMachine))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(EnemyVisuals))]
public class Enemy : Entity
{
    [SerializeField] private EnemyStats _stats;

    private EnemyStateMachine _stateMachine;
    private EnemyMovement _movement;
    private EnemyAttack _attack;
    private EnemyVisuals _visuals;

    private NavMeshAgent _agent;

    private ITargetable _target;

    private void Awake()
    {
        GetComponents();

        _visuals.Initialize(_attack);
        _movement.Initialize(_target, _stats, _agent);
        _attack.Initialize(_target, _stats);
        _stateMachine.Initialize(_movement, _attack, _stats, this);  

        if (_stats != null) _startingHealth = _stats.MaxHealth;
    }

    protected override void Start()
    {
        base.Start();
    }

    public void SetTarget(ITargetable target)
    {
        if (target == null) return;

        _target = target;
        _movement.UpdateTarget(target);
        _attack.UpdateTarget(target);
        _stateMachine.SetTarget(target);
    }

    private void GetComponents()
    {
        _movement = GetComponent<EnemyMovement>();
        _attack = GetComponent<EnemyAttack>();
        _agent = GetComponent<NavMeshAgent>();
        _visuals = GetComponent<EnemyVisuals>();
        _stateMachine = GetComponent<EnemyStateMachine>();
    }
}
