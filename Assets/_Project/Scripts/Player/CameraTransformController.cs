using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransformController : NetworkBehaviour
{
    //������ �� ��������� ������ � ��������, ������������� ���� ������ ���� ���� ������
    private Transform cameraTransform;
    public Transform CameraTransform
    {
        get
        {
            if (cameraTransform == null)
                cameraTransform = Camera.main.transform;
            return cameraTransform;
        }

        set
        {
            cameraTransform = value;
        }
    }

    private Transform cameraOriginReference; 

    private void Start()
    {
        if (!isLocalPlayer)
        {
            Destroy(this);
        }
        else
        {
            cameraOriginReference = transform;
        }
        
    }
    private void Update()
    {
        UpdateCameraPosition();
    }


    private void UpdateCameraPosition()
    {
        if (cameraOriginReference != null && CameraTransform != null)
        {
            cameraTransform.SetPositionAndRotation(cameraOriginReference.position, cameraOriginReference.rotation);
        }
    }


}
