using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CollisionDetection : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnObstacleCollisionEvent = new();
    [HideInInspector] public UnityEvent OnStarCollisionEvent = new();
    [HideInInspector] public UnityEvent OnFuelCollisionEvent = new();

    private const string STAR_COLLECT_TRIGGER = "StarCollect";
    private const string FUEL_COLLECT_TRIGGER = "FuelCollect";

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Obstacle":
                AudioManager.Instance.PlaySound(AudioManager.Sound.COLLISION);
                OnObstacleCollisionEvent.Invoke();
                break;
            case "Star":
                other.tag = "Collected";
                OnStarCollisionEvent.Invoke();
                StartCoroutine(CollectTimer(other.gameObject, STAR_COLLECT_TRIGGER));
                break;
            case "Fuel":
                other.tag = "Collected";
                OnFuelCollisionEvent.Invoke();
                StartCoroutine(CollectTimer(other.gameObject, FUEL_COLLECT_TRIGGER));
                break;
        }
    }

    private IEnumerator CollectTimer(GameObject collectedObject, string triggerName)
    {
        Animator animator = collectedObject.GetComponent<Animator>();

        if (!animator)
        {
            throw new MissingComponentException("Animator component is missing from Collected Object");
        }

        animator.SetTrigger(triggerName);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float duration = stateInfo.length;
        yield return new WaitForSeconds(duration);

        if (collectedObject)
        {
            Destroy(collectedObject);
        }
    }
}
