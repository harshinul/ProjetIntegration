using UnityEngine;
using UnityEngine.SceneManagement;

namespace SCRIPTS_MARC
{
	public class CharacterSelection : MonoBehaviour
	{
        CharacterSelectionManager manager;
        public GameObject[] characters;
		public int selectedCharacter = 0;

        void Start()
        {
            manager = FindAnyObjectByType<CharacterSelectionManager>();
        }

        public void NextCharacter()
		{
			characters[selectedCharacter].SetActive(false);
			selectedCharacter = (selectedCharacter + 1) % characters.Length;
			characters[selectedCharacter].SetActive(true);
		}

		public void PreviousCharacter()
		{
			characters[selectedCharacter].SetActive(false);
			selectedCharacter--;
			if (selectedCharacter < 0)
			{
				selectedCharacter += characters.Length;
			}
			characters[selectedCharacter].SetActive(true);
		}

		public void SendInformation()
		{
            
			manager.AddPlayer(characters[selectedCharacter].name);

        }

		//public void StartGame()
		//{
  //          PlayerPrefs.SetString("classTypePlayer1", characters[selectedCharacter].name);
  //          PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
		//	SceneManager.LoadScene("AreneSelection", LoadSceneMode.Single);
		//}
	}
}
