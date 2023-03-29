using System.Collections;
using Geo.UI;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CopyTextComponent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isPointerDown; // флаг нажатия на компонент
    
    [SerializeField, IsntNull]
    Text textToCopy; // поле для хранения текста, который будет копироваться
    
    [SerializeField, IsntNull]
    OverlayPanel overlayPanel;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
        StartCoroutine(LongPressCoroutine()); 
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;
    }

    // метод для копирования текста
    private void CopyText()
    {
        GUIUtility.systemCopyBuffer = textToCopy.text;
        overlayPanel.Show("Текст скопирован");
        Debug.Log("Text copied: " + GUIUtility.systemCopyBuffer);
    }

    // метод для проверки длительности нажатия на компонент
    private IEnumerator LongPressCoroutine()
    {
        yield return new WaitForSeconds(2f); // время для определения длительного нажатия (2 секунды)
        if (_isPointerDown)
        {
            CopyText();
        }
    }
}