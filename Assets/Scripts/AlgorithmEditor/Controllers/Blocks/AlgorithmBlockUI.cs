﻿using UnityEngine;
using UnityEngine.Events;
using AlgorithmizmModels.Blocks;
using TMPro;
using UnityEngine.EventSystems;
using Assets.Scripts.AlgorithmEditor.Events;
using UnityEngine.UI;
using Assets.Scripts.AlgorithmEditor.Controllers.ResourceProviders;

namespace Assets.Scripts.AlgorithmEditor.Controllers.Blocks
{
    public class AlgorithmBlockUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private RectTransform _tabulation;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _image;

        [SerializeField] private AlgorithmTreeResourceProvider _resourceProvider;

        public IAlgorithmBlock BlockData { get; set; }

        public UnityEvent<AlgorithmBlockUI> OnClick { get; set; } =
            new AlgorithmBlockUIEvent();

        public void RefreshAnData()
        {
            _text.text = BlockData.Name;
            _image.sprite = _resourceProvider.BlockTypeSprites[BlockData.Type];
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnClick?.Invoke(this);
            }
        }
    }
}
