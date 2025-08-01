using UnityEngine;
using System.Collections.Generic;

namespace MyGame.Navigation
{
    public class Path : MonoBehaviour
    {
        public List<Transform> waypoints = new List<Transform>();

        void Start()
        {
            // Optional: Debug.Log("Path initialized with " + waypoints.Count + " waypoints.");
        }

        void Update()
        {
            // Your update logic here (if needed)
        }
    }
}
