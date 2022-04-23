using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMouse : MonoBehaviour
{
    [SerializeField]
    private float rotCamXAxixSpeed = 5; //X�� ȸ�� �ӵ�
    [SerializeField]
    private float rotCamYAxixSpeed = 3;//Y�� ȸ�� �ӵ�

    private float limitMinX = -80;//ī�޶� X�� �ּ� ȸ�� ����
    private float limitMaxX = 80;//ī�޶� X�� �ִ� ȸ�� ����
    private float eulerAngleX;
    private float eulerAngleY;

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX* rotCamXAxixSpeed;//���콺 ��/�� �̵����� ī�޶� Y�� ȸ��
        eulerAngleX -= mouseY* rotCamYAxixSpeed;//���콺 ��/�� �̵����� ī�޶� X�� ȸ��

        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);//ī�޶� X���� ȸ�� ������ ����
        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }
    public float ClampAngle(float angle, float min , float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);

    }

}
