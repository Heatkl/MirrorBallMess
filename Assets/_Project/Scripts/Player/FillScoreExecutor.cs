using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillScoreExecutor : NetworkBehaviour
{

    [SerializeField] TMP_Text playerText;

    public void UpdateScoreText(int score)
    {
        playerText.text = score.ToString();
    }
}
