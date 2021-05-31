using System.Collections.Generic;
using AssetBundlesClass.Game.InputSystem;
using UnityEngine.InputSystem;

namespace AssetBundlesClass.Game.Vehicles.Input
{
    public interface ICarInputListener
    {
        void AccelerationUpdate(float acceleration);
        void SteeringUpdate(float steering);
    }

    public class CarInput
    {
        private readonly InputActions _actions;
        private readonly List<ICarInputListener> _listeners;

        public CarInput()
        {
            _actions = new InputActions();
            _listeners = new List<ICarInputListener>();

            _actions.Car.Steering.performed += SteeringAction;
            _actions.Car.Steering.canceled += SteeringAction;
            _actions.Car.Acceleration.performed += AccelerationAction;
            _actions.Car.Acceleration.canceled += AccelerationAction;
        }

        public void EnableInputs() => _actions.Car.Enable();

        public void DisableInputs() => _actions.Car.Disable();

        public void AddListener(ICarInputListener listener) => _listeners.Add(listener);
        public void RemoveListener(ICarInputListener listener) => _listeners.Remove(listener);

        private void SteeringAction(InputAction.CallbackContext ctx)
        {
            for (int index = 0; index < _listeners.Count; index++)
            {
                _listeners[index].SteeringUpdate(ctx.ReadValue<float>());
            }
        }

        private void AccelerationAction(InputAction.CallbackContext ctx)
        {
            for (int index = 0; index < _listeners.Count; index++)
            {
                _listeners[index].AccelerationUpdate(ctx.ReadValue<float>());
            }
        }
    }
}