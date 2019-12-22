using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public GameObject greenPointLightPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        MouseCast();

        ShowPointLight();
    }

    private void MouseCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            transform.position = hit.point;
        }
    }

    private void ShowPointLight()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Ground")
                {
                    Instantiate(greenPointLightPrefab, transform.position + (transform.up * 0.2f), Quaternion.identity);
                }

                if (hit.collider.gameObject.tag == "Bomb")
                {
                    hit.collider.GetComponent<Bomb>().ShowPointLight();
                }
            }
        }     
    }


}
