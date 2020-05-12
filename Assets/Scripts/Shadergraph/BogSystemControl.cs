using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agate.ShaderGraph
{
    public class BogSystemControl : MonoBehaviour
    {
        [SerializeField]
        public Material BogMaterial;

        public void SetWaterColor(Color color)
        {
            BogMaterial.SetColor("_Color_WaterColor", color);
        }

        public void SetHeightPuddle(float heightPuddle)
        {
            BogMaterial.SetFloat("_Vector1_HeightPuddle", heightPuddle);
        }

    }

}
