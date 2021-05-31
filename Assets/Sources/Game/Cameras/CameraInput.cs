using System;
using AssetBundlesClass.Game.InputSystem;
using UnityEngine.InputSystem;

namespace AssetBundlesClass.Game.Cameras
{
    public interface ICameraInputListener
    {
        void MouseVerticalUpdate(float y);
        void MouseHorizontalUpdate(float x);
    }

    public class CameraInput
    {
        private readonly InputActions _actions = default;

        public ICameraInputListener listener { get; set; }

        public CameraInput()
        {
            _actions = new InputActions();

            _actions.Camera.Vertical.performed += MouseVerticalAction;
            _actions.Camera.Horizontal.performed += MouseHorizontalAction;
        }

        public void EnableInputs() => _actions.Camera.Enable();
        public void DisableInputs() => _actions.Camera.Disable();

        private void MouseVerticalAction(InputAction.CallbackContext ctx)
            => listener.MouseVerticalUpdate(ctx.ReadValue<float>());

        private void MouseHorizontalAction(InputAction.CallbackContext ctx)
            => listener.MouseHorizontalUpdate(ctx.ReadValue<float>());
    }
}