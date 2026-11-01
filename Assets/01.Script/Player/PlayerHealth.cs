using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public static readonly int IdDie = Animator.StringToHash("Die");

    //체력 있고


    public ParticleSystem hitEffect;
    public Collider playerCollider;
    public Renderer playerRenderer;
    public AudioClip hitClip;
    public AudioClip deathClip;

    public AudioSource playerAudioSource;

    //맞다 다른 컴포넌트 끄자
    private PlayerController playerController;
    private PlayerShooter playerShooter;
    private Animator playerAnimator;
    private PlayerInput playerInput;
    private LookAtMouse lookAtMouse;

    public UIManager uiManager;
    public Image imagePlayerHP;

    //재장전엔 그냥 모션 넣지 말죠
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerShooter = GetComponent<PlayerShooter>();
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerAudioSource = GetComponent<AudioSource>();
        lookAtMouse = GetComponent<LookAtMouse>();
    }


    protected override void OnEnable()
    {
        base.OnEnable();

        playerController.enabled = true;
        playerInput.enabled = true;

        imagePlayerHP.fillAmount = Health / startingHealth;
        uiManager.SetHpText(Health, startingHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
        {

            return;
        }
        base.OnDamage(damage, hitPoint, hitNormal);

        
        playerAudioSource.PlayOneShot(hitClip);
        hitEffect.transform.position = hitPoint;
        hitEffect.transform.forward = hitNormal;
        hitEffect.Play();

        imagePlayerHP.fillAmount = Health / startingHealth;
        uiManager.SetHpText(Health, startingHealth);
    }

    public override void Die()
    {
        base.Die();
        playerAudioSource.PlayOneShot(deathClip);
        playerController.enabled = false;
        playerShooter.enabled = false;
        lookAtMouse.enabled = false;
        playerAnimator.SetTrigger("isDead");
        uiManager.SetActiveGameOverUI(true);
    }
}
