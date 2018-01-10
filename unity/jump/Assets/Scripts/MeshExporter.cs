using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

public abstract class Parser{
	public virtual void DealWith (string line){ }
}

public class Builder{
	
	public virtual void BuildMesh (Mesh mesh){ }
}


public enum AttrType{
	Vertex,
	Normal,
	VertexColor,
	Texcoord1,
	Texcoord2
}

[Serializable]
public class Mapper{

	public AttrType src;
	public AttrType dest;

	public Parser parser;
	public Builder builder;

	public void Init(){
		switch(src){
		case AttrType.Vertex:
			parser = new VertexParser ();
			break;
		case AttrType.Normal:
			parser = new NormalParser ();
			break;
		case AttrType.Texcoord1:
		case AttrType.Texcoord2:
			parser = new UVParser ();
			break;
		}

		switch (dest) {
		case AttrType.Vertex:
			builder = new VertexBuilder (parser);
			break;
		case AttrType.Normal:
			builder = new NormalBuilder (parser);
			break;
		case AttrType.VertexColor:
			builder = new ColorBuilder (parser);
			break;
		case AttrType.Texcoord1:
			builder = new UVBuilder (parser, 0);
			break;
		case AttrType.Texcoord2:
			builder = new UVBuilder (parser, 1);
			break;
		}
	}
	public void Parse(string line){
		parser.DealWith (line);
	}

	public void Build(Mesh mesh){
		builder.BuildMesh (mesh);
	}
}

[Serializable]
public class DataMapper{
	public TextAsset dataFile;
	public Mapper[] mappers;

	public void Init(){
		for (int i = 0; i < mappers.Length; i++) {
			mappers [i].Init ();
		}
	}

	public void Parse(){
		if (dataFile == null)
			return;

		StringReader reader = new StringReader (dataFile.text);
		string line;
		while ((line = reader.ReadLine ()) != null) {
			Debug.Log ("line: " + line);
			for (int i = 0; i < mappers.Length; i++) {
				mappers [i].Parse (line);
			}
		}
	}


	public void Build(Mesh mesh){
		for (int i = 0; i < mappers.Length; i++) {
			mappers [i].Build (mesh);
		}
	}
}


public class VertexParser : Parser{
	public List<Vector3> vertexs;
	public List<int> triangles;

	public VertexParser(){
		vertexs   = new List<Vector3> ();
		triangles = new List<int> ();
	}

	public override void DealWith (string line){

		string[] fields = line.Split (' ' );
		if (fields.Length < 1)
			return;
		string flag = fields [0];
		if (flag.Equals("v")) {
			Vector3 vec = new Vector3 (
				float.Parse (fields [1]),
				float.Parse (fields [2]),
				float.Parse (fields [3])
			);
			vertexs.Add (vec);
		}
		if (flag.Equals ("f")) {
			triangles.Add (int.Parse (fields [1].Split ('/') [0])-1);
			triangles.Add (int.Parse (fields [2].Split ('/') [0])-1);
			triangles.Add (int.Parse (fields [3].Split ('/') [0])-1);
		}
	}
}

public class NormalParser : Parser{
	public List<Vector4> normals;

	public NormalParser(){
		normals = new List<Vector4> ();
	}

	public override void DealWith (string line){
		string[] fields = line.Split (new char[]{' '});
		if (fields.Length < 1)
			return;
		string flag = fields [0];
		if (flag.Equals ("vn")) {
			Vector4 vec = new Vector4 (
				float.Parse (fields [1]),
				float.Parse (fields [2]),
				float.Parse (fields [3]),
				fields.Length > 4? float.Parse (fields [4]) : 1
			);
			normals.Add (vec);
		}
	}
}

public class UVParser:Parser{
	public List<Vector2> uvs;

	public UVParser(){
		uvs = new List<Vector2> ();
	}

	public override void DealWith(string line){
		string[] fields = line.Split (' ');
		if (fields.Length < 1)
			return;
		string flag = fields [0];
		if (flag.Equals ("vt")) {
			Vector2 vec = new Vector2 (
				float.Parse (fields [1]),
				float.Parse (fields [2])
			);
			uvs.Add (vec);
		}
	}
}



public class VertexBuilder : Builder{

	protected Parser parser;
	public VertexBuilder(Parser p){
		parser = p;
	}
	public override void BuildMesh (Mesh mesh){
		if (parser is VertexParser) {
			VertexParser p = parser as VertexParser;
			mesh.vertices  = p.vertexs.ToArray ();
			mesh.triangles = p.triangles.ToArray ();
		}
	} 
}

public class ColorBuilder : Builder{
	protected Parser parser;
	public ColorBuilder(Parser p){
		parser = p;
	}
	public override void BuildMesh (Mesh mesh){
		List<Vector4> data;
		Color[] colors = null;
		if(parser is NormalParser){
			data   = (parser as NormalParser).normals;
			colors = new Color[data.Count];
			for(int i=0;i<data.Count;i++){
				colors [i] = new Color (data[i].x,data[i].y,data[i].z,data[i].w);
			}
		}

		mesh.colors = colors;
	}
}

public class NormalBuilder : Builder{
	protected Parser parser;
	public NormalBuilder(Parser p){
		parser = p;
	}
	public override void BuildMesh (Mesh mesh){
		if (parser is NormalParser) {
			NormalParser p = parser as NormalParser;
			List<Vector3> ns = new List<Vector3> ();
			for(int i = 0;i<p.normals.Count;i++){
				ns.Add(
					new Vector3(
						p.normals[i].x,
						p.normals[i].y,
						p.normals[i].z
					)
				);

			}
			mesh.normals = ns.ToArray ();
		}
	}
}

public class UVBuilder : Builder{
	protected Parser parser;
	private int channel;
	public UVBuilder(Parser p,int uvchannel){
		parser = p;
		channel = uvchannel;
	}

	public override void BuildMesh (Mesh mesh){
		if (parser is UVParser) {
			UVParser p = parser as UVParser;
			mesh.SetUVs (channel, p.uvs);
		}
	}
}




public class MeshExporter : MonoBehaviour {
	
	[SerializeField]
	public DataMapper[] datas;

	void Awake(){
//		Application.targetFrameRate = 30;
	}

	public void Export(){
		for (int i = 0; i < datas.Length; i++) {
			datas [i].Init ();
		}

		Mesh mesh = new Mesh ();
		for (int i = 0; i < datas.Length; i++) {
			datas [i].Parse ();
			datas [i].Build(mesh);
		}
		AssetDatabase.CreateAsset (mesh, "Assets/Output1.asset");
		AssetDatabase.SaveAssets ();
	}
}
