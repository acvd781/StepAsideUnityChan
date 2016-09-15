using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnityChanController : MonoBehaviour {
    //アニメーションするためのコンポーネントを入れる(変数の設定)
    private Animator myAnimator;
    //アニメの状態の情報を入れる
    private AnimatorStateInfo animState;
    //Unityちゃんを移動させるコンポーネントを入れる
    private Rigidbody myRigidbody;
    //前進するための力
    private float forwardForce = 800.0f;
    //左右に移動するための力
    private float turnForce = 500.0f;
    //ジャンプするための力
    private float upForce = 500.0f;
    //左右に移動出来る範囲
    private float movableRange = 3.4f;
    //動きを減速させる係数
    private float coefficient = 0.95f;

    //ゲーム終了の判定
    private bool isEnd = false;
    //ゲーム終了時に表示するテキスト
    private GameObject stateText;

    //スコアを表示するテキスト
    private GameObject scoreText;
    //得点用の変数
    private int score = 0;

    //左ボタン押下の判定
    private bool isLButtonDown = false;
    //右ボタン押下の判定
    private bool isRButtonDown = false;

	// Use this for initialization
	void Start () {
        //Animatorコンポーネントを取得
        this.myAnimator = GetComponent<Animator>();
        //走るアニメーションを開始(アニメーションの開始を指示している)
        this.myAnimator.SetFloat("Speed", 1);
        //Rgidbodyコンポーネントを取得
        this.myRigidbody = GetComponent<Rigidbody>();

        //シーン中のstateTextオブジェクトを取得(stateText変数にゲームオブジェクトを入れ込む)
        this.stateText = GameObject.Find("GameResultText");
        //シーン中のscoreTextオブジェクトを取得(stateText変数にゲームオブジェクトを入れ込む)
        this.scoreText = GameObject.Find("ScoreText");
	
	}
	
	// Update is called once per frame
	void Update () {
        //ゲーム終了ならUnityちゃんの動きを減衰する(isEnd変数がtrueの時)
        if (this.isEnd) {
            this.forwardForce *= this.coefficient;
            this.turnForce *= this.coefficient;
            this.upForce *= this.coefficient;
            this.myAnimator.speed *= this.coefficient;
        }

        //Unityちゃんに前方向の力を加える(追加)
        this.myRigidbody.AddForce(this.transform.forward * this.forwardForce);
        
        //Unityちゃんを矢印キーまたはボタンに応じて左右に移動させる
        //左右を入力かつ、指定範囲内の時に移動が出来る！＝移動限界の設定
        //【タッチ操作追加】ボタンに応じて移動させる
        if((Input.GetKey(KeyCode.LeftArrow) || this.isLButtonDown) && -this.movableRange < this.transform.position.x){
            //左に移動
            this.myRigidbody.AddForce(-this.turnForce, 0, 0);
        }else if ((Input.GetKey(KeyCode.RightArrow) || this.isRButtonDown) && this.transform.position.x < this.movableRange){
            //右に移動
            this.myRigidbody.AddForce(this.turnForce, 0, 0);
        }

        //jumpステートの場合はjumpにfalseをセットする(ジャンプモーションを止める)
        if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
            this.myAnimator.SetBool("Jump", false);
        }

        //ジャンプしていない時にスペースが押されたらジャンプする
        if(Input.GetKeyDown(KeyCode.Space) && this.transform.position.y < 0.5f) {
            //ジャンプアニメを再生
            this.myAnimator.SetBool("Jump", true);
            //Unityちゃんに上方向の力を加える(upは上方向に力を加える)
            this.myRigidbody.AddForce(this.transform.up * this.upForce);
        }
	
	}
    //トリガーモードで他のオブジェクトと接触した場合の処理
    void OnTriggerEnter(Collider other) {

        //障害物に衝突した場合
        if(other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag") {
            this.isEnd = true;
            //stateTextにGAME OVERを表示
            this.stateText.GetComponent<Text>().text = "GAME OVER";
        }
        //ゴール地点に到達した場合
        if(other.gameObject.tag == "GoalTag") {
            this.isEnd = true;
            //stateTextにGAME CLEARを表示
            this.stateText.GetComponent<Text>().text = "CLEAR!!";
        }
        //コインに衝突した場合
        if(other.gameObject.tag == "CoinTag") {
            //スコアを加算
            this.score += 10;
            //ScoreText買う特した点数を表示
            this.scoreText.GetComponent<Text>().text = "Score " + this.score + "pt";

            //パーティクルを再生
            GetComponent<ParticleSystem>().Play();
            //接触したコインのオブジェクトを破棄
            Destroy(other.gameObject);
        }
    }
    //ジャンプボタンを押した場合の処理
    public void GetMyjumButtonDown() {
        if(this.transform.position.y < 0.5f) {
            this.myAnimator.SetBool("Jump", true);
            this.myRigidbody.AddForce(this.transform.up * this.upForce);
        }
    }

    //左ボタンを押し続けた場合の処理
    public void GetMyLeftButtonDown() {
        this.isLButtonDown = true;
    }
    //左ボタンを離した場合の処理
    public void GetMyLeftButtonUp() {
        this.isLButtonDown = false;
    }
    //右ボタンを押し続けた場合の処理
    public void GetMyRightButtonDown() {
        this.isRButtonDown = true;
    }
    //右ボタンを離した場合の処理
    public void GetMyRightButtonUp() {
        this.isRButtonDown = false;
    }
}
