using System.Linq;
using UnityEngine;
using Agate.WaskitaInfra1.UserInterface.ChecklistList;
using Agate.WaskitaInfra1;

public class ChecklistTestSceneControl : MonoBehaviour
{
    [SerializeField]
    private LevelProgressCheckListDisplay _checklistDisplay;

    [SerializeField]
    private ScriptableLevelProgress _checklistsProgress;

    private void Start()
    {
        _checklistDisplay.Open(_checklistsProgress, Debug.Log, () => Debug.Log("Finish"));
        //Debug.Log(_checklistsProgress.Level.Quizzes);
    }
}