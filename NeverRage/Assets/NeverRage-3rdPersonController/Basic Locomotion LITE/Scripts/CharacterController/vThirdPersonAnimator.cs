using UnityEngine;
using System.Collections;

namespace Com.TriggerAppsProduction.NeverRage
{
  
    public abstract class vThirdPersonAnimator : vThirdPersonMotor
    {/// <summary>
        ///This Script is Manage the Speed/Movement/States/Animation of the Player
        ///the receiving input part is done by the thirdpersonInput
        /// </summary>

        public virtual void UpdateAnimator()
        {
            if (animator == null || !animator.enabled) return;
            #region Strafing animation boolean
            animator.SetBool("IsStrafing", isStrafing);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("GroundDistance", groundDistance);

            if (!isGrounded)
                animator.SetFloat("VerticalVelocity", verticalVelocity);

            if (isStrafing)
            {
                // strafe movement get the input 1 or -1
                animator.SetFloat("InputHorizontal", direction, 0.1f, Time.deltaTime);
            }

            // free movement get the input 0 to 1
            animator.SetFloat("InputVertical", speed, 0.1f, Time.deltaTime);
            #endregion
        }

        public void OnAnimatorMove()
        {
            // Check for Player State (in_air or grounded)/ Determine how fast player can move
            if (isGrounded)
            {
                #region Check If Player Is On A Surface: Player Motion Calculation
                transform.rotation = animator.rootRotation;

                var speedDir = Mathf.Abs(direction) + Mathf.Abs(speed);
                speedDir = Mathf.Clamp(speedDir, 0, 1);
                var strafeSpeed = (isSprinting ? 1.5f : 1f) * Mathf.Clamp(speedDir, 0f, 1f);
                #endregion

                #region Strafing Acceleration
                // strafe extra speed
                if (isStrafing)
                {
                    if (strafeSpeed <= 0.5f)
                        ControlSpeed(strafeWalkSpeed);
                    else if (strafeSpeed > 0.5f && strafeSpeed <= 1f)
                        ControlSpeed(strafeRunningSpeed);
                    else
                        ControlSpeed(strafeSprintSpeed);
                }
                else if (!isStrafing)
                {
                    // free movement extra speed                
                    if (speed <= 0.5f)
                        ControlSpeed(freeWalkSpeed);
                    else if (speed > 0.5 && speed <= 1f)
                        ControlSpeed(freeRunningSpeed);
                    else
                        ControlSpeed(freeSprintSpeed);
                }
                #endregion
            }
        }
    }
}