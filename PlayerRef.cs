using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRef : MonoBehaviour
{
    #region Singleton
    public static PlayerRef instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject player;
}
