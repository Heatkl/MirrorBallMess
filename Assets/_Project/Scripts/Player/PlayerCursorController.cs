using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursorController 
{
    public bool isCursorVisible = true;
    
    public void ShowHideCursor()
    {
        if(isCursorVisible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isCursorVisible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isCursorVisible = true;
        }
    }
}
