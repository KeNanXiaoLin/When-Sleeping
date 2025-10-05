using System.Collections;
using System.Collections.Generic;
using GJ;
using UnityEngine;

public class VilliageDoor : MonoBehaviour
{
    public string AimScene;
    private List<Collider2D> colliders = new List<Collider2D>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        colliders.Add(collision);

        CheckIfSceneChangeChange();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        colliders.Add(collision);
    }

    private void CheckIfSceneChangeChange()
    {
        foreach (Collider2D i in colliders)
        {
            if (i.GetComponent<VilliagePlayer>() == true)
            {
                EventListener.VilliageSceneChange(AimScene);
            }
        }
    }
}
