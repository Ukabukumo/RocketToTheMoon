using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class LoadManager : MonoBehaviour
{
    public static LoadManager Instance;

    [SerializeField] private GameObject _loadUI;
    [SerializeField] private Animator _leftLeafAnimator;
    [SerializeField] private Animator _rightLeafAnimator;

    private const string LEFT_LEAF_OPEN_NAME = "LeftLeafOpen";
    private const string LEFT_LEAF_CLOSE_NAME = "LeftLeafClose";
    private const string RIGHT_LEAF_OPEN_NAME = "RightLeafOpen";
    private const string RIGHT_LEAF_CLOSE_NAME = "RightLeafClose";

    private const string LEFT_LEAF_OPEN_TRIGGER_NAME = "OpeningTrigger";
    private const string LEFT_LEAF_CLOSE_TRIGGER_NAME = "ClosingTrigger";
    private const string RIGHT_LEAF_OPEN_TRIGGER_NAME = "OpeningTrigger";
    private const string RIGHT_LEAF_CLOSE_TRIGGER_NAME = "ClosingTrigger";

    private void Awake()
    {
        InitializeInstance();
    }

    private void InitializeInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void ExecuteAfterLoad(UnityAction Action)
    {
        StartCoroutine(AnimationPlayback(Action));
    }

    private void BeginLoad()
    {
        _leftLeafAnimator.SetTrigger(LEFT_LEAF_CLOSE_TRIGGER_NAME);
        _rightLeafAnimator.SetTrigger(RIGHT_LEAF_CLOSE_TRIGGER_NAME);
    }

    private void EndLoad()
    {
        _leftLeafAnimator.SetTrigger(LEFT_LEAF_OPEN_TRIGGER_NAME);
        _rightLeafAnimator.SetTrigger(RIGHT_LEAF_OPEN_TRIGGER_NAME);
    }

    private IEnumerator AnimationPlayback(UnityAction Action)
    {
        _loadUI.SetActive(true);

        BeginLoad();

        while (!_leftLeafAnimator.GetCurrentAnimatorStateInfo(0).IsName(LEFT_LEAF_CLOSE_NAME) &&
               !_rightLeafAnimator.GetCurrentAnimatorStateInfo(0).IsName(RIGHT_LEAF_CLOSE_NAME))
        {
            yield return null;
        }

        Action();

        EndLoad();

        while (!_leftLeafAnimator.GetCurrentAnimatorStateInfo(0).IsName(LEFT_LEAF_OPEN_NAME) &&
               !_rightLeafAnimator.GetCurrentAnimatorStateInfo(0).IsName(RIGHT_LEAF_OPEN_NAME))
        {
            yield return null;
        }

        _loadUI.SetActive(false);
    }
}
