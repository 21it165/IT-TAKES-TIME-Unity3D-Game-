using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateText(string promptMassage){
        promptText.text = promptMassage;
    }
    
}
