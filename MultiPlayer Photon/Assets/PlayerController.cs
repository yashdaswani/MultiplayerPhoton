using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public class PlayerController : MonoBehaviour
{

    public Joystick joystick;
    private RigidbodyFirstPersonController rigidbodyFirstPersonController;
    public FixedTouchField touchField;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rigidbodyFirstPersonController = GetComponent<RigidbodyFirstPersonController>();
       // touchField = GetComponent<FixedTouchField>();
       animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbodyFirstPersonController.joystickInputaxis.x = joystick.Horizontal;
        rigidbodyFirstPersonController.joystickInputaxis.y = joystick.Vertical;
        rigidbodyFirstPersonController.mouseLook.lookInputAxis = touchField.TouchDist;
        animator.SetFloat("Horizontal", joystick.Horizontal);
        animator.SetFloat("vertical", joystick.Vertical);

        if(Mathf.Abs(joystick.Horizontal)>0.9f || Mathf.Abs(joystick.Vertical)>0.9f)
        {
            animator.SetBool("IsRunning", true);
            rigidbodyFirstPersonController.movementSettings.ForwardSpeed = 8;
        }
        else
        {
            animator.SetBool("IsRunning", false);
            rigidbodyFirstPersonController.movementSettings.ForwardSpeed = 4;
        }
    }
}
