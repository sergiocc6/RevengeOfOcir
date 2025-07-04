using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    [Tooltip("Elemento UI que debe recibir el foco al activarse este men�")]
    public GameObject defaultUIElement;

    void OnEnable()
    {
        StartCoroutine(SetSelectedUI());
    }

    /// <summary>
    /// Sets the default UI element as the currently selected object in the event system.
    /// </summary>
    System.Collections.IEnumerator SetSelectedUI()
    {
        // Espera un frame para que el men� est� completamente activo
        yield return null;

        if (defaultUIElement != null && defaultUIElement.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultUIElement);
        }
    }
}
