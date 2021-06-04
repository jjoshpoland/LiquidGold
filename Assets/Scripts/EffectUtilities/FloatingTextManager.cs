using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject floatingTextPrefab;
    public Transform spawnPoint;

    private void Awake()
    {
    }
    public void DoPositiveFloatingText(Good good)
    {
        GameObject floatingText = Instantiate(floatingTextPrefab, transform.position + (Vector3.up * 3f), spawnPoint.rotation);
        DamagePopup dp = floatingText.GetComponent<DamagePopup>();
        //dp.transform.LookAt(Camera.main.transform);
        dp.displayText = "+1 <sprite index=" + good.SpriteIndex + ">";
        floatingText.GetComponent<TMP_Text>().color = Color.green;
        dp.PopUp();
    }

    public void DoNegativeFloatingText(Good good)
    {
        GameObject floatingText = Instantiate(floatingTextPrefab, transform.position + (Vector3.up * 3f), spawnPoint.rotation);
        DamagePopup dp = floatingText.GetComponent<DamagePopup>();
        //dp.transform.LookAt(Camera.main.transform);
        dp.displayText = "-1 <sprite index=" + good.SpriteIndex + ">";
        floatingText.GetComponent<TMP_Text>().color = Color.red;
        dp.PopDown();
    }
}
