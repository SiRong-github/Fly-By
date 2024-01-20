using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private string powerupName;

    public string Name {
        get {
            return powerupName;
        }
    }
}