using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Health_Manager : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;
    // Start is called before the first frame update
    void Start(){
        currentHealth = maxHealth;
    }
}
