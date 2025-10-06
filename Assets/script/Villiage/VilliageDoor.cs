using System.Collections;
using System.Collections.Generic;
using GJ;
using UnityEngine;

namespace GJ
{

public class VilliageDoor : MonoBehaviour
{
    public string AimScene;
    protected List<Collider2D> colliders = new List<Collider2D>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        colliders.Add(collision);

        CheckIfSceneChangeChange();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        colliders.Add(collision);
    }

    public virtual void CheckIfSceneChangeChange()
    {
        foreach (Collider2D i in colliders)
        {
            if (i.GetComponent<VilliagePlayer>() == true)
            {
                EventListener.VilliageSceneChange(AimScene,E_SceneLoadType.None);
            }
        }
    }

}

}