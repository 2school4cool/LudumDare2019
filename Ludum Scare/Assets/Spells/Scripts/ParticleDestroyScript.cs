using UnityEngine;

namespace Spells
{
    // Class for particle systems which wish to be destroyed after 1 execution 
    public class ParticleDestroyScript : MonoBehaviour
    {
        private ParticleSystem ps;
        public void Start()
        {
            ps = GetComponent<ParticleSystem>();
        }

        public void Update()
        {
            if (!ps) return;
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
