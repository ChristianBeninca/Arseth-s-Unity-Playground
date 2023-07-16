using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLocker : MonoBehaviour
{
    private void Awake()
    {
        ToggleCursor(1);
    }
    void Update()
    {
        InputHolder();
    }

    void InputHolder()
    {
        // Just on Editor
        if (Input.GetKeyDown(KeyCode.X)) { ToggleCursor(); }
    }

        void ToggleCursor(int mode = -1)
    {
        switch (mode)
        {
            case 0:
                Cursor.lockState = CursorLockMode.None;
                break;

            case 1:
                Cursor.lockState = CursorLockMode.Locked;
                break;

            default:
                if (Cursor.lockState == CursorLockMode.Locked)
                    Cursor.lockState = CursorLockMode.None;
                else
                    Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }
}
