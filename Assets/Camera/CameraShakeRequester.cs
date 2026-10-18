using UnityEngine;

public class CameraShakeRequester : MonoBehaviour
{
    [SerializeField]
    private float _shakeAmount;

    [SerializeField]
    private float _shakeTime;

    [SerializeField]
    private CameraShaker _shaker;

    public void RequestShake()
    {
        _shaker.RequestShake(_shakeAmount, _shakeTime);
    }

}
