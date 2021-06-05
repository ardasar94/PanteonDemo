using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollView : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    Vector3 mouseStartPos;
    Vector3 mouseCurrentPos;

    int listCount;
    List<Transform> scrollViewItems;
    // Start is called before the first frame update
    void Start()
    {
        scrollViewItems = new List<Transform>();
        listCount = transform.childCount;

        for (int i = 0; i < listCount; i++)
        {
            scrollViewItems.Add(transform.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeLocations();
    }


    // To get mouse position on canvas. 
    public void OnDrag(PointerEventData eventData)
    {
        mouseCurrentPos = Input.mousePosition;

        // Change scroll view items according to mouse position.
        foreach (var item in scrollViewItems)
        {
            item.position = new Vector3(item.transform.position.x, item.transform.position.y + (mouseCurrentPos.y / 30 - mouseStartPos.y / 30), item.transform.position.z);
        }
    }

    // To get mouse position on canvas when gragging begin.
    public void OnBeginDrag(PointerEventData eventData)
    {
        mouseStartPos = Input.mousePosition;
    }

    // Change scroll view items order to make infinite scroll view.
    void ChangeLocations()
    {
        if (scrollViewItems != null)
        {
            var listItem1 = scrollViewItems.FirstOrDefault(x => x.transform.position.y <= -120);
            var listItem2 = scrollViewItems.FirstOrDefault(x => x.transform.position.y >= 625f);


            if (listItem1 != null)
            {
                listItem1.position = new Vector3(listItem1.position.x, 624.9f, listItem1.position.z);
            }

            if (listItem2 != null)
            {
                listItem2.position = new Vector3(listItem2.position.x, -119.9f, listItem2.position.z);
            }
        }
    }
}
