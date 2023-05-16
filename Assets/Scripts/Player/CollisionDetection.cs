using UnityEngine;
using UnityEngine.Events;

public class CollisionDetection : MonoBehaviour
{
    public UnityEvent OnEnemyCollisionEvent = new();
    public UnityEvent OnCoinCollisionEvent = new();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                OnEnemyCollisionEvent.Invoke();
                break;
            case "Coin":
                OnCoinCollisionEvent.Invoke();
                Destroy(collision.gameObject);
                break;
        }
    }
}
