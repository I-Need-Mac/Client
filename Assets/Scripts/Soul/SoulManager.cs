using BFM;
using SKILLCONSTANT;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SoulManager : SingletonBehaviour<SoulManager>
{
    private List<Soul> soulList;
    private Dictionary<SoulEffect, float> soulEffects;

    protected override void Awake()
    {
        base.Awake();
        soulList = new List<Soul>();
        soulEffects = new Dictionary<SoulEffect, float>();
    }

    public void Add(Soul soul)
    {
        try
        {
            foreach (Soul s in soulList)
            {
                if (s.soulData.soulId == soul.soulData.soulId)
                {
                    DebugManager.Instance.PrintError("[SoulManager] 이미 장착된 혼 입니다. (SoulID: {0})", soul.soulData.soulId);
                    return;
                }
            }
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
        catch
        {
            DebugManager.Instance.PrintError("[Error: SoulManager] 혼 테이블을 체크해 주세요. (SoulID: {0})", soul.soulData.soulId);
        }
        
    }

    public void PrintSoulList()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Soul soul in soulList)
        {
            sb.Append(soul.soulData.soulId);
            sb.Append(", ");
        }
        sb.Remove(sb.Length - 2, 2);
        DebugManager.Instance.PrintDebug(sb.ToString());
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
