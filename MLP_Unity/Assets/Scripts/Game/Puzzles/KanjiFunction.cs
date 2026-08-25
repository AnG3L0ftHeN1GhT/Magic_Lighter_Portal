using UnityEngine;
using UnityEngine.InputSystem;

public class KanjiFunction : MonoBehaviour
{
    public GameObject kanjiD;
    public GameObject kanjiA;
    public GameObject kanjiC;
    public GameObject kanjiP;
    public GameObject pyramid;

    public Item kanji1;
    public Item kanji2;
    public Item kanji3;
    public Item kanji4;

    
   
   public static KanjiFunction instance;
   [SerializeField] private int pegouKanji1;
   [SerializeField] private int pegouKanji2;
   [SerializeField] private int pegouKanji3;
   [SerializeField] private int pegouKanji4;
   [SerializeField] private float allKanji;
    public InputActionAsset inputActions;

    public PlayerInteraction playerCollections;

    void Awake()
    {
        instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        

        if (pegouKanji1 > 1)
        {
            allKanji = 0.25f;
        }

        if (pegouKanji2 > 1)
        {
            allKanji = 0.25f;
        }

        if (pegouKanji3 > 1)
        {
            allKanji = 0.25f;
        }

        if (pegouKanji4 > 1)
        {
            allKanji = 0.25f;
        }
    }

    void PegarKanjis(int kanjiPego)
    {
        if (kanjiPego == 1)
        {
            if (playerCollections.kanji1)
            {
                kanjiD.SetActive(true);
                kanjiA.SetActive(false);
                kanjiC.SetActive(false);
                kanjiP.SetActive(false);
            }
        }
        else if (kanjiPego == 2)
        {
            if (playerCollections.kanji2)
            {
                kanjiD.SetActive(false);
                kanjiA.SetActive(true);
                kanjiC.SetActive(false);
                kanjiP.SetActive(false);
            }
        }
        else if(kanjiPego == 3)
        {
            if (playerCollections.kanji3)
            {
                kanjiD.SetActive(false);
                kanjiA.SetActive(false);
                kanjiC.SetActive(true);
                kanjiP.SetActive(false);
            }
        }
        else if(kanjiPego == 4)
        {
            if (playerCollections.kanji4)
            {
                kanjiD.SetActive(false);
                kanjiA.SetActive(false);
                kanjiC.SetActive(false);
                kanjiP.SetActive(true);
            }
        }

        if (allKanji >= 1)
        {
            Instantiate(pyramid, transform.position, Quaternion.identity);
        }
    }
}
