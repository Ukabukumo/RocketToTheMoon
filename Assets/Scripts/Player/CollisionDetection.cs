using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CollisionDetection : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnObstacleCollisionEvent = new();
    [HideInInspector] public UnityEvent OnStarCollisionEvent = new();
    [HideInInspector] public UnityEvent OnFuelCollisionEvent = new();

    private const string STAR_COLLECT_TRIGGER = "StarCollected";
    private const string STAR_COLLECT_NAME = "Collect";

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Obstacle":
                OnObstacleCollisionEvent.Invoke();
                break;
            case "Star":
                OnStarCollisionEvent.Invoke();
                StartCoroutine(StarCollect(other.gameObject));
                break;
            case "Fuel":
                OnFuelCollisionEvent.Invoke();
                Destroy(other.gameObject);
                break;
        }
    }

    private IEnumerator StarCollect(GameObject star)
    {
        Animator starAnimator = star.GetComponent<Animator>();

        if (!starAnimator)
        {
            throw new MissingComponentException("Animator component is missing from StarObject");
        }

        starAnimator.SetTrigger(STAR_COLLECT_TRIGGER);

        while (star && !starAnimator.GetCurrentAnimatorStateInfo(0).IsName(STAR_COLLECT_NAME))
        {
            yield return null;
        }

        if (star)
        {
            Destroy(star);
        }
    }
}
