using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace UnityStandardAssets.Characters.FirstPerson
{
	public class FootStepController : MonoBehaviour {
			[SerializeField] public float m_StepInterval=2f;
			[SerializeField] public AudioSource m_AudioSource;
			[SerializeField] public AudioClip[] m_FootstepSounds;
			private float m_StepCycle=0;
			private float m_NextStep=.1f;
			public RigidbodyFirstPersonController rigidbodyFirstPersonController;
		// Use this for initialization
		private void FixedUpdate()
			{
				if (rigidbodyFirstPersonController.Velocity.magnitude > 0 && rigidbodyFirstPersonController.Grounded)
				{
					ProgressStepCycle(rigidbodyFirstPersonController.Velocity.magnitude);
				}
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