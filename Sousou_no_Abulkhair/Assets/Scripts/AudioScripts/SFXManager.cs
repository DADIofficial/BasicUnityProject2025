using System;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    Footstep,
    Attack,
    FireExplosion

}

[Serializable]
public struct SFXItem
{
    public SFXType type;
    public AudioClip clip;
}

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public List<SFXItem> sfxList = new();

    public void PlaySFX(SFXType type)
    {
        if (audioSource == null) return;

        // »щем первый клип с нужным типом
        for (int i = 0; i < sfxList.Count; i++)
        {
            if (sfxList[i].type == type)
            {
                var clip = sfxList[i].clip;
                if (clip == null)
                {
                    Debug.LogWarning($"{nameof(SFXManager)}: No {type}");
                    return;
                }

                audioSource.PlayOneShot(clip);
                return;
            }
        }

        Debug.LogWarning($"{nameof(SFXManager)}: Didn't find {type}");
    }
}
