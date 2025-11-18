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

        [SerializeField] private Button joinButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;

        private int playerIndex;
        private bool isReady = false;
        private bool isJoined = false;
        private int selectedCharacter = 0;
        public List<GameObject> characters = new List<GameObject>();
        private CharacterSelectionManager manager;

        void Awake()
        {
            // Définir l'état visuel par défaut "En attente de joueur"
            isJoined = false;
            isReady = false;

            if (playerNameText != null)
            {
                playerNameText.text = "APPUYEZ POUR REJOINDRE";
            }
            if (readyIndicator != null)
            {
                readyIndicator.SetActive(false);
            }
            if (readyButton != null)
            {
                readyButton.gameObject.SetActive(false);
            }
            if (nextButton != null)
            {
                nextButton.gameObject.SetActive(false);
            }
            if (previousButton != null)
            {
                previousButton.gameObject.SetActive(false);
            }
            
            // On s'assure aussi qu'aucun personnage n'est montré
            characters.ForEach(c => { if (c != null) c.SetActive(false); });
        }

        // ÉTAPE 2 : MODIFIER LA MÉTHODE INITIALIZE()
        // Cette méthode est appelée par le Manager lorsqu'un contrôleur se connecte.
        // On enlève toute la logique d'UI pour la laisser dans Awake() et JoinSelection().
        public void Initialize(int index, CharacterSelectionManager managerRef, GameObject assignedSocle)
        {
            this.playerIndex = index;
            this.manager = managerRef;

            // L'état visuel est déjà géré par Awake().
            // On se contente de préparer la liste des personnages (qui sont sur le socle).
            
            characters.Clear(); // Vider la liste au cas où
            
            if (assignedSocle != null && assignedSocle.transform.childCount > 0)
            {
                // Assumant que les personnages sont sur le *premier* enfant du socle
                foreach (Transform child in assignedSocle.transform.GetChild(0)) 
                {
                    if (child != null)
                    {
                        characters.Add(child.gameObject);
                    }
                }
            }

            // On s'assure qu'ils sont tous désactivés avant la sélection
            characters.ForEach(c => { if (c != null) c.SetActive(false); });
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

            // S'assurer qu'on a bien des personnages avant d'y accéder
            if (characters.Count > 0)
            {
                characters[selectedCharacter].SetActive(true);
            }
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