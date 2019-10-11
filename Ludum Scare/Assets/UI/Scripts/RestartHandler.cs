using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    // Class which handles restarting the game
    public class RestartHandler : MonoBehaviour
    {
        private float _input;
        private Text _text;

        // Update is called once per frame
        private void Start()
        {
            _text = GetComponent<Text>();
        }

        void Update()
        {
            if (!_text.enabled) return;
            _input = Input.GetAxisRaw("RestartGame");
            if (_input > 0.9f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        // Updates text to log new number of spells cast
        public void UpdateText(int n)
        {
            _text.text = "Number of spells cast: " + n.ToString();
        }
    }
}
