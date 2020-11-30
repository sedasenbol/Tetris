using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject squarePrefab;
    [SerializeField]
    private GameObject tetroPrefab;

    private const float SQUARE_LENGTH = 0.5f;
    private const int SQUARE_COUNT = 4;
    private Transform tetroContainer;
    private Vector2 spawnPos = SQUARE_LENGTH * new Vector2(4f, 26f);
    private Vector2[,] squarePositions = new Vector2[7, 4] {{ new Vector2( 0, 0 ), new Vector2( -1 , 0), new Vector2( 1, 0), new Vector2( 2, 0) }, // I //
                                                            { new Vector2( 0, 0 ), new Vector2( 1, 0 ),new Vector2( -1, 0 ), new Vector2( -1, 1) }, // L //
                                                            { new Vector2( 0, 0 ), new Vector2( 1, 0 ), new Vector2( -1, 0 ), new Vector2( 1, 1) }, // Rotated L //
                                                            { new Vector2( 0, 0 ), new Vector2( 1, 0 ), new Vector2( 1, 1 ), new Vector2( 0, 1) }, // O //
                                                            { new Vector2( 0, 0 ), new Vector2( -1, 0 ), new Vector2( 0, 1 ), new Vector2( 1, 1) }, // Z //
                                                            { new Vector2( 0, 0 ), new Vector2( -1, 0 ), new Vector2( 1, 0), new Vector2( 0, 1) }, // T //
                                                            { new Vector2( 0, 0 ), new Vector2( 1, 0 ), new Vector2( 0, 1 ), new Vector2( -1, 1) }}; // Rotated Z //
    private int[,] randomColorMatrix = new int[6, 3] { { 0, 1, 1 }, { 0, 1, 0 }, { 1, 0, 0 }, { 0, 0, 1 }, { 1, 1, 0 }, { 1, 0, 1 } };

    private void Start()
    {
        squarePrefab.transform.localScale = new Vector3(1,1,1) * 0.388f * SQUARE_LENGTH; 
        tetroContainer = GameObject.Find("TetroContainer").transform;

        for (int i = 0; i < squarePositions.GetLength(0); i++)
        {
            for (int j = 0; j < squarePositions.GetLength(1); j++)
            {
                squarePositions[i,j] *= SQUARE_LENGTH;
            }
        }
    }

    private void OnEnable()
    {
        Tetro.OnTetroGrounded += CreateTetro;
        UIManager.OnPlayButtonClicked += CreateTetro;
    }

    private void OnDisable()
    {
        Tetro.OnTetroGrounded -= CreateTetro;
        UIManager.OnPlayButtonClicked -= CreateTetro;
    }

    private void CreateTetro()
    {
        int randomColor = Random.Range(0, 6);
        Color color = new Color(randomColorMatrix[randomColor,0], randomColorMatrix[randomColor, 1], randomColorMatrix[randomColor, 2]);

        GameObject tetro = Instantiate(tetroPrefab, spawnPos, Quaternion.identity, tetroContainer);
        int randomTetro = Random.Range(0, 7);

        for (int i = 0; i < SQUARE_COUNT; i++)
        {
            GameObject square = Instantiate(squarePrefab, squarePositions[randomTetro, i] + spawnPos, Quaternion.identity, tetro.transform);
            square.GetComponent<Renderer>().material.color = color;
        }
    }
}
