using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ComboAnim : MonoBehaviour
{
    UnityEvent<int> onCombo;
    List<GameObject> mesObjects = new List<GameObject>();

    [SerializeField]
    ComboEvents ComboEvents;
    Vector3 position = new Vector3();


    void Start()
    {
        onCombo = ComboEvents.GetComponent<ComboEvents>().comboChangeEvent;
        onCombo.AddListener(OnComboStart);
        
    }


    void OnComboStart(int multiplicatorCombo){

        float height = (float)multiplicatorCombo/2;
        Debug.Log("height"+height);

        gameObject.transform.localScale = new Vector3(
                        gameObject.transform.localScale.x,
                        height, 
                        gameObject.transform.localScale.z) ;

        
       ClearAndDestroy();
       Debug.Log("comboAnim " + multiplicatorCombo);
       position = gameObject.transform.position;

        for (int i = 1; i <= multiplicatorCombo; i++)
        {
            position.z += gameObject.transform.localScale.z * 2;//
            GameObject instance = Instantiate(gameObject,position,Quaternion.identity);
            ComboAnim scriptCombo = instance.GetComponent<ComboAnim>();
            Destroy(scriptCombo);
            mesObjects.Add(instance);
        }    
    }


    void ClearAndDestroy(){
        foreach (var item in mesObjects)
        {
            Destroy(item);
        }
        mesObjects.Clear();
        //mesObjects.Add(gameObject);
    }


    IEnumerator TweenAnim(){
        foreach (var item in mesObjects)
        {
           // iTween.MoveTo(item,)
            yield return new WaitForSeconds(0.2f);
        }

    }

}
