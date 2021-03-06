﻿using Assets.Scripts.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public abstract class BaseActiveModels: MonoBehaviour
    {
        protected Vector3 targetPosition;
        protected enum directionType { Forward, Reverse, Left, Right };
        protected directionType direction;
        protected bool isMoving;
        protected Animator animationController;
        protected bool banMoving;
        public AudioClip stepSound;
        public AudioClip dieSound;
        protected AudioSource source;
        protected const float soundVolume = 0.5f;
        //public abstract bool CanExecute();


        protected virtual bool IsCollisionWithWallOrCube(Vector3 transformPositions, Vector3 targetPositions)
        {
            RaycastHit hit;
            if (Physics.Linecast(transformPositions, targetPositions, out hit))
            {
                if ((hit.collider.gameObject.tag == "UnbreakingCube") 
                    || (hit.collider.gameObject.tag == "BreakingCube")
                        || (hit.collider.gameObject.tag == "ExitCube"))
                {
                    //print("No way man!");
                    return true;
                }
                
            }
            return false;
            
        }
        protected void PlayStepSound()
        {
            source.PlayOneShot(stepSound, soundVolume);
        }
        protected bool SetTargetPositionAndCheckCollisions(Vector3 direction, directionType dirType)
        {
            targetPosition = SetTargetPosition(transform.position, direction);
            if (!IsCollisionWithWallOrCube(transform.position, targetPosition))
            {
                TurnFace(dirType);
                return true;
            }
            return false;
        }
        protected Vector3 SetTargetPosition(Vector3 transformPosition, Vector3 target)
        {
            return new Vector3(
                Mathf.Round(transformPosition.x) + target.x,
                Mathf.Round(transformPosition.y) + target.y,
                Mathf.Round(transformPosition.z) + target.z);
        }
        protected void PlaceModel(float i, float j)
        {
            CreateModel(i, j);
            isMoving = false;
        }
        protected abstract void CreateModel(float i, float j);
        protected abstract void OnDestroy();

        protected void TurnFace(directionType faceDirection)
        {
            switch (faceDirection)
            {
                case directionType.Forward:
                    if ((this.transform.eulerAngles.y < 0) || (this.transform.eulerAngles.y > 1))
                    {
                        this.transform.Rotate(this.transform.rotation.x,
                            Mathf.Round(0.0f - this.transform.eulerAngles.y),
                            this.transform.rotation.z
                            );
                    }
                    break;
                case directionType.Reverse:
                    if ((this.transform.eulerAngles.y < 179) || (this.transform.eulerAngles.y > 181))
                    {
                        this.transform.Rotate(this.transform.rotation.x,
                        Mathf.Round(180.0f - this.transform.eulerAngles.y),
                        this.transform.rotation.z
                        );
                    }
                    break;
                case directionType.Left:
                    if ((this.transform.eulerAngles.y < 269) || (this.transform.eulerAngles.y > 271))
                    {
                        this.transform.Rotate(this.transform.rotation.x,
                        Mathf.Round(270.0f - this.transform.eulerAngles.y),
                        this.transform.rotation.z);
                    }
                    break;
                case directionType.Right:
                    if ((this.transform.eulerAngles.y < 89) || (this.transform.eulerAngles.y > 91))
                    {
                        this.transform.Rotate(this.transform.rotation.x,
                    Mathf.Round(90.0f - this.transform.eulerAngles.y),
                    this.transform.rotation.z);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
