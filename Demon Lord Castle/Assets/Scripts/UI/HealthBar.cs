using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject playerGameObject;
    private PlayerController player;

    void Start()
    {
        player = playerGameObject.GetComponent<PlayerController>();
    }

    void Update()
    {
        this.transform.localScale = new Vector3(player.health/100, 1, 1);
    }
}
