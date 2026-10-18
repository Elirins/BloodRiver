using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private readonly List<ShakeRequest>  _requests = new();
    private CinemachineBasicMultiChannelPerlin _noise;

    [SerializeField]
    private float _shakeDecreaseAmount = 10f;

    private void Awake()
    {
        _noise = GetComponent<CinemachineVirtualCamera>()
             .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if(_requests.Count == 0)
        {
            _noise.m_AmplitudeGain = 0;
            return;
        }

        var strongestShake = _requests.Max(shake => shake.ShakeAmount);

        _noise.m_AmplitudeGain = strongestShake;

        for (var i = _requests.Count - 1; i >= 0; i--)
        {
            var request = _requests[i];
            request.ShakeTime -= Time.deltaTime;
            if(request.ShakeTime <= 0)
            {
                request.ShakeAmount = Mathf.Max(0, request.ShakeAmount - Time.deltaTime * _shakeDecreaseAmount); 
            }

            if(request.ShakeAmount == 0) _requests.Remove(request);
        }
    }
    public void RequestShake(float amount)
    {
        RequestShake(amount, 0);
    }

    public void RequestShake(float amount, float time)
    {
        _requests.Add(new ShakeRequest
        {
            ShakeAmount = amount,
            ShakeTime = time
        });
    }
    private class ShakeRequest
    {
        public float ShakeAmount { get; set; }
        public float ShakeTime { get; set; }
    }
}
