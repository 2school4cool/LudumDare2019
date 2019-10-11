using System.Collections.Generic;
using Spells;
using UnityEngine;

namespace ResourceSystem
{
    // Class responsible for generating essence and scroll placement on the map
    public class ResourceGenerator : MonoBehaviour
    {
        private const int ActiveEssenceSpawnPoints = 49;
        private const int ActiveScrollSpawnPoints = 14;
    
        // Class defining a spawn point for essence
        class EssenceSpawnPoint
        {
            static int _totalCount = 0;
        
            public readonly Vector3 position;
            public readonly int number;
            public readonly int redPercent;
            public readonly int bluePercent;
            public readonly int greenPercent;
            public int yellowPercent;

            // Initiate spawn point at specific position with probability values for colors
            public EssenceSpawnPoint (Vector3 pos, int rp =0, int bp =0, int gp =0, int yp =0)
            {
                position = pos;
                number = _totalCount;
                _totalCount += 1;
                redPercent = rp;
                bluePercent = bp;
                greenPercent = gp;
                yellowPercent = yp;
            }
        } 

        // Class defining a spawn point for a scroll
        class ScrollSpawnPoint
        {
            public Vector3 position;
            public int number;
            static int _totalCount = 0;

            public int redPercent;
            public int bluePercent;
            public int greenPercent;
            public int yellowPercent;
            public ScrollSpawnPoint (Vector3 pos, int flash = 0, int stick = 0, int lemon = 0, int fire = 0)
            {
                position = pos;
                number = _totalCount;
                _totalCount += 1;

                redPercent = fire;
                bluePercent = flash;
                greenPercent = stick;
                yellowPercent = lemon;
            }
        }
        private IList<EssenceSpawnPoint> _essenceSpawnPoints;
        private IList<int> _takenEssenceSpawns;
        private int _activeSpawnPoints = 39;
        public GameObject essencePrefab;
        private readonly object _lockObj = new Object();
        public GameObject greenEssence;
        public GameObject blueEssence;
        public GameObject redEssence;
        public GameObject yellowEssence;

        private IList<ScrollSpawnPoint> scrollSpawnPoints;
        private IList<int> takenScrollSpawns;
        private int activeScrollSpawnPoints = 14;
        public GameObject scrollPrefab1;
        public GameObject scrollPrefab2;
        public GameObject scrollPrefab3;
        public GameObject scrollPrefab4;
        private object lockObj2 = new object();

        // Start is called before the first frame update
        private void Start()
        {
            // Change essence spawn points here 
            _essenceSpawnPoints = new List<EssenceSpawnPoint>();
            _takenEssenceSpawns = new List<int>();
            // Essence spawns in bottom left
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-15.7f, 18.3f, 0), rp: 2));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-12.6f, 13.4f, 0), 2));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-25.1f, 8.2f, 0), 2));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-10f, 6.6f, 0), 2));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-16f, 7.2f, 0), 2));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-9f, 1.2f, 0), 2));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-4.6f, 7.8f, 0), 2));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(1.2f, 9.1f, 0), 2));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-8.6f, -2.8f, 0), 2));

            // Essence spawns in bottom right
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(11.1f, 12.2f, 0), 2, 0, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(23.3f, 14.4f, 0), 2, 0, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(22.1f, 19.5f, 0), 2, 0, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(27.7f, 19.6f, 0), 2, 0, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(28f, 11.7f, 0), 2, 0, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(19.3f, 10.3f, 0), 2, 0, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(9.2f, 6.6f, 0), 2, 0, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(24.1f, 3.5f, 0), 2, 0, 10));

            // Essence spawns in top left
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(11.5f, -0.8f, 0), 2, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(17.9f, -6.9f, 0), 2, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(11f, -12f, 0), 2, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(6.4f, -20.7f, 0), 2, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(9.7f, -21.1f, 0), 2, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(21.6f, -20.4f, 0), 2, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(24.9f, -16.1f, 0), 2, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(28.4f, -11.7f, 0), 2, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(28.7f, -9.8f, 0), 2, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(28.8f, -6.6f, 0), 2, 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(29.1f, -5.6f, 0), 2, 10));

            // Essence spawns in top right
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-22.9f, -15.8f, 0), 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-13.7f, -18.9f, 0), 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-6.3f, -16.7f, 0), 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-1.6f, -13.8f, 0), 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-3.1f, -10.1f, 0), 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-8.8f, -9.7f, 0), 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-15.6f, -7.7f, 0), 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-21.5f, -2.4f, 0), 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(1.6f, 0.4f, 0), 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(-1.2f, 19f, 0), 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(2.1f, 16.6f, 0), 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(10f, 19.9f, 0), 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(20f, 1.6f, 0), 10));
            _essenceSpawnPoints.Add(new EssenceSpawnPoint(new Vector3(25.7f, 0.1f, 0), 10));

            // Change scroll spawn points here
            scrollSpawnPoints = new List<ScrollSpawnPoint>
            {
                // Bottom left
                new ScrollSpawnPoint(new Vector3(-8.1f, -15.3f, 0), 2, 4, 5),
                new ScrollSpawnPoint(new Vector3(-20.3f, -10.5f, 0), 2, 4, 5),

                // Top left
                new ScrollSpawnPoint(new Vector3(-17.8f, -1.2f, 0), 2, 4, 8),
                new ScrollSpawnPoint(new Vector3(-1.9f, 2.1f, 0), 2, 4, 8),
                new ScrollSpawnPoint(new Vector3(-10.2f, 7.8f, 0), 2, 4, 8),
                new ScrollSpawnPoint(new Vector3(-3.2f, 16.5f, 0), 2, 4, 8),
                new ScrollSpawnPoint(new Vector3(-20.2f, 13.5f, 0), 2, 4, 8),
            
                // Top right
                new ScrollSpawnPoint(new Vector3(10.8f, 14f, 0), 2, 7, 8),
                new ScrollSpawnPoint(new Vector3(13.4f, 6.8f, 0), 2, 7, 8),
                new ScrollSpawnPoint(new Vector3(23.6f, 11.7f, 0), 2, 7, 8),
                new ScrollSpawnPoint(new Vector3(28f, 16.5f, 0), 0, 10),

                // Bottom right
                new ScrollSpawnPoint(new Vector3(13.7f, -8.4f, 0), 5, 7, 8),
                new ScrollSpawnPoint(new Vector3(28.8f, -8f, 0), 10),
                new ScrollSpawnPoint(new Vector3(25.4f, -19.4f, 0), 5, 7, 8),
                new ScrollSpawnPoint(new Vector3(8.1f, -20.4f, 0), 5, 7, 8),

            };
            takenScrollSpawns = new List<int>();

            // Place the first set of essences
            IList<EssenceSpawnPoint> tempEssenceSpawnPoints = new List<EssenceSpawnPoint>(_essenceSpawnPoints);
            IList<EssenceSpawnPoint> selectedSpawnPoints = new List<EssenceSpawnPoint>();
            for (int i = 0; i < _activeSpawnPoints; i++)
            {
                int selectedSpawn = Random.Range(0, tempEssenceSpawnPoints.Count);
                selectedSpawnPoints.Add(tempEssenceSpawnPoints[selectedSpawn]);
                tempEssenceSpawnPoints.RemoveAt(selectedSpawn);
            }

            // Place the first set of scrolls
            IList<ScrollSpawnPoint> tempScrollSpawnPoints = new List<ScrollSpawnPoint>(scrollSpawnPoints);
            IList<ScrollSpawnPoint> selectedScrollSpawnPoints = new List<ScrollSpawnPoint>();
            for (int i = 0; i < activeScrollSpawnPoints; i++)
            {
                int selectedSpawn = Random.Range(0, tempScrollSpawnPoints.Count);
                selectedScrollSpawnPoints.Add(tempScrollSpawnPoints[selectedSpawn]);
                tempScrollSpawnPoints.RemoveAt(selectedSpawn);
            }

            // Spawn the starting essences
            foreach (var essenceSpawn in selectedSpawnPoints)
            {
                var newEssence = Instantiate(selectColor(essenceSpawn), essenceSpawn.position, Quaternion.identity) as GameObject;
                newEssence.GetComponent<Essence.Essence>().SetSpawnNumber(essenceSpawn.number);
                _takenEssenceSpawns.Add(essenceSpawn.number);
            }

            // Spawn the starting scrolls
            foreach (var scrollSpawn in selectedScrollSpawnPoints)
            {
                var newScroll = Instantiate(selectScroll(scrollSpawn), scrollSpawn.position, Quaternion.identity) as GameObject;
                newScroll.GetComponent<SpellScroll>().SetSpawnDetails(gameObject, scrollSpawn.number);
                takenScrollSpawns.Add(scrollSpawn.number);
            }
        }

        // Selects an essence color
        private GameObject selectColor (EssenceSpawnPoint s)
        {
            //Selecting a color
            int color = Random.Range(1, 10);
            if (color <= s.redPercent)
            {
                return redEssence;
            }
            else if (color <= s.bluePercent)
            {
                return blueEssence;
            }
            else if (color <= s.greenPercent)
            {
                return  greenEssence;
            }
            else
            {
                return yellowEssence;
            }
        }

        // Selects a scroll type
        private GameObject selectScroll (ScrollSpawnPoint s)
        {
            int color = Random.Range(1, 10);
            if (color < s.bluePercent)
            {
                return scrollPrefab2;
            }
            else if (color < s.greenPercent)
            {
                return scrollPrefab4;
            
            }
            else if (color < s.yellowPercent)
            {
                return scrollPrefab3;
            }
            else
            {
            
                return scrollPrefab1;
            }
        }
    
        // Clears n from taken list of essence spawns and spawns new essence at non-n spawn
        public void NotifyCollectedEssence(int n)
        {
            lock (_lockObj)
            {

                IList<EssenceSpawnPoint> tempEssenceSpawnPoints = new List<EssenceSpawnPoint>(_essenceSpawnPoints);
                for (int i = 0; i < _takenEssenceSpawns.Count; i++)
                {
                    for (int j = 0; j < tempEssenceSpawnPoints.Count; j++)
                    {
                        if (tempEssenceSpawnPoints[j].number == _takenEssenceSpawns[i])
                        {
                            tempEssenceSpawnPoints.RemoveAt(j);
                            break;
                        }
                    }
                }
                EssenceSpawnPoint selectedEssenceSpawn = tempEssenceSpawnPoints[Random.Range(0, tempEssenceSpawnPoints.Count)];
                GameObject newEssence = Instantiate(selectColor(selectedEssenceSpawn), selectedEssenceSpawn.position, Quaternion.identity) as GameObject;
                newEssence.GetComponent<Essence.Essence>().SetSpawnNumber(selectedEssenceSpawn.number);
                _takenEssenceSpawns.Add(selectedEssenceSpawn.number);
                _takenEssenceSpawns.Remove(n);
            }
        }

        // Clears n from taken list of scroll spawns and spawns new essence at non-n spawn
        public void NotifyCollectedScroll(int n)
        {
            lock (lockObj2)
            {

                IList<ScrollSpawnPoint> tempScrollSpawnPoints = new List<ScrollSpawnPoint>(scrollSpawnPoints);
                for (int i = 0; i < takenScrollSpawns.Count; i++)
                {
                    for (int j = 0; j < tempScrollSpawnPoints.Count; j++)
                    {
                        if (tempScrollSpawnPoints[j].number == takenScrollSpawns[i])
                        {
                            tempScrollSpawnPoints.RemoveAt(j);
                            break;
                        }
                    }
                }
                ScrollSpawnPoint selectedSpawn = tempScrollSpawnPoints[Random.Range(0, tempScrollSpawnPoints.Count)];
                GameObject newScroll = Instantiate(selectScroll(selectedSpawn), selectedSpawn.position, Quaternion.identity) as GameObject;
                newScroll.GetComponent<SpellScroll>().SetSpawnDetails(gameObject, selectedSpawn.number);
                takenScrollSpawns.Add(selectedSpawn.number);
                takenScrollSpawns.Remove(n);
            }
        }
    }
}
