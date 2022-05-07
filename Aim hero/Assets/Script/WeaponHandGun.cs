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
    [SerializeField]
    private Transform bulletSpawnPoint;
   
    private AudioSource audioSource;
    private PlayeranimatorController animator;
    private CasingMemoryPool casingMemorypool;
    private ImpactMemoryPool impactMemoryPool;
    private Camera mainCamera;



    private float lastAttackTime = 0;
    private bool isReload = false;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
        animator = GetComponentInParent<PlayeranimatorController>();
        casingMemorypool = GetComponent<CasingMemoryPool>();
        weaponSetting.currentAmmo = weaponSetting.HandGunMagazine;
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        mainCamera = Camera.main;
        

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
        if(weaponSetting.HandGunMagazine == weaponSetting.currentAmmo ||weaponSetting.maxAmmo <=0) return;
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
            TwoStepRaycast();
        }
    }
    private IEnumerator OnReload()
    {
        int fillammo = weaponSetting.HandGunMagazine - weaponSetting.currentAmmo;
        isReload = true;
        animator.OnReload();
        PlaySound(audioClipReload);
        yield return new WaitForSeconds(1.2f);
        if (fillammo > weaponSetting.maxAmmo)
        {
            weaponSetting.currentAmmo +=weaponSetting.maxAmmo;
            weaponSetting.maxAmmo = 0;
        }
        else
        { 
            weaponSetting.currentAmmo = weaponSetting.HandGunMagazine;
            weaponSetting.maxAmmo = weaponSetting.maxAmmo - fillammo;
        }

        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

        while (true)
        {     
            if (audioSource.isPlaying == false && animator.CurrentAnimationIs("Movement"))
            {
                isReload =false;
                
                yield break;
            }
            yield return null;
        }
        
    }
    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        if(Physics.Raycast(ray,out hit, weaponSetting.attackDistance))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin+ray.direction*weaponSetting.attackDistance;
        }

        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;
        if(Physics.Raycast(bulletSpawnPoint.position,attackDirection,out hit, weaponSetting.attackDistance))
        {
            impactMemoryPool.SpawnImpact(hit);
        }
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
