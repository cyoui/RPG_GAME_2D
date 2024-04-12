using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private Player player;

    private SpriteRenderer sr;

    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpTextPrefab;

    [Header("Screen shake Fx")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDamage;


    [Header("After img")]
    [SerializeField] private float afterImgCooldown;
    [SerializeField] private GameObject afterImgPrefab;
    [SerializeField] private float colorLooseRate;
    private float afterImageTimer;


    [Header("Flash FX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("Ailment Colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Aliment particles")]
    [SerializeField] private ParticleSystem igniteFx;
    [SerializeField] private ParticleSystem chillFx;
    [SerializeField] private ParticleSystem shockFx;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFx;
    [SerializeField] private GameObject critHitFx;

    [Space]
    [SerializeField] private ParticleSystem dustFx;
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        player = PlayerManager.instance.player;
        screenShake = GetComponent<CinemachineImpulseSource>();
        originalMat = sr.material;
    }

    private void Update()
    {
        afterImageTimer -= Time.deltaTime;
    }

    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(3, 5);

        Vector3 positionOffset = new Vector3(randomX, randomY, 0);

        GameObject newText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();

    }


    public void CreateAfterImage()
    {
        if(afterImageTimer < 0)
        {
            afterImageTimer = afterImgCooldown;
            GameObject newAfterImg = Instantiate(afterImgPrefab, transform.position, transform.rotation);
            newAfterImg.GetComponent<AfterImageFx>().SetupAfterImg(colorLooseRate, sr.sprite);
        }
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        sr.material = originalMat;
    }

    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    private void RedColorBlink()
    {
        if(sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;

        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
    }

    public void IgniteFxFor(float _seconds)
    {
        igniteFx.Play();

        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockFxFor(float _seconds)
    {
        shockFx.Play();

        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ChillFxFor(float _seconds)
    {
        chillFx.Play();

        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }


    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }


    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }


    public void CreateHitFx(Transform _target, bool _crit)
    {
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFxRotation = new Vector3(0, 0, zRotation);

        GameObject hitPrefab = hitFx;

        if (_crit)
        {
            hitPrefab = critHitFx;

            float yRotation = 0;
            zRotation = Random.Range(-45, 45);

            if (GetComponent<Entity>().facingDir == -1)
                yRotation = 180;

            hitFxRotation = new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitFx = Instantiate(hitPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);

        newHitFx.transform.Rotate(hitFxRotation);

        Destroy(newHitFx, .5f);
    }

    public void PlayDustFx()
    {
        if(dustFx != null)
            dustFx.Play();
    }


}
