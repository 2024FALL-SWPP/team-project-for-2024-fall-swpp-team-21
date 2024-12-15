using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHitEffect : HitEffect
{
    private PlayerController player;

    public PlayerHitEffect(GameObject gameObject) : base(gameObject)
    {
        this.player = gameObject.GetComponent<PlayerController>();

        player.statEventCaller.StatChangedHandler += OnPlayerStatChange;

        Volume volume = GameObject.FindObjectOfType<Volume>();
        volume.profile.TryGet(out vignette);
    }

    // 화면 비네팅
    private static float _vignetteThresholdHP = 0.5f;
    private static float _temporaryIntensity = 0.5f;
    private static float _intensityUpwardTime = 4f / 60f;
    private static float _intensityResolveTime = 30f / 60f;
    private static float _permanentIntensityMax = 0.8f;
    private Vignette vignette;
    private Coroutine vignetteCoroutine;

    public void OnPlayerStatChange(object o, StatChangedEventArgs args)
    {
        if (args.StatName == nameof(PlayerStat.CurrentHP))
        {
            PlayVignette();
            base.Play();
        }
        else if (args.StatName == nameof(PlayerStat.MaxHP))
        {
            SetVignette(GetIntensity(player.playerStat.CurrentHP / player.playerStat.MaxHP));
        }
    }

    private float GetIntensity(float hpRatio)
    {
        if (hpRatio > _vignetteThresholdHP) return 0;
        return Mathf.Lerp(_permanentIntensityMax, 0.3f, hpRatio / _vignetteThresholdHP);
    }

    private void PlayVignette()
    {
        float hpRatio = player.playerStat.CurrentHP / (float) player.playerStat.MaxHP;
        Debug.Log("PlayVignette" + hpRatio);
        if (vignetteCoroutine != null)
        {
            player.StopCoroutine(vignetteCoroutine);
        }
        vignetteCoroutine = player.StartCoroutine(TemporaryVignette(GetIntensity(hpRatio)));
    }

    private void SetVignette(float intensity)
    {
        vignette.intensity.value = intensity;
    }

    private IEnumerator TemporaryVignette(float offsetIntensity)
    {

        Debug.Log("TemporaryVignette" + offsetIntensity);
        float elapsedTime = 0;
        while (elapsedTime < _intensityUpwardTime)
        {
            elapsedTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(offsetIntensity, offsetIntensity + _temporaryIntensity, elapsedTime / _intensityUpwardTime);
            yield return null;
        }

        elapsedTime = 0;

        while (elapsedTime < _intensityResolveTime)
        {
            elapsedTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(offsetIntensity + _temporaryIntensity, offsetIntensity, elapsedTime / _intensityResolveTime);
            yield return null;
        }

        SetVignette(offsetIntensity);
    }

}
