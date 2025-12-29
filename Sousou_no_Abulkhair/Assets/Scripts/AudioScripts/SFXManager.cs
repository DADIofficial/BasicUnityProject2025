using System;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    Footstep,
    Attack,
    Explosion,
    Ench_attack
}

[Serializable]
public struct SFXItem
{
    public SFXType type;
    public AudioClip clip;
}

public class SFXManager : MonoBehaviour
{
    private const string PREF_KEY = "SFX_VOLUME";

    [SerializeField] private AudioSource audioSource;
    public List<SFXItem> sfxList = new();

    public float Volume => audioSource != null ? audioSource.volume : 1f;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError($"{nameof(SFXManager)}: На объекте нет AudioSource.");
            enabled = false;
            return;
        }

        SetVolume(PlayerPrefs.GetFloat(PREF_KEY, 1f));
    }

    public void SetVolume(float value01)
    {
        if (audioSource == null) return;

        value01 = Mathf.Clamp01(value01);
        audioSource.volume = value01;

        PlayerPrefs.SetFloat(PREF_KEY, value01);
        PlayerPrefs.Save();
    }

    public void PlaySFX(SFXType type)
    {
        if (audioSource == null) return;

        for (int i = 0; i < sfxList.Count; i++)
        {
            if (sfxList[i].type == type && sfxList[i].clip != null)
            {
                audioSource.PlayOneShot(sfxList[i].clip);
                return;
            }
        }

        Debug.LogWarning($"{nameof(SFXManager)}: Не найден клип для {type}");
    }
}
