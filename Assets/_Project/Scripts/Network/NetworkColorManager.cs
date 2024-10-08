using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class NetworkColorManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnColorChanged))]
    public Color playerColor;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnColorChanged(Color oldColor, Color newColor)
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = newColor;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        meshRenderer.material.color = playerColor;
    }

    // Метод, чтобы сервер установил цвет
    [Command]
    public void CmdSetColor(Color color, int colorIndex)
    {

    }
}