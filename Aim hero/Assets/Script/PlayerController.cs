using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input KeyCodes")]
    [SerializeField]
    private KeyCode keyCodeRun = KeyCode.LeftShift;//�޸��� Ű�� �Է¹޾�����
    [SerializeField]
    private KeyCode keyCodeJump = KeyCode.Space;
    [SerializeField]
    private AudioClip runAudioClip;
    [SerializeField]
    private AudioClip walkAudioClip;

    private AudioSource audioSource;//���� ��� ������Ʈ
    private RotateMouse rotateMouse;
    private MovementCharacterController movementCharacterController;
    private Status status;
    private PlayeranimatorController playerAnimatorController;
    private WeaponHandGun weapon;

    

    private void Awake()
    {
        status = GetComponent<Status>();
        movementCharacterController = GetComponent<MovementCharacterController>();
        rotateMouse = GetComponent<RotateMouse>();
        playerAnimatorController = GetComponent<PlayeranimatorController>();
        audioSource = GetComponent<AudioSource>();
        weapon = GetComponentInChildren<WeaponHandGun>();
        Cursor.lockState = CursorLockMode.Locked;//���콺 ��ġ�� ���� �� Ŀ���� �Ⱥ��̰� ����
        Cursor.visible = false;
    }
    private void Update()
    {
        UpdateRotate();
        UpdateMovement();
        UpdateJump();
        UpdateWeaponAction();
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
            playerAnimatorController.MoveSpeed = isRun ? 1 : 0.5f;//�ִϸ����� ��Ʈ�ѷ��� �Ķ���͸� �ٴ� ���¸� 1 �ƴϸ� 0.5�� �ٲ�
            audioSource.clip = isRun ? runAudioClip : walkAudioClip;
            if (audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            movementCharacterController.MoveSpeed = 0;
            playerAnimatorController.MoveSpeed = 0;
            if(audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
        }
        movementCharacterController.MoveTo(new Vector3(x, 0, z));
    }
    private void UpdateJump()
    {
        if (Input.GetKeyDown(keyCodeJump))
        {
            movementCharacterController.Jump();
        }
    }
    private void UpdateWeaponAction()
    {
        if (Input.GetMouseButtonDown(0))//���� ��ư�� ������ ��
        {
            weapon.StartWeaponAction();
        }
        else if (Input.GetMouseButtonUp(0))//���� ��ư�� ���� ��
        {
            weapon.StopWeaponAction();
        }
    }
}
