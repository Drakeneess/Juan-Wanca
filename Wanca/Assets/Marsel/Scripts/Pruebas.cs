using System.Collections.Generic;
using UnityEngine;

public class ProceduralRooms : MonoBehaviour
{
    public Mesh wallMesh; // Malla de la pared
    public Material wallMaterial0; // Material para las paredes
    public Material wallMaterial1; // Material para las paredes
    public Vector3 roomSize = new Vector3(20f, 10f, 20f); // Tamaño de la habitación
    public Vector3 wallSize = new Vector3(5f, 10f, 1f); // Tamaño de cada pared

    private List<Matrix4x4> wallMatricesN; // Lista de matrices de transformación para las paredes

    void Start()
    {
        createWalls(); // Generar las paredes al iniciar
    }

    void Update()
    {
        createWalls(); // Actualizar las paredes en tiempo real si el tamaño de la habitación cambia
        renderWalls(); // Renderizar las paredes
    }

    // Generar las paredes en los cuatro lados del cuarto
    void createWalls()
    {
        wallMatricesN = new List<Matrix4x4>();

        // Lado superior e inferior (horizontal)
        int wallCountX = Mathf.Max(1, (int)(roomSize.x / wallSize.x));
        float scaleX = (roomSize.x / wallCountX) / wallSize.x;

        // Paredes superiores e inferiores
        for (int i = 0; i < wallCountX; i++)
        {
            var tBottom = transform.position + new Vector3(-roomSize.x / 2 + wallSize.x * scaleX / 2 + i * scaleX * wallSize.x, 0, -roomSize.z / 2);
            var tTop = transform.position + new Vector3(-roomSize.x / 2 + wallSize.x * scaleX / 2 + i * scaleX * wallSize.x, 0, roomSize.z / 2);
            var r = transform.rotation;
            var s = new Vector3(scaleX, 1, 1);

            wallMatricesN.Add(Matrix4x4.TRS(tBottom, r, s)); // Añadir matriz para pared inferior
            wallMatricesN.Add(Matrix4x4.TRS(tTop, r, s));    // Añadir matriz para pared superior
        }

        // Lados izquierdo y derecho (vertical)
        int wallCountZ = Mathf.Max(1, (int)(roomSize.z / wallSize.x));
        float scaleZ = (roomSize.z / wallCountZ) / wallSize.x;

        // Paredes izquierda y derecha
        for (int i = 0; i < wallCountZ; i++)
        {
            var tLeft = transform.position + new Vector3(-roomSize.x / 2, 0, -roomSize.z / 2 + wallSize.x * scaleZ / 2 + i * scaleZ * wallSize.x);
            var tRight = transform.position + new Vector3(roomSize.x / 2, 0, -roomSize.z / 2 + wallSize.x * scaleZ / 2 + i * scaleZ * wallSize.x);
            var r = Quaternion.Euler(0, 90, 0); // Rotación para paredes en los lados
            var s = new Vector3(scaleZ, 1, 1);

            wallMatricesN.Add(Matrix4x4.TRS(tLeft, r, s));   // Añadir matriz para pared izquierda
            wallMatricesN.Add(Matrix4x4.TRS(tRight, r, s));  // Añadir matriz para pared derecha
        }
    }

    // Renderizar las paredes instanciadas
    void renderWalls()
    {
        if (wallMatricesN != null)
        {
            // Renderiza las paredes con los dos materiales usando la malla instanciada
            Graphics.DrawMeshInstanced(wallMesh, 0, wallMaterial0, wallMatricesN.ToArray(), wallMatricesN.Count);
            Graphics.DrawMeshInstanced(wallMesh, 1, wallMaterial1, wallMatricesN.ToArray(), wallMatricesN.Count);
        }
    }
}
