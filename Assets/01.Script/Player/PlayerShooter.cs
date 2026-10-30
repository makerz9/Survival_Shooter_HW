using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public static readonly int IdReload = Animator.StringToHash("Reload");

    public GunNormal gun;

    public Transform gunPivot;

    private Animator playerAnimator;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }


    void Start()
    {
        
    }


    void Update()
    {
        if(playerInput.Fire)
        {
            gun.Fire();
        }    
        else if (playerInput.Reload && gun.Reload())
        {
            playerAnimator.SetTrigger(IdReload);
        }

    }
}
