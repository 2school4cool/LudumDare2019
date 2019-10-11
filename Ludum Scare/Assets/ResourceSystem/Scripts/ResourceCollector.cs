using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceSystem
{
    // Class which handles the updating of essence collection
    public class ResourceCollector : MonoBehaviour
    {
        public IDictionary<string, int> essences;
        private Text _redEssenceCounter;
        private Text _yellowEssenceCounter;
        private Text _greenEssenceCounter;
        private Text _blueEssenceCounter;

        // Start is called before the first frame update
        private void Start()
        {
            // Related UI elements
            _redEssenceCounter = GameObject.FindGameObjectWithTag("RedEssenceCounter").GetComponent<Text>();
            _yellowEssenceCounter = GameObject.FindGameObjectWithTag("YellowEssenceCounter").GetComponent<Text>();
            _greenEssenceCounter = GameObject.FindGameObjectWithTag("GreenEssenceCounter").GetComponent<Text>();
            _blueEssenceCounter = GameObject.FindGameObjectWithTag("BlueEssenceCounter").GetComponent<Text>();
            essences = new Dictionary<string, int>
            {
                {"green", 0 },
                {"blue", 0 },
                {"red", 0 },
                {"yellow", 0 }
            };
            _redEssenceCounter.text = essences["red"].ToString();
            _yellowEssenceCounter.text = essences["yellow"].ToString();
            _greenEssenceCounter.text = essences["green"].ToString();
            _blueEssenceCounter.text = essences["blue"].ToString();
        }

        // Adds essence to a specific color
        public void AddEssence(string color, int num)
        {
            essences[color] += num;
            switch (color)
            {
                case "red":
                    _redEssenceCounter.text = essences["red"].ToString();
                    break;
                case "yellow":
                    _yellowEssenceCounter.text = essences["yellow"].ToString();
                    break;
                case "green":
                    _greenEssenceCounter.text = essences["green"].ToString();
                    break;
                case "blue":
                    _blueEssenceCounter.text = essences["blue"].ToString();
                    break;
                default:
                    break;
            }
        }
    }
}
