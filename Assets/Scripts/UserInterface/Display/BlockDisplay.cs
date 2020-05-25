using A3.UserInterface;
using UnityEngine;

namespace Agate.WaskitaInfra1.UserInterface.Display
{
    public class BlockDisplay : DisplayBehavior
    {
        public override void Init()
        {
            //do nothing
        }

        public override void Open()
        {
            gameObject.SetActive(true);
        }

        public override void Close()
        {
            gameObject.SetActive(false);
        }

        public override bool IsOpen => gameObject.activeSelf;
    }
}