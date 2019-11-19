using UnityEngine;
using System.Collections;

public class Firesteps1 : MonoBehaviour
{
    private ParticleSystem ps;
    public float fireStep = 0f;
    float getValuefromCharacter;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        getValuefromCharacter = CustomCharacterController.particleVariable;
        var emission = ps.emission;
        emission.rateOverTime = getValuefromCharacter;
    }

}