using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Specialized;
public class GameManager : MonoBehaviour
{
    #region SINGLETON_REFERENCES

    public static GameManager Instance;
    void Awake()
    {
        Instance = this;
    }

    #endregion

    #region REFRERENCES

    [SerializeField] TextMeshProUGUI Quest;

    [SerializeField] Transform Options_Panel, optionA_, optionB_, optionC_, optionD_;

    [SerializeField] string QuestText,optionA,optionB,optionC,optionD;

    [SerializeField] string Answer, Clicked_Answer;

    #endregion

    #region METHODS

    private void Start()
    {
        Load();
    }

    private void Load()
    {
        Load_Text();
        Button_Click();
    }

    private void Button_Click()
    {
        optionA_.GetComponent<Button>().onClick.AddListener(()=>Option_A_Click());
        optionB_.GetComponent<Button>().onClick.AddListener(()=>Option_B_Click());
        optionC_.GetComponent<Button>().onClick.AddListener(()=>Option_C_Click());
        optionD_.GetComponent<Button>().onClick.AddListener(()=>Option_D_Click());
    }

    private void Load_Text()
    {
        Quest.text = QuestText;
        optionA_.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = "A) " + optionA;
        optionB_.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = "B) " + optionB;
        optionC_.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = "C) " + optionC;
        optionD_.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = "D) " + optionD;
    }

    private void Check_Answer(Transform button)
    {
         Clicked_Answer = button.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text;

        if (Clicked_Answer.Contains(Answer))
        {
            button.GetComponent<Image>().color = Color.green;
        }
        else
        {
            button.GetComponent<Image>().color = Color.red;
            for (int i = 0; i < Options_Panel.childCount; i++)
            {

            }
        }
    }

    #region Button_Click_Events

    private void Option_A_Click()
    {
       Check_Answer(optionA_);
    }
    private void Option_B_Click()
    {
       Check_Answer(optionB_);
    }
    private void Option_C_Click()
    {
       Check_Answer(optionC_);
    }
    private void Option_D_Click()
    {
       Check_Answer(optionD_);
    }

    #endregion

    #endregion

}
