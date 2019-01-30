using System;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class HeadBob : MonoBehaviour
    {
        public Camera Camera;
        public CurveControlledBob motionBob = new CurveControlledBob();
        public LerpControlledBob jumpAndLandingBob = new LerpControlledBob();
        public RigidbodyFirstPersonController rigidbodyFirstPersonController;
        public float StrideInterval;
        [SerializeField] public float m_StepInterval=2f;
        [SerializeField] public AudioSource m_AudioSource;
        [SerializeField] public AudioClip[] m_FootstepSounds;
        private float m_StepCycle=0;
        private float m_NextStep=.1f;
        [Range(0f, 1f)] public float RunningStrideLengthen;

       // private CameraRefocus m_CameraRefocus;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;


        private void Start()
        {
            motionBob.Setup(Camera, StrideInterval);
            m_OriginalCameraPosition = Camera.transform.localPosition;
            //     m_CameraRefocus = new CameraRefocus(Camera, transform.root.transform, Camera.transform.localPosition);
        }


        private void FixedUpdate()
        {
          //  m_CameraRefocus.GetFocusPoint();
            Vector3 newCameraPosition;
            if (rigidbodyFirstPersonController.Velocity.magnitude > 0 && rigidbodyFirstPersonController.Grounded)
            {
                Camera.transform.localPosition = motionBob.DoHeadBob(rigidbodyFirstPersonController.Velocity.magnitude*(rigidbodyFirstPersonController.Running ? RunningStrideLengthen : 1f));
                newCameraPosition = Camera.transform.localPosition;
                newCameraPosition.y = Camera.transform.localPosition.y - jumpAndLandingBob.Offset();
                ProgressStepCycle(rigidbodyFirstPersonController.Velocity.magnitude * (rigidbodyFirstPersonController.Running ? RunningStrideLengthen : 1f));
            }
            else
            {
                newCameraPosition = Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - jumpAndLandingBob.Offset();
            }
            Camera.transform.localPosition = newCameraPosition;

            if (!m_PreviouslyGrounded && rigidbodyFirstPersonController.Grounded)
            {
                StartCoroutine(jumpAndLandingBob.DoBobCycle());
            }

            m_PreviouslyGrounded = rigidbodyFirstPersonController.Grounded;
          //  m_CameraRefocus.SetFocusPoint();
        }
        private void ProgressStepCycle(float speed)
        {
            if (speed > 0)
            {
                m_StepCycle += speed * Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {

                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;
            //Debug.Log(m_NextStep);
            PlayFootStepAudio();
        }
        private void PlayFootStepAudio()
        {
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = UnityEngine.Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }
    }
}
