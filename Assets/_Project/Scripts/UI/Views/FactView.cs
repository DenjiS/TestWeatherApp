using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FactView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private Transform _transform;

    [field: SerializeField] public Button Button { get; private set; }
    [field: SerializeField] public Image LoadingIcon { get; private set; }

    private void Awake() => 
        _transform = transform;

    public void Setup(FactData data, Transform rootPanel)
    {
        _text.text = $"{data.Number} - {data.Name}";
        _transform.parent = rootPanel;
    }

    public class Pool : MonoMemoryPool<FactData, Transform, FactView>
    {
        protected override void Reinitialize(FactData data, Transform rootPanel, FactView item) => 
            item.Setup(data, rootPanel);
    }
}
