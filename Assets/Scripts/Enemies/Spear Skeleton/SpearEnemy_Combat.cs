using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearEnemy_Combat : MonoBehaviour, IEnemyCombat, IDealDamage
{
    public bool blocking { get; set; }

    [SerializeField] private Transform player;
    [HideInInspector] public float playerDistance;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void FixedUpdate() {        
        playerDistance = Mathf.Abs(transform.position.x - player.position.x);
        if (playerDistance < 2 && GetComponent<SpearEnemy_Moviment>().hasSight == true) {
            GetComponent<SpearEnemy_Moviment>().enabled = false;
        }
        else {
            GetComponent<SpearEnemy_Moviment>().enabled = true;
        }
    }
    public IEnumerator Attack() {
        yield return new WaitForSeconds(0);
    }

    public void DealDamage() {
        
    }
}
