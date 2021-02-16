// This addon for unity is made by Xafar
// It implements a way to edge snap modular game objects

using UnityEngine;
using UnityEditor;

public class StitchSnap : EditorWindow
{
	bool isSnapCompleted;
	bool isReadyForSnapping;

	int activeVID, targetVID; // VID stand for vertex index

	GameObject activeObject, targetObject;
	MeshFilter activeMeshFilter, targetMeshFilter;

	Vector3 activeVert, targetVert;

	GameObject tempPivot;
	Transform originalParent;
	Vector3 oldLocation;

	[MenuItem("XGI Tools/Stitch - Snap")]
	public static void ShowWindow() => GetWindow<StitchSnap>("Stitch - Snap");    // Adding editor window as a menu item

	private void OnGUI()
	{
		activeObject = Selection.activeGameObject;

		isReadyForSnapping = false;
		if (activeObject == null)
		{
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("No Objects are selected", MessageType.Info);
		}
		else
		{
			if (Selection.transforms.Length < 2)
			{
				EditorGUILayout.Space();
				GUILayout.Label("Target Object");
				EditorGUILayout.ObjectField(activeObject, typeof(GameObject), false);

				EditorGUILayout.Space();
				EditorGUILayout.HelpBox("Select the object you want to Snap", MessageType.Warning);
			}
			else
			{
				if (Selection.transforms.Length > 2)
				{
					EditorGUILayout.Space();
					EditorGUILayout.HelpBox("More than two objects selected", MessageType.Error);
				}
				else
				{
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					EditorGUILayout.BeginHorizontal();
					GUILayout.Label("Object to Snap");
					EditorGUILayout.ObjectField(activeObject, typeof(GameObject), false);
					EditorGUILayout.EndHorizontal();

					foreach (Transform selectedObject in Selection.transforms) if (selectedObject.gameObject != activeObject) targetObject = selectedObject.gameObject; // Detecting target object
					EditorGUILayout.BeginHorizontal();
					GUILayout.Label("Target Object");
					EditorGUILayout.ObjectField(targetObject, typeof(GameObject), false);
					EditorGUILayout.EndHorizontal();
					for (int i = 0; i < 6; i++) EditorGUILayout.Space();

					if (activeObject.TryGetComponent(out activeMeshFilter) && targetObject.TryGetComponent(out targetMeshFilter))   // If selected object has Mesh Filter component, extract it
					{
						// Things are set up, Main code starts here
						isReadyForSnapping = true;

						GUILayout.Label("Active Object Vertex Index");
						activeVID = EditorGUILayout.IntSlider(activeVID, 0, activeMeshFilter.sharedMesh.vertexCount - 1);
						activeVert = activeObject.transform.TransformPoint(activeMeshFilter.sharedMesh.vertices[activeVID]);

						EditorGUILayout.Space();
						EditorGUILayout.Space();

						GUILayout.Label("Target Object Vertex Index");
						targetVID = EditorGUILayout.IntSlider(targetVID, 0, targetMeshFilter.sharedMesh.vertexCount - 1);
						targetVert = targetObject.transform.TransformPoint(targetMeshFilter.sharedMesh.vertices[targetVID]);
						for (int i = 0; i < 6; i++) EditorGUILayout.Space();

						EditorGUILayout.BeginHorizontal();
						if (isSnapCompleted)
						{
							if (GUILayout.Button("Stitch")) // If stitch button is pressed
							{
								for (int i = 0; i < 3; i++)
								{
									Vector2 requiredRotation;
									requiredRotation.x = Vector3.SignedAngle(Vector3.ProjectOnPlane(activeVert - tempPivot.transform.position, targetObject.transform.up), targetVert - tempPivot.transform.position, Vector3.up);
									tempPivot.transform.rotation = Quaternion.Euler(tempPivot.transform.rotation.eulerAngles.x, tempPivot.transform.rotation.eulerAngles.y + requiredRotation.x, tempPivot.transform.rotation.eulerAngles.z);

									Vector3 rotationAxis = Vector3.Cross(targetObject.transform.up, targetVert - tempPivot.transform.position);
									rotationAxis.Normalize();

									activeObject.transform.SetParent(null);
									tempPivot.transform.right = rotationAxis;
									activeObject.transform.SetParent(tempPivot.transform);

									requiredRotation.y = Vector3.SignedAngle(activeVert - tempPivot.transform.position, targetVert - tempPivot.transform.position, rotationAxis);
									tempPivot.transform.localRotation = Quaternion.Euler(tempPivot.transform.localRotation.eulerAngles.x + requiredRotation.y, tempPivot.transform.localRotation.eulerAngles.y, tempPivot.transform.localRotation.eulerAngles.z);

									activeVert = activeObject.transform.TransformPoint(activeMeshFilter.sharedMesh.vertices[activeVID]);
									targetVert = targetObject.transform.TransformPoint(targetMeshFilter.sharedMesh.vertices[targetVID]);
								} // Process finished

								Disable();
							}
							else if (GUILayout.Button("Undo"))
							{
								Disable();
								activeObject.transform.position = oldLocation;
							}
						}
						else
						{
							if (GUILayout.Button("Snap"))
							{
								oldLocation = activeObject.transform.position;  // Recording current location for undoing this action
								activeObject.transform.position += targetVert - activeVert; // Snapping
								isSnapCompleted = true; // Snapping is done

								// Creating the temporary GameObject
								tempPivot = new GameObject("Temporary_Pivot_Point");
								tempPivot.transform.position = targetVert;
								originalParent = activeObject.transform.parent;
								activeObject.transform.SetParent(tempPivot.transform);
							}
							else if (GUILayout.Button("Level")) activeObject.transform.position = new Vector3(activeObject.transform.position.x, targetObject.transform.position.y, activeObject.transform.position.z);
						}
						EditorGUILayout.EndHorizontal();
					}
					else EditorGUILayout.HelpBox("Make sure that " + activeObject.gameObject.name + " and " + targetObject.gameObject.name + " have a Mesh Filter component attached and enabled", MessageType.Error);
				}
			}
		}

		for (int i = 0; i < 6; i++) EditorGUILayout.Space();
		EditorGUILayout.ObjectField(originalParent, typeof(Transform), false);

		// The process of stitching (edge aligning) after snapping works as follows
		// Create a temparory GameObject at the snaped point
		// Make it a parent of the active GameObject
		// Rotate the newly created GameObject to an angle equal to the angle between the egdes of active and GameObject
		// Reparent active GameObject to its previous parent
		// Destroy temporary GameObject

		Repaint();
	}

	void OnSceneGUI(SceneView sceneView)    // Draws GUI in scene view
	{
		Handles.color = Color.green;
		if (isReadyForSnapping) Handles.DrawLine(activeVert, targetVert);
		sceneView.Repaint();
	}

	void TurnOnSceneGUI() => SceneView.duringSceneGui += this.OnSceneGUI;
	void TurnOffSceneGUI() => SceneView.duringSceneGui -= this.OnSceneGUI;
	void Disable()
	{
		if (isSnapCompleted) isSnapCompleted = false;
		activeObject.transform.SetParent(originalParent);
		DestroyImmediate(tempPivot);
	}

	private void OnFocus()
	{
		isSnapCompleted = false;
		TurnOffSceneGUI();
		TurnOnSceneGUI();
	}

	private void OnLostFocus()
	{
		TurnOffSceneGUI();
		Disable();
	}
	private void OnDestroy()
	{
		TurnOffSceneGUI();
		Disable();
	}
}