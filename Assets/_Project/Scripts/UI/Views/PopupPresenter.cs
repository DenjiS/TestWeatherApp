using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupPresenter : MonoBehaviour
{
    public readonly UnityEvent CloseButtonClicked = new();

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            CloseButtonClicked.Invoke();
        });

    }

    public void Setup(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;
    }
}