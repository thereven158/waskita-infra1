using A3.UserInterface;
using Agate.WaskitaInfra1;
using Agate.WaskitaInfra1.Animations;
using Agate.WaskitaInfra1.UserInterface.Display;
using UnityEngine;

namespace Experimental
{

    public class EnviSceneController : MonoBehaviour
    {
        [SerializeField]
        private AnimationScenesManager _aScMn = default;
        [SerializeField]
        private AnimationSceneControl animRef = default;
        [SerializeField]
        private BlockDisplay block = default;
        [SerializeField]
        private FadeTweenDisplay fade = default;
        private UiDisplaysSystem<GameObject> _dispSystem;
        [SerializeField]
        private PopUpDisplay _infoPopup = default;
        bool playing;
        // Start is called before the first frame update
        void Start()
        {
            _dispSystem = Main.GetRegisteredComponent<UiDisplaysSystemBehavior>();
            _aScMn.OnStop += (anim) => playing = false;

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.X))
            {
                _aScMn.OnStop += (anim) => _dispSystem.GetOrCreateDisplay<FadeTweenDisplay>(fade).Open();
                _aScMn.OnStart += (anim) => _dispSystem.GetOrCreateDisplay<FadeTweenDisplay>(fade).Close();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                _aScMn.PlayAnimation(animRef, null, () => _dispSystem.GetOrCreateDisplay<PopUpDisplay>(_infoPopup).Open("ngapain keq",null));
                playing = true;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (playing)
                    Time.timeScale = 0;
                else
                    Time.timeScale = 1;
                playing = !playing;
            }

        }
    }
}

