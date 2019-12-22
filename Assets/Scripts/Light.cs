using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestoryLight());
    }

    void Update()
    {
        
    }

    private IEnumerator DestoryLight()
    {
        yield return new WaitForSeconds(0.25f);

        Destroy(this.gameObject);
    }


}
