using BFM;
using SKILLCONSTANT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulManager : SingleTon<SoulManager>
{
    private List<Soul> soulList;
    private Dictionary<SoulEffect, float> soulEffects;

    public SoulManager()
    {
        soulList = new List<Soul>();
        soulEffects = new Dictionary<SoulEffect, float>();
    }

    public void Add(Soul soul)
    {
        soulList.Add(soul);
        for (int i = 0; i < soul.soulData.soulEffects.Count; i++)
        {
            if (soulEffects.ContainsKey(soul.soulData.soulEffects[i]))
            {
                soulEffects[soul.soulData.soulEffects[i]] += soul.soulData.effectParams[i];
            }
            else
            {
                soulEffects.Add(soul.soulData.soulEffects[i], soul.soulData.effectParams[i]);
            }
        }
    }

    //Default Mode: Plus
    //Plus: return 0
    //Multi: return 1
    public float GetEffect(SoulEffect soulEffect, float value)
    {
        return this.GetEffect(soulEffect, CALC_MODE.PLUS, value);
    }

    public float GetEffect(SoulEffect soulEffect, CALC_MODE mode, float value)
    {
        if (soulEffects.ContainsKey(soulEffect))
        {
            return mode == CALC_MODE.PLUS ? soulEffects[soulEffect] : soulEffects[soulEffect] * value;
        }

        return 0.0f;
    }
}
