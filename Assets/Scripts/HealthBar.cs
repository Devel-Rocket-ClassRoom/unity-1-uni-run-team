using UnityEngine;
using UnityEngine.UI;

// 체력 바 구현
public class HealthBar : MonoBehaviour
{
    public Slider HpBar;
    public PlayerController player;

    private float MaxHp = 100f;

    // Update is called once per frame
    void Update()
    {
        HpBar.value = player.Health / MaxHp;
    }
}
