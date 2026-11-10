using System;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
   //Access Components
   private Animator anim;
   
   //Parameters
   private bool isCrouching;
   private bool isAiming;
   private bool isGrounded;
   private bool isMoving;
   private int currentAmmo;

   private void Start()
   {
      anim = GetComponent<Animator>();
   }
   
   #region BostonCity Campus
   /* This code is property of Boston City Campus, and should not be used, or copied for any other subject.
    If you would copy and or use this in an assessment outside the assessment that this was supplied for
    action will be taken against you for plaigirism.
   */
   #endregion
   
   private void Update()
   {
      Aiming();
      Crouching();
      UpdateAnimationParameters();
   }

   public void SetCurrentAmmo(int ammo)
   {
      currentAmmo = ammo;
   }

   public void SetIsCrouching(bool isCrouching)
   {
      this.isCrouching = isCrouching;
   }

   public void SetIsAiming(bool isAiming)
   {
      this.isAiming = isAiming;
   }

   public void SetIsGrounded(bool isGrounded)
   {
      this.isGrounded = isGrounded;
   }

   public void SetIsMoving(bool isMoving)
   {
      this.isMoving = isMoving;
   }

   public void UpdateAnimationParameters()
   {
      anim.SetBool("isMoving", isMoving); //Animation Call
      anim.SetBool("isGrounded", isGrounded); //Animation Call
      anim.SetInteger("Ammo", currentAmmo); //Animation Call
   }
   
   public void Crouching()
   {
      if (isCrouching)
      {
         anim.SetLayerWeight(1,1);
      }
      else
      {
         anim.SetLayerWeight(1,0);
      }
   }

   public void Aiming()
   {
      if (isAiming)
      {
         anim.SetLayerWeight(2, 1);
      }
      else
      {
         anim.SetLayerWeight(2, 0);
      }
   }

   public void Jump()
   {
      anim.SetTrigger("Jump"); //Animation Call
   }

   public void Shoot()
   {
      anim.SetTrigger("Shoot"); //Animation Call
   }

   public void OnBeginShoot()
   {
      anim.SetTrigger("onBeginShoot"); //Animation Call
   }

}
