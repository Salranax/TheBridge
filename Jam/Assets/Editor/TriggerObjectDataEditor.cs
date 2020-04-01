using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(TriggerObjectData))]
public class TriggerObjectDataEditor : Editor
{
    public override void OnInspectorGUI(){
        TriggerObjectData _triggerData = (TriggerObjectData)target;

        
        TriggerType _type = (TriggerType)EditorGUILayout.EnumPopup("Trigger Type" ,_triggerData.triggerType);
        _triggerData.triggerType = _type;

        if(_triggerData.triggerType == TriggerType.PillarofDarkness){
            _triggerData.singleTrigger = false;
           _triggerData.triggerDistance = EditorGUILayout.FloatField("Trigger Distance",_triggerData.triggerDistance);
           _triggerData.duration = EditorGUILayout.IntField("Duration", _triggerData.duration);
           _triggerData.cooldown = EditorGUILayout.IntField("Cooldown", _triggerData.cooldown);
        }
    }
}
