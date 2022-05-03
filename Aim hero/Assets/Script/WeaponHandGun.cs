using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }
public class WeaponHandGun : MonoBehaviour
{
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioTakeOutWeapon;
    [SerializeField]
    private AudioClip audioClipReload;

    [SerializeField]
    private WeaponSetting weaponSetting;
    public WeaponName WeaponName => weaponSetting.weaponname;

    [SerializeField]
    private GameObject muzzleFalsh;
    
    [SerializeField]
    private AudioClip FireSound;

    [Header("Spawn Point")]
    [SerializeField]
    private Transform casingSpawnPoint;
   
    private AudioSource audioSource;
    private PlayeranimatorController animator;
    private CasingMemoryPool casingMemorypool;


    private float lastAttackTime = 0;
    private bool isReload = false;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
        animator = GetComponentInParent<PlayeranimatorController>();
        casingMemorypool = GetComponent<CasingMemoryPool>();
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;

    }

    private void OnEnable()
    {
        muzzleFalsh.SetActive(false);//�ѱ� ȭ���� �Ⱥ��̰�
        PlaySound(audioTakeOutWeapon);
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo); //������ ź���� ����
    }
    public void StartWeaponAction(int type = 0)
    {
        if (isReload) return;
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
    public void StartReload()
    {
        if(isReload) return;
        StopWeaponAction();
        StartCoroutine("OnReload");
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
            if (weaponSetting.currentAmmo <= 0)
            {
                return;
            }
            weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.currentAmmo,weaponSetting.maxAmmo);
            animator.Play("Fire", -1, 0);//���� �ִϸ��̼��� �ݺ��� �� �ִϸ��̼��� ���� ó������ ���
            PlaySound(FireSound);
            StartCoroutine("OnMuzzel");
            casingMemorypool.SpawnCasing(casingSpawnPoint.position, transform.right);
        }
    }
    private IEnumerator OnReload()
    {
        isReload = true;

        animator.OnReload();
        PlaySound(audioClipReload);
        while (true)
        {
            if(audioSource.isPlaying == false && animator.CurrentAnimationIs("Movement"))
            {
                isReload =false;

                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
                yield break;
            }       
        }
        yield return null;
    }
    private IEnumerator OnMuzzel()
    {
        muzzleFalsh.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.attackRate*0.3f);
        muzzleFalsh.SetActive(false);
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
 

}
