//#define DIALOG_IN_GAME_UI

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : SingletonMonoBehaviour<SystemManager>
{
    static bool isAutoCreate = true;

    /// <summary>
    /// ゲーム起動時に自動生成
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void AutoCreate()
    {
        if (!isAutoCreate) return;

        if (instance == null)
        {
            // ゲーム中に常に存在するオブジェクトを生成、およびシーンの変更時にも破棄されないようにする。
            var go = new GameObject(typeof(SystemManager).Name);
            instance = go.AddComponent<SystemManager>();
            GameObject.DontDestroyOnLoad(go);
        }
    }

    /// <summary>
    /// システムダイアログを開く
    /// </summary>
    /// <param name="message"></param>
    /// <param name="title"></param>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    public static void OpenSystemDialog(string message, string title = "", SYSTEM_DIALOG_BUTTON_TYPE type = SYSTEM_DIALOG_BUTTON_TYPE.OK, Action<SYSTEM_DIALOG_BUTTON_RESULT> callback = null)
    {
#if DIALOG_IN_GAME_UI
        // ゲーム内で作成するならdefineをonにしてその仕組みを作ってここに書く
#else
        SystemDialog.Open(title, message, type, callback);
#endif
    }

    /// <summary>
    /// ブラウザでURLを開く
    /// </summary>
    /// <param name="url"></param>
    public static void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
