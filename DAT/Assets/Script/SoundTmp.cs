using UnityEngine;

public class SEManager : MonoBehaviour
{
    // インスペクターからAudioSourceコンポーネントを追加する
    // インスペクターからこのスクリプトのseClipという配列にSEを登録する
    // このスクリプトを取得して PlaySE(int idx) を呼び出す

    private AudioSource audioSource;
    // 再生するSEをインスペクターから設定する
    [SerializeField] AudioClip[] seClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    // 指定された番号のSEを再生する関数（再生するときはこれだけ呼べばＯＫ）
    public void PlaySE(int idx)
    {
        if (audioSource == null || seClip[idx] == null) return; // 素材が入ってないなら行わない

        // SEを再生する処理
        audioSource.PlayOneShot(seClip[idx]);
    }
}
