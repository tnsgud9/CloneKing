using System;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] private List<GameObject> players;
        public static GameManager instance;
    
        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        public void AddPlayer(GameObject player)
        {
            players.Add(player);
        }

    }
}
