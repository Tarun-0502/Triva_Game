using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PolyScript
{
    public class RotateComponent : MonoBehaviour
    {
        [SerializeField] private Vector3 rotateSpeed;

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(rotateSpeed * Time.deltaTime);
        }
    }
}