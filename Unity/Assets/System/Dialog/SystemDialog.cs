using System;
#if PLATFORM_STANDALONE_WIN
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum SYSTEM_DIALOG_BUTTON_TYPE
{
    OK,
    OKCancel,
    YesNo,
    RetryCancel,
}

public enum SYSTEM_DIALOG_BUTTON_RESULT
{
    None,
    YesOrOk,
    NoOrCancel,
}

/// <summary>
/// システムのダイアログ
/// </summary>
public class SystemDialog
{
    /// <summary>
    /// ダイアログを開く
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    public static void Open(string title, string message, SYSTEM_DIALOG_BUTTON_TYPE type, Action<SYSTEM_DIALOG_BUTTON_RESULT> callback)
    {
        SYSTEM_DIALOG_BUTTON_RESULT resultType = SYSTEM_DIALOG_BUTTON_RESULT.None;
#if PLATFORM_STANDALONE_WIN
        MessageBoxButtons messageBoxButtons;
        switch (type)
        {
            default:
            case SYSTEM_DIALOG_BUTTON_TYPE.OK:
                messageBoxButtons = MessageBoxButtons.OK;
                break;
            case SYSTEM_DIALOG_BUTTON_TYPE.OKCancel:
                messageBoxButtons = MessageBoxButtons.OKCancel;
                break;
            case SYSTEM_DIALOG_BUTTON_TYPE.YesNo:
                messageBoxButtons = MessageBoxButtons.YesNo;
                break;
            case SYSTEM_DIALOG_BUTTON_TYPE.RetryCancel:
                messageBoxButtons = MessageBoxButtons.RetryCancel;
                break;
        }

        var result = MessageBox.Show(
            text: message,
            caption: title,
            buttons: messageBoxButtons
        );

        switch (result)
        {
            case DialogResult.OK:
            case DialogResult.Yes:
            case DialogResult.Retry:
                resultType = SYSTEM_DIALOG_BUTTON_RESULT.YesOrOk;
                break;
            case DialogResult.No:
            case DialogResult.Cancel:
                resultType = SYSTEM_DIALOG_BUTTON_RESULT.NoOrCancel;
                break;
            case DialogResult.None:
            case DialogResult.Abort:
            case DialogResult.Ignore:
                //基本使わないもの
                break;
        }

        if (callback != null)
        {
            callback.Invoke(resultType);
        }
#elif UNITY_EDITOR_OSX
        // MacのEditorでは生成できないのでEditorUtilityで代用
        string okName = "";
        string cancelName = "";
        switch (type)
        {
            default:
            case SYSTEM_DIALOG_BUTTON_TYPE.OK:
                okName = "OK";
                break;
            case SYSTEM_DIALOG_BUTTON_TYPE.OKCancel:
                okName = "OK";
                cancelName = "Cancel";
                break;
            case SYSTEM_DIALOG_BUTTON_TYPE.YesNo:
                okName = "Yes";
                cancelName = "No";
                break;
            case SYSTEM_DIALOG_BUTTON_TYPE.RetryCancel:
                okName = "Retry";
                cancelName = "Cancel";
                break;
        }
        var result = EditorUtility.DisplayDialog(title: title, message: message, ok:okName, cancel: cancelName);

        if (result)
        {
            resultType = SYSTEM_DIALOG_BUTTON_RESULT.YesOrOk;
        }
        else
        {
            resultType = SYSTEM_DIALOG_BUTTON_RESULT.NoOrCancel;
        }
        if (callback != null)
        {
            callback.Invoke(resultType);
        }
#elif UNITY_STANDALONE_OSX
// plugin作るしかない
#elif UNITY_IOS
// plugin作るしかない
#elif UNITY_ANDROID
// plugin作るしかない
#endif
    }
}