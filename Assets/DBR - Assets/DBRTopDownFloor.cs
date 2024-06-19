using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// El atributo [ExecuteInEditMode] permite que el script se ejecute en el editor de Unity
// incluso cuando el juego no est� en marcha.
[ExecuteInEditMode]
public class DBRTopDownFloor : MonoBehaviour
{
    // Variables p�blicas para definir las dimensiones de la plataforma.
    public float platformWidth;
    public float platformHeight;
    private float _platformWidth;
    private float _platformHeight;

    // Variable p�blica para definir la ubicaci�n de la plataforma.
    public Vector2 locationPoint;
    private Vector2 realLocationPoint;

    // Referencia al componente BoxCollider2D.
    private BoxCollider2D boxCollider2D;




    // Variables para definir el tama�o de los tiles y su �ndice seleccionado.
    [HideInInspector]
    public int tileSizeIndex = 1;
    [HideInInspector]
    public string[] tileSize = new string[] { "10x10", "16x16", "32x32", "Other" };

    private float x;
    private float y;

    // Referencia a un sprite gu�a.
    [SerializeField]
    private GameObject guideSprite;

    // Booleano para mostrar el color de prueba mientras se juega.
    public bool showTestColorWhilePlaying;

    // M�todo Start que se ejecuta al inicio del juego.
    public void Start()
    {
        if (Application.isPlaying)
        {
            PlatformConstruction();
        }
    }

    // M�todo Update que se ejecuta una vez por frame.
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

    // M�todo que construye la plataforma.
    public void PlatformConstruction()
    {
        // Obtener los componentes BoxCollider2D y EdgeCollider2D.
        boxCollider2D = GetComponent<BoxCollider2D>();
        

        // Ajustar el tama�o del mapa seg�n el �ndice de tama�o del tile.
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

        // Calcular la ubicaci�n real de la plataforma.
        realLocationPoint = new Vector2(locationPoint.x * x, locationPoint.y * y);
        transform.position = realLocationPoint;

        // Ajustar el tama�o y la posici�n del BoxCollider2D.
        boxCollider2D.size = new Vector2(platformWidth * x, platformHeight * y);
        boxCollider2D.offset = new Vector2(platformWidth * x / 2, platformHeight * y / 2);

        // Ajustar el tama�o y la posici�n del sprite gu�a.
        guideSprite.transform.localScale = new Vector2(platformWidth * x, platformHeight * y);
        guideSprite.transform.localPosition = new Vector2(platformWidth * x / 2, platformHeight * y / 2);

      

        
    }
}