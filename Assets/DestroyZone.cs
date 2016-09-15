using UnityEngine;
using System.Collections;

public class DestroyZone : MonoBehaviour {
    //Unityちゃんのオブジェクト
    private GameObject unitychan;
    //Unityちゃんとカメラの距離
    private float difference;

	// Use this for initialization
	void Start () {
        //Unityちゃんのオブジェクトを取得
        this.unitychan = GameObject.Find("unitychan");
        //UnityちゃんとDestroyZoneの位置(z座標)の差を求める
        this.difference = unitychan.transform.position.z - this.transform.position.z;
	
	}
	
	// Update is called once per frame
	void Update () {
        //Unityちゃんの位置に合わせて移動
        this.transform.position = new Vector3(0, this.transform.position.y, this.unitychan.transform.position.z-difference);
	
	}
    //接触したオブジェクトを全て削除する。
    void OnTriggerEnter(Collider other) {
            Destroy(other.gameObject);
    }
}
