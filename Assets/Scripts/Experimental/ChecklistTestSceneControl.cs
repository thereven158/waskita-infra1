using System.Collections.Generic;
using System.Linq;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.UserInterface.LevelList;
using UnityEngine;
using Agate.WaskitaInfra1.UserInterface.ChecklistList;

public class ChecklistTestSceneControl : MonoBehaviour
{
    [SerializeField]
    private ChecklistDataListDisplay _checklistDisplay;
    [SerializeField]
    private LevelDataScriptableObject _checklists;

    private void Start()
    {
        _checklistDisplay.Init();
        _checklistDisplay.OnDataInteraction += data => Debug.Log(data.Question);
        _checklistDisplay.OpenList(_checklists.Quizzes.Select(checklist => checklist.Quiz));
    }
}
