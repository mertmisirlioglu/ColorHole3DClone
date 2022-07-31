using System.Security.Cryptography;
using DefaultNamespace;
using DG.Tweening;
using Managers;
using UnityEngine;

public class BottomHitController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.isGameOver)
        {
            return;
        }

        if (other.CompareTag("Object"))
        {
            Debug.Log("its a object");
            Level.Instance.objectsInScene--;
            UIManager.Instance.UpdateProgressBar();

            Magnet.Instance.RemoveFromMagnetField(other.attachedRigidbody);

            if (Level.Instance.objectsInScene == 0)
            {
                UIManager.Instance.PlayWinFx();
            }
        }

        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("its a obstacle");
            GameManager.isGameOver = true;
            Camera.main.transform.DOShakePosition(1f, 0.2f, 20, 90f).OnComplete(() =>
            {
                Level.Instance.RestartLevel();
            });
        }

        Destroy(other.gameObject);
    }
}
