using UnityEngine;

public class SEManager : MonoBehaviour
{
    private AudioSource audioSource;
    // 再生するSEをインスペクターから設定する
    [SerializeField] AudioClip[] audioClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    // 指定された番号のSEを再生する関数（再生するときはこれだけ呼べばＯＫ）
    public void PlaySE(int idx)
    {
        if (audioSource == null || audioClip[idx] == null) return; // 素材が入ってないなら行わない

        // SEを再生する処理
        audioSource.PlayOneShot(audioClip[idx]);
    }
}
