using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Gimmick
{
    public static void GimmickActivate(List<GimmickEnum> gimmicks, List<float> param)
    {
        int i = 0; //param 참조 순서 (기믹마다 개수가 달라서 사용)
        foreach (GimmickEnum gimmick in gimmicks)
        {
            switch (gimmick)
            {
                case GimmickEnum.DAMAGE:
                    break;
                //case GimmickEnum.DAMAGE:
                //    break;
                //case GimmickEnum.DAMAGE:
                //    break;
                //case GimmickEnum.DAMAGE:
                //    break;
                case GimmickEnum.BEAMWIDTH:
                    break;
                case GimmickEnum.BEAMVERTICAL:
                    break;
                case GimmickEnum.BEAMCIRCLE:
                    break;
                case GimmickEnum.BEAMCROSS:
                    break;
                //case GimmickEnum.DAMAGE:
                //    break;
                //case GimmickEnum.DAMAGE:
                //    break;
                //case GimmickEnum.DAMAGE:
                //    break;
                //case GimmickEnum.DAMAGE:
                //    break;
                //case GimmickEnum.DAMAGE:
                //    break;
                default:
                    DebugManager.Instance.PrintWarning("[Gimmick]: 존재하지 않는 기믹 효과입니다");
                    break;
            }
        }
    }
}
