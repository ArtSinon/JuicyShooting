using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelLoader : MonoBehaviour
{
    public void LoadNextLevel()
    {
        Debug.Log("��� ������� �� ������������, ������ ����������� �����");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
