using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "joystick")]
public class JoystickValue : ScriptableObject
{
    public Vector2 joyTouch;
    // Punch
    public bool aTouch;
    // Inventory
    public bool bTouch;
    // Settings
    public bool cTouch;
    // PickUp
    public bool dTouch;
}
