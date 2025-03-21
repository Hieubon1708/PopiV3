using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Hunter
{
    public class Inventory : MonoBehaviour
    {
        public Transform container;

        public List<EquipInformation> equipInfos = new List<EquipInformation>();

        bool isQuality;

        public void Start()
        {
            Sort();
        }

        public void OnClickSort()
        {
            isQuality = !isQuality;
            Sort();
        }

        public void Sort()
        {
            if (isQuality) SortByQuality();
            else SortByClass();
        }

        public void AddEquip(EquipInformation equipInformation)
        {
            equipInfos.Add(equipInformation);
        }
        
        public void RemoveEquip(EquipInformation equipInformation)
        {
            equipInfos.Remove(equipInformation);
        }

        public void SortByClass()
        {
            List<EquipInformation> weapons = new List<EquipInformation>();
            List<EquipInformation> hats = new List<EquipInformation>();
            List<EquipInformation> armors = new List<EquipInformation>();
            List<EquipInformation> shoes = new List<EquipInformation>();
            List<EquipInformation> necklaces = new List<EquipInformation>();
            List<EquipInformation> rings = new List<EquipInformation>();


            foreach (EquipInformation equip in equipInfos)
            {
                switch (equip.equipType)
                {
                    case PlayerInformation.EquipType.Weapon:
                        weapons.Add(equip); break;
                    case PlayerInformation.EquipType.Hat:
                        hats.Add(equip); break;
                    case PlayerInformation.EquipType.Armor:
                        armors.Add(equip); break;
                    case PlayerInformation.EquipType.Shoe:
                        shoes.Add(equip); break;
                    case PlayerInformation.EquipType.Necklace:
                        necklaces.Add(equip); break;
                    case PlayerInformation.EquipType.Ring:
                        rings.Add(equip); break;
                }
            }

            List<List<EquipInformation>> all = new List<List<EquipInformation>>() { weapons, hats, armors, shoes, necklaces, rings };

            foreach (var equipType in all)
            {
                SortByQualityLevel(equipType);
            }

            for (int i = 0; i < all.Count; i++)
            {
                for (int j = 0; j < all[i].Count; j++)
                {
                    all[i][j].transform.SetAsLastSibling();
                }
            }
        }

        public void SortByQuality()
        {
            List<EquipInformation> reds = new List<EquipInformation>();
            List<EquipInformation> purples = new List<EquipInformation>();
            List<EquipInformation> yellows = new List<EquipInformation>();
            List<EquipInformation> blues = new List<EquipInformation>();
            List<EquipInformation> greens = new List<EquipInformation>();
            List<EquipInformation> grays = new List<EquipInformation>();

            foreach (EquipInformation equip in equipInfos)
            {
                switch (equip.qualityLevel)
                {
                    case PlayerInformation.QualityLevel.Red:
                        reds.Add(equip); break;
                    case PlayerInformation.QualityLevel.Purple:
                        purples.Add(equip); break;
                    case PlayerInformation.QualityLevel.Yellow:
                        yellows.Add(equip); break;
                    case PlayerInformation.QualityLevel.Blue:
                        blues.Add(equip); break;
                    case PlayerInformation.QualityLevel.Green:
                        greens.Add(equip); break;
                    case PlayerInformation.QualityLevel.Gray:
                        grays.Add(equip); break;
                }
            }

            List<List<EquipInformation>> all = new List<List<EquipInformation>>() { reds, purples, yellows, blues, greens, grays };

            foreach (var equipQuality in all)
            {
                SortByEquipType(equipQuality);
            }

            for (int i = 0; i < all.Count; i++)
            {
                for (int j = 0; j < all[i].Count; j ++)
                {
                    all[i][j].transform.SetAsLastSibling();
                }
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SortByClass();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                SortByQuality();
            }
        }

        void SortByEquipType(List<EquipInformation> equips)
        {
            for (int i = 0; i < equips.Count - 1; i++)
            {
                for (int j = i + 1; j < equips.Count; j++)
                {
                    if (equips[i].equipType < equips[j].equipType)
                    {
                        EquipInformation temp = equips[i];
                        equips[i] = equips[j];
                        equips[j] = temp;
                    }
                }
            }
        }

        void SortByQualityLevel(List<EquipInformation> equips)
        {
            for (int i = 0; i < equips.Count - 1; i++)
            {
                for (int j = i + 1; j < equips.Count; j++)
                {
                    if (equips[i].qualityLevel < equips[j].qualityLevel)
                    {
                        EquipInformation temp = equips[i];
                        equips[i] = equips[j];
                        equips[j] = temp;
                    }
                }
            }
        }
    }
}
