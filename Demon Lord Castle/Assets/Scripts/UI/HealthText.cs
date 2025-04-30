using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private TextMeshProUGUI text;
    private PlayerController player;


    void Start()
    {
        player = playerGameObject.GetComponent<PlayerController>();
    }

    void Update()
    {
        text.SetText("{0}%", player.health);
    }
}
