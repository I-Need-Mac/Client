using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Gimmick
{
    public static void GimmickActivate(List<GimmickEnum> gimmicks, List<string> param)
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
                    Beam beamWidth = GameObject.Instantiate(ResourcesManager.Load<BeamWidth>("Prefabs/InGame/FieldStructure/BeamWidth"));
                    beamWidth.BeamActivation(float.Parse(param[i]), float.Parse(param[++i]));
                    break;
                case GimmickEnum.BEAMVERTICAL:
                    Beam beamVertical = GameObject.Instantiate(ResourcesManager.Load<BeamWidth>("Prefabs/InGame/FieldStructure/BeamWidth"));
                    beamVertical.BeamActivation(float.Parse(param[i]), float.Parse(param[++i]));
                    break;
                case GimmickEnum.BEAMCIRCLE:
                    Beam beamCircle = GameObject.Instantiate(ResourcesManager.Load<BeamWidth>("Prefabs/InGame/FieldStructure/BeamWidth"));
                    beamCircle.BeamActivation(float.Parse(param[i]), 0.0f);
                    break;
                case GimmickEnum.BEAMCROSS:
                    Beam beamW = GameObject.Instantiate(ResourcesManager.Load<BeamWidth>("Prefabs/InGame/FieldStructure/BeamWidth"));
                    Beam beamV = GameObject.Instantiate(ResourcesManager.Load<BeamWidth>("Prefabs/InGame/FieldStructure/BeamWidth"));
                    beamW.BeamActivation(float.Parse(param[i]), float.Parse(param[i + 1]));
                    beamV.BeamActivation(float.Parse(param[i]), float.Parse(param[i + 1]));
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
                    DebugManager.Instance.PrintWarning("[Gimmick]: 미구현된 기믹입니다");
                    break;
            }
            ++i;
        }
    }

}
