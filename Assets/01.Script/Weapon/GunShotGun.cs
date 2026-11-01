using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class GunShotGun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading,
    }

    public State CurrentState { get; private set; }

    public Transform fireTransform; //발사위치

    public ParticleSystem muzzleFlashEffect;
    //public ParticleSystem shellEjectEffect;

    private LineRenderer bulletLineRenderer;

    private AudioSource gunAudioPlayer;

    public GunData gunData;
    private float fireDistance = 50f;
    private int remainAmmo; //남은탄약
    private int currentAmmo; //지금탄약

    private float lastFireTime;

    public UIManager uiManager;

    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        remainAmmo = gunData.startAmmoRemain;
        currentAmmo = gunData.magCapacity;
        uiManager.SetAmmoText(currentAmmo, remainAmmo);

        CurrentState = State.Ready;
        lastFireTime = 0f;

    }

    private void OnDisable()
    {
        Debug.LogWarning("GunNormal 비활성화됨", this);
    }


    //총알 발사 자체를 호출하는 중간과정
    public void Fire()
    {
        if (CurrentState == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            lastFireTime = Time.time;
            Shoot();
        }

    }

    public LayerMask targetLayer;

    private void Shoot()
    {
        // 좌우로 퍼질 발사 각도
        float[] angles = { -10f, -5f, 0f, 5f, 10f };

        foreach (float angle in angles)
        {
            // 총구의 앞 방향을 Y축 기준으로 회전
            Quaternion rotation = Quaternion.Euler(
                0f,
                angle,
                0f
            );

            Vector3 direction = rotation * fireTransform.forward;

            // //총구 위치와 방향으로 Ray 생성
            Ray ray = new Ray(fireTransform.position, fireTransform.forward);


            if (Physics.Raycast(ray, out RaycastHit hit, fireDistance, targetLayer))
            {

                //③ IDamageable 가져오기 맞은 물체가 데미지를 받을 수 있는지 확인
                IDamageable target = hit.collider.GetComponent<IDamageable>();
                if (target != null)
                {
                    //④ target.OnDamage() 데미지 수치와 맞은 위치·방향 전달
                    target.OnDamage(gunData.damage, hit.point, hit.normal);
                }

                StartCoroutine(ShootEffect(hit.point));

                currentAmmo-=5;
                uiManager.SetAmmoText(currentAmmo, remainAmmo);

                if (currentAmmo == 0)
                {
                    CurrentState = State.Empty;
                }
            }
        }






    }

    private IEnumerator ShootEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        //shellEjectEffect.Play();
        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false;
    }


    public bool Reload()
    {
        if (CurrentState == State.Reloading || remainAmmo <= 0 || currentAmmo >= gunData.magCapacity)
        {
            return false;
        }
        StartCoroutine(Reloading());
        return true;
    }

    private IEnumerator Reloading()
    {
        CurrentState = State.Reloading;
        gunAudioPlayer.PlayOneShot(gunData.reloadClip);
        yield return new WaitForSeconds(gunData.reloadTime);

        int ammoToFill = Mathf.Min(gunData.magCapacity - currentAmmo, remainAmmo);

        currentAmmo += ammoToFill;
        remainAmmo -= ammoToFill;

        uiManager.SetAmmoText(currentAmmo, remainAmmo);

        CurrentState = State.Ready;
    }










    //총알아이템 획득
    public void AddAmmo(int add)
    {
        remainAmmo += add;
        //uiManager
    }

}
