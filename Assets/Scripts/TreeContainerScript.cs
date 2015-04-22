using UnityEngine;
using System.Collections;

//Dummy parent as a way to organize thre trees into childrens.
public class TreeContainerScript : MonoBehaviour {

    //Sets the alpha of the tree that this container contains to a
    public void setAlpha(float a){
        TreeModel mainTree = transform.GetChild(0).GetComponent<TreeModel>();
        mainTree.setAlphaRecursive(a);
    }
}
