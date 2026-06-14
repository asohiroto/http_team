using System;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    GameObject playerObj;
    PlayerController playerCtrl;
    enum ShotState { Aim, Shot} // ミサイル攻撃の状態を管理する
    ShotState shotState;
    [SerializeField] GameObject missilePrefab;
    [SerializeField] GameObject missileRangePrefab;
    [SerializeField] GameObject missileEffectPrefab;
    GameObject missileObj = null;
    GameObject RangeObj = null;
    Vector3 missileLandingPos;
    Vector3 landingPosAdj; // 着弾地点を若干ずらすための変数
    Vector3 missilePos;
    Vector3 moveDir;
    int frameTimer = 0;
    int missileWaitingFrame = 30;
    bool isShot = false;
    bool isAttackReach = false;
    float moveSpeed = 0.12f;
    float landingBackAdj = 1.0f;
    float landingForwardAdj = 2.0f;
    void Start()
    {
        // プレイヤーオブジェクトとスクリプトの取得
        playerObj = GameObject.Find("Player");
        playerCtrl = playerObj.GetComponent<PlayerController>();
        // 着地地点を若干ずらすための変数を設定する
        landingPosAdj = new Vector3(UnityEngine.Random.Range(-landingBackAdj, landingForwardAdj) * playerCtrl.lastDir.x, 
            UnityEngine.Random.Range(-landingBackAdj,landingForwardAdj) * playerCtrl.lastDir.y);
    }

    void FixedUpdate()
    {
        switch (shotState)
        {
            // プレイヤーを狙う処理
            case ShotState.Aim:
                frameTimer++;
                if (!isAttackReach) // 一度だけ生成するための条件処理
                {
                    // 着弾地点を決定
                    missileLandingPos = playerObj.transform.position + landingPosAdj;
                    // 攻撃範囲を表示するプレハブを生成
                    RangeObj = Instantiate(missileRangePrefab);
                    RangeObj.transform.position = missileLandingPos;
                    isAttackReach = true;
                }
                if(frameTimer >= missileWaitingFrame) shotState = ShotState.Shot;
                break;
            // ミサイルを発射する処理（一発だけ）
            case ShotState.Shot:
                if (!isShot) // 一度だけ生成するための条件処理
                {
                    // ミサイルを生成
                    missileObj = Instantiate(missilePrefab);
                    // ミサイルのポジションを設定
                    missilePos = transform.position;
                    missileObj.transform.position = missilePos;
                    // ミサイルの向きを設定
                    Vector3 missileDir = missileLandingPos - missilePos;
                    missileObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, missileDir);
                    isShot = true;
                }
                // 着弾後は以下の処理は行わない
                if (missileObj == null) return;
                // 着弾地点まで移動したときの処理（オブジェクトの削除、爆発エフェクトの生成）
                if (Math.Abs(missilePos.x - missileLandingPos.x) <= 0.06f && 
                    Math.Abs(missilePos.y - missileLandingPos.y) <= 0.06f)
                {
                    GameObject fxObj = Instantiate(missileEffectPrefab);
                    fxObj.transform.position = missileLandingPos;
                    Destroy(missileObj);
                    Destroy(RangeObj);
                    Destroy(gameObject);
                }
                // ミサイルから着弾点までのベクトルを計算
                moveDir = missileLandingPos - missilePos;
                // 方向ベクトルの正規化
                moveDir = moveDir.normalized;
                // ミサイルの移動処理
                missilePos += moveDir * moveSpeed;
                missileObj.transform.position = missilePos;
                break;
        }
    }
}
