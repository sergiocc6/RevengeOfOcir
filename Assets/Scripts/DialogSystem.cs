using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts
{
    public class DialogSystem : MonoBehaviour
    {
        [System.Serializable]
        public class DialogueLine
        {
            public string text;
            public Sprite portrait;
            public bool isLeft;
        }

        public DialogueLine[] dialogueLines; // Array of dialogue lines
        private int currentLineIndex = 0; // Index of the current dialogue line

        public Image portraitLeft;
        public Image portraitRight;
        public TextMeshProUGUI dialogueText;
        public GameManager gameManager; // Reference to the GameManager
        public Animator playerAnimator; // Reference to the Player's Animator

        // Use this for initialization
        void Start()
        {
            //Time.timeScale = 0f; // Pause game time when dialog starts
            gameManager.isDialogueActive = true; // Set dialogue active state in GameManager
            //playerAnimator.Play("Player_iddle");
            StopsPlayerAnimation();
            ShowDialogueLine();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                currentLineIndex++;
                if (currentLineIndex < dialogueLines.Length)
                {
                    ShowDialogueLine();
                }
                else
                {
                    //Time.timeScale = 1f; // Resume game time
                    gameManager.isDialogueActive = false; // Set dialogue inactive state in GameManager
                    gameObject.SetActive(false); // Hide dialog system when done
                }
            }
        }

        void ShowDialogueLine()
        {
            DialogueLine line = dialogueLines[currentLineIndex];

            if (line.isLeft)
            {
                portraitLeft.sprite = line.portrait;
                portraitLeft.gameObject.SetActive(true);
                portraitRight.gameObject.SetActive(false);
                dialogueText.alignment = TextAlignmentOptions.MidlineLeft;
            }
            else
            {
                portraitRight.sprite = line.portrait;
                portraitRight.gameObject.SetActive(true);
                portraitLeft.gameObject.SetActive(false);
                dialogueText.alignment = TextAlignmentOptions.MidlineRight;
            }

            dialogueText.text = line.text;
        }

        private void StopsPlayerAnimation()
        {
            if (playerAnimator != null)
            {
               playerAnimator.SetFloat("Run", 0f); // Stop player animation
                playerAnimator.SetBool("Jump", false); // Stop jump animation
                playerAnimator.SetBool("Fall", false); // Stop fall animation
                playerAnimator.SetBool("DoubleJump", false); // Stop double jump animation
                playerAnimator.SetBool("Wall", false); // Stop wallslide animation
                playerAnimator.SetBool("Shield", false); // Stop shield animation
            }
        }
    }
}