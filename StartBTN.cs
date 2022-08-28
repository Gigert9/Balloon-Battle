using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartBTN : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler , IPointerClickHandler
{
    public Text pauseText;

    public AudioSource buttonClick;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Is Highlighted!");

        pauseText.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pauseText.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        buttonClick.Play();
        SceneManager.LoadScene("SampleScene");
    }
}
