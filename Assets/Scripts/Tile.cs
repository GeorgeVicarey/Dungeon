using UnityEngine;
using System.Collections;

public class Tile {
	public Vector2 pos;
	public Color color;
	public Type type;
		
	public enum Type {Room, Path, Door, Blank, Border};

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
