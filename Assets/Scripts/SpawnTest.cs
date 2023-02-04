using Animations;
using UnityEngine;

public class SpawnTest : MonoBehaviour
{
    public EnemyAnimation EnemyAnimation;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            var newObject = Instantiate(EnemyAnimation);
            newObject.transform.position = transform.position;
        }
    }
}