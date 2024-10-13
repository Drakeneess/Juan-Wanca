using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DungeonCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonLength;
    public int roomWidthMin, roomLengthMin;
    public int maxIterations;
    public int corridorWidth;
    public Material material;
    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;
    [Range(0.7f, 1.0f)]
    public float roomTopCornerMidifier;
    [Range(0, 2)]
    public int roomOffset;
    public GameObject wallVertical, wallHorizontal;
    [SerializeField]
    private List<DungeonTheme> themes = new List<DungeonTheme>();
    [SerializeField]
    private List<GameObject> ObjetosEscenario= new List<GameObject>();
    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;
    private AudioSource audioSource;
    private List<GameObject> dungeonMeshes = new List<GameObject>();
    public GameObject Player;
    public GameObject GetFirstMesh()
    {
        if (dungeonMeshes.Count > 0)
        {
            return dungeonMeshes[0]; // Devuelve el primer mesh
        }
        return null; // Si no hay meshes, devuelve null
    }

    public GameObject GetLastMesh()
    {
        if (dungeonMeshes.Count > 0)
        {
            return dungeonMeshes[dungeonMeshes.Count - 1]; // Devuelve el último mesh
        }
        return null; // Si no hay meshes, devuelve null
    }

    public void TeleportToSurface()
    {
        GameObject MeshInicial = GetFirstMesh();
        if (Player != null && MeshInicial != null)
        {
            // Obtener la posición de la superficie
            Vector3 targetPosition = MeshInicial.transform.position;

            // Asegurarse de que el jugador se teletransporta a la superficie
            // con una altura adecuada (puedes ajustar el valor de "y" si es necesario)
            Player.transform.position = new Vector3(targetPosition.x, targetPosition.y + 1, targetPosition.z);

            // También podrías querer ajustar la rotación del jugador para que coincida con la de la superficie
            Player.transform.rotation = MeshInicial.transform.rotation;

            Debug.Log("Jugador teletransportado a la superficie: " + MeshInicial.name);
        }
    }
    void Start()
    {
        // Selecciona una temática aleatoria y aplícala
        SelectRandomTheme();
        CreateDungeon();
    }

    // Método para seleccionar una temática aleatoria
    public void SelectRandomTheme()
    {
        int randomIndex = UnityEngine.Random.Range(0, themes.Count);
        DungeonTheme selectedTheme = themes[randomIndex];
       // selectedTheme.themeName = "Boliviana";
        // Asignar el material y los objetos seleccionados a los campos
        material = selectedTheme.floorMaterial;
        wallVertical = selectedTheme.wallVerticalPrefab;
        wallHorizontal = selectedTheme.wallHorizontalPrefab;
        audioSource =GetComponent<AudioSource>();
        if (audioSource != null && selectedTheme.audioSource!=null)
        {
            // Asignar el AudioClip al AudioSource
            audioSource.clip = selectedTheme.audioSource;

            // Reproducir el sonido automáticamente
            audioSource.Play();
        }

        Debug.Log("Tema seleccionado: " + selectedTheme.themeName);
    }

    public void CreateDungeon()
    {
        DestroyAllChildren();
        DugeonGenerator generator = new DugeonGenerator(dungeonWidth, dungeonLength);
        var listOfRooms = generator.CalculateDungeon(maxIterations,
            roomWidthMin,
            roomLengthMin,
            roomBottomCornerModifier,
            roomTopCornerMidifier,
            roomOffset,
            corridorWidth);
        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }
        CreateWalls(wallParent);
    }

    private void CreateWalls(GameObject wallParent)
    {
        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition, wallHorizontal);
        }
        foreach (var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, wallPosition, wallVertical);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        
        GameObject dungeonFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        dungeonFloor.transform.position = Vector3.zero; 
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.transform.parent = transform;
        dungeonMeshes.Add(dungeonFloor);
        /*MeshCollider meshCollider = dungeonFloor.AddComponent<MeshCollider>();

        // Asignar el mesh al MeshCollider
        meshCollider.sharedMesh = mesh;

        // Habilitar convex
        meshCollider.convex = true;*/
        BoxCollider boxCollider = dungeonFloor.AddComponent<BoxCollider>();

        // Ajustar el tamaño del BoxCollider al tamaño del Mesh
        /*boxCollider.size = new Vector3(topRightCorner.x - bottomLeftCorner.x, 1, topRightCorner.y - bottomLeftCorner.y);
        boxCollider.center = new Vector3((bottomLeftCorner.x + topRightCorner.x) / 2, 0.5f, (bottomLeftCorner.y + topRightCorner.y) / 2);*/
        // Añadir objetos aleatorios al Mesh
        AddRandomObjectsToMesh(ObjetosEscenario,dungeonFloor, bottomLeftCorner, topRightCorner);
        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
    }
    private void AddRandomObjectsToMesh(List<GameObject> ListaDeObjetosPalEscenario,GameObject meshObject, Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        int countObj= ListaDeObjetosPalEscenario.Count;
        int numberOfObjects = 30; // Define cuántos objetos quieres generar
        float minDistance = 3.5f; // Mínima distancia que debe haber entre objetos

        for (int i = 0; i < numberOfObjects; i++)
        {
            int attempts = 0;
            bool canPlace = false;
            Vector3 randomPosition = Vector3.zero;

            // Intentar encontrar una posición válida hasta un máximo de 10 intentos
            while (!canPlace && attempts < 35)
            {
                // Generar una posición aleatoria dentro del área del mesh
                float randomX = UnityEngine.Random.Range(bottomLeftCorner.x, topRightCorner.x);
                float randomZ = UnityEngine.Random.Range(bottomLeftCorner.y, topRightCorner.y);
                randomPosition = new Vector3(randomX, 0.75f, randomZ); // Posición sobre el plano XZ

                // Verificar si hay algún objeto en la misma área XZ
                if (IsPositionValid(randomPosition, minDistance, meshObject))
                {
                    canPlace = true; // La posición es válida
                }

                attempts++; // Incrementar el número de intentos
            }

            if(canPlace)
{
                // Instanciar un objeto aleatorio desde la lista
                int randomObj = UnityEngine.Random.Range(0, countObj);
                GameObject objInstance = Instantiate(ListaDeObjetosPalEscenario[randomObj], randomPosition, Quaternion.identity); // Instanciar el objeto

                // Cambiar la escala del objeto
                objInstance.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.5f, 1.5f); // Escala aleatoria

                // Hacer que el objeto sea hijo del mesh
                objInstance.transform.parent = meshObject.transform; // Esto es correcto porque objInstance no es un prefab
            }
            else
            {
                Debug.Log("No se encontró una posición válida para el objeto " + i);
            }
        }
    }

    // Método para verificar si una posición es válida
    private bool IsPositionValid(Vector3 position, float minDistance, GameObject meshObject)
    {
        // Obtener todos los hijos del meshObject (que pueden ser otros objetos colocados)
        foreach (Transform child in meshObject.transform)
        {
            // Comparar solo las posiciones en XZ ignorando la Y
            Vector3 childPosition = child.position;
            float distanceXZ = Vector2.Distance(new Vector2(position.x, position.z), new Vector2(childPosition.x, childPosition.z));

            // Si algún objeto está dentro de la distancia mínima, la posición no es válida
            if (distanceXZ < minDistance)
            {
                return false;
            }
        }

        // Si no hay objetos cercanos en el plano XZ, la posición es válida
        return true;
    }



    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point)){
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }

    private void DestroyAllChildren()
    {
        while(transform.childCount != 0)
        {
            foreach(Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}
