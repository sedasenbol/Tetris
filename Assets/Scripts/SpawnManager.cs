using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject squarePrefab;
    [SerializeField]
    private GameObject tetroPrefab;
    private Vector2[,] squarePositions = new Vector2[5, 4] { { new Vector2( 3, 30 ), new Vector2( 4 ,30), new Vector2( 5, 30), new Vector2( 6, 30) }, // I //
                                                            { new Vector2( 3, 30 ), new Vector2( 4, 30 ),new Vector2( 5, 30 ), new Vector2( 5, 31) }, // L //
                                                            { new Vector2( 3, 30 ), new Vector2( 4, 30 ), new Vector2( 4, 31 ), new Vector2( 3, 31) }, // O //
                                                            { new Vector2( 3, 30 ), new Vector2( 4, 30 ), new Vector2( 4, 31 ), new Vector2( 5, 31) }, // Z //
                                                            { new Vector2( 3, 30 ), new Vector2( 4, 30 ), new Vector2( 4, 31), new Vector2( 5, 30) } }; // T //
    private Vector2[] tetroCenter = new Vector2[5] { new Vector2(4f, 30), new Vector2(4f,30f), new Vector2(3.5f,30.5f), new Vector2(4f, 30f), new Vector2(4f, 30f) };
    private int[,] randomColorMatrix = new int[6, 3] { { 0, 1, 1 }, { 0, 1, 0 }, { 1, 0, 0 }, { 0, 0, 1 }, { 1, 1, 0 }, { 1, 0, 1 } };
    private void Start()
    {
        CreateTetro();
    }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        Tetro.OnTetroGrounded += CreateTetro;
    }

    private void OnDisable()
    {
        Tetro.OnTetroGrounded -= CreateTetro;
    }

    private void CreateTetro()
    {
        int randomColor = Random.Range(0, 6);
        Color color = new Color(randomColorMatrix[randomColor,0], randomColorMatrix[randomColor, 1], randomColorMatrix[randomColor, 2]);

        int randomTetro = Random.Range(0, 5);
        GameObject tetro = Instantiate(tetroPrefab, tetroCenter[randomTetro], Quaternion.identity);

        for (int i = 0; i < 4; i++)
        {
            GameObject square = Instantiate(squarePrefab, squarePositions[randomTetro, i], Quaternion.identity, tetro.transform);
            square.GetComponent<Renderer>().material.color = color;
        }
    }
}
