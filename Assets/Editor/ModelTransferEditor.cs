using UnityEngine;
using UnityEditor;
using System;

public class MaterialTransferEditor : EditorWindow
{
    private GameObject sourceModel;
    private GameObject destinationModel;

    [MenuItem("Tools/Material Transfer")]
    public static void ShowWindow()
    {
        GetWindow<MaterialTransferEditor>("Material Transfer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Material Transfer Tool", EditorStyles.boldLabel);

        sourceModel = (GameObject)EditorGUILayout.ObjectField("Source Model", sourceModel, typeof(GameObject), true);
        destinationModel = (GameObject)EditorGUILayout.ObjectField("Destination Model", destinationModel, typeof(GameObject), true);

        if (GUILayout.Button("Transfer Materials"))
        {
            TransferMaterials();
        }
    }

    private void TransferMaterials()
    {
        if (sourceModel == null || destinationModel == null)
        {
            Debug.LogError("Both Source and Destination models must be set.");
            return;
        }

        SkinnedMeshRenderer[] sourceRenderers = sourceModel.GetComponentsInChildren<SkinnedMeshRenderer>();
        SkinnedMeshRenderer[] destinationRenderers = destinationModel.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (var destinationRenderer in destinationRenderers)
        {
            Material[] newDestinationMaterials = new Material[destinationRenderer.sharedMaterials.Length];

            for (int destMatIndex = 0; destMatIndex < destinationRenderer.sharedMaterials.Length; destMatIndex++)
            {
                Material destinationMaterial = destinationRenderer.sharedMaterials[destMatIndex];

                // Find matching material in source renderers based on the material name
                foreach (var sourceRenderer in sourceRenderers)
                {
                    Material sourceMaterial = Array.Find(sourceRenderer.sharedMaterials, m => m != null && m.name.Equals(destinationMaterial.name));

                    if (sourceMaterial != null)
                    {
                        newDestinationMaterials[destMatIndex] = sourceMaterial;
                        break;
                    }
                }
            }

            destinationRenderer.sharedMaterials = newDestinationMaterials;
        }

        Debug.Log("Materials transferred successfully.");
    }
}
