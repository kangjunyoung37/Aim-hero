using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private RotateMouse rotateMouse;
    private MovementCharacterController movementCharacterController;
    private void Awake()
    {
        movementCharacterController = GetComponent<MovementCharacterController>();
        rotateMouse = GetComponent<RotateMouse>();
        Cursor.lockState = CursorLockMode.Locked;//���콺 ��ġ�� ���� �� Ŀ���� �Ⱥ��̰� ����
        Cursor.visible = false;
    }
    private void Update()
    {
        UpdateRotate();
        UpdateMovement();
    }
    private void UpdateRotate()
    {
        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");
        Debug.Log(MouseX);
        Debug.Log(MouseY);
        rotateMouse.UpdateRotate(MouseX, MouseY);
    }
    private void UpdateMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        movementCharacterController.MoveTo(new Vector3(x, 0, z));
    }
}
