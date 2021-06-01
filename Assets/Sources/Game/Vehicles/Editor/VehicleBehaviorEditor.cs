#if UNITY_EDITOR
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
        private GUIStyle _headerStyle;
        
        private WheelAxisCorrection _correction = default;
        
        public override void OnInspectorGUI()
        {
            _headerStyle ??= new GUIStyle(EditorStyles.boldLabel) {fontSize = 16};
            
            EditorGUILayout.LabelField("Editor region", _headerStyle);
            
            EditorGUILayout.Separator();
            bool setupWheels = GUILayout.Button("Setup wheels");
            _correction = (WheelAxisCorrection)EditorGUILayout.EnumPopup("Wheel correction:", _correction);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Script region", _headerStyle);

            EditorGUILayout.Separator();
            base.OnInspectorGUI();
            if (setupWheels) SetupWheels();
        }

        private void SetupWheels()
        {
            VehicleBehaviour vehicle = (VehicleBehaviour)target;
            Transform transform = vehicle.transform;

            Transform[] wheels = transform.FindChildren("wheel");
            Transform[] frontWheels = wheels.Filter(each => each.name.Contains("-F-"));
            Transform[] rearWheels = wheels.Filter(each => each.name.Contains("-R-"));

            vehicle.frontWheels = new WheelCollection(frontWheels.Map(each => new WheelController(each, _correction)));
            vehicle.rearWheels = new WheelCollection(rearWheels.Map(each => new WheelController(each, _correction)));

            EditorUtility.SetDirty(vehicle);
        }
    }
}
#endif