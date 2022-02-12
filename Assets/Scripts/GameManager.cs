using System.Collections;
using TMPro;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] ActiveObjects;
    [SerializeField] private GameObject[] setPlayers;
    [SerializeField] private GameObject CountDownText;

    private TextMeshProUGUI text;
    private Animator animator;

    private readonly Vector3[] playerPlases = new Vector3[2]
    {
        new Vector3(22, -4, -17),
        new Vector3(22,-4 , -22)
    };
    private readonly Quaternion[] playerRotations = new Quaternion[2]
    {
        Quaternion.Euler(0, 180, 0),
        Quaternion.identity
    };
    private readonly GameObject[] players = new GameObject[2];


    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(CountDown());
        text = CountDownText.GetComponent<TextMeshProUGUI>();
        animator = CountDownText.GetComponent<Animator>();
        for (byte i = 0; i < 2; i++)
        {
            players[i] = Instantiate(setPlayers[i], playerPlases[i], playerRotations[i]);
            players[i].GetComponent<PlayerController>().Init(i);
        }
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
            text.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        text.text = "GO!";
        animator.SetTrigger("Counted");
        yield return new WaitForSeconds(1);
        text.text = "";
        yield return new WaitForSeconds(1.5f);
        CountDownText.SetActive(false);
    }
}
