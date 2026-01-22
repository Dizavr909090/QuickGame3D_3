using System;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable, ITargetable
{
    public event Action<Entity> OnDeath;

    [SerializeField] protected float _startingHealth;
    [SerializeField] protected float _currentHealth;

    private Transform _transform;
    private bool _isDead = false;
    
    public float CurrentHealth => _currentHealth;
    public Transform Transform => _transform;
    public bool IsDead => _isDead;

    private void Awake()
    {
        _transform = transform;
    }

    protected virtual void Start()
    {
        _currentHealth = _startingHealth;
    }

    public void TakeHit(float damage, RaycastHit hit)
    {
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0 && !_isDead)
        {
            Die();
        }
    }

    protected void Die()
    {
        _isDead = true;
        StopAllCoroutines();
        OnDeath?.Invoke(this);
        GameObject.Destroy(gameObject);
    }

    public float GetRadius()
    {
        if (TryGetComponent<CapsuleCollider>(out var capsule))
            return capsule.radius;

        return 0f;
    }
}
