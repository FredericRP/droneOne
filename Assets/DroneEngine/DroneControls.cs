// GENERATED AUTOMATICALLY FROM 'Assets/DroneEngine/DroneControls.inputactions'

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace FredericRP.DroneEngine
{
    public class DroneControls : IInputActionCollection
    {
        private InputActionAsset asset;
        public DroneControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""DroneControls"",
    ""maps"": [
        {
            ""name"": ""race"",
            ""id"": ""a8956111-5e00-4ae5-9e43-f858aa23ceaf"",
            ""actions"": [
                {
                    ""name"": ""pitch"",
                    ""type"": ""PassThrough"",
                    ""id"": ""47d815bb-dced-44ed-a145-45ecec03d890"",
                    ""expectedControlType"": ""Double"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""power"",
                    ""type"": ""PassThrough"",
                    ""id"": ""14f41c7b-efa0-480e-a71a-d8497dd5c90b"",
                    ""expectedControlType"": ""Double"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""roll"",
                    ""type"": ""PassThrough"",
                    ""id"": ""df44d9cb-600b-47e4-8c14-ea7a843e979b"",
                    ""expectedControlType"": ""Double"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""yaw"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0aadf69e-4cb4-443d-b4b7-b41774721469"",
                    ""expectedControlType"": ""Double"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1f30a7c2-93f7-46de-94b8-dfac701ac892"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Race"",
                    ""action"": ""pitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e800691-4476-480b-804a-d925d2533b7e"",
                    ""path"": ""<Gamepad>/rightStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Race"",
                    ""action"": ""power"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9280fc8b-ee6d-4dcb-9886-dfe86a170000"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Race"",
                    ""action"": ""roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Race"",
            ""bindingGroup"": ""Race"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // race
            m_race = asset.FindActionMap("race", throwIfNotFound: true);
            m_race_pitch = m_race.FindAction("pitch", throwIfNotFound: true);
            m_race_power = m_race.FindAction("power", throwIfNotFound: true);
            m_race_roll = m_race.FindAction("roll", throwIfNotFound: true);
            m_race_yaw = m_race.FindAction("yaw", throwIfNotFound: true);
        }

        ~DroneControls()
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

        // race
        private readonly InputActionMap m_race;
        private IRaceActions m_RaceActionsCallbackInterface;
        private readonly InputAction m_race_pitch;
        private readonly InputAction m_race_power;
        private readonly InputAction m_race_roll;
        private readonly InputAction m_race_yaw;
        public struct RaceActions
        {
            private DroneControls m_Wrapper;
            public RaceActions(DroneControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @pitch => m_Wrapper.m_race_pitch;
            public InputAction @power => m_Wrapper.m_race_power;
            public InputAction @roll => m_Wrapper.m_race_roll;
            public InputAction @yaw => m_Wrapper.m_race_yaw;
            public InputActionMap Get() { return m_Wrapper.m_race; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(RaceActions set) { return set.Get(); }
            public void SetCallbacks(IRaceActions instance)
            {
                if (m_Wrapper.m_RaceActionsCallbackInterface != null)
                {
                    pitch.started -= m_Wrapper.m_RaceActionsCallbackInterface.OnPitch;
                    pitch.performed -= m_Wrapper.m_RaceActionsCallbackInterface.OnPitch;
                    pitch.canceled -= m_Wrapper.m_RaceActionsCallbackInterface.OnPitch;
                    power.started -= m_Wrapper.m_RaceActionsCallbackInterface.OnPower;
                    power.performed -= m_Wrapper.m_RaceActionsCallbackInterface.OnPower;
                    power.canceled -= m_Wrapper.m_RaceActionsCallbackInterface.OnPower;
                    roll.started -= m_Wrapper.m_RaceActionsCallbackInterface.OnRoll;
                    roll.performed -= m_Wrapper.m_RaceActionsCallbackInterface.OnRoll;
                    roll.canceled -= m_Wrapper.m_RaceActionsCallbackInterface.OnRoll;
                    yaw.started -= m_Wrapper.m_RaceActionsCallbackInterface.OnYaw;
                    yaw.performed -= m_Wrapper.m_RaceActionsCallbackInterface.OnYaw;
                    yaw.canceled -= m_Wrapper.m_RaceActionsCallbackInterface.OnYaw;
                }
                m_Wrapper.m_RaceActionsCallbackInterface = instance;
                if (instance != null)
                {
                    pitch.started += instance.OnPitch;
                    pitch.performed += instance.OnPitch;
                    pitch.canceled += instance.OnPitch;
                    power.started += instance.OnPower;
                    power.performed += instance.OnPower;
                    power.canceled += instance.OnPower;
                    roll.started += instance.OnRoll;
                    roll.performed += instance.OnRoll;
                    roll.canceled += instance.OnRoll;
                    yaw.started += instance.OnYaw;
                    yaw.performed += instance.OnYaw;
                    yaw.canceled += instance.OnYaw;
                }
            }
        }
        public RaceActions @race => new RaceActions(this);
        private int m_RaceSchemeIndex = -1;
        public InputControlScheme RaceScheme
        {
            get
            {
                if (m_RaceSchemeIndex == -1) m_RaceSchemeIndex = asset.FindControlSchemeIndex("Race");
                return asset.controlSchemes[m_RaceSchemeIndex];
            }
        }
        public interface IRaceActions
        {
            void OnPitch(InputAction.CallbackContext context);
            void OnPower(InputAction.CallbackContext context);
            void OnRoll(InputAction.CallbackContext context);
            void OnYaw(InputAction.CallbackContext context);
        }
    }
}
