using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public string[] itemNames;

    ProjectScreenView _projectScreenView = new ProjectScreenView();

    // Start is called before the first frame update
    void Start()
    {
        _projectScreenView.CreateContent(SpawnPoint, content, contentRect, numberOfItems, itemNames);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectProject()
    {

    }
}
