using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    public static VFX instance { get; private set; }

    ParticleSystem leftParticle;
    ParticleSystem rightParticle;
    private void Awake()
    {
        instance = this;
        leftParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
        rightParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
    }
    public void ActiveVFX()
    {
        leftParticle.Play();
        rightParticle.Play();
    }
}
