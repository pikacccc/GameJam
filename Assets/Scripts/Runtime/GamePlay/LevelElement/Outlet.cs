using System;
using Runtime.GamePlay.Level;
using UnityEngine;

namespace Runtime.GamePlay.LevelElement
{
    public class Outlet : LevelElementBase
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Racket"))
            {
                LevelManager.Instance.EndLevel(true);
                ;
            }
        }
    }
}