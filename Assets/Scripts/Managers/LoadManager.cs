using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class LoadManager : MonoBehaviour
{
    public static LoadManager Instance;

    [SerializeField] private GameObject _loadUI;
    [SerializeField] private Animator _leftLeafAnimator;
    [SerializeField] private Animator _rightLeafAnimator;

    private string _leftLeafOpenName = "LeftLeafOpen";
    private string _leftLeafCloseName = "LeftLeafClose";
    private string _rightLeafOpenName = "RightLeafOpen";
    private string _rightLeafCloseName = "RightLeafClose";

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
        _leftLeafAnimator.SetTrigger("OpeningTrigger");
        _rightLeafAnimator.SetTrigger("OpeningTrigger");
    }

    private void EndLoad()
    {
        _leftLeafAnimator.SetTrigger("ClosingTrigger");
        _rightLeafAnimator.SetTrigger("ClosingTrigger");
    }

    private IEnumerator AnimationPlayback(UnityAction Action)
    {
        _loadUI.SetActive(true);

        BeginLoad();

        while (!_leftLeafAnimator.GetCurrentAnimatorStateInfo(0).IsName(_leftLeafOpenName) &&
               !_rightLeafAnimator.GetCurrentAnimatorStateInfo(0).IsName(_rightLeafOpenName))
        {
            yield return null;
        }

        Action();

        EndLoad();

        while (!_leftLeafAnimator.GetCurrentAnimatorStateInfo(0).IsName(_leftLeafCloseName) &&
               !_rightLeafAnimator.GetCurrentAnimatorStateInfo(0).IsName(_rightLeafCloseName))
        {
            yield return null;
        }

        _loadUI.SetActive(false);
    }
}
