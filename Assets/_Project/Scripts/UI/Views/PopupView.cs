using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupView : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;

    [field: SerializeField] public Button CloseButton { get; private set; }

    public void Setup(string title, string description)
    {
        _titleText.text = title;
        _descriptionText.text = description;
    }
}