using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter
{
    public class EquipInformation : MonoBehaviour
    {
        public PlayerInformation.EquipType equipType;
        public PlayerInformation.QualityLevel qualityLevel;

        public TextMeshProUGUI text;
        public RectTransform rectTransform;

        Button button;
        ButtonScale buttonScale;
        Image frame;

        public bool isWeared;

        public void Awake()
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
            button = GetComponent<Button>();
            frame = GetComponent<Image>();
            buttonScale = GetComponent<ButtonScale>();
            rectTransform = GetComponent<RectTransform>();

            button.onClick.AddListener(OnClick);
            button.targetGraphic = frame;
        }

        public void Init(string name, PlayerInformation.EquipType equipType, PlayerInformation.QualityLevel qualityLevel, bool isWeared)
        {
            text.text = name;
            this.equipType = equipType;
            this.qualityLevel = qualityLevel;
            this.isWeared = isWeared;
        }

        public void Wearing()
        {
            isWeared = true;

            buttonScale.enabled = false;

            button.transition = Selectable.Transition.ColorTint;

            rectTransform.SetParent(PlayerInformation.instance.GetParentEquipCurrency(equipType));

            rectTransform.anchoredPosition3D = new Vector3(0, 16.6f, 0);

            rectTransform.sizeDelta = new Vector2(190f, 190f);
            rectTransform.localScale = Vector3.one;

            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        }

        public void NotWearing()
        {
            isWeared = false;

            buttonScale.enabled = true;

            button.transition = Selectable.Transition.None;

            rectTransform.SetParent(PlayerInformation.instance.inventory.container);

            rectTransform.anchoredPosition3D = new Vector3(0, 16.6f, 0);
            rectTransform.localScale = Vector3.one;
        }

        void OnClick()
        {
            PlayerInformation.instance.SwapEquipCurrency(this);

        }

        public Equip GetEquip()
        {
            return new Equip(equipType, qualityLevel, isWeared);
        }
    }
}
