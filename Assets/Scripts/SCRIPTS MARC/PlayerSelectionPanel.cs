using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SCRIPTS_MARC
{
    public class PlayerSelectionPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private GameObject readyIndicator;
        [SerializeField] private Button readyButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;

        private int playerIndex;
        private bool isReady = false;
        private int selectedCharacter = 0;
        public List<GameObject> characters = new List<GameObject>();
        private CharacterSelectionManager manager;

        public void Initialize(int index, CharacterSelectionManager managerRef, GameObject assignedSocle)
        {
            this.playerIndex = index;
            this.manager = managerRef;

            playerNameText.text = "JOUEUR " + (playerIndex + 1);
            readyIndicator.SetActive(false);

            foreach (Transform child in assignedSocle.transform.GetChild(0)) 
            {
                characters.Add(child.gameObject);
            }

            for (int i = 0; i < characters.Count; i++)
            {
                characters[i].SetActive(i == selectedCharacter);
            }
        }


        public void NextCharacter()
        {
            if (isReady) return; 

            characters[selectedCharacter].SetActive(false);
            selectedCharacter = (selectedCharacter + 1) % characters.Count;
            characters[selectedCharacter].SetActive(true);
        }

        public void PreviousCharacter()
        {
            if (isReady) return; 

            characters[selectedCharacter].SetActive(false);
            selectedCharacter--;
            if (selectedCharacter < 0)
            {
                selectedCharacter += characters.Count;
            }
            characters[selectedCharacter].SetActive(true);
        }

        public void ConfirmSelection()
        {
            if (isReady) return;

            isReady = true;
            readyIndicator.SetActive(true); 
    
            if (nextButton) nextButton.interactable = false;
            if (previousButton) previousButton.interactable = false;
            if (readyButton) readyButton.interactable = false;

            manager.PlayerIsReady(playerIndex, characters[selectedCharacter].name);
        }
    }
}