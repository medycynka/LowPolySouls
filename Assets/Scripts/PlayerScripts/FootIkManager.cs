using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{
    public class FootIkManager : MonoBehaviour
    {
        private Animator anim;
        private Vector3 rightFootPosition;
        private Vector3 leftFootPosition;
        private Vector3 rightFootIkPosition;
        private Vector3 leftFootIkPosition;
        private Quaternion leftFootIkRotation;
        private Quaternion rightFootIkRotation;
        private float lastPelvisPositionY;
        private float lastRightFootPositionY;
        private float lastLeftFootPositionY;

        [Header("Foot IK Manager", order = 0)] 
        [Header("Feet Grounded Variables", order = 1)]
        public bool enableFeetIk = true;
        public LayerMask environmentLayer;
        [Range(0f, 1f)] public float weightPositionRight = 1f;
        [Range(0f, 1f)] public float weightRotationRight = 0f;
        [Range(0f, 1f)] public float weightPositionLeft = 1f;
        [Range(0f, 1f)] public float weightRotationLeft = 0f;
        public Vector3 offsetFoot;
        
        private const string environmentTag = "Environment";
        private const AvatarIKGoal leftFoot = AvatarIKGoal.LeftFoot;
        private const AvatarIKGoal rightFoot = AvatarIKGoal.RightFoot;
        private RaycastHit hit;
        
        private void Start()
        {
            anim = GetComponent<Animator>();
            environmentLayer = 1 << LayerMask.NameToLayer(environmentTag);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (enableFeetIk)
            {
                #region Right Foot IK
                Vector3 FootPos = anim.GetIKPosition(rightFoot);

                if (Physics.Raycast(FootPos + Vector3.up, Vector3.down, out hit, 1.2f, environmentLayer))
                {
                    anim.SetIKPositionWeight(rightFoot, weightPositionRight);
                    anim.SetIKRotationWeight(rightFoot, weightRotationRight);
                    anim.SetIKPosition(rightFoot, hit.point + offsetFoot);

                    if (weightRotationRight > 0f)
                    {
                        Quaternion footRotation =
                            Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal),
                                hit.normal);
                        anim.SetIKRotation(rightFoot, footRotation);
                    }
                }
                else
                {
                    anim.SetIKPositionWeight(rightFoot, 0f);
                    anim.SetIKRotationWeight(rightFoot, 0f);
                }
                #endregion

                #region Left Foot IK
                FootPos = anim.GetIKPosition(leftFoot);
                if (Physics.Raycast(FootPos + Vector3.up, Vector3.down, out hit, 1.2f, environmentLayer))
                {
                    anim.SetIKPositionWeight(leftFoot, weightPositionLeft);
                    anim.SetIKRotationWeight(leftFoot, weightRotationLeft);
                    anim.SetIKPosition(leftFoot, hit.point + offsetFoot);

                    if (weightRotationLeft > 0f)
                    {
                        Quaternion footRotation =
                            Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal),
                                hit.normal);
                        anim.SetIKRotation(leftFoot, footRotation);
                    }
                }
                else
                {
                    anim.SetIKPositionWeight(leftFoot, 0f);
                    anim.SetIKRotationWeight(leftFoot, 0f);
                }
                #endregion
            }
        }
    }
}