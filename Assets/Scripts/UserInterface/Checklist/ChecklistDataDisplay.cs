﻿using A3.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.ChecklistList
{
    public class ChecklistDataDisplay : InteractiveDisplayBehavior<CheckListViewData>
    {
        [SerializeField]
        private Button _button = default;

        [SerializeField]
        private TMP_Text _nameText = default;

        [SerializeField]
        private Toggle _toggle = default;

        internal Button Button => _button;

        private void Awake()
        {
            _button.onClick.AddListener(Interaction);
        }

        protected override void ConfigureDisplay(CheckListViewData data)
        {
            _nameText.text = data.Item.Category;
            _toggle.isOn = data.State;
        }

        private void Interaction()
        {
            Interact();
        }
    }
}