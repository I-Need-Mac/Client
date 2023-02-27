using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRequesterBtn : SoundRequester
{



    private void OnMouseDown()
    {
        DebugManager.Instance.PrintDebug("SoundRequest : Press");
        this.ChangeSituation(SoundSituation.SOUNDSITUATION.PRESS); 
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
