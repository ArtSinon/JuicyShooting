using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private ParticleSystem smallParticles, hugeParticles;
    
    [Header("Popup")]
    [SerializeField] private HitPopup hitPopup;

    [SerializeField] private Color colorOnWeakPoint;
    //[SerializeField] private string textOnWeakPoint;
    [SerializeField] private Color colorOnNormalPoint;
    //[SerializeField] private string textOnNormalPoint;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
        _animator.SetInteger("AnimationNumber", Random.Range(1, 4));
    }

    public void MakeVisualEffectsOnHit(bool weakPoint, Vector3 pos)
    {
        SpreadParticles(weakPoint, pos);
        ShowHitPopup(weakPoint, pos);
    }

    private void SpreadParticles(bool weakPoint, Vector3 pos)
    {
        Instantiate(weakPoint ? hugeParticles : smallParticles, pos, Quaternion.identity);
    }


    private void ShowHitPopup(bool weakPoint, Vector3 pos)
    {
        if (HitPopup.PopupPool == null)
        {
            HitPopup.SetupPool(hitPopup);
        }

        var popup = HitPopup.PopupPool.Get();
        popup.transform.position = pos;
        popup.SetupPopup(weakPoint ? Color.red : Color.white);
    }
}
