using System;
using UnityEngine;

namespace Spells
{
    // Class used for indicator which follows joystick or mouse and aims spells
    public class Indicator : MonoBehaviour
    {
        // Deadzone out of 1 so that you don't have to hold the analog stick fully out for max speed
        private const float JoyChop = 0.8f;
        
        private bool _mouseMode;

        // Exposed to Unity editor
        public Rigidbody2D Rb;
        public Vector2 input;
        public float maxRange = 3f;
        public Vector2 lastGoodInput;

        // Start is called before the first frame update
        void Start()
        {
            _mouseMode = false;
        }

        // Update is called once per frame
        void Update()
        {
            input.x = Input.GetAxisRaw("Mouse X");
            input.y = Input.GetAxisRaw("Mouse Y");
            if (Math.Abs(input.x) > 0.001 || Math.Abs(input.y) > 0.001)
            {
                _mouseMode = false;
            }

            if (Math.Abs(Input.GetAxis("RealMouseX")) > 0.001 || Math.Abs(Input.GetAxis("RealMouseY")) > 0.001)
            {
                _mouseMode = true;
            }

            if (_mouseMode)
            {
                var mouseCoords = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 delt = mouseCoords - transform.parent.position;
                delt.Normalize();
                input.x = delt.x;
                input.y = delt.y;
            }

            if (input.magnitude > 0.1)
            {
                lastGoodInput = input;
            }
        }

        void FixedUpdate()
        {
            if (_mouseMode)
            {
                var mouseCoords = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 delta = mouseCoords - transform.parent.position;
                if (delta.magnitude > maxRange)
                {
                    delta.Normalize();
                    delta *= maxRange;
                }

                Rb.position = transform.parent.position + new Vector3(delta.x, delta.y, 0);
            }
            else
            {
                var chop = Math.Min(input.magnitude, JoyChop);
                input.Normalize();
                input.Scale(new Vector2(chop, chop));

                Vector3 relativePosition = input * maxRange;
                relativePosition.z = 0;
                Rb.position = transform.parent.position + relativePosition;
            }
        }
    }
}
