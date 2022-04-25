using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]//ĳ���� ��Ʈ�ѷ� ������Ʈ�� �ʿ�� ��
public class MovementCharacterController : MonoBehaviour
{
    private CharacterController characterController;
    
    [SerializeField]
    private float moveSpeed;
    private Vector3 moveForce;
    
    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);//�ӵ��� ������ ������� �ʵ��� Max�� ���
        get => moveSpeed;
    }
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    

    private void Update()
    {
        characterController.Move(moveForce*Time.deltaTime);//1�ʴ� moveForce �ӷ����� �̵�
    }
    public void MoveTo(Vector3 direction)
    {
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);
        moveForce = new Vector3(direction.x * moveSpeed, moveForce.y, direction.z * moveSpeed);

    }
}
