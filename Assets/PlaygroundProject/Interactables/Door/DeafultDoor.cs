using System.Collections;
using UnityEngine;

public class DeafultDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _doorTransform;
    private bool _isOpen;
    private Coroutine _animationRoutine;

    enum OpenMethod
    {
        Normal,
        Fast
    }

    public void Interact()
    {
        Open(OpenMethod.Normal);
    }

    private void Open(OpenMethod mode)
    {
        switch (mode)
        {
            case OpenMethod.Normal:
                if (_animationRoutine != null) StopCoroutine(_animationRoutine);
                if (_isOpen) _animationRoutine = StartCoroutine(DoorAnimation(Quaternion.Euler(0, 0, 0)));
                else _animationRoutine = StartCoroutine(DoorAnimation(Quaternion.Euler(0, -90, 0)));
                break;
            case OpenMethod.Fast:
                Debug.LogWarning("Fast mode not implemented it, oppening in antoher way");
                Open(OpenMethod.Normal); //Only when not implemented
                break;
        }
        _isOpen = !_isOpen;
    }

    IEnumerator DoorAnimation(Quaternion targetRotation)
    {
        Quaternion currentRotation = _doorTransform.localRotation;

        float time = 0;
        while (time < 1f)
        {
            _doorTransform.localRotation = Quaternion.Lerp(currentRotation, targetRotation, time);
            time += Time.deltaTime * 2f;
            yield return null;
        }
    }
}