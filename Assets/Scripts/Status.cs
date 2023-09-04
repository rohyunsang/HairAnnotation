using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    public GameObject foreheadStatusObj;
    public GameObject glabellusStatusObj;
    public GameObject lPeriocularStatusObj;
    public GameObject rPeriocularStatusObj;
    public GameObject lCheekStatusObj;
    public GameObject rCheekStatusObj;
    public GameObject lipStatusObj;
    public GameObject chinStatusObj;

    // Added Spawn GameObjects
    public GameObject foreheadSpawn;
    public GameObject glabellusSpawn;
    public GameObject lPeriocularSpawn;
    public GameObject rPeriocularSpawn;
    public GameObject lCheekSpawn;
    public GameObject rCheekSpawn;
    public GameObject lipSpawn;
    public GameObject chinSpawn;


    public GameObject Btns;

    public Text foreheadWrinkleText;
    public Text foreheadPigmentationText;
    public Text glabellusWrinkleText;
    public Text lPeriocularWrinkleText;
    public Text rPeriocularWrinkleText;
    public Text lCheekPigmentationText;
    public Text lCheekPoresText;
    public Text rCheekPigmentationText;
    public Text rCheekPoresText;
    public Text lipDrynessText;
    public Text chinPtosisText;

    public string currentName = "";

    public GameObject JsonSerialObj;
    public GameObject panelManagerObj;

    public Text uncheckingStatusText;

    // 각 상태에 대한 데이터 구조 정의
    public class ConditionStatus
    {
        public string ConditionName;
        public int MaxScore;

        public ConditionStatus(string name, int score)
        {
            ConditionName = name;
            MaxScore = score;
        }
    }

    public ConditionStatus ForeheadWrinkle = new ConditionStatus("Wrinkle", 8);
    public ConditionStatus ForeheadPigmentation = new ConditionStatus("Pigmentation", 5);
    public ConditionStatus GlabellusWrinkle = new ConditionStatus("Wrinkle", 8);
    public ConditionStatus LPeriocularWrinkle = new ConditionStatus("Wrinkle", 6);
    public ConditionStatus RPeriocularWrinkle = new ConditionStatus("Wrinkle", 6);
    public ConditionStatus LCheekPigmentation = new ConditionStatus("Pigmentation", 7);
    public ConditionStatus LCheekPores = new ConditionStatus("Pores", 5);
    public ConditionStatus RCheekPigmentation = new ConditionStatus("Pigmentation", 7);
    public ConditionStatus RCheekPores = new ConditionStatus("Pores", 5);
    public ConditionStatus LipDryness = new ConditionStatus("DrynessLip", 4);
    public ConditionStatus ChinPtosis = new ConditionStatus("ChinPtosis", 6);

    void Start()
    {
        for (int i = 0; i < Btns.transform.childCount; i++)
        {
            Button btn = Btns.transform.GetChild(i).GetComponent<Button>();
            btn.onClick.AddListener(() => OnScoreButtonClicked(btn));
        }
    }

    public void CheckingStatusText()
    {
        Debug.Log("OnCheckingStatusText");
        string uncheckingStatus = "";

        if (foreheadWrinkleText.text.Equals("-1"))
            uncheckingStatus += foreheadWrinkleText.gameObject.name + ", ";

        if (foreheadPigmentationText.text.Equals("-1"))
            uncheckingStatus += foreheadPigmentationText.gameObject.name + ", ";

        if (glabellusWrinkleText.text.Equals("-1"))
            uncheckingStatus += glabellusWrinkleText.gameObject.name + ", ";

        if (lPeriocularWrinkleText.text.Equals("-1"))
            uncheckingStatus += lPeriocularWrinkleText.gameObject.name + ", ";

        if (rPeriocularWrinkleText.text.Equals("-1"))
            uncheckingStatus += rPeriocularWrinkleText.gameObject.name + ", ";

        if (lCheekPigmentationText.text.Equals("-1"))
            uncheckingStatus += lCheekPigmentationText.gameObject.name + ", ";

        if (lCheekPoresText.text.Equals("-1"))
            uncheckingStatus += lCheekPoresText.gameObject.name + ", ";

        if (rCheekPigmentationText.text.Equals("-1"))
            uncheckingStatus += rCheekPigmentationText.gameObject.name + ", ";

        if (rCheekPoresText.text.Equals("-1"))
            uncheckingStatus += rCheekPoresText.gameObject.name + ", ";

        if (lipDrynessText.text.Equals("-1"))
            uncheckingStatus += lipDrynessText.gameObject.name + ", ";

        if (chinPtosisText.text.Equals("-1"))
            uncheckingStatus += chinPtosisText.gameObject.name + ", ";

        if (uncheckingStatus == "")
        {
            Debug.Log("true checking");
            JsonSerialObj.GetComponent<JsonSerialization>().SaveJson();
            panelManagerObj.GetComponent<PanelManager>().OnSaveCompleteInfo();
        }
        else
        {
            uncheckingStatusText.text += uncheckingStatus;
            panelManagerObj.GetComponent<PanelManager>().OnSaveInfo();
        }
    }

    public void InitStatusText()
    {
        // Initialize text values to -1
        foreheadWrinkleText.text = "-1";
        foreheadPigmentationText.text = "-1";
        glabellusWrinkleText.text = "-1";
        lPeriocularWrinkleText.text = "-1";
        rPeriocularWrinkleText.text = "-1";
        lCheekPigmentationText.text = "-1";
        lCheekPoresText.text = "-1";
        rCheekPigmentationText.text = "-1";
        rCheekPoresText.text = "-1";
        lipDrynessText.text = "-1";
        chinPtosisText.text = "-1";
    }

    public void LoadStatusText(List<ScoreData> scoreDatas)
    {
        InitStatusText();


        foreach (ScoreData scoreData in scoreDatas)
        {
            foreach (var score in scoreData.scores)
            {
                string name = score.name.ToLower(); // Convert to lowercase for easier matching
                string value = score.score.ToString();

                switch (name)
                {
                    case "foreheadwrinkle":
                        foreheadWrinkleText.text = value;
                        break;
                    case "foreheadpigmentation":
                        foreheadPigmentationText.text = value;
                        break;
                    case "glabelluswrinkle":
                        glabellusWrinkleText.text = value;
                        break;
                    case "lperiocularwrinkle":
                        lPeriocularWrinkleText.text = value;
                        break;
                    case "rperiocularwrinkle":
                        rPeriocularWrinkleText.text = value;
                        break;
                    case "lcheekpigmentation":
                        lCheekPigmentationText.text = value;
                        break;
                    case "lcheekpores":
                        lCheekPoresText.text = value;
                        break;
                    case "rcheekpigmentation":
                        rCheekPigmentationText.text = value;
                        break;
                    case "rcheekpores":
                        rCheekPoresText.text = value;
                        break;
                    case "lipdryness":
                        lipDrynessText.text = value;
                        break;
                    case "chinptosis":
                        chinPtosisText.text = value;
                        break;
                    default:
                        Debug.LogError("Unknown score name in LoadStatusText: " + name);
                        break;
                }
            }
        }
    }

    private void OnScoreButtonClicked(Button clickedButton)
    {
        switch (currentName)
        {
            case "foreheadWrinkle":
                foreheadWrinkleText.text = clickedButton.name;
                break;
            case "foreheadPigmentation":
                foreheadPigmentationText.text = clickedButton.name;
                break;
            case "glabellusWrinkle":
                glabellusWrinkleText.text = clickedButton.name;
                break;
            case "lPeriocularWrinkle":
                lPeriocularWrinkleText.text = clickedButton.name;
                break;
            case "rPeriocularWrinkle":
                rPeriocularWrinkleText.text = clickedButton.name;
                break;
            case "lCheekPigmentation":
                lCheekPigmentationText.text = clickedButton.name;
                break;
            case "lCheekPores":
                lCheekPoresText.text = clickedButton.name;
                break;
            case "rCheekPigmentation":
                rCheekPigmentationText.text = clickedButton.name;
                break;
            case "rCheekPores":
                rCheekPoresText.text = clickedButton.name;
                break;
            case "lipDryness":
                lipDrynessText.text = clickedButton.name;
                break;
            case "chinPtosis":
                chinPtosisText.text = clickedButton.name;
                break;
            default:
                Debug.LogError("Unknown condition name in OnScoreButtonClicked: " + currentName);
                break;
        }
    }

    public void OnForehead()
    {
        foreheadStatusObj.SetActive(true);
        foreheadSpawn.SetActive(true);
    }
    public void OffForehead()
    {
        foreheadStatusObj.SetActive(false);
        foreheadSpawn.SetActive(false);
    }

    public void OnGlabellus()
    {
        glabellusStatusObj.SetActive(true);
        glabellusSpawn.SetActive(true);
    }

    public void OffGlabellus()
    {
        glabellusStatusObj.SetActive(false);
        glabellusSpawn.SetActive(false);
    }

    public void OnLPeriocular()
    {
        lPeriocularStatusObj.SetActive(true);
        lPeriocularSpawn.SetActive(true);
    }

    public void OffLPeriocular()
    {
        lPeriocularStatusObj.SetActive(false);
        lPeriocularSpawn.SetActive(false);
    }

    public void OnRPeriocular()
    {
        rPeriocularStatusObj.SetActive(true);
        rPeriocularSpawn.SetActive(true);
    }

    public void OffRPeriocular()
    {
        rPeriocularStatusObj.SetActive(false);
        rPeriocularSpawn.SetActive(false);
    }

    public void OnLCheek()
    {
        lCheekStatusObj.SetActive(true);
        lCheekSpawn.SetActive(true);
    }

    public void OffLCheek()
    {
        lCheekStatusObj.SetActive(false);
        lCheekSpawn.SetActive(false);
    }

    public void OnRCheek()
    {
        rCheekStatusObj.SetActive(true);
        rCheekSpawn.SetActive(true);
    }

    public void OffRCheek()
    {
        rCheekStatusObj.SetActive(false);
        rCheekSpawn.SetActive(false);
    }

    public void OnLip()
    {
        lipStatusObj.SetActive(true);
        lipSpawn.SetActive(true);
    }

    public void OffLip()
    {
        lipStatusObj.SetActive(false);
        lipSpawn.SetActive(false);
    }

    public void OnChin()
    {
        chinStatusObj.SetActive(true);
        chinSpawn.SetActive(true);
    }

    public void OffChin()
    {
        chinStatusObj.SetActive(false);
        chinSpawn.SetActive(false);
    }

    public void OnClickScoreButton(string name)
    {
        int maxScore = 0;
        currentName = name;
        switch (name)
        {
            case "foreheadWrinkle":
                maxScore = ForeheadWrinkle.MaxScore;
                break;
            case "foreheadPigmentation":
                maxScore = ForeheadPigmentation.MaxScore;
                break;
            case "glabellusWrinkle":
                maxScore = GlabellusWrinkle.MaxScore;
                break;
            case "lPeriocularWrinkle":
                maxScore = LPeriocularWrinkle.MaxScore;
                break;
            case "rPeriocularWrinkle":
                maxScore = RPeriocularWrinkle.MaxScore;
                break;
            case "lCheekPigmentation":
                maxScore = LCheekPigmentation.MaxScore;
                break;
            case "lCheekPores":
                maxScore = LCheekPores.MaxScore;
                break;
            case "rCheekPigmentation":
                maxScore = RCheekPigmentation.MaxScore;
                break;
            case "rCheekPores":
                maxScore = RCheekPores.MaxScore;
                break;
            case "lipDryness":
                maxScore = LipDryness.MaxScore;
                break;
            case "chinPtosis":
                maxScore = ChinPtosis.MaxScore;
                break;
            default:
                Debug.LogError("Unknown condition name: " + name);
                return;
        }

        // Now, based on maxScore, enable the appropriate number of buttons:
        for (int i = 0; i < Btns.transform.childCount; i++)
        {
            if (i <= maxScore)
            {
                Btns.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                Btns.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void OffBtns()
    {
        for (int i = 0; i < Btns.transform.childCount; i++)
        {
            Btns.transform.GetChild(i).gameObject.SetActive(false);

        }

        foreheadSpawn.SetActive(false);
        glabellusSpawn.SetActive(false);
        lPeriocularSpawn.SetActive(false);
        rPeriocularSpawn.SetActive(false);
        lCheekSpawn.SetActive(false);
        rCheekSpawn.SetActive(false);
        lipSpawn.SetActive(false);
        chinSpawn.SetActive(false);

        // 현재 켜져있는 StatusObj 객체들을 끔
        foreheadStatusObj.SetActive(false);
        glabellusStatusObj.SetActive(false);
        lPeriocularStatusObj.SetActive(false);
        rPeriocularStatusObj.SetActive(false);
        lCheekStatusObj.SetActive(false);
        rCheekStatusObj.SetActive(false);
        lipStatusObj.SetActive(false);
        chinStatusObj.SetActive(false);
    }
}