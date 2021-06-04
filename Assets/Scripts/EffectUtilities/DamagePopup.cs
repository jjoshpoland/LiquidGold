using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(TMP_Text))]
public class DamagePopup : MonoBehaviour
{
    [HideInInspector]
    public string displayText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PopUp()
    {
        TMP_Text tmp_text = GetComponent<TMP_Text>();
        tmp_text.text = displayText;
        tmp_text.DOFade(0f, Random.Range(1f, 2f));
        transform.DOMove(transform.position + transform.up + (Vector3.right * Random.Range(-3f, 3f)), Random.Range(1, 2f)).OnComplete(() => {
            Destroy(gameObject);
        });
    }

    public void PopDown()
    {
        TMP_Text tmp_text = GetComponent<TMP_Text>();
        tmp_text.text = displayText;
        tmp_text.DOFade(0f, 0.7f);
        transform.DOMove(transform.position - transform.up + (Vector3.right * Random.Range(-3f, 3f)), Random.Range(1, 2f)).OnComplete(() => {
            Destroy(gameObject);
        });
    }
}
