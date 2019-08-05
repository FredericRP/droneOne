using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialOnTrigger : MonoBehaviour
{
    [SerializeField]
    Material triggeredMaterial;

    Renderer[] rendererList;

    // Start is called before the first frame update
    void Start()
    {
        rendererList = transform.GetComponentsInChildren<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i=0;i< rendererList.Length;i++)
        {
            rendererList[i].material = triggeredMaterial;
            Debug.Log("Set new mat on " + name + ">" + rendererList[i].name + " : " + rendererList[i].material + " is now " + triggeredMaterial);
            /*
            for (int j=0;j<rendererList[i].materials.Length;j++)
            {
                Debug.Log("Set new mat on " + name + ">" + rendererList[i].name + " : " + rendererList[i].materials[j] + " is now " + triggeredMaterial);
                rendererList[i].materials[j] = triggeredMaterial;
                rendererList[i].materials[j].color = Color.blue;
            }
            // */
        }
    }
}
