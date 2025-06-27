using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    [Header("状态Sprites")]
    public Sprite onSprite;
    public Sprite offSprite;

    private Image buttonImage;
    private bool isOn = false;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(ToggleState);
    }

    public void ToggleState()
    {
        isOn = !isOn;
        buttonImage.sprite = isOn ? onSprite : offSprite;

        // 这里可以添加开关状态变化的逻辑
        Debug.Log("Switch state: " + (isOn ? "ON" : "OFF"));
    }
}