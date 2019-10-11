using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // Class which handles keeping track of player score
    public class ScoreCounter : MonoBehaviour
    {
        public int score;
        private Text _text;
        private RestartHandler _restartHandler;

        // Start is called before the first frame update
        private void Start()
        {
            _restartHandler = GameObject.FindWithTag("RestartText").GetComponent<RestartHandler>();
            _text = GetComponent<Text>();
            score = 0;
        }

        public void UpdateText(int n)
        {
            score += n;
            _text.text = score.ToString();
            _restartHandler.UpdateText(score);
        }
    }
}
