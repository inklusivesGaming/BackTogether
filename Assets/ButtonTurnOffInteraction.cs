using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTurnOffInteraction : MonoBehaviour, IDeselectHandler
{
    private bool mDeselected = false;

    public void OnDeselect(BaseEventData eventData)
    {
        mDeselected = true;
    }

    private void Update()
    {
        // doing this in the update function to prevent errors
        if (mDeselected)
            GetComponent<Button>().interactable = false;
    }
}
