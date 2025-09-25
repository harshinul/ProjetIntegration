using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerSelectionController : MonoBehaviour
{
   [SerializeField] private GameObject[] characters;

   [SerializeField] private TextMeshProUGUI playerNameText;

   private int selectedCharacterIndex = 0;
   private int playerIndex = 0;


   public void Initialize(int playerId)
   {
      playerIndex = playerId;
      if (playerNameText != null)
      {
         playerNameText.text = $"Joueur {this.playerIndex + 1}";
      }
      UpdateCharacterDisplay();
   }


   public void NextCharacter()
   {
      characters[selectedCharacterIndex].SetActive(false);
      selectedCharacterIndex = (selectedCharacterIndex + 1) % characters.Length;
      characters[selectedCharacterIndex].SetActive(true);
   }

   public void PreviousCharacter()
   {
      characters[selectedCharacterIndex].SetActive(false);
      selectedCharacterIndex = (selectedCharacterIndex - 1 + characters.Length) % characters.Length;
      characters[selectedCharacterIndex].SetActive(true);
   }

   private void UpdateCharacterDisplay()
   {
      for (int i = 0; i < characters.Length; i++)
      {
         characters[i].SetActive(i == selectedCharacterIndex);
      }
   }

   public void ConfirmSelection()
   {
      string playerPrefKey = $"Player_{playerIndex}_SelectedCharacter";
      PlayerPrefs.SetInt(playerPrefKey, selectedCharacterIndex);
      PlayerPrefs.Save();

      Debug.Log($"Joueur {playerIndex + 1} selected character {selectedCharacterIndex}");
   }
}
