//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/GISSample/GISSampleInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @GISSampleInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GISSampleInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GISSampleInputActions"",
    ""maps"": [
        {
            ""name"": ""GISSample"",
            ""id"": ""e5b8e29e-3654-4249-9e14-dd8648f8dbf0"",
            ""actions"": [
                {
                    ""name"": ""SelectObject"",
                    ""type"": ""Button"",
                    ""id"": ""3f91495c-1606-429c-9575-041b48dc1990"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""69169da1-8496-46e4-a123-41bb827e82df"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""SelectObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardAndMouse"",
            ""bindingGroup"": ""KeyboardAndMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // GISSample
        m_GISSample = asset.FindActionMap("GISSample", throwIfNotFound: true);
        m_GISSample_SelectObject = m_GISSample.FindAction("SelectObject", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // GISSample
    private readonly InputActionMap m_GISSample;
    private List<IGISSampleActions> m_GISSampleActionsCallbackInterfaces = new List<IGISSampleActions>();
    private readonly InputAction m_GISSample_SelectObject;
    public struct GISSampleActions
    {
        private @GISSampleInputActions m_Wrapper;
        public GISSampleActions(@GISSampleInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @SelectObject => m_Wrapper.m_GISSample_SelectObject;
        public InputActionMap Get() { return m_Wrapper.m_GISSample; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GISSampleActions set) { return set.Get(); }
        public void AddCallbacks(IGISSampleActions instance)
        {
            if (instance == null || m_Wrapper.m_GISSampleActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GISSampleActionsCallbackInterfaces.Add(instance);
            @SelectObject.started += instance.OnSelectObject;
            @SelectObject.performed += instance.OnSelectObject;
            @SelectObject.canceled += instance.OnSelectObject;
        }

        private void UnregisterCallbacks(IGISSampleActions instance)
        {
            @SelectObject.started -= instance.OnSelectObject;
            @SelectObject.performed -= instance.OnSelectObject;
            @SelectObject.canceled -= instance.OnSelectObject;
        }

        public void RemoveCallbacks(IGISSampleActions instance)
        {
            if (m_Wrapper.m_GISSampleActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGISSampleActions instance)
        {
            foreach (var item in m_Wrapper.m_GISSampleActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GISSampleActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GISSampleActions @GISSample => new GISSampleActions(this);
    private int m_KeyboardAndMouseSchemeIndex = -1;
    public InputControlScheme KeyboardAndMouseScheme
    {
        get
        {
            if (m_KeyboardAndMouseSchemeIndex == -1) m_KeyboardAndMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardAndMouse");
            return asset.controlSchemes[m_KeyboardAndMouseSchemeIndex];
        }
    }
    public interface IGISSampleActions
    {
        void OnSelectObject(InputAction.CallbackContext context);
    }
}
