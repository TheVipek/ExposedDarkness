using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionContainer : MonoBehaviour,IInteractionActions
{
    public abstract void OnInteractionStart();
}
