﻿#if UNITY_EDITOR
using AssetBundlesClass.Game.Vehicles;
using AssetBundlesClass.Extensions;
using UnityEditor;
using UnityEngine;
using AssetBundlesClass.Game.Vehicles.Wheel;

namespace AssetBundlesClass.Editor.Vehicles
{
    [CustomEditor(typeof(VehicleBehaviour))]
    public class VehicleBehaviourEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            bool setupWheels = GUILayout.Button("Setup wheels");
            EditorGUILayout.Separator();
            base.OnInspectorGUI();
            if (setupWheels) SetupWheels();
        }

        private void SetupWheels()
        {
            VehicleBehaviour vehicle = target as VehicleBehaviour;
            Transform transform = vehicle.transform;

            Transform[] wheels = transform.FindChildren("wheel");
            Transform[] frontWheels = wheels.Filter(each => each.name.Contains("-F-"));
            Transform[] rearWheels = wheels.Filter(each => each.name.Contains("-R-"));

            WheelCollection frontWheelCollection = new WheelCollection(frontWheels.Map(each => new WheelController(each)));
            WheelCollection rearWheelCollection = new WheelCollection(rearWheels.Map(each => new WheelController(each)));

            serializedObject.FindProperty("_frontWheels").managedReferenceValue = frontWheelCollection;
            serializedObject.FindProperty("_rearWheels").managedReferenceValue = rearWheelCollection;

            EditorUtility.SetDirty(vehicle);
        }
    }
}
#endif