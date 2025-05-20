using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    [Tooltip("Elemento UI que debe recibir el foco al activarse este menú")]
    public GameObject defaultUIElement;

    void OnEnable()
    {
        StartCoroutine(SetSelectedUI());
    }

    System.Collections.IEnumerator SetSelectedUI()
    {
        // Espera un frame para que el menú esté completamente activo
        yield return null;

        if (defaultUIElement != null && defaultUIElement.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultUIElement);
        }
    }
}
