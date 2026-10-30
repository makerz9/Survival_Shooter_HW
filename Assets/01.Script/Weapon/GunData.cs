using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip;
    public AudioClip reloadClip;

    public float damage = 10f;
    public int startAmmoRemain = 60;
    public int magCapacity = 30;
    public float timeBetFire = 0.10f;
    public float reloadTime = 1.5f;
}
