using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PolyScript
{

    public class Vehicle : MonoBehaviour
    {

        [SerializeField] private Material baseMaterial;

        [SerializeField] private Material textureMaterial;

        public void SetTexture(Texture texture) => textureMaterial.mainTexture = texture;
        public void UpdateColor(Component sender, object data)
        {
            if (data is Color)
                baseMaterial.color = (Color)data;
        }


    }

}
