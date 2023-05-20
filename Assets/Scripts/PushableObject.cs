using UnityEngine;
using System.Collections.Generic;

public class PushableObject : MonoBehaviour
{
    [SerializeField] private List<string> _blockTags;
    [SerializeField] private float _pushForceMagnitude = 0.3f;
    [SerializeField] private float _moveAwayForceMagnitude = 1.0f;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _blockTags = new();
        _rigidbody = GetComponent<Rigidbody2D>();

        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_blockTags.Contains(other.tag))
        {
            return;
        }

        Vector2 pushForce = CalculatePushForce(other.transform.position);
        _rigidbody.AddForce(pushForce, ForceMode2D.Impulse);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_blockTags.Contains(other.tag))
        {
            return;
        }

        Vector2 moveAwayForce = CalculateMoveAwayForce(other.transform.position);
        _rigidbody.AddForce(moveAwayForce, ForceMode2D.Force);
    }

    private Vector2 CalculatePushForce(Vector2 otherPosition)
    {
        Vector2 oppositeDirection = (Vector2)transform.position - otherPosition;
        Vector2 pushForce = oppositeDirection.normalized * _pushForceMagnitude;

        return pushForce;
    }

    private Vector2 CalculateMoveAwayForce(Vector2 otherPosition)
    {
        Vector2 oppositeDirection = (Vector2)transform.position - otherPosition;
        Vector2 moveAwayForce = oppositeDirection.normalized * _moveAwayForceMagnitude;

        return moveAwayForce;
    }
}
