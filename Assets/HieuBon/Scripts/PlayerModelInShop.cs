using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class PlayerModelInShop : MonoBehaviour
    {
        Animator animator;

        public GameController.PlayerType playerType;

        public void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Upgrade()
        {
            animator.SetTrigger("Upgrade");
        }

        public void Use()
        {
            animator.SetTrigger("Use");
        }
    }
}
