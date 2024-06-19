using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// El atributo [ExecuteInEditMode] permite que el script se ejecute en el editor de Unity
// incluso cuando el juego no está en marcha.
[ExecuteInEditMode]
public class DBRTopDownFloor : MonoBehaviour
{
    // Variables públicas para definir las dimensiones de la plataforma.
    public float platformWidth;
    public float platformHeight;
    private float _platformWidth;
    private float _platformHeight;

    // Variable pública para definir la ubicación de la plataforma.
    public Vector2 locationPoint;
    private Vector2 realLocationPoint;

    // Referencia al componente BoxCollider2D.
    private BoxCollider2D boxCollider2D;




    // Variables para definir el tamaño de los tiles y su índice seleccionado.
    [HideInInspector]
    public int tileSizeIndex = 1;
    [HideInInspector]
    public string[] tileSize = new string[] { "10x10", "16x16", "32x32", "Other" };

    private float x;
    private float y;

    // Referencia a un sprite guía.
    [SerializeField]
    private GameObject guideSprite;

    // Booleano para mostrar el color de prueba mientras se juega.
    public bool showTestColorWhilePlaying;

    // Método Start que se ejecuta al inicio del juego.
    public void Start()
    {
        if (Application.isPlaying)
        {
            PlatformConstruction();
        }
    }

    // Método Update que se ejecuta una vez por frame.
    public void Update()
    {
        if (Application.isEditor)
        {
            PlatformConstruction();
            guideSprite.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (showTestColorWhilePlaying)
        {
            if (Application.isPlaying)
            {
                guideSprite.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else
        {
            if (Application.isPlaying)
            {
                guideSprite.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    // Método que construye la plataforma.
    public void PlatformConstruction()
    {
        // Obtener los componentes BoxCollider2D y EdgeCollider2D.
        boxCollider2D = GetComponent<BoxCollider2D>();
        

        // Ajustar el tamaño del mapa según el índice de tamaño del tile.
        if (tileSizeIndex == 0)
        {
            x = 0.1f;
            y = 0.1f;
        }
        else if (tileSizeIndex == 1)
        {
            x = 0.16f;
            y = 0.16f;
        }
        else if (tileSizeIndex == 2)
        {
            x = 0.32f;
            y = 0.32f;
        }
        else if (tileSizeIndex == 3)
        {
            Debug.Log("Todavia no defino el 'otros'");
        }

        // Calcular la ubicación real de la plataforma.
        realLocationPoint = new Vector2(locationPoint.x * x, locationPoint.y * y);
        transform.position = realLocationPoint;

        // Ajustar el tamaño y la posición del BoxCollider2D.
        boxCollider2D.size = new Vector2(platformWidth * x, platformHeight * y);
        boxCollider2D.offset = new Vector2(platformWidth * x / 2, platformHeight * y / 2);

        // Ajustar el tamaño y la posición del sprite guía.
        guideSprite.transform.localScale = new Vector2(platformWidth * x, platformHeight * y);
        guideSprite.transform.localPosition = new Vector2(platformWidth * x / 2, platformHeight * y / 2);

      

        
    }
}