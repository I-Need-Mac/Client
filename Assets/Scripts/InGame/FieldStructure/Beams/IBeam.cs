using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBeam
{
    public void BeamActivation(float n);
    public IEnumerator Run();
    public void DestroyBeam();
}
