using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// used to move the player usong the joystick
// not used in current game 

public class PlayerMovement : MonoBehaviour
{
    public float vitesse = 1;
    public XRNode inputController;
    private XRRig rig;
    private Vector2 inputAxis;
    private CharacterController character;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputController);
        device.TryGetFeatureValue(CommonUsages.secondary2DAxis, out inputAxis);
    }
    private void FixedUpdate()
    {
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        character.Move(direction*Time.fixedDeltaTime);
    }
}
