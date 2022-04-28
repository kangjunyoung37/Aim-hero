using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandGun : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioTakeOutWeapon;

    private AudioSource audioSource;
    private PlayeranimatorController animator;

    [SerializeField]
    private WeaponSetting weaponSetting;

    [SerializeField]
    private GameObject muzzleFalsh;
    [SerializeField]
    private AudioClip FireSound;

    private float lastAttackTime = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
        animator = GetComponentInParent<PlayeranimatorController>();

    }
 
    public void StartWeaponAction(int type = 0)
    {
        //���콺 ���� Ŭ��
        if (type == 0)
        {
            if(weaponSetting.isAutomaticAttack == true)//����
            {
                StartCoroutine("OnAttackLoop");
            }
            else//�ܹ�
            {
                OnAttack();
            }
        }
    }
    public void StopWeaponAction(int type = 0)
    {
        if(type == 0)//���� ����
        {
            StopCoroutine("OnAttackLoop");
        }
    }
    private IEnumerator OnAttackLoop()//�ڵ�����
    {
        while (true)
        {
            OnAttack();
            yield return null;
        }

    }
    private void OnAttack()
    {
        if(Time.time-lastAttackTime > weaponSetting.attackRate)
        {
            if(animator.MoveSpeed > 0.5f)//�ٰ� ���� �� ���� X
            {
                return;
            }

            lastAttackTime = Time.time;//�����ֱⰡ �Ǿ�� ������ �� �ֵ��� ���� �ð� ����
            animator.Play("Fire", -1, 0);//���� �ִϸ��̼��� �ݺ��� �� �ִϸ��̼��� ���� ó������ ���
            PlaySound(FireSound);
            StartCoroutine("OnMuzzel");
        }
    }
    private IEnumerator OnMuzzel()
    {
        muzzleFalsh.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.attackRate*0.3f);
        muzzleFalsh.SetActive(false);
    }
    private void OnEnable()
    {
        muzzleFalsh.SetActive(false);//�ѱ� ȭ���� �Ⱥ��̰�
        PlaySound(audioTakeOutWeapon);
    }
    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
 

}
