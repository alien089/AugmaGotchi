/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections;
using Meta.XR.Samples;
using UnityEngine;
using UnityEngine.Serialization;

namespace Meta.XR.MRUtilityKitSamples.PassthroughRelighting
{
    /// <summary>
    ///     Listens to the user's input, moves and animates Oppy accordingly.
    /// </summary>
    [MetaCodeSample("MRUKSample-PassthroughRelighting")]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class OppyCharacterController : MonoBehaviour
    {
        /// <summary>
        ///     The vertical speed that Oppy will have if the jump button is pressed
        /// </summary>
        [SerializeField] private float jumpSpeed = 4;

        [SerializeField] private float maximumLinearSpeed = 0.9f;
        [SerializeField] private float gravity = -9.8f;

        [FormerlySerializedAs("_animator")] private Animator _animator;
        private CharacterController _characterController;

        private Vector3 _moveVelocity;
        private Quaternion _rotation;
        private Vector2 _motionInput;
        private bool _jumpRequested;
        private JumpingState _jumpingState = JumpingState.Grounded;

        private const float JumpDelay = 0.16f;

        public enum JumpingState
        {
            Grounded,
            JumpStarted,
            JumpedAndAirborne
        }

        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            // GetLocomotionInput();
            // HandleLocomotion();
            // HandleJumping();
            // ApplyMotion();
        }

        private void GetLocomotionInput()
        {
            var hInput =
                OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x;
            var vInput =
                OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;
            _motionInput = new Vector2(hInput, vInput);
        }

        private void ApplyMotion()
        {
            _moveVelocity.y += gravity * Time.deltaTime;
            _characterController.Move(_moveVelocity * Time.deltaTime);
            if (Mathf.Abs(_motionInput.y) > 0 || Mathf.Abs(_motionInput.x) > 0)
            {
                transform.rotation = _rotation;
            }
        }

        private void HandleLocomotion()
        {
            bool noMovementInput = Mathf.Abs(_motionInput.y) == 0 && Mathf.Abs(_motionInput.x) == 0;
            _animator.SetBool("Running", !noMovementInput && _characterController.isGrounded);
        }

        private void HandleJumping()
        {
            if (_jumpRequested)
            {
                _moveVelocity.y = jumpSpeed;
                _jumpRequested = false;
            }

            if (_jumpingState == JumpingState.JumpStarted && !_characterController.isGrounded)
            {
                _jumpingState = JumpingState.JumpedAndAirborne;
            }
            
            if (_jumpingState == JumpingState.Grounded && _characterController.isGrounded)
            {
                _jumpingState = JumpingState.JumpStarted;
                StartCoroutine(RequestJumpAfterSeconds(JumpDelay));
                _animator.SetTrigger("Jumping");
            }
            else if (_characterController.isGrounded && _jumpingState == JumpingState.JumpedAndAirborne)
            {
                _animator.SetTrigger("Landed");
                _jumpingState = JumpingState.Grounded;
            }
        }

        private IEnumerator RequestJumpAfterSeconds(float delay)
        {
            yield return new WaitForSeconds(delay);
            _jumpRequested = true;
        }

        public void DisableAnimator()
        {
            _animator.enabled = false;
        }
        
        public void SetAnimation(string animationName, bool value)
        {
            _animator.SetBool(animationName, value);
        }
        
        public void TriggerAnimation(string animationName)
        {
            _animator.SetTrigger(animationName);
        }

        public bool IsGrounded()
        {
            return _characterController.isGrounded;
        }

        public JumpingState GetJumpingState()
        {
            return _jumpingState;
        }
        
        public void SetJumpingState(JumpingState state)
        {
            _jumpingState = state;
        }
    }
}
