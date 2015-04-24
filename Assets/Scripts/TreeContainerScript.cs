using UnityEngine;
using System.Collections;

//Dummy parent as a way to organize thre trees into childrens.
public class TreeContainerScript : MonoBehaviour {

    public string creator;
    public string date;

    public void Start(){
        date = "Planeted in " + System.DateTime.Now.Month + "/" + System.DateTime.Now.Day + "/" + System.DateTime.Now.Year;
    }

    //Sets the alpha of the tree that this container contains to a
    public void setAlpha(float a){
        TreeModel mainTree = transform.GetChild(0).GetComponent<TreeModel>();
        mainTree.setAlphaRecursive(a);
    }
}
