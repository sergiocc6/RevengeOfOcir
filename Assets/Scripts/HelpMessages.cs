using UnityEngine;
using TMPro;
using System.Collections;

public class HelpMessages : MonoBehaviour
{
    public GameObject cartelUI;
    public TextMeshProUGUI mensajeUI;
    public string mensaje;
    public float velocidadEscritura = 0.05f;

    private Coroutine escrituraActual;

    void Start()
    {
        if (cartelUI != null)
            cartelUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && cartelUI != null)
        {
            cartelUI.SetActive(true);
            if (escrituraActual != null)
                StopCoroutine(escrituraActual);
            escrituraActual = StartCoroutine(WriteCharacterByCharacter(mensaje));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && cartelUI != null)
        {
            if (escrituraActual != null)
                StopCoroutine(escrituraActual);

            cartelUI.SetActive(false);
            mensajeUI.text = "";
        }
    }

    IEnumerator WriteCharacterByCharacter(string texto)
    {
        velocidadEscritura = 0;
        mensajeUI.text = "";
        foreach (char letra in texto)
        {
            mensajeUI.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }
    }
}
