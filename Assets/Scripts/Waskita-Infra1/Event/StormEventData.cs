using A3.DataDrivenEvent;
using A3.UserInterface;
using Agate.WaskitaInfra1.LevelProgress;
using UnityEngine;
using UserInterface.Display;

namespace Agate.WaskitaInfra1.Level
{
    [CreateAssetMenu(menuName = "WaskitaInfra1/Event/Storm", fileName = "StormEvent")]
    public class StormEventData : ScriptableDayEventData
    {
        [SerializeField]
        private PopUpDisplay _information;

        [SerializeField]
        private ConfirmationPopUpDisplay _confirmation;

        private UiDisplaysSystem<GameObject> _displaysSystem;
        private LevelProgressControl _levelProgress;
        public uint Day;

        public override bool IsSuitable(EventTriggerData data)
        {
            Debug.Log($"suitable check {data.Day ==Day}");
            return data.Day == Day;
        }

        public override void Trigger(EventTriggerData data)
        {
            _displaysSystem.GetOrCreateDisplay<ConfirmationPopUpDisplay>(_confirmation)
                .Open("ada badai lanjut ?", FailurePopUps, null);
        }

        public override void Init(IEventTriggerSystem<EventTriggerData> triggerSystem)
        {
            Debug.Log("Init");
            _levelProgress = triggerSystem.GetEventComponent<LevelProgressControl>();
            _displaysSystem = triggerSystem.GetEventComponent<UiDisplaysSystem<GameObject>>();
        }

        private void FailurePopUps()
        {
            _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_information).Open(
                "Projek gagal karena ada kendala  . . . . anda harus mengulangi projek",
                _levelProgress.RetryFromCheckPoint);
        }


        public override bool IsReady => _levelProgress != null && _displaysSystem != null;
    }
}