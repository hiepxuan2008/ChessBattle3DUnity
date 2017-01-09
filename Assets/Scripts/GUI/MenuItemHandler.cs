using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuItemHandler : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    AudioClip soundEffect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<AudioSource>().PlayOneShot(soundEffect);
    }
}
