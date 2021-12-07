using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatDamageText : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float alphaSpeed;
    [SerializeField] float destroyTime;

    TextMeshProUGUI text;
    RectTransform recTr;
    Color txtColor;
    public float damage;

    void Start()
    {
        moveSpeed = 3;
        alphaSpeed = 2;
        destroyTime = 2;
        text = GetComponent<TextMeshProUGUI>();
        txtColor = text.color;
        text.text = $"{Mathf.RoundToInt(damage):0}";
        recTr = GetComponent<RectTransform>();
        Invoke("Destroy", destroyTime);
    }

    void Update()
    {
        recTr.Translate(new Vector3(0, moveSpeed, 0),Space.Self);
        //transform.LookAt(Camera.main.transform.position);
        txtColor.a = Mathf.Lerp(txtColor.a, 0, Time.deltaTime * alphaSpeed);
        text.color = txtColor;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
