using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerQuest : MonoBehaviour
{
    int tutorialDegree = 0;
    int inputTuto = 0;      //이동 튜토리얼 전용
    bool[] inputTutoBool = { false, false, false, false };
    bool tutoCollectQuestActive=false;
    public Action inputTutoComplete;        //입력 튜토리얼 종료
    public Action attackTutoComplete;       //공격 튜토리얼 종료
    public Action huntingTutoComplete;      //사냥 튜토리얼 종료
    public Action tutoPlayerCollectItem;      //수집 튜토리얼 종료
    public Action PlayerCollectItem;      //Questmanager에게 수집알림

    PlayerWeaponController playerWeapon;

    public bool tutoActive = false;

    private void Awake()
    {
        playerWeapon=FindObjectOfType<PlayerWeaponController>();
        playerWeapon.PlayerWeaponHit += TutoHuntingCheck;
    }
    private void OnDisable()
    {
        //Debug.Log("PlayerQuest Bye");
        playerWeapon.PlayerWeaponHit -= TutoHuntingCheck;
    }
    public bool TutoEndQuestion()
    {
        if (tutorialDegree < 4)
            return false;
        else
            return true;
    }


    private void Update()
    {
        if(tutoActive)
        {
            switch(tutorialDegree)
            {
                case 1:
                    TutoInputCheck();
                    break;
                case 2:
                    TutoAttackCheck();
                    break;
                case 3:
                    //3번째 퀘스트의경우 PlayerWeaponController의 Invoke가 들어와야 실행됨!
                    break;
                case 4:
                    //4번째 퀘스트의경우 QuestCollect에서 Invoke가 들어와야 실행됨!
                    //메인 미션 종료. 튜토리얼 종료를 나타내기 위해 사용되는 케이스
                    break;
                default:
                    Debug.Log("Tutorial End");
                    //얘 왜 자꾸 실행되냐
                    break;
            }
        }
    }

    private void TutoInputCheck()
    {

        if (Input.GetKeyDown(KeyCode.W) && inputTutoBool[0]==false)
        {
            inputTutoBool[0] = true;
            inputTuto++;
            //Debug.Log("Press W");
        }
        else if (Input.GetKeyDown(KeyCode.A) && inputTutoBool[1]==false)
        {
            inputTutoBool[1] = true;
            inputTuto++;
            //Debug.Log("Press A");
        }
        else if(Input.GetKeyDown(KeyCode.S) && inputTutoBool[2]==false)
        {
            inputTutoBool[2] = true;
            inputTuto++;
            //Debug.Log("Press S");
        }
        else if(Input.GetKeyDown(KeyCode.D) && inputTutoBool[3]==false)
        {
            inputTutoBool[3] = true;
            inputTuto++;
            //Debug.Log("Press D");
        }
        
        if(inputTuto==4)
        {
            //QuestManager 에게 신호 보낼 것.
            //Debug.Log("InputQuest Complete");
            inputTutoComplete?.Invoke();
            tutoActive = false;       //퀘스트 추적을 못하도록..!
        }
    }
    public void TutoPlayerQuestActive()
    {
        if(tutorialDegree<4)
        {
            tutorialDegree++;
            Debug.Log($"Player.. Quest {tutorialDegree} is Active!");
            tutoActive = true;

            if (tutorialDegree == 4)
                tutoCollectQuestActive = true;        //이때부터 아이템들에 대해 수집이 가능해짐
        }
        
    }
    private void TutoAttackCheck()
    {
        if(Input.GetMouseButton(0)==true)
        {
            //Debug.Log("Press Mouse Left");
            attackTutoComplete?.Invoke();
            tutoActive = false;       //퀘스트 추적을 못하도록..!
        }
    }
    private void TutoHuntingCheck()
    {
        playerWeapon.PlayerWeaponHit -= TutoHuntingCheck;
        huntingTutoComplete?.Invoke();
        tutoActive = false;       //퀘스트 추적을 못하도록..!
    }

    //Collectable 아이템이 Player에 접촉되었을 때 실행할 함수.
    public void TutoCollectCheck()
    {
        tutoPlayerCollectItem?.Invoke();        //QuestManager에게 수집을 완료했다고 액션 보냄.
    }
    public void CollectCheck()
    {
        //메인 미션용 콜렉트 체크
        PlayerCollectItem?.Invoke();        //QuestManager에게 수집을 완료했다고 액션 보냄.
    }

    public void PlayerEndingSetting(Transform endingPos)
    {
        //transform.position = endingPos.position;
        //transform.SetParent(endingPos);
        //transform.GetComponent<PlayerInput>().enabled = false;      //권한 제어
        //transform.GetComponent<ThirdPersonController>().enabled = false;
        transform.gameObject.SetActive(false);
    }

}
