using UnityEngine;
using UnityEngine.EventSystems;

public class ControlArm : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private GameObject _armBase;
    [SerializeField] private GameObject _arm;

    private Vector2 _touchBeginPosition;
    private float _armChangeLimit = 200.0f;
    private float _armValue= 0.0f;
    private bool _isControlling = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        _touchBeginPosition = eventData.position;

        _armBase.SetActive(true);
        _arm.SetActive(true);

        _armBase.transform.position = _touchBeginPosition;
        _arm.transform.position = _touchBeginPosition;

        _isControlling = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _armBase.SetActive(false);
        _arm.SetActive(false);

        _isControlling = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Distance between start touch and current touch
        float changeX = _touchBeginPosition.x - eventData.position.x;

        _armBase.transform.position = _touchBeginPosition;

        Vector3 newArmPosition = new Vector3(eventData.position.x, _arm.transform.position.y, _arm.transform.position.z);
        _arm.transform.position = newArmPosition;

        // Cast changeX to range [-200.0f; 200.0f]
        if (Mathf.Abs(changeX) > _armChangeLimit)
        {
            float sign = changeX / Mathf.Abs(changeX);
            _touchBeginPosition -= new Vector2(sign * (Mathf.Abs(changeX) - _armChangeLimit), 0.0f);
            changeX = _touchBeginPosition.x - eventData.position.x;
        }

        // Cast changeX to range [-1.0f; 1.0f]
        _armValue = changeX / _armChangeLimit;
    }

    public float GetArmValue()
    {
        return _armValue;
    }

    public bool IsControlling()
    {
        return _isControlling;
    }

    private void OnEnable()
    {
        _armBase.SetActive(false);
        _arm.SetActive(false);
        _armValue = 0.0f;

        _isControlling = false;
    }
}
