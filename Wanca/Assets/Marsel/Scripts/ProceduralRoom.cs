using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProceduralRoom : MonoBehaviour
{
  
    public int roomWidth, roomLenght;
    public int roomwidhtMin, roomLenghtMin;
    public int maxIterations;
    public int corridorWidht;
    public Material meterial;
    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;
    [Range(0.7f,1.0f)]
    public float roomTopCornerModifier;
    [Range(0, 2)]
    public int roomOffset;
    public GameObject wallVertical ,wallHorizontal;
    List<Vector3Int> PossibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticaPosition;

    private void Start()
    {
        CreateDungenon();
        
    }

    public void CreateDungenon()
    {
       //destroyAllChildren();
        RoomGeneratr generator =new RoomGeneratr(roomWidth, roomLenght);
        var listifRooms = generator.CalculateRooms
            (maxIterations, roomwidhtMin, roomLenghtMin,roomBottomCornerModifier,roomTopCornerModifier,roomOffset,corridorWidht);
        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        PossibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>(); ;
        possibleWallHorizontalPosition = new List<Vector3Int>(); ;
        possibleWallVerticaPosition = new List<Vector3Int>(); ;

        for (int i = 0; i < listifRooms.Count; i++) {

            CreateMesh(listifRooms[i].BottomLeftAreaCorner, listifRooms[i].TopRightAreaCorner);
        }
        CreateWalls(wallParent);
        
    }

    private void CreateWalls(GameObject wallParent)
    {
        foreach (var wallPositionv in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, wallPositionv, wallHorizontal);
        }
        foreach (var wallPosition in possibleWallVerticaPosition)
        {
            CreateWall(wallParent,wallPosition, wallVertical);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPositionv, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPositionv, Quaternion.identity,wallParent.transform);
    }

    private void CreateMesh(Vector2 bottomLeftCorner , Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x,0,bottomLeftCorner.y);
        Vector3 bottomRighV =new Vector3 (topRightCorner.x,0,bottomLeftCorner.y);
        Vector3 topLeftV =new Vector3(bottomLeftCorner.x,0,topRightCorner.y);
        Vector3 topRightV =new Vector3 (topRightCorner.x ,0,topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,topRightV,bottomLeftV,bottomRighV
        };
        Vector2[]uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i]= new Vector2(vertices[i].x,vertices[i].z);
        }
        int[] triangles = new int[]
        {
            0,1,2,2,1,3
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        GameObject dngeFloor = new GameObject("Mesh"+bottomLeftCorner,typeof(MeshFilter),typeof(MeshRenderer)); 
        dngeFloor.transform.position= Vector3.zero;
        dngeFloor.transform.localScale= Vector3.one;
        dngeFloor.GetComponent<MeshFilter>().mesh = mesh;
        dngeFloor.GetComponent<MeshRenderer>().material= meterial;
       // dngeFloor.transform.parent = transform;
        for (int row = (int)bottomLeftV.x; row < (int)(bottomRighV.x); row++)
        {
            var wallPosition= new Vector3(row,0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int row =(int)topLeftV.x; row<(int)topRightCorner.x;row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int col= (int)bottomLeftV.z; col < (int)topLeftV.z;col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticaPosition, PossibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRighV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRighV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticaPosition, PossibleDoorVerticalPosition);
        }
    }

    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }
    private void destroyAllChildren()
    {
        while (transform.childCount != 0) 
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }

    }
}