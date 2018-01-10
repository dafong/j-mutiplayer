using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KeywordTest : MonoBehaviour {

	public bool IsTintOn;
	[Range(1,300)]
	public int lod;

	public Material mat;

	void OnValidate(){
		
		if(IsTintOn){
			Shader.EnableKeyword("_TINT_ON");
		}else{
			Shader.DisableKeyword("_TINT_ON");
		}
		if(mat == null)
			mat = GetComponent<MeshRenderer>().sharedMaterial;
		mat.shader.maximumLOD = lod;
//		Shader.globalMaximumLOD= lod;
	}

}
