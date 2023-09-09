using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class QuestManager : MonoBehaviour
{
    //퀘스트 내용에 대해 UI에 전달, Quest를 클리어했는지 감지하기 위한 역할.
    int questIndex = -1 ;       //초기값은 -1
    int questLimit;
    public int targetCollectNum = 6;
    public Tuple<string, string>[] quest;
    string questTitle;
    string questContent;
    UIManager ui;
    PlayerQuest playerQuest;

    string mainQuest1Text = "Current : ";
    int collectQuestItem = 0;

    public GameObject questObject1;
    public GameObject questObject2;
    public TutorialCubeDelete tutorialCube;

    public EscapeArea escapeArea;
    public PlayerEscapeMarker playerEscapeMarker;

    public Transform  escapePlayerPosition;
    public GameObject escapePlayerDrivingPos;
    public GameObject escapeCar;

    private void Awake()
    {
        ui = FindObjectOfType<UIManager>();
        quest = new Tuple<string, string>[]
        {
            Tuple.Create("! Move(Tutorial)","Move with W,A,S,D "),
            Tuple.Create("! Attack(Tutorial)","Attack with Left Mouse"),
            Tuple.Create("! Hunt(Tutorial)","Kill the Zombie"),
            Tuple.Create("! Collect(Tutorial)","Collect item"),
            Tuple.Create("! Find "+targetCollectNum+" Items",mainQuest1Text + collectQuestItem),
            Tuple.Create("! Escape","Find a Escape Area")
        };
        questLimit = quest.Length;
        //Debug.Log($"QuestLimit : {questLimit}");
        playerQuest = FindObjectOfType<PlayerQuest>();

        playerQuest.inputTutoComplete += CompleteQuest;           //입력 튜토리얼 종료
        playerQuest.attackTutoComplete += CompleteQuest;         //공격 튜토리얼 종료
        playerQuest.huntingTutoComplete += CompleteQuest;        //사냥 튜토리얼 종료
        playerQuest.tutoPlayerCollectItem += CompleteQuest;        //수집 튜토리얼 종료
        playerQuest.PlayerCollectItem += CollectItemRefresh;


        if (tutorialCube == null)
        {
            tutorialCube = FindObjectOfType<TutorialCubeDelete>();
        }

        if(escapeArea==null)
            escapeArea = FindObjectOfType<EscapeArea>();
        if(playerEscapeMarker==null)
            playerEscapeMarker=FindObjectOfType<PlayerEscapeMarker>();

        if (escapePlayerPosition == null)
            escapePlayerPosition = escapeArea.transform.parent.GetChild(14).transform;
        if(escapePlayerDrivingPos==null)
            escapePlayerDrivingPos = escapeArea.transform.parent.GetChild(12).gameObject;
        if (escapeCar == null)
            escapeCar = escapeArea.transform.parent.gameObject;
    }
    private void OnDisable()
    {
        playerQuest.tutoPlayerCollectItem -= CompleteQuest;        //수집 튜토리얼 종료
        playerQuest.huntingTutoComplete -= CompleteQuest;        //사냥 튜토리얼 종료
        playerQuest.attackTutoComplete -= CompleteQuest;         //공격 튜토리얼 종료
        playerQuest.inputTutoComplete -= CompleteQuest;           //입력 튜토리얼 종료
    }
    public void CollectItemRefresh()
    {
        collectQuestItem++;
        ui.QuestRefresh("! Find "+ targetCollectNum+" Items", mainQuest1Text + collectQuestItem);
        if (collectQuestItem == targetCollectNum)
        {
            //퀘스트가 완료될 때 실행될 부분.
            Debug.Log("Quest Complete");
            playerQuest.PlayerCollectItem -= CollectItemRefresh;
            ui.QuestEnd();
        }
    }

    //튜토리얼중인지, 아닌지에 따라 다름.
    public void CompleteQuest()
    {
        ui.QuestEnd();
    }
    public bool NextQuestReady()
    {
        
        // True리턴시 퀘스트 진행 가능. False리턴시 퀘스트 모두 완료!
        if (questIndex+1 < questLimit)
        {
            questIndex++;
            //Debug.Log($"Current QuestDegree : {questIndex + 1}");
            questTitle = quest[questIndex].Item1;
            questContent = quest[questIndex].Item2;
            if (ui != null)
                ui.QuestRefresh(questTitle, questContent);
            else
                Debug.LogWarning("QuestManager : NextQuest.... Ui Missing...!");
            return true;
        }
        else
        {
            // 모든 퀘스트가 끝났음을 의미함. 엔딩으로 인도할 것.
            return false;
        }
    }
    public void NextQuestStart()
    {
        //UI효과가 끝나고 퀘스트가 시작 될 떄 최초1회 실행되는 함수


        if(questIndex<=4)       //4이하는 튜토리얼
        {
            playerQuest.TutoPlayerQuestActive();

            if (questIndex == 2)
            {
                GameObject obj = Instantiate(questObject1);
                obj.transform.position = new Vector3(0, 0, 0);
            }
            else if (questIndex == 3)
            {
                GameObject obj = Instantiate(questObject2);
                obj.transform.position = new Vector3(0, 1, 5);
            }
            else if (questIndex == 4)
            {
                //tutlrial종료 후 첫 이벤트. 10개 아이템 수집하기
                tutorialCube.TutorialCubeDisappear();
            }
        }
        else if(questIndex==5)
        {
            //탈출 시작~
            Debug.Log("Final Quest");
            // 레이어의 escape Marker를 활성화시키기.
            // 플레이어 이동속도 좀 올려주고, 몬스터 공격 모션 애니메이션 컨트롤러 손보기
            //Destroy(playerQuest);


            escapeArea.gameObject.SetActive(true);
            playerEscapeMarker.gameObject.SetActive(true);
            escapeArea.gameObject.SetActive(true);

        }
    }

    public void GoToEnding()
    {
        escapePlayerDrivingPos.SetActive(true);
        EndingCameraAction cameraAction = FindObjectOfType<EndingCameraAction>();
        cameraAction.EndingCameraSetting();
        EscapeCarController carController = escapeCar.GetComponent<EscapeCarController>();
        carController.EndingCarStart();
        playerQuest.PlayerEndingSetting(escapePlayerPosition);
        ui.EndingSetting();
        //UI도 해제하기
        //Player의 모든 권한 해제하기
        //escapeCar의 Agent에게 경로를 알려주기
        //DollyCart의 속도를 2로, Priority를 11로 조정하기.
    }
    public void GameOver()
    {
        ui.GameOver();
    }
    
}
