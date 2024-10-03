using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransformController : MonoBehaviour
{
    //—сылка на трансформ камеры и свойство, подт€гивающее мейн камеру если поле пустое
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

    [Header("—сылка на источник позиции дл€ камеры")]

    [SerializeField] private Transform cameraOriginReference; 
    public Transform CameraOriginReference => cameraOriginReference;


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
