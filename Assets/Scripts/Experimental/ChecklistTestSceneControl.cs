using System.Collections.Generic;
using System.Linq;
using Agate.WaskitaInfra1.Level;
using UnityEngine;
using Agate.WaskitaInfra1.UserInterface.ChecklistList;
using Agate.WaskitaInfra1;

public class ChecklistTestSceneControl : MonoBehaviour
{
    [SerializeField]
    private ChecklistDataListDisplay _checklistDisplay;
    [SerializeField]
    private ScriptableLevelProgress _checklistsProgress;

    private void Start()
    {
        _checklistDisplay.Init();
        _checklistDisplay.OpenList(_checklistsProgress.Level.Quizzes);
        //Debug.Log(_checklistsProgress.Level.Quizzes);
    }
}
