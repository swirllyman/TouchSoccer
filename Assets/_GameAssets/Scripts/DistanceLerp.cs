using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceLerp : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector2 minMaxHeight;
    [SerializeField] Vector2 minMaxOffsetClose;
    [SerializeField] Vector2 minMaxOffsetFar;

    [SerializeField] Vector2 minMaxVCamAimOffset;
    [SerializeField] float fieldWidth = 30;
    [SerializeField] Cinemachine.CinemachineVirtualCamera vCam;


    // Update is called once per frame
    void Update()
    {
        float movePerc = Mathf.Abs(target.position.x) / fieldWidth;
        Vector3 cameraPosition = transform.position;

        if(target.position.x < 0)
        {
            cameraPosition.x = Mathf.Lerp(minMaxOffsetFar.x, minMaxOffsetFar.y, movePerc);
            vCam.GetCinemachineComponent<Cinemachine.CinemachineComposer>().m_TrackedObjectOffset.y = Mathf.Lerp(minMaxVCamAimOffset.x, minMaxVCamAimOffset.y, movePerc);
        }
        else
        {
            cameraPosition.x = Mathf.Lerp(minMaxOffsetClose.x, minMaxOffsetClose.y, movePerc);
        }
        transform.position = cameraPosition;
    }
}
