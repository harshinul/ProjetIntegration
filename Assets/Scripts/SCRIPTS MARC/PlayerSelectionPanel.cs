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
        private bool isJoined = false;
        private int selectedCharacter = 0;
        public List<GameObject> characters = new List<GameObject>();
        private CharacterSelectionManager manager;

        public void Initialize(int index, CharacterSelectionManager managerRef, GameObject assignedSocle)
        {
            this.playerIndex = index;
            this.manager = managerRef;

            playerNameText.text = "APPUYEZ POUR REJOINDRE";
            readyIndicator.SetActive(false);
            readyButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            previousButton.gameObject.SetActive(false);

            foreach (Transform child in assignedSocle.transform.GetChild(0)) 
            {
                characters.Add(child.gameObject);
            }

            characters.ForEach(c => c.SetActive(false));
        }


        public void NextCharacter()
        {
            if (isReady || !isJoined) return; 

            characters[selectedCharacter].SetActive(false);
            selectedCharacter = (selectedCharacter + 1) % characters.Count;
            characters[selectedCharacter].SetActive(true);
        }

        public void PreviousCharacter()
        {
            if (isReady || !isJoined) return; 

            characters[selectedCharacter].SetActive(false);
            selectedCharacter--;
            if (selectedCharacter < 0)
            {
                selectedCharacter += characters.Count;
            }
            characters[selectedCharacter].SetActive(true);
        }

        public void JoinSelection()
        {
            if (isJoined) return;

            isJoined = true;
            playerNameText.text = "JOUEUR " + (playerIndex + 1);

            readyButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
            previousButton.gameObject.SetActive(true);

            characters[selectedCharacter].SetActive(true);
        }

        public void ConfirmSelection()
        {
            if (isReady || !isJoined) return;
            
            isReady = true;
            readyIndicator.SetActive(true); 
    
            if (nextButton) nextButton.interactable = false;
            if (previousButton) previousButton.interactable = false;
            if (readyButton) readyButton.interactable = false;

            manager.PlayerIsReady(playerIndex, characters[selectedCharacter].name);
        }
    }
}