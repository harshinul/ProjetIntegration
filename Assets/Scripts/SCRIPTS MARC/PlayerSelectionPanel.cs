using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SCRIPTS_MARC
{
    public class PlayerSelectionPanel : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private GameObject readyIndicator;
        [SerializeField] private Button readyButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;

        private int playerIndex;
        private bool isReady = false;
        private int selectedCharacter = 0;
        private List<GameObject> characters = new List<GameObject>();
        private CharacterSelectionManager manager;

        // Est appelée par le manager pour activer et configurer ce panneau
        public void Initialize(int index, CharacterSelectionManager managerRef, GameObject assignedSocle)
        {
            this.playerIndex = index;
            this.manager = managerRef;

            playerNameText.text = "JOUEUR " + (playerIndex + 1);
            readyIndicator.SetActive(false);

            // Récupère les personnages 3D du socle assigné
            foreach (Transform child in assignedSocle.transform.GetChild(0)) // On cherche dans l'objet "Characters" à l'intérieur du socle
            {
                characters.Add(child.gameObject);
            }

            // Active le premier personnage et désactive les autres
            for (int i = 0; i < characters.Count; i++)
            {
                characters[i].SetActive(i == selectedCharacter);
            }
        }

        // --- Méthodes pour les boutons (à lier dans l'inspecteur) ---

        public void NextCharacter()
        {
            if (isReady) return; // Si le joueur est prêt, on ne peut plus changer

            characters[selectedCharacter].SetActive(false);
            selectedCharacter = (selectedCharacter + 1) % characters.Count;
            characters[selectedCharacter].SetActive(true);
        }

        public void PreviousCharacter()
        {
            if (isReady) return; // Si le joueur est prêt, on ne peut plus changer

            characters[selectedCharacter].SetActive(false);
            selectedCharacter--;
            if (selectedCharacter < 0)
            {
                selectedCharacter += characters.Count;
            }
            characters[selectedCharacter].SetActive(true);
        }

        // Dans votre fichier PlayerSelectionPanel.cs

        public void ConfirmSelection()
        {
            if (isReady) return;

            isReady = true;
    
            // On informe le manager que ce joueur est prêt AVANT de désactiver le panneau.
            // C'est important pour que le comptage se fasse correctement.
            manager.PlayerIsReady(playerIndex, characters[selectedCharacter].name);

            // Cette ligne désactive l'objet du panneau entier (Panel_Template).
            gameObject.SetActive(false);
        }
    }
}