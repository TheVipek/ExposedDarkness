using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionContainer : ScriptableObject,IInteractionActions
{
    public abstract void OnInteractionStart();
}
