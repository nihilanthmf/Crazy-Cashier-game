using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] Text hunger;

    [SerializeField] PlayerController playerController;

    private void Update()
    {
        SettingText();
    }

    void SettingText()
    {
        hunger.text = playerController.hunger.ToString();
    }
} 
