using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttachable
{
    // Start is called before the first frame update
    public void Attach(Transform attachChild, Vector3 attachPoint);
    public void Detach(Transform detachChild);
    public void Interact(bool state);
}
