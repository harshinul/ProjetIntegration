using UnityEngine;
using UnityEngine.SceneManagement;

namespace SCRIPTS_MARC
{
	public class CharacterSelection : MonoBehaviour
	{
		public GameObject[] characters;
		public int selectedCharacter = 0;

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

		public void StartGame()
		{
            foreach (GameObject character in characters)
            {
                Debug.Log(character.name);
            }
            PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
			SceneManager.LoadScene("AreneSelection", LoadSceneMode.Single);
		}
	}
}
