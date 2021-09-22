﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{

    [SerializeField] private int _cost;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();

        if (player != null)
        {
            player.AddMoney(_cost);
            Destroy(gameObject);
        }


    }

}
