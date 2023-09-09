using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Cinemachine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private HeartEffect[] hearts;
    private int childCount;
    Image gameoverPanel;
    int tutoDegree;
    int heartIndex=3;
    QuestManager questManager;

    RectTransform questBoard;
    TextMeshProUGUI questTitle;
    TextMeshProUGUI questContent;
    float questBoardWidth;      //퀘스트 효과를 위한 너비 구하기
    float questBoardInitPosY;

    float questBoardSpeed=1f;      //퀘스트 보드창이 움직이는 속도

    GameObject questCompleteBoard;
    GameObject minimap;

    Image gotoEndingPanel;

        


    private void Awake()
    {
        minimap = transform.GetChild(2).gameObject;
        questBoardWidth = transform.GetChild(1).GetComponent<RectTransform>().rect.width;
        //Debug.Log(questBoardWidth);
        childCount = transform.childCount;
        hearts = new HeartEffect[transform.GetChild(0).childCount];
        for(int i=0;i< transform.GetChild(0).childCount; i++)
            hearts[i]   = transform.GetChild(0).GetChild(i).GetComponent<HeartEffect>();
        gameoverPanel   = transform.GetChild(childCount - 1).GetComponent<Image>();
        questManager = FindObjectOfType<QuestManager>();

        questBoard = transform.GetChild(1).GetComponent<RectTransform>();
        questBoardInitPosY = transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.y;
        questTitle = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        questContent = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();

        questCompleteBoard = transform.GetChild(3).gameObject;
        gotoEndingPanel = transform.GetChild(4).GetComponent<Image>();
    }

    private void Start()
    {
        //quest보드창을 화면밖에 위치시켜서 등장효과주기
        questBoard.anchoredPosition = new Vector2(questBoard.anchoredPosition.x + questBoardWidth, questBoard.anchoredPosition.y);

        if(questManager!=null)
        {
            //Debug.Log("First Quest Start!");
            QuestStart();
        }
    }


    public void QuestRefresh(string title,string content)
    {
        questTitle.text = title;
        questContent.text = content;
    }
    public void QuestStart()
    {
        StartCoroutine(QuestBoardAppear());
    }
    public void QuestEnd()
    {
        StartCoroutine(QuestBoardDisappear());
    }
    
    
    IEnumerator QuestBoardAppear()
    {
        //퀘스트 내용이 바뀌고...

        if(questManager.NextQuestReady())
        {
            //Debug.Log("Quest준비끝");
            float elapsedTime = 0f;
            Vector2 dir = new Vector2(0, questBoardInitPosY);
            while (questBoard.anchoredPosition.x > 1f)
            {
                //Debug.Log("Appear....");
                elapsedTime += Time.deltaTime;
                Vector2 interpolatedPosition = Vector2.Lerp(questBoard.anchoredPosition, dir, elapsedTime / questBoardSpeed);
                questBoard.anchoredPosition = interpolatedPosition;
                yield return null;
            }
            questBoard.anchoredPosition = dir;
            //퀘스트를 활성화시키고
            //Debug.Log("Quest활성화");
            questManager.NextQuestStart();
        }
        else
        {
            Debug.Log("퀘스트가 모두 종료되었습니다.");
            //questCompleteBoard.SetActive(true);
            //SceneManager에게 씬 이동을 부탁하기
            questManager.GoToEnding();
        }
        
    }

    IEnumerator QuestBoardDisappear()
    {
        float elapsedTime = 0f;
        Vector2 dir = new Vector2(questBoardWidth, questBoardInitPosY);
        while (questBoard.anchoredPosition.x < questBoardWidth-1)
        {
            //Debug.Log("Disappear...");
            elapsedTime += Time.deltaTime;
            Vector2 interpolatedPosition = Vector2.Lerp(questBoard.anchoredPosition, dir, elapsedTime / questBoardSpeed);
            questBoard.anchoredPosition = interpolatedPosition;
            yield return null;
        }
        questBoard.anchoredPosition = dir;


        yield return new WaitForSeconds(2f);
        QuestStart();
    }

    public void EndingSetting()
    {
        //
        minimap.SetActive(false);
        hearts[0].transform.parent.gameObject.SetActive(false);
        StartCoroutine(GoToEndingPanelEffect());
    }

    IEnumerator GoToEndingPanelEffect()
    {
        AudioSource audio;
        audio = FindObjectOfType<CinemachineBrain>().GetComponent<AudioSource>();
        Color c = gotoEndingPanel.color;
        float effectSpeed = 0.3f;
        while (c.a <1f)
        {
            c.a += effectSpeed * Time.deltaTime;
            gotoEndingPanel.color = c;
            audio.volume -= effectSpeed * 0.5f *Time.deltaTime;
            yield return null;
        }
        audio.Stop();
        c.a = 1f;
        gotoEndingPanel.color = c;
        gotoEndingPanel.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex-1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void PlayerHit()
    {
        //플레이어 체력 하나 줄이기
        heartIndex--;
        hearts[heartIndex].gameObject.SetActive(false);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverEffect());
    }
    IEnumerator GameOverEffect()
    {
        AudioSource audio;
        audio = FindObjectOfType<CinemachineBrain>().GetComponent<AudioSource>();
        Color c = gameoverPanel.color;
        float effectSpeed = 0.3f;
        while (c.a < 1f)
        {
            c.a += effectSpeed * Time.deltaTime;
            gameoverPanel.color = c;
            audio.volume -= effectSpeed * 0.5f * Time.deltaTime;
            yield return null;
        }
        audio.Stop();
        c.a = 1f;
        gameoverPanel.color = c;
        yield return new WaitForSeconds(3f);
        gameoverPanel.gameObject.SetActive(true);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex - 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}


