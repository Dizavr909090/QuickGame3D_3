using System.Collections;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private float _timeBetweenChecks = .1f;
    private EnemyMovement _movement;
    private EnemyAttack _attack;
    private EnemyStats _stats;
    private ITargetable _target;
    private Entity _selfEntity;

    private Coroutine _stateUpdateCoroutine;

    public enum State { Idle, Chasing, Attacking, Dead };
    private State _currentState;

    public void Initialize(EnemyMovement movement, EnemyAttack attack, EnemyStats stats, Entity self )
    {
        _movement = movement;
        _attack = attack;
        _stats = stats;
        _selfEntity = self;

        if (_stateUpdateCoroutine != null) StopCoroutine(_stateUpdateCoroutine);
        _stateUpdateCoroutine = StartCoroutine(StateUpdateTick());
    }

    public void SetTarget(ITargetable target)
    {
        _target = target;
        CheckStateTransition();
    }

    private void ChangeState(State newState)
    {
        if (_currentState == newState) return;

        if (_currentState == State.Attacking)
            _movement.SetKinematic(false);

        _currentState = newState;

        switch (_currentState)
        {
            case State.Idle:
                _movement.StopMoving();
                break;
            case State.Chasing:
                _movement.StartMoving();
                break;
            case State.Attacking:
                _movement.StopMoving();
                _movement.SetKinematic(true);
                _attack.PerformAttack();
                break;
            case State.Dead:
                _movement.StopMoving();
                _movement.SetKinematic(true);
                break;
        }
    }

    private void CheckStateTransition()
    {
        if (_selfEntity.IsDead)
        {
            ChangeState(State.Dead);
            return;
        }

        if (_target == null || _target.IsDead)
        {
            ChangeState(State.Idle);
            return;
        }

        if (_attack.IsAttacking) return;

        if (_movement.DistanceToTarget <= _stats.AttackDistanceThreshold)
        {
            ChangeState(State.Attacking);
        }
        else
        {
            ChangeState(State.Chasing);
        }
    }

    private IEnumerator StateUpdateTick()
    {
        while (_selfEntity != null)
        {
            if (_selfEntity.IsDead)
            {
                ChangeState(State.Dead);
                yield break;
            }

            if (_target == null)
            {
                ChangeState(State.Idle);
            }
            else
            {
                CheckStateTransition();

                if (_currentState == State.Attacking && !_attack.IsAttacking)
                    _attack.PerformAttack();
            }
            yield return new WaitForSeconds(_timeBetweenChecks);
        }

        ChangeState(State.Dead);
    }
}
