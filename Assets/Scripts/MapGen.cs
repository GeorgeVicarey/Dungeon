using UnityEngine;
using System.Collections.Generic;

public class MapGen : MonoBehaviour {
	public Transform tile;
	public Transform border;
	public int width, height, noOfRooms, roomWidthMin = 5, roomWidthMax = 10, roomHeightMin = 20 , roomHeightMax = 50;
    public GameObject prefab;
    public GameObject Player;

	private Tile[,] tiles;
	public List<Room> rooms;

    public int noOfEnemies;

	// Use this for initialization
	void Start () {
		//Random.seed = 974379850;
		Debug.Log (Random.seed);

		//Propogate tile array with new instances of Tile so that the area is not empty when it gets used.
		tiles = new Tile[width, height];
		for (int x = 0; x < width; x++ ) {
			for (int y = 0; y < height; y++ ) {
				tiles[x, y] = new Tile();
				tiles[x, y].type = Tile.Type.Blank;
				tiles[x, y].color = Color.grey;
			}
		}

		//Assign rooms as a new List of Room(s)
		rooms = new List<Room>();

		//Call map gen functions
		genRooms ();
		roomCollision ();
		modelRooms ();
		renderTiles ();
        placeEnemy();

		//place player in center of map
        Instantiate(Player, new Vector3(width/2 , height/2 , -1), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
        EndGame();
	}

	/**
	 * Using a Random Number Generator the rooms List is propogated with procedural rooms.
	 * Up to the value of the noOfRooms variable.
	 * A room is created by choosing a random X and Y position within the maps bounds.
	 * The width and height are then set for teh room by getting a random value between a 
	 * user set min and max width/height.
	 * Then a color is ranodmly assigned to the room.
	 * 
	 * Once a room is generated then a Door location needs to be set .
	 * This is done by choosing a number from 0-4 which number is chosen dictates which side of 
	 * the room gets a door. Ocne a side is known then a position along the side is randomly chosen.
	 */
	void genRooms() {
		for (int i = 0; i < noOfRooms; i++) {
			rooms.Add(new Room());
			rooms [i].pos = new Vector2 (Random.Range (0.0f, (float)width * 0.8f), Random.Range (0.0f, (float)height * 0.8f));
			rooms [i].width = Random.Range (roomWidthMin, roomWidthMax);
			rooms [i].height = Random.Range (roomHeightMin, roomHeightMax);
			rooms [i].color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

			//side will be 0,1,2, or 3
			int side = Random.Range(0, 4);

			// 0 = north, 1 = east, 2 = south, 3 = west
			if (side == 0) 
			{
				rooms[i].doorX = Random.Range((int)rooms[i].pos.x + 1, (int)rooms[i].pos.x + rooms[i].width - 2);
				rooms[i].doorY = (int)rooms[i].pos.y + rooms[i].height - 1;
			}
			else if (side == 1)
			{
				rooms[i].doorX = (int)rooms[i].pos.x + rooms[i].width - 1;
				rooms[i].doorY = Random.Range((int)rooms[i].pos.y + 1, (int)rooms[i].pos.y  + rooms[i].height - 2);
			}
			else if (side == 2)
			{
				rooms[i].doorX = Random.Range((int)rooms[i].pos.x + 1, (int)rooms[i].pos.x + rooms[i].width - 2);
				rooms[i].doorY = (int)rooms[i].pos.y;
			}
			else if (side == 3)
			{
				rooms[i].doorX = (int)rooms[i].pos.x;
				rooms[i].doorY = Random.Range((int)rooms[i].pos.y + 1, (int)rooms[i].pos.y  + rooms[i].height - 2);
			}
		}
	}


	/**
	 * Model rooms cycles through each room in the list and translates it's properties
	 * over to the tiles on the map with which it occupies.
	 * 
	 * Once all the rooms have been set as tiles then teh setBorders function can place borders around rooms.
	 */
	void modelRooms(){
		for (int i = 0; i < rooms.Count; i++) {			
			for (int x = (int)rooms[i].pos.x; x < (int)rooms[i].pos.x + rooms[i].width; x++ ) {
				for (int y = (int)rooms[i].pos.y; y < (int)rooms[i].pos.y + rooms[i].height; y++ ) {
					tiles[x,y].pos = new Vector2(x, y);
					if(x == rooms[i].doorX && y == rooms[i].doorY)
					{
						tiles[x,y].color = Color.grey;
						tiles[x,y].type = Tile.Type.Door;
					}
					else
					{
						tiles[x,y].color = rooms[i].color;
						tiles[x,y].type = Tile.Type.Room;
					}
				}
			}
		}

		setBorders ();
	}


	/**
	 * Cycles through tiles in the array and checks if any room tiles
	 * have a neighbouring blank tile and if they do the tile gets set 
	 * to a border type.
	 */
	void setBorders() {	
		for (int x = 0; x < width; x++ ) {
			for (int y = 0; y < height; y++ ) {
				if (tiles[x,y].type == Tile.Type.Room) {
					if (tiles[x + 1,y].type == Tile.Type.Blank ||
					    tiles[x - 1,y].type == Tile.Type.Blank ||
					    tiles[x,y + 1].type == Tile.Type.Blank ||
					    tiles[x,y - 1].type == Tile.Type.Blank ) {
							tiles[x,y].type = Tile.Type.Border;
					}
				}
			}
		}
	}

	/**
	 * Cycles trhough every tile instantiatign a prefab and setting it's render color depending
	 * on the tiles color.
	 */
	void renderTiles() {
		for (int x = 0; x < width; x++ ) {
			for (int y = 0; y < height; y++ ) {
				if(tiles[x,y].type != Tile.Type.Border) {
					Transform t = (Transform)GameObject.Instantiate(tile, new Vector2(x, y), Quaternion.identity);
					SpriteRenderer SR =  t.GetComponent<SpriteRenderer>();
					SR.color = tiles[x, y].color;
				}
				else
				{
					Transform t = (Transform)GameObject.Instantiate(border, new Vector2(x, y), Quaternion.identity);
					SpriteRenderer SR =  t.GetComponent<SpriteRenderer>();
					SR.color = Color.black;
				}
			}
		}
	}

	/**
	 * Cycles though every pair of rooms and checkd for a collision
	 * if theres overlappign rooms it destroys one of them.
	 */
	void roomCollision() {
		for (int A = 0; A < rooms.Count; A++) {
			for (int B = 0; B < rooms.Count; B++) {
				if(A != B)
				{
					if(checkOverlap(A, B))
					{
						int C = 0;

						if( A > B) 
							C = A;
						if (A < B)
							C = B;

						rooms.RemoveAt(rooms.IndexOf(rooms[C]));
					}
				}
			}
		}
	}

	/*
	 * Fucntion used by roomCollision() to see if two rectangles overlap
	 * 
	 * @return bool - True if there's a overlap
	 */
	bool checkOverlap(int a, int b) 
	{ 
		return !(rooms[a].pos.x + rooms[a].width < rooms[b].pos.x || 
		         rooms[a].pos.y + rooms[a].height < rooms[b].pos.y || 
		         rooms[a].pos.x > rooms[b].pos.x + rooms[b].width || 
		         rooms[a].pos.y > rooms[b].pos.y + rooms[b].height);
	}

	/*
	 * places an enemy in each room.
	 */
    void placeEnemy()
    {
        foreach (Room r in rooms)
        {
            Vector3 pos = new Vector3(r.pos.x + (r.width/2) ,  r.pos.y + (r.height/2) , -1);
            Instantiate(prefab, pos, Quaternion.identity);
            noOfEnemies++;
           
        }
    }

	/*
	 * an endgame condition is checked if there's 0 enemies left.
	 */
    void EndGame()
    {
        if (noOfEnemies == 0)
        {
            
        }
    }
}
