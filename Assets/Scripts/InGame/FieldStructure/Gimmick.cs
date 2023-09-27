using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public struct Gimmick
{
    private const string BEAM_PATH = "Prefabs/InGame/FieldStructure/";

    public static void GimmickActivate(Transform transform, List<GimmickEnum> gimmicks, List<string> param)
    {
        int i = 0; //param 참조 순서 (기믹마다 개수가 달라서 사용)

        foreach (GimmickEnum gimmick in gimmicks)
        {
            switch (gimmick)
            {
                case GimmickEnum.DAMAGE:
                    break;
                case GimmickEnum.BEAMLINE:
                    Beam beamLine = GameObject.Instantiate(ResourcesManager.Load<Beam>(BEAM_PATH + "BeamLine"), transform);
                    beamLine.BeamInit(float.Parse(param[i]), float.Parse(param[++i]));
                    break;
                case GimmickEnum.BEAMCIRCLE:
                    Beam beamCircle = GameObject.Instantiate(ResourcesManager.Load<Beam>(BEAM_PATH + "BeamCircle"), transform);
                    beamCircle.BeamInit(float.Parse(param[i]), float.Parse(param[i]));
                    break;
                case GimmickEnum.BEAMCROSS:
                    Beam beamCrossFirst = GameObject.Instantiate(ResourcesManager.Load<Beam>(BEAM_PATH + "BeamLine"), transform);
                    Beam beamCrossSecond = GameObject.Instantiate(ResourcesManager.Load<Beam>(BEAM_PATH + "BeamLine"), transform);
                    beamCrossFirst.BeamInit(float.Parse(param[i]), float.Parse(param[i + 1]));
                    beamCrossSecond.BeamInit(float.Parse(param[i + 1]), float.Parse(param[i++]));
                    break;
                default:
                    DebugManager.Instance.PrintWarning("[Gimmick]: 미구현된 기믹입니다");
                    break;
            }
            ++i;
        }
    }

}
