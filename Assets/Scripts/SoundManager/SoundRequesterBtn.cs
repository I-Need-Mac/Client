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


}
