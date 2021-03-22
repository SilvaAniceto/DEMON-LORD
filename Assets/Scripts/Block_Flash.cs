using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Flash : MonoBehaviour
{    
    void Update()
    {
        StartCoroutine("Destroy");
    }     
    IEnumerator Destroy() {
        yield return new WaitForSeconds(0.4f);

        Destroy(gameObject);
    }
}
