using PlasticGui.Diff;
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyStateManager))]
public class EnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemyStateManager enemy = (EnemyStateManager)target;

        // Instantiate the array if it is null
        if (enemy.wanderPoints == null)
        {
            enemy.wanderPoints = new GameObject[0];
        }

        if (GUILayout.Button("Add Wander Point"))
        {
            // Load the WanderPoint prefab from the correct path
            string path = "Assets/Prefabs/Enemy/WanderPointPrefab.prefab";
            GameObject newWanderPoint = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path)) as GameObject;
            newWanderPoint.transform.parent = enemy.transform.parent;
            newWanderPoint.name = "WanderPoint" + enemy.wanderPoints.Length;
            newWanderPoint.transform.position = enemy.transform.position;
            Refactor(enemy);
            // Add the new wander point to the enemy's wanderPoints array
            ArrayUtility.Add(ref enemy.wanderPoints, newWanderPoint);
        }

        for (int i = 0; i < enemy.wanderPoints.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();

            enemy.wanderPoints[i] = (GameObject)EditorGUILayout.ObjectField(enemy.wanderPoints[i], typeof(GameObject), true);

            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                GameObject obj = enemy.wanderPoints[i];
                ArrayUtility.RemoveAt(ref enemy.wanderPoints, i);
                DestroyImmediate(obj);
                Refactor(enemy);
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    public void Refactor(EnemyStateManager enemy)
    {
        foreach (GameObject item in enemy.wanderPoints)
        {
            item.name = "WanderPoint" + Array.IndexOf(enemy.wanderPoints, item);
        }
    }

}

