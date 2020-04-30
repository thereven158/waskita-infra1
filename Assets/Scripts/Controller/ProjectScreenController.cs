using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.BriefDisplay;
using System;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.ProjectDisplay
{
    public class ProjectScreenController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _projectContent;

        [SerializeField]
        private RectTransform _contentRectProject;

        [SerializeField]
        private LevelDataScriptableObject[] _levelData;

        private int _numberOfItems;

        [SerializeField]
        private GameObject _projectCanvas;

        [SerializeField]
        private BriefScreenController _briefScreenController;

        public void Start()
        {
            _numberOfItems = _levelData.Length;
            CreateContent();
        }

        public void CreateContent()
        {
            for (int i = 0; i < _numberOfItems; i++)
            {
                int temp = i;

                //instantiate item
                GameObject SpawnedItem = Instantiate(_projectContent, Vector3.zero, this.transform.rotation);

                //setParent
                SpawnedItem.transform.SetParent(_contentRectProject, false);

                //get button component
                Button btnSpawnItem = SpawnedItem.GetComponent<Button>();

                //register event
                btnSpawnItem.onClick.AddListener(() => {
                    OnSelectProjectAction(_levelData[temp]);
                });

                //get ItemDetails Component
                ProjectDetail itemDetails = SpawnedItem.GetComponent<ProjectDetail>();

                //set name
                itemDetails.text.text = _levelData[i].name;

            }
        }

        public void OnSelectProjectAction(LevelDataScriptableObject levelData)
        {
            _projectCanvas.SetActive(false);
            _briefScreenController.DisplayBrief(levelData);

        }
    }

}
