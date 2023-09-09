using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void OnButtonClick()
    {
        Debug.Log("ButtonClick!");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
            OnButtonClick();
    }


}
