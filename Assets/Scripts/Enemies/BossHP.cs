using TMPro;
using UnityEngine;

public class BossHP : MonoBehaviour
{
    [SerializeField] private EnemyHP hp;
    [SerializeField] private TextMeshPro hpText;

    private void Update()
    {
        hpText.text = (hp.health + "%");
    }
}