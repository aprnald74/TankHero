using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class LineTrajectory : MonoBehaviour
{
    public Sprite changeImg;
    public Sprite nowImg;
    public GameObject enemyFind;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) {
            enemyFind.GetComponent<Image>().sprite = changeImg;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enemyFind.GetComponent<Image>().sprite = nowImg;
    }
}
