using A3.DataDrivenEvent;
using A3.UserInterface;
using Agate.WaskitaInfra1.LevelProgress;
using UnityEngine;
using Agate.WaskitaInfra1.UserInterface.Display;

namespace Agate.WaskitaInfra1.Level
{
    [CreateAssetMenu(menuName = "WaskitaInfra1/Event/Storm", fileName = "StormEvent")]
    public class StormEventData : ScriptableDayEventData
    {
        [SerializeField]
        private PopUpDisplay _information = default;

        [SerializeField]
        private ConfirmationPopUpDisplay _confirmation = default;

        private UiDisplaysSystem<GameObject> _displaysSystem;
        private LevelProgressControl _levelProgress;
        public uint Day;

        public override bool IsSuitable(EventTriggerData data)
        {
            Debug.Log($"suitable check {data.Day == Day}");
            return data.Day == Day;
        }

        public override void Trigger(EventTriggerData data)
        {
            _displaysSystem
                .GetOrCreateDisplay<ConfirmationPopUpDisplay>(_confirmation)
                .Open(new ConfirmationPopUpViewData()
                {
                    MessageText = "hari ini kelihatannya akan hujan deras!\n" +
                                  "Lanjutkan proyek seperti biasa ?",
                    CloseAction = null,
                    CloseButtonText = "Tunda",
                    ConfirmAction = FailurePopUps
                });
        }

        public override void Init(IEventTriggerSystem<EventTriggerData> triggerSystem)
        {
            _levelProgress = triggerSystem.GetEventComponent<LevelProgressControl>();
            _displaysSystem = triggerSystem.GetEventComponent<UiDisplaysSystem<GameObject>>();
        }

        private void FailurePopUps()
        {
            _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_information).Open(
                "Proyek gagal karena tidak dijalankan seusai SOP. Silakan ulangi proyek untuk mencoba kembali!",
                _levelProgress.RetryFromCheckPoint);
        }


        public override bool IsReady => _levelProgress != null && _displaysSystem != null;
    }
}