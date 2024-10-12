using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralRoom))]
public class RoomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ProceduralRoom roomCreator = (ProceduralRoom)target;
        if (GUILayout.Button("CreateNewLevel"))
        {
            roomCreator.CreateDungenon();
        }
    }

}
