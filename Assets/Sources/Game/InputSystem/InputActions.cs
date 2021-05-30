// GENERATED AUTOMATICALLY FROM 'Assets/Sources/Game/InputSystem/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace AssetBundlesClass.Game.InputSystem
{
    public class @InputActions : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Car"",
            ""id"": ""08bfdd89-a2d0-408e-ad11-f8a423e4c8ed"",
            ""actions"": [
                {
                    ""name"": ""Steering"",
                    ""type"": ""Value"",
                    ""id"": ""859e8a40-baab-4eaf-944b-5064bf971d76"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Acceleration"",
                    ""type"": ""Button"",
                    ""id"": ""7b14ac4a-1443-44cb-a920-3e827eb9c4b2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""LeftRight"",
                    ""id"": ""353e3341-cefc-4e2c-9191-0f4f5926f74f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""583e353d-54a8-4db6-9548-9ed7fc238d1c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Steering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""2f807ea8-3478-4f4e-bfbb-fa00ed0defc2"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Steering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""AccelerateBreak"",
                    ""id"": ""fffd81dd-b930-41ff-9157-171839d947e5"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""0c8301be-b118-45fc-8cc0-b5aedf8af29d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Acceleration"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a55a314d-e58c-487c-a1cb-67d9c015737d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Acceleration"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PC"",
            ""bindingGroup"": ""PC"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Car
            m_Car = asset.FindActionMap("Car", throwIfNotFound: true);
            m_Car_Steering = m_Car.FindAction("Steering", throwIfNotFound: true);
            m_Car_Acceleration = m_Car.FindAction("Acceleration", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Car
        private readonly InputActionMap m_Car;
        private ICarActions m_CarActionsCallbackInterface;
        private readonly InputAction m_Car_Steering;
        private readonly InputAction m_Car_Acceleration;
        public struct CarActions
        {
            private @InputActions m_Wrapper;
            public CarActions(@InputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Steering => m_Wrapper.m_Car_Steering;
            public InputAction @Acceleration => m_Wrapper.m_Car_Acceleration;
            public InputActionMap Get() { return m_Wrapper.m_Car; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CarActions set) { return set.Get(); }
            public void SetCallbacks(ICarActions instance)
            {
                if (m_Wrapper.m_CarActionsCallbackInterface != null)
                {
                    @Steering.started -= m_Wrapper.m_CarActionsCallbackInterface.OnSteering;
                    @Steering.performed -= m_Wrapper.m_CarActionsCallbackInterface.OnSteering;
                    @Steering.canceled -= m_Wrapper.m_CarActionsCallbackInterface.OnSteering;
                    @Acceleration.started -= m_Wrapper.m_CarActionsCallbackInterface.OnAcceleration;
                    @Acceleration.performed -= m_Wrapper.m_CarActionsCallbackInterface.OnAcceleration;
                    @Acceleration.canceled -= m_Wrapper.m_CarActionsCallbackInterface.OnAcceleration;
                }
                m_Wrapper.m_CarActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Steering.started += instance.OnSteering;
                    @Steering.performed += instance.OnSteering;
                    @Steering.canceled += instance.OnSteering;
                    @Acceleration.started += instance.OnAcceleration;
                    @Acceleration.performed += instance.OnAcceleration;
                    @Acceleration.canceled += instance.OnAcceleration;
                }
            }
        }
        public CarActions @Car => new CarActions(this);
        private int m_PCSchemeIndex = -1;
        public InputControlScheme PCScheme
        {
            get
            {
                if (m_PCSchemeIndex == -1) m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
                return asset.controlSchemes[m_PCSchemeIndex];
            }
        }
        public interface ICarActions
        {
            void OnSteering(InputAction.CallbackContext context);
            void OnAcceleration(InputAction.CallbackContext context);
        }
    }
}