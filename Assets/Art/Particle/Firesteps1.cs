using UnityEngine;
using System.Collections;

public class Firesteps1 : MonoBehaviour
{
    private ParticleSystem ps;
    public float fireStep = 0.5f;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        var emission = ps.emission;
        emission.rateOverTime = fireStep;
    }

}