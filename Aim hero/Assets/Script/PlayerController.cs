using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private RotateMouse rotateMouse;

    private void Awake()
    {
        rotateMouse = GetComponent<RotateMouse>();
        Cursor.lockState = CursorLockMode.Locked;//���콺 ��ġ�� ���� �� Ŀ���� �Ⱥ��̰� ����
        Cursor.visible = false;
    }
    private void Update()
    {
        UpdateRotate();
    }
    private void UpdateRotate()
    {
        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");
        Debug.Log(MouseX);
        Debug.Log(MouseY);
        rotateMouse.UpdateRotate(MouseX, MouseY);
    }
}
