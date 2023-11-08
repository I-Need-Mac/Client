using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRequesterBtn : SoundRequesterSFX
{

    bool isNagative = true;

    public void SetIsNagative(bool isNagative) { 
        this.isNagative = isNagative;
    }
    private void OnMouseDown()
    {
        DebugManager.Instance.PrintDebug("SoundRequest : Press");
        if (isNagative) { 
            this.ChangeSituation(SoundSituation.SOUNDSITUATION.PRESS);
        }
        else {
            this.ChangeSituation(SoundSituation.SOUNDSITUATION.NAGATIVE);

        }
    }

    private void OnMouseEnter()
    {
        DebugManager.Instance.PrintDebug("SoundRequest : Enter");
        this.ChangeSituation(SoundSituation.SOUNDSITUATION.ENTER);

    }

    private void OnMouseUp()
    {
        DebugManager.Instance.PrintDebug("SoundRequest : Relese");
        this.ChangeSituation(SoundSituation.SOUNDSITUATION.RELEASE);
    }

    private void OnMouseExit()
    {
        DebugManager.Instance.PrintDebug("SoundRequest : OUT");
        this.ChangeSituation(SoundSituation.SOUNDSITUATION.OUT);
    }
    private void OnMouseOver()
    {
        //DebugManager.Instance.PrintDebug("SoundRequest : Hover");
        this.ChangeSituation(SoundSituation.SOUNDSITUATION.HOVER);
    }
}
