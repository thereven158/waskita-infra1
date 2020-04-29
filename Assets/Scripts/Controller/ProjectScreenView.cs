using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agate.WaskitaInfra1.Level;

public class ProjectScreenView : MonoBehaviour
{
    

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateContent(Transform SpawnPoint, GameObject content, RectTransform contentRect, int numberOfItems, LevelDataScriptableObject[] levelData)
    {
        //setContent Holder Height;
        //contentRect.sizeDelta = new Vector2(0, numberOfItems * 60);
        Debug.Log(SpawnPoint.localPosition.y);

        for (int i = 0; i < numberOfItems; i++)
        {
            // 60 width of item
            float spawnY = i * 70;

            //newSpawn Position
            Vector3 pos = new Vector3(SpawnPoint.localPosition.x, SpawnPoint.localPosition.y + (-spawnY), SpawnPoint.position.z);

            //instantiate item
            GameObject SpawnedItem = Instantiate(content, pos, SpawnPoint.rotation);

            //setParent
            SpawnedItem.transform.SetParent(contentRect, false);

            //get ItemDetails Component
            ProjectDetail itemDetails = SpawnedItem.GetComponent<ProjectDetail>();

            //set name
            itemDetails.text.text = levelData[i].name;

        }
    }
}
