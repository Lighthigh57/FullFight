using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] ActiveObjects;
    [SerializeField] private TextMeshPro CountDownText;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDown());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(2);
        foreach (var item in ActiveObjects)
        {
            item.SetActive(true);
        }
        for (int i = 3; i > 0; i--)
        {

        }
    }
}
