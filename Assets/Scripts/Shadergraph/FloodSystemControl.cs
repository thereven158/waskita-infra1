using UnityEngine;

namespace Agate.ShaderGraph
{
    public class FloodSystemControl : MonoBehaviour
    {
        [SerializeField]
        public Material FloodMaterial;


        public void SetWaterSpeed(float waterSpeed)
        {
            FloodMaterial.SetFloat("_Vector1_WaterSpeed", waterSpeed);
        }

        public void SetWaterColor(Color color)
        {
            FloodMaterial.SetColor("_Color_Water", color);
        }

        public void SetTopWaterColor(Color color)
        {
            FloodMaterial.SetColor("_Color_TopWater", color);
        }

        public void SetHeightWater(float heightWater)
        {
            FloodMaterial.SetFloat("_Vector1_WaterHeight", heightWater);
        }

    }

}
