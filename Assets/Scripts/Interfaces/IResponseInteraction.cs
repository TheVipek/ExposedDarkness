using UnityEngine;

public interface IResponseInteraction
{
    void OnSelect(GameObject _lookingAt);
    void OnDeselect();
}
