using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agate.WaskitaInfra1.Level;

public class ProjectScreenController : MonoBehaviour
{
    [SerializeField]
    private Transform SpawnPoint;

    [SerializeField]
    private GameObject content;

    [SerializeField]
    private RectTransform contentRect;

    [SerializeField]
    private int numberOfItems = 5;

    [SerializeField]
    private LevelDataScriptableObject[] levelData;

    ProjectScreenView _projectScreenView = new ProjectScreenView();

    // Start is called before the first frame update
    void Start()
    {
        _projectScreenView.CreateContent(SpawnPoint, content, contentRect, levelData.Length, levelData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
