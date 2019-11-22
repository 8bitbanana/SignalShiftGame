using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float MoveForce = 5;
    public float MaxVelocity = 100;
    private Vector2 moveVelocity, pushVelocity;

    SpriteRenderer spriteRenderer;
    Rigidbody2D body;

    /* Structs don't appear in Unity's inspector, so these 4 variables are defined
    / and are then just packed into the struct (see the playerSprite getter)
    SerializeField makes a private variable show up in the Unity inspector.*/
    [SerializeField] Sprite spriteDown, spriteLeft, spriteRight, spriteUp;

    // When we get to animations there is probably a way of doing all this in unity's animation system
    public struct PlayerSprite {
        public Sprite up, down, left, right;

        public Sprite FromDir(Vector2 direction) {
            // Get the sprite that is the closest to the given direction (e.g. a dir of mostly up gives up)
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
                // Horizontal
                return (direction.x > 0) ? right : left;
            } else {
                // Vertical
                return (direction.y > 0) ? up : down;
            }
        }
    }

    public PlayerSprite playerSprite {
        get {
            var s = new PlayerSprite();
            s.up = spriteUp;
            s.down = spriteDown;
            s.left = spriteLeft;
            s.right = spriteRight;
            return s;
        }
    }

    void Awake() {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (moveVelocity.magnitude > 0)
            spriteRenderer.sprite = playerSprite.FromDir(moveVelocity);
    }

    public void Knockback(Vector2 knockbackVector) {
        pushVelocity += knockbackVector;
    }

    void FixedUpdate() {
        if (pushVelocity.magnitude > 0.1) {
            pushVelocity *= 0.7f;
        } else {
            pushVelocity = Vector2.zero;
        }
        moveVelocity = PlayerInput.instance.direction * MoveForce;

        body.velocity = Vector3.ClampMagnitude(moveVelocity + pushVelocity, MaxVelocity);
    }
}
