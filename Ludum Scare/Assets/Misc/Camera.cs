using System;
using UnityEngine;

// Class for camera following player
public class Camera : MonoBehaviour
{
    public GameObject player;
    public GameObject background;
    private UnityEngine.Camera _camera;
    private Tuple<Vector2, Vector2> _backgroundVectors;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.visible = false;
        _camera = UnityEngine.Camera.main;

        Vector3 position = background.transform.position;
        SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
        Vector2 size = spriteRenderer.size;
        _backgroundVectors = new Tuple<Vector2, Vector2>(
            new Vector2(
                position.x - size.x / 2,
                position.y - size.y / 2),
            new Vector2(
                position.x + size.x / 2,
                position.y + size.y / 2));
    }

    void FixedUpdate()
    {
        // Assign camera to follow dead player if no player is found
        if (!player)
        {
            player = GameObject.FindWithTag("DeadPlayer");
        }
        if (!player) return;

        // Camera follow player 
        Vector3 playerPosition = player.transform.position;
        Vector3 selfPosition = transform.position;
        Vector3 newPosition = new Vector3(playerPosition.x, playerPosition.y, selfPosition.z);

        // Keep camera within game bounds
        float height = _camera.orthographicSize;
        float width = height * _camera.aspect;
        newPosition.x = Math.Max(newPosition.x, _backgroundVectors.Item1.x + width);
        newPosition.x = Math.Min(newPosition.x, _backgroundVectors.Item2.x - width);
        newPosition.y = Math.Max(newPosition.y, _backgroundVectors.Item1.y + height);
        newPosition.y = Math.Min(newPosition.y, _backgroundVectors.Item2.y - height);

        // Assign new position to transform
        transform.position = Vector3.Slerp(selfPosition, newPosition, Time.fixedDeltaTime * 5);
    }
}
