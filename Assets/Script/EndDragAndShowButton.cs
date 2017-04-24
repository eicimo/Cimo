using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDragAndShowButton : MonoBehaviour
{
    public Book book;
    public GameObject Master;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDragEndAndShowButton()
    {

        StartCoroutine(WaitAndShowLog());
    }

    IEnumerator WaitAndShowLog()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("show!" + book.currentPage.ToString());
        Master.SetActive(true);
    }

    public void OnMouseDragBeginAndHideButton()
    {
        Master.SetActive(false);
    }
}
