using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Hunter.PlayerInformation;
using static Hunter.PlayerInShop;
using static UnityEditorInternal.ReorderableList;

namespace Hunter
{
    public class PlayerInformation : MonoBehaviour
    {
        public static PlayerInformation instance;

        [HideInInspector]
        public Inventory inventory;

        public ButtonHeroAndEquip[] buttonHeroAndEquips;

        public GameObject preFrameGray;
        public GameObject preFrameGreen;
        public GameObject preFrameBlue;
        public GameObject preFrameYellow;
        public GameObject preFramePurple;
        public GameObject preFrameRed;

        public CurrencyEquipTypeInformation currentWeapon;
        public CurrencyEquipTypeInformation currentHat;
        public CurrencyEquipTypeInformation currentArmor;
        public CurrencyEquipTypeInformation currentShoe;
        public CurrencyEquipTypeInformation currentNecklace;
        public CurrencyEquipTypeInformation currentRing;

        List<EquipInformation> equipInfos = new List<EquipInformation>();

        public Color healthColor;
        public Color damageColor;
        public Color damageCritColor;
        public Color defaultColor;

        public WeaponData weaponData;

        public GameObject equipParent;
        public GameObject heroParent;

        public ScrollRect scrollRectIndex;

        public PlayerModelInShop[] prePlayers;

        [HideInInspector]
        public List<PlayerModelInShop> playerModels = new List<PlayerModelInShop>();

        public RectTransform background;

        List<PlayerData> playerDatas = new List<PlayerData>();

        PlayerInShop[] playerInShops;
        IndexPlayerLevel indexPlayerLevel;

        public enum ColorType
        {
            Health, Damage, DamageCrit, Default
        }

        public enum EquipType
        {
            Ring, Necklace, Shoe, Armor, Hat, Weapon
        }

        public enum QualityLevel
        {
            Gray, Green, Blue, Yellow, Purple, Red
        }

        public enum WeaponUpgradeType
        {
            Weapon, Hat, Armor, Shoe, Ring, Necklace
        }

        public enum PlayerUpgradeType
        {
            Level, Piece, IsUnlock, IsUsed
        }

        public GameObject GetPrePlayer(GameController.PlayerType playerType)
        {
            for (int i = 0; i < prePlayers.Length; i++)
            {
                if (prePlayers[i].playerType == playerType) return prePlayers[i].gameObject;
            }

            return prePlayers[0].gameObject;
        }

        public void ReloadIndexLevel(GameController.PlayerType playerType)
        {
            indexPlayerLevel.Reload(playerType);
        }

        public GameObject GetPreFrame(QualityLevel qualityLevel)
        {
            switch (qualityLevel)
            {
                case QualityLevel.Green: return preFrameGreen;
                case QualityLevel.Blue: return preFrameBlue;
                case QualityLevel.Yellow: return preFrameYellow;
                case QualityLevel.Purple: return preFramePurple;
                case QualityLevel.Red: return preFrameRed;
                default: return preFrameGray;
            }
        }

        public Transform GetParentEquipCurrency(EquipType equipType)
        {
            switch (equipType)
            {
                case EquipType.Necklace: return currentNecklace.transform;
                case EquipType.Shoe: return currentShoe.transform;
                case EquipType.Armor: return currentArmor.transform;
                case EquipType.Hat: return currentHat.transform;
                case EquipType.Weapon: return currentWeapon.transform;
                default: return currentRing.transform;
            }
        }

        CurrencyEquipTypeInformation GetCurrencyEquipInfor(EquipType equipType)
        {
            switch (equipType)
            {
                case EquipType.Necklace:
                    return currentNecklace;
                case EquipType.Shoe:
                    return currentShoe;
                case EquipType.Armor:
                    return currentArmor;
                case EquipType.Hat:
                    return currentHat;
                case EquipType.Weapon:
                    return currentWeapon;
                default:
                    return currentRing;
            }
        }

        public Color GetColorType(ColorType colorType)
        {
            switch (colorType)
            {
                case ColorType.Health:
                    return healthColor;
                case ColorType.DamageCrit:
                    return damageCritColor;
                case ColorType.Damage:
                    return damageColor;
                default:
                    return defaultColor;
            }
        }

        public void UpgradeWeapon(WeaponUpgradeType weaponUpgradeType)
        {
            switch (weaponUpgradeType)
            {
                case WeaponUpgradeType.Weapon:
                    weaponData.weaponLevel++;
                    break;
                case WeaponUpgradeType.Shoe:
                    weaponData.shoeLevel++;
                    break;
                case WeaponUpgradeType.Hat:
                    weaponData.hatLevel++;
                    break;
                case WeaponUpgradeType.Ring:
                    weaponData.ringLevel++;
                    break;
                case WeaponUpgradeType.Necklace:
                    weaponData.necklaceLevel++;
                    break;
                case WeaponUpgradeType.Armor:
                    weaponData.armorLevel++;
                    break;
            }

            GameManager.instance.WeaponData = weaponData;
        }

        public void UpgradePlayerData(PlayerUpgradeType playerUpgradeType, GameController.PlayerType playerType, int amountPiece = 0)
        {
            PlayerData playerData = GameManager.instance.GetPlayerData(playerType);

            switch (playerUpgradeType)
            {
                case PlayerUpgradeType.Level:
                    playerData.level++;
                    break;
                case PlayerUpgradeType.Piece:
                    playerData.amountPiece += amountPiece;
                    break;
                case PlayerUpgradeType.IsUnlock:
                    playerData.isUnlocked = true;
                    break;
                case PlayerUpgradeType.IsUsed:
                    WhoUsed(playerType);
                    break;
            }

            GameManager.instance.SetPlayerData(playerData);
        }

        void WhoUsed(GameController.PlayerType playerType)
        {
            foreach (var playerInShop in playerInShops)
            {
                if (playerInShop.playerInShopData.playerType == playerType) playerInShop.isUsed = true;
                else playerInShop.isUsed = false;
            }
        }

        public void Awake()
        {
            PlayerPrefs.DeleteAll();
            instance = this;
            inventory = GetComponentInChildren<Inventory>();
            indexPlayerLevel = GetComponentInChildren<IndexPlayerLevel>(true);
            playerInShops = GetComponentsInChildren<PlayerInShop>(true);
        }

        public void Start()
        {
            GameController.PlayerType[] playerEnums = (GameController.PlayerType[])Enum.GetValues(typeof(GameController.PlayerType));

            for (int i = 0; i < playerEnums.Length; i++)
            {
                PlayerData playerData = GameManager.instance.GetPlayerData(playerEnums[i]);
                playerDatas.Add(playerData);
            }

            GameController.PlayerType playerType = GameManager.instance.CurrentPlayer;

            WhoUsed(playerType);

            OnClickButtonHeroAndEquip(buttonHeroAndEquips[0]);

            List<Equip> equips = GameManager.instance.Equipments;

            foreach (Equip equip in equips)
            {
                GameObject preFrame = GetPreFrame(equip.qualityLevel);

                GameObject e = Instantiate(preFrame);

                EquipInformation equipInformation = e.GetComponent<EquipInformation>();

                equipInfos.Add(equipInformation);

                CurrencyEquipTypeInformation currencyEquipInformation = GetCurrencyEquipInfor(equip.equipType);

                equipInformation.Init(equip.equipType.ToString(), equip.equipType, equip.qualityLevel, equipInformation.isWeared);

                if (equip.isWeared)
                {
                    Wearing(currencyEquipInformation, equipInformation);
                }
                else
                {
                    NotWearing(equipInformation);
                }

            }

            SelectPlayer(playerType);
        }

        public void DisableFrameSelect()
        {
            foreach (var playerInShop in playerInShops)
            {
                playerInShop.DisableFrameSelect();
            }
        }

        public PlayerInShop GetPlayerInShop(GameController.PlayerType playerType)
        {
            foreach(var player in playerInShops)
            {
                if(player.playerInShopData.playerType == playerType) return player;
            }
            return null;
        }

        public void SelectPlayer(GameController.PlayerType playerType)
        {
            bool isContain = false;
            for (int i = 0; i < playerModels.Count; i++)
            {
                if (playerModels[i].playerType == playerType)
                {
                    isContain = true;
                    playerModels[i].gameObject.SetActive(true);
                }
                else
                {
                    playerModels[i].gameObject.SetActive(false);
                }
            }
            if (!isContain)
            {
                GameObject pre = GetPrePlayer(playerType);
                if (pre != null)
                {
                    GameObject p = Instantiate(pre, GameController.instance.playerViewContainer);
                    PlayerModelInShop playerModelInShop = p.GetComponent<PlayerModelInShop>();

                    playerModelInShop.playerType = playerType;

                    playerModels.Add(playerModelInShop);
                }
                else
                {
                    Debug.LogError("!");
                }
            }
        }

        public void OnClickButtonHeroAndEquip(ButtonHeroAndEquip buttonHeroAndEquip)
        {
            for (int i = 0; i < buttonHeroAndEquips.Length; i++)
            {
                if (buttonHeroAndEquips[i] == buttonHeroAndEquip)
                {
                    buttonHeroAndEquips[i].Enable();
                }
                else
                {
                    buttonHeroAndEquips[i].Disable();
                }
            }
        }

        public void SwapEquipCurrency(EquipInformation equipSelect)
        {
            CurrencyEquipTypeInformation currencyEquipInformation = GetCurrencyEquipInfor(equipSelect.equipType);

            if (equipSelect.isWeared)
            {
                NotWearing(equipSelect);
                currencyEquipInformation.NotWearing();
            }
            else
            {
                if (currencyEquipInformation.equipInformation != null)
                {
                    NotWearing(currencyEquipInformation.equipInformation);
                    currencyEquipInformation.NotWearing();
                }

                Wearing(currencyEquipInformation, equipSelect);
            }

            inventory.Sort();

            SaveEquips();
        }

        void Wearing(CurrencyEquipTypeInformation currencyEquipInformation, EquipInformation equipInformation)
        {
            currencyEquipInformation.equipInformation = equipInformation;

            equipInformation.Wearing();

            currencyEquipInformation.Wearing();

            inventory.RemoveEquip(equipInformation);
        }

        void NotWearing(EquipInformation equipInformation)
        {
            equipInformation.NotWearing();

            inventory.AddEquip(equipInformation);
        }

        void SaveEquips()
        {
            List<Equip> equips = new List<Equip>();

            foreach (EquipInformation equip in equipInfos)
            {
                equips.Add(equip.GetEquip());
            }

            GameManager.instance.Equipments = equips;
        }

        public void ShowEquip()
        {
            if (equipParent.activeSelf) return;

            background.DOKill();

            background.DOAnchorPosX(0f, 0.25f).SetEase(Ease.Linear);

            equipParent.SetActive(true);
            heroParent.SetActive(false);
        }

        public void ShowHero()
        {
            if (heroParent.activeSelf) return;

            background.DOKill();

            background.DOAnchorPosX(-265f, 0.25f).SetEase(Ease.Linear);

            equipParent.SetActive(false);
            heroParent.SetActive(true);

            scrollRectIndex.normalizedPosition = new Vector2(0, 1);
        }

    }
    public class Equip
    {
        public EquipType equipType;
        public PlayerInformation.QualityLevel qualityLevel;

        public bool isWeared;

        public Equip(EquipType equipType, PlayerInformation.QualityLevel qualityLevel, bool isWeared)
        {
            this.equipType = equipType;
            this.qualityLevel = qualityLevel;
            this.isWeared = isWeared;
        }
    }


    public class PlayerData
    {
        public int level;
        public bool isUnlocked;
        public int amountPiece;
    }

    public class WeaponData
    {
        public int weaponLevel;
        public int armorLevel;
        public int hatLevel;
        public int necklaceLevel;
        public int ringLevel;
        public int shoeLevel;
    }
}

