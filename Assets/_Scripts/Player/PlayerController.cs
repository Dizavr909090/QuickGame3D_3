using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Rigidbody _myRb;

    private void Awake()
    {
        if (_playerHealth == null) _playerHealth = GetComponent<PlayerHealth>();
        if (_playerMovement == null) _playerMovement = GetComponent<PlayerMovement>();

        SetupConnections();
        SetRigidbody();
    }

    private void OnDisable()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnDeath -= HandleDeath;
        }
    }

    private void SetRigidbody()
    {
        if (_myRb == null) _myRb = GetComponent<Rigidbody>();
        _myRb.interpolation = RigidbodyInterpolation.Interpolate;
        _myRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        _myRb.freezeRotation = true;

        _myRb.linearDamping = 0f;
        _myRb.angularDamping = 0f;
    }

    private void SetupConnections()
    {
        _playerHealth.OnDeath += HandleDeath;
    }

    private void HandleDeath(Entity entity)
    {
        _playerMovement.DisableMovement();
    }
}
