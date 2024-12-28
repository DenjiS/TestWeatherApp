using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TabButton : MonoBehaviour
{
    [Inject] private readonly ITabController _tabController;

    [SerializeField] private TabType _tabType;

    private Button _tabButton;

    private void Awake() =>
        _tabButton = GetComponent<Button>();

    private void Start() =>
        _tabButton.onClick.AddListener(() => _tabController.SwitchTab(_tabType));
}
