using System;
using UnityEngine;

public class LanderVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThruster;
    [SerializeField] private ParticleSystem middleThruster;
    [SerializeField] private ParticleSystem rightThruster;
    private Lander lander;

    private void Awake()
    {
        lander = GetComponent<Lander>();

        SetEnableThruster(leftThruster, false);
        SetEnableThruster(middleThruster, false);
        SetEnableThruster(rightThruster, false);

        lander.onUpForce += Lander_onUpForce;
        lander.onLeftForce += Lander_onLeftForce;
        lander.onRightForce += Lander_onRightForce;
        lander.onBeforeForce += Lander_onBeforeForce;
    }

    private void Lander_onBeforeForce(object sender, EventArgs e)
    {
        SetEnableThruster(leftThruster, false);
        SetEnableThruster(middleThruster, false);
        SetEnableThruster(rightThruster, false);
    }

    private void Lander_onRightForce(object sender, EventArgs e)
    {
        SetEnableThruster(leftThruster, true);
        SetEnableThruster(middleThruster, false);
        SetEnableThruster(rightThruster, false);
    }

    private void Lander_onLeftForce(object sender, EventArgs e)
    {
        SetEnableThruster(leftThruster, false);
        SetEnableThruster(middleThruster, false);
        SetEnableThruster(rightThruster, true);
    }

    private void Lander_onUpForce(object sender, System.EventArgs e)
    {
        SetEnableThruster(leftThruster, true);
        SetEnableThruster(middleThruster, true);
        SetEnableThruster(rightThruster, true);
    }

    private void SetEnableThruster(ParticleSystem thruster, bool enabled)
    {
        ParticleSystem.EmissionModule emissionModule = thruster.emission;
        emissionModule.enabled = enabled;
    }
}