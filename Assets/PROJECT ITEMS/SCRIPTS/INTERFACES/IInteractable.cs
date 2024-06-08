using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttachable
{
    // Start is called before the first frame update
    public void Attach(Vector3 attachmentPoint);
    public void Interact(bool state);
}
