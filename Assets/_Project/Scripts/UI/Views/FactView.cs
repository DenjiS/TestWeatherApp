using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactView : MonoBehaviour
{
    [field: SerializeField] public TMP_Text Text { get; private set; }
    [field: SerializeField] public Button Button { get; private set; }
    [field: SerializeField] public Image LoadingIcon { get; private set; }
}
