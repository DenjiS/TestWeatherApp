using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FactView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    [field: SerializeField] public Button Button { get; private set; }
    [field: SerializeField] public Image LoadingIcon { get; private set; }

    public Transform Transform { get; private set; }

    private void Awake() =>
        Transform = transform;

    public void Setup(FactData data) =>
        _text.text = $"{data.Id} - {data.Name}";

    public class Pool : MonoMemoryPool<FactData, FactView>
    {
        protected override void Reinitialize(FactData data, FactView item) =>
            item.Setup(data);
    }
}
