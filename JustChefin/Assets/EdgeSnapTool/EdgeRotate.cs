using UnityEngine;
using UnityEditor;

public class EdgeRotate : EditorWindow
{
	bool isRotationActive, scriptState;
	int startID, endID;

	GameObject active;
	GameObject tempAxis;
	GameObject originalActive;
	Vector3 startPt, endPt;
	Mesh activeMesh;
	MeshFilter activeMeshFilter;
	Transform originalParent;
	Tool originalTool;

	[MenuItem("XGI Tools/Edge Rotate")]
	public static void ShowWindow() => GetWindow<EdgeRotate>("Edge Rotate");

	private void OnGUI()
	{
		scriptState = EditorGUILayout.Toggle(scriptState);
		if (scriptState)
		{
			active = Selection.activeGameObject;
			TurnOnSceneGUI();
			if (active == null) EditorGUILayout.HelpBox("No GameObject Selected", MessageType.Info);
			else if (active.TryGetComponent(out activeMeshFilter))
			{
				activeMesh = activeMeshFilter.sharedMesh;
				startID = EditorGUILayout.IntSlider(startID, 0, activeMesh.vertexCount - 1);
				endID = EditorGUILayout.IntSlider(endID, 0, activeMesh.vertexCount - 1);
				startPt = active.transform.TransformPoint(activeMesh.vertices[startID]);
				endPt = active.transform.TransformPoint(activeMesh.vertices[endID]);

				if (GUILayout.Button("Edge Rotate"))
				{
					isRotationActive = true;
					Undo.RecordObject(active, "Edge Rotate");
					tempAxis = new GameObject("Temporary_Axis");
					tempAxis.transform.position = startPt;
					Vector3 edge = endPt - startPt;
					tempAxis.transform.forward = edge.normalized;
					originalParent = active.transform.parent;
					originalTool = Tools.current;
					originalActive = active;
					active.transform.SetParent(tempAxis.transform);
					Tools.current = Tool.Rotate;
					Selection.activeObject = tempAxis;
				}
			}
			else if (isRotationActive)
			{
				if (GUILayout.Button("Apply"))
				{
					isRotationActive = false;
					Tools.current = originalTool;
					originalActive.transform.SetParent(originalParent);
					TurnOffSceneGUI();
					DestroyImmediate(tempAxis);
				}
			}
			else EditorGUILayout.HelpBox("Selected object does not have an active MeshFilter component", MessageType.Error);
		}
		else TurnOffSceneGUI();
		Repaint();
	}

	void TurnOnSceneGUI() => SceneView.duringSceneGui += OnSceneGUI;
	void TurnOffSceneGUI() => SceneView.duringSceneGui -= OnSceneGUI;

	void OnSceneGUI(SceneView sceneView)
	{
		Handles.color = Color.green;
		Handles.DrawLine(startPt, endPt);
		sceneView.Repaint();
	}

	private void OnEnable()
	{
		isRotationActive = false;
	}

	private void OnDisable()
	{
		TurnOffSceneGUI();
		Tools.current = originalTool;
		active.transform.SetParent(originalParent);
		DestroyImmediate(tempAxis);
	}
}