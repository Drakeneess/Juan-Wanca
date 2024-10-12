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
    private void Start()
    {
        CreateDungenon();
    }

    private void CreateDungenon()
    {
        RoomGeneratr generator =new RoomGeneratr(roomWidth, roomLenght);
        var listifRooms = generator.CalculateRooms
            (maxIterations, roomwidhtMin, roomLenghtMin,roomBottomCornerModifier,roomTopCornerModifier,roomOffset);
        for (int i = 0; i < listifRooms.Count; i++) {

            CreateMesh(listifRooms[i].BottomLeftAreaCorner, listifRooms[i].TopRightAreaCorner);
        }
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
    }

}