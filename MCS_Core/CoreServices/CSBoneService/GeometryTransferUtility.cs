// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MCS.FOUNDATIONS;
using MCS.CONSTANTS;
using MCS.COSTUMING;

namespace MCS.CORESERVICES
{
	internal static class GeometryTransferUtility
	{
		/// <summary>
		/// Attaches a clone of the costume item to given CIbody figure.
		/// Send the GameObject of a costumeItem here, and we'll clone it, bind it and return the result
		/// </summary>
		/// <returns>The cloned GameObject of the costume_item</returns>
		/// <param name="costume_item">Costume_item.</param>
		/// <param name="figure">Figure.</param>
		public static GameObject CloneAndAttachCostumeItemToFigure (GameObject costume_item, BoneUtility.BoneMap figure_bone_map, CIbody figure, Transform root_bone)
		{
			// instatiate copy
			GameObject clone = GameObject.Instantiate (costume_item);
			clone.name = costume_item.name;

			// use the basic Attachment method
			GeometryTransferUtility.AttachCostumeItemToFigure (clone, figure_bone_map, figure, root_bone);

			// return the cloned costume_item GameObject
			return clone;
		}



		/// <summary>
		/// Attachs the costume_item to the given CIbody figure.
		/// </summary>
		/// <returns>The GameObject costume_item</returns>
		/// <param name="costume_item">Costume item.</param>
		/// <param name="figure_bone_map">Figure bone map.</param>
		/// <param name="figure">Figure.</param>
		/// <param name="root_bone">Root bone.</param>
		/// <remarks>Unneccesary return of parameter GameObject as both are the same GameObject</remarks>
		public static GameObject AttachCostumeItemToFigure(GameObject costume_item, BoneUtility.BoneMap figure_bone_map, CIbody figure, Transform root_bone)
		{
			// add to figure
			costume_item.transform.parent = figure.transform;

			// does not need "costume_item = " as BindGeometryToTransform alters the costume_item GameObject anyway
			costume_item = BindGeometryToTransform (costume_item, figure.transform);

			// need figure bones for targeting
			// BoneMap body_bones = new BoneMap(transform);
			
			// go through each coremesh and rebind bones n what not
			CoreMesh[] meshes = costume_item.GetComponentsInChildren<CoreMesh> (true);
			foreach (CoreMesh mesh in meshes) {
                if (mesh.meshType == MESH_TYPE.PROP)
                {
                    continue;
                }

                /*
                if(mesh.skinnedMeshRenderer.rootBone.name != "hip")
                {
                    Transform[] bones = mesh.skinnedMeshRenderer.bones;
                    foreach(Transform bone in bones)
                    {
                        if(bone.name == "hip")
                        {
                            UnityEngine.Debug.Log("Re-assigned hip bone as root");
                            mesh.skinnedMeshRenderer.rootBone = bone;
                            break;
                        }
                    }
                }
                */

                mesh.skinnedMeshRenderer.bones = BoneUtility.RemapBones (mesh.skinnedMeshRenderer.bones, mesh.skinnedMeshRenderer.rootBone, figure_bone_map, root_bone);
                //UnityEngine.Debug.Log("Remapped: " + mesh.name + " bones: " + mesh.skinnedMeshRenderer.bones.Length + " root: " + mesh.skinnedMeshRenderer.rootBone.name);
                mesh.skinnedMeshRenderer.rootBone = root_bone;
			}
			
			return costume_item;
		}



		/// <summary>
		/// Binds the reference Geometry to a given Transform, matching position, rotation, and scale.
		/// </summary>
		/// <returns>The reference Geometry GameObject</returns>
		/// <param name="referenceGeometry">Reference geometry.</param>
		/// <param name="destination_transform">Destination transform.</param>
		/// <remarks>Unneccesary return of parameter GameObject as both are the same GameObject</remarks>
		public static GameObject BindGeometryToTransform (GameObject referenceGeometry, Transform destination_transform)
		{
			referenceGeometry.transform.parent = destination_transform;
			referenceGeometry.transform.localPosition = referenceGeometry.transform.localPosition;
			referenceGeometry.transform.localRotation = referenceGeometry.transform.localRotation;
			referenceGeometry.transform.localScale = referenceGeometry.transform.localScale;
			return referenceGeometry;
		}



	}
}
