using UnityEngine;
using DG.Tweening;

public class ObjectFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float duration = 0.1f;

    private void Update()
    {
        if (target == null) return;

        //
        transform.DOMove(target.position, duration).SetEase(Ease.Linear);
    }



}
