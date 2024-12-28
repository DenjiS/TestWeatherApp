using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupView : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(() => Destroy(gameObject));
    }

    public void Setup(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;
    }
}