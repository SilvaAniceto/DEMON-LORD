using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyCombat 
{
    bool blocking { get; set; }  

    IEnumerator Attack();
}
