using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input KeyCodes")]
    [SerializeField]
    private KeyCode keyCodeRun = KeyCode.LeftShift;//�޸��� Ű�� �Է¹޾�����

    private RotateMouse rotateMouse;
    private MovementCharacterController movementCharacterController;
    private Status status;

    

    private void Awake()
    {
        status = GetComponent<Status>();
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
        if (x != 0 || z != 0)//�̵����� �� �Ȱų� �ٱ� ���϶�
        {
            bool isRun = false;

            if (z > 0) isRun = Input.GetKey(keyCodeRun);
            movementCharacterController.MoveSpeed = isRun ? status.RunSpeed : status.WalkSpeed; //isRun�� Ʈ��� �ٴ¼ӵ��� �ƴϸ� �ȴ� �ӵ���

        }
        movementCharacterController.MoveTo(new Vector3(x, 0, z));
    }
}
