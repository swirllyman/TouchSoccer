using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform dollyTransform;
    [SerializeField] CinemachineVirtualCamera vCam;
    [SerializeField] Vector2 minMaxHeight = new Vector2(30f, 20f);
    [SerializeField] Vector2 minMaxOffset = new Vector2(25f, 50f);
    [SerializeField] Vector2 minMaxVCamAimOffset = new Vector2(-3f, 3f);
    [SerializeField] Vector2 minMaxVCamVertDamping = new Vector2(.2f, .8f);
    [SerializeField] float fieldWidth = 29f;

    CinemachineComposer composer;

    private void Awake()
    {
        composer = vCam.GetCinemachineComponent<CinemachineComposer>();
        UpdateCam();

    }
    void FixedUpdate()
    {
        UpdateCam();
    }

    void UpdateCam()
    {
        float fieldDistPerc = Utils.GetPerc(-fieldWidth, fieldWidth, target.position.x);

        composer.m_ScreenY = Mathf.Lerp(minMaxVCamVertDamping.x, minMaxVCamVertDamping.y, fieldDistPerc);
        composer.m_TrackedObjectOffset.y = Mathf.Lerp(minMaxVCamAimOffset.x, minMaxVCamAimOffset.y, fieldDistPerc);

        Vector3 cameraPosition = dollyTransform.position;
        cameraPosition.y = Mathf.Lerp(minMaxHeight.x, minMaxHeight.y, fieldDistPerc);

        cameraPosition.x = Mathf.Lerp(minMaxOffset.x, minMaxOffset.y, fieldDistPerc);
        dollyTransform.position = cameraPosition;
    }
}
