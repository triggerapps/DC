using UnityEngine;
using System.Collections;

namespace Com.TriggerAppsProduction.NeverRage
{ /*
    This Script is a second level script, the base script (1st level) is vThirdPersonMotor
    This Script is is a MotorManager type, it gets functions from the motor and 
    Present them in an accessable format to public classes, all the presented methods are virtual because
    they are boxes that contain functions from the PlayerMotor script
    */
    public class vThirdPersonController : vThirdPersonAnimator
    {
        protected virtual void Start()
        {

        }

        //cc.Init() is going to be called from this script using PlayerInput but, its fine
        //if cc.Init() is not here...its in the vThirdPersonMotor Script...and that is the 1st priority script
        //so its going to get read withno problem.

        public virtual void Sprint(bool value)
        {                                   
            isSprinting = value;            
        }

        public virtual void Strafe()
        {
            if (locomotionType == LocomotionType.OnlyFree) return;
            isStrafing = !isStrafing;
        }

        public virtual void Jump()
        {
            // conditions to do this action
            bool jumpConditions = isGrounded && !isJumping;
            // return if jumpCondigions is false
            if (!jumpConditions) return;
            // trigger jump behaviour
            jumpCounter = jumpTimer;            
            isJumping = true;
            // trigger jump animations            
            if (_rigidbody.velocity.magnitude < 1)
                animator.CrossFadeInFixedTime("Jump", 0.1f);
            else
                animator.CrossFadeInFixedTime("JumpMove", 0.2f);
        }

        public virtual void RotateWithAnotherTransform(Transform referenceTransform)
        {
            var newRotation = new Vector3(transform.eulerAngles.x, referenceTransform.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRotation), strafeRotationSpeed * Time.fixedDeltaTime);
            targetRotation = transform.rotation;
        }
    }
}