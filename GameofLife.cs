using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife3D : MonoBehaviour
{
    public int gridSizeX = 10;
    public int gridSizeY = 10;
    public int gridSizeZ = 10;
    public float cubeSize = 1f;
    public float updateInterval = 1f;

    private bool[,,] grid; // 3D array to store the cell states (alive or dead)
    private GameObject[,,] cubes; // 3D array to store the visual cubes for cells
    private bool[,,] nextGridState;

    // Start is called before the first frame update
    void Start()
    {
        grid = new bool[gridSizeX, gridSizeY, gridSizeZ];
        nextGridState = new bool[gridSizeX, gridSizeY, gridSizeZ];
        cubes = new GameObject[gridSizeX, gridSizeY, gridSizeZ];

        InitializeGrid();
        StartCoroutine(UpdateGrid());
    }

    // Initialize grid with random states
    void InitializeGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    grid[x, y, z] = Random.value > 0.5f;
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(x * cubeSize, y * cubeSize, z * cubeSize);
                    cube.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
                    cubes[x, y, z] = cube;
                    UpdateCube(x, y, z);
                }
            }
        }
    }

    // Update the grid according to the rules of Game of Life
    IEnumerator UpdateGrid()
    {
        while (true)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    for (int z = 0; z < gridSizeZ; z++)
                    {
                        int aliveNeighbors = CountAliveNeighbors(x, y, z);

                        // Game of Life rules in 3D:
                        if (grid[x, y, z])
                        {
                            // Cell is alive
                            if (aliveNeighbors < 2 || aliveNeighbors > 4)
                            {
                                nextGridState[x, y, z] = false; // Cell dies
                            }
                            else
                            {
                                nextGridState[x, y, z] = true; // Cell stays alive
                            }
                        }
                        else
                        {
                            // Cell is dead
                            if (aliveNeighbors == 4)
                            {
                                nextGridState[x, y, z] = true; // Cell becomes alive
                            }
                            else
                            {
                                nextGridState[x, y, z] = false; // Cell stays dead
                            }
                        }
                    }
                }
            }

            // Apply the new state to the grid and update visuals
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    for (int z = 0; z < gridSizeZ; z++)
                    {
                        grid[x, y, z] = nextGridState[x, y, z];
                        UpdateCube(x, y, z);
                    }
                }
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }

    // Update cube visibility based on its alive/dead state
    void UpdateCube(int x, int y, int z)
    {
        cubes[x, y, z].SetActive(grid[x, y, z]);
    }

    // Count the number of alive neighbors for a given cell
    int CountAliveNeighbors(int x, int y, int z)
    {
        int count = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    if (i == 0 && j == 0 && k == 0) continue; // Skip the cell itself
                    int nx = x + i;
                    int ny = y + j;
                    int nz = z + k;

                    if (nx >= 0 && nx < gridSizeX && ny >= 0 && ny < gridSizeY && nz >= 0 && nz < gridSizeZ)
                    {
                        if (grid[nx, ny, nz]) count++;
                    }
                }
            }
        }

        return count;
    }
}