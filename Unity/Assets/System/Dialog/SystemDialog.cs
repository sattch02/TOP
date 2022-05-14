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
    public static void Open(string title, string message, SYSTEM_DIALOG_BUTTON_TYPE type, Action<SYSTEM_DIALOG_BUTTON_RESULT> callback, string okButtonName = "", string cancelButtonName = "")
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

        var messageBox = new ButtonTextCustomizableMessageBox();
        if (!string.IsNullOrEmpty(okButtonName))
        {
            messageBox.ButtonText.OK = okButtonName;
            messageBox.ButtonText.Yes = okButtonName;
        }

        if (!string.IsNullOrEmpty(cancelButtonName))
        {
            messageBox.ButtonText.No = cancelButtonName;
            messageBox.ButtonText.Cancel = cancelButtonName;
        }

        var result = messageBox.Show(
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
                if (!string.IsNullOrEmpty(okButtonName))
                {
                    okName = okButtonName;
                }
                else
                {
                    okName = "OK";
                }
                break;
            case SYSTEM_DIALOG_BUTTON_TYPE.OKCancel:
                if (!string.IsNullOrEmpty(okButtonName))
                {
                    okName = okButtonName;
                }
                else
                {
                    okName = "OK";
                }

                if (!string.IsNullOrEmpty(cancelButtonName))
                {
                    cancelName = cancelButtonName;
                } else
                {
                    cancelName = "Cancel";
                }
                break;
            case SYSTEM_DIALOG_BUTTON_TYPE.YesNo:
                if (!string.IsNullOrEmpty(okButtonName))
                {
                    okName = okButtonName;
                }
                else
                {
                    okName = "Yes";
                }

                if (!string.IsNullOrEmpty(cancelButtonName))
                {
                    cancelName = cancelButtonName;
                }
                else
                {
                    cancelName = "No";
                }
                break;
            case SYSTEM_DIALOG_BUTTON_TYPE.RetryCancel:
                if (!string.IsNullOrEmpty(okButtonName))
                {
                    okName = okButtonName;
                }
                else
                {
                    okName = "Retry";
                }

                if (!string.IsNullOrEmpty(cancelButtonName))
                {
                    cancelName = cancelButtonName;
                }
                else
                {
                    cancelName = "Cancel";
                }
                break;
        }
        var result = EditorUtility.DisplayDialog(title: title, message: message, ok:okName, cancel: cancelName);

        if (result)
        {
            resultType = SYSTEM_DIALOG_BUTTON_RESULT.YesOrOk;
        }
        else
        {
            if (type == SYSTEM_DIALOG_BUTTON_TYPE.OK)
            {
                resultType = SYSTEM_DIALOG_BUTTON_RESULT.YesOrOk;
            }
            else
            {
                resultType = SYSTEM_DIALOG_BUTTON_RESULT.NoOrCancel;
            }
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


#if PLATFORM_STANDALONE_WIN
/// <summary>
/// ボタンのテキストをカスタマイズできるメッセージボックスです。
/// </summary>
public class ButtonTextCustomizableMessageBox
{
    private IntPtr hHook = IntPtr.Zero;

    #region メッセージのテキストのクラス定義
    public class CustomButtonText
    {
        public string OK { get; set; }
        public string Cancel { get; set; }
        public string Abort { get; set; }
        public string Retry { get; set; }
        public string Ignore { get; set; }
        public string Yes { get; set; }
        public string No { get; set; }
    }
    #endregion
    /// <summary>
    /// ボタンに表示するテキストを指定します。
    /// </summary>
    public CustomButtonText ButtonText { get; set; }

    /// <summary>
    /// コンストラクタ。
    /// </summary>
    public ButtonTextCustomizableMessageBox()
    {
        this.ButtonText = new CustomButtonText();
    }

    /// <summary>
    /// ダイアログボックスを表示します。
    /// </summary>
    public DialogResult Show(string text, string caption, MessageBoxButtons buttons)
    {
        try
        {
            BeginHook();
            return MessageBox.Show(text, caption, buttons);
        }
        finally
        {
            EndHook();
        }
    }

    /// <summary>
    /// フックを開始します。
    /// </summary>
    void BeginHook()
    {
        EndHook();
        this.hHook = SetWindowsHookEx(WH_CBT, new HOOKPROC(this.HookProc), IntPtr.Zero, GetCurrentThreadId());
    }

    IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode == HCBT_ACTIVATE)
        {
            if (this.ButtonText.Abort != null) SetDlgItemText(wParam, ID_BUT_ABORT, this.ButtonText.Abort);
            if (this.ButtonText.Cancel != null) SetDlgItemText(wParam, ID_BUT_CANCEL, this.ButtonText.Cancel);
            if (this.ButtonText.Ignore != null) SetDlgItemText(wParam, ID_BUT_IGNORE, this.ButtonText.Ignore);
            if (this.ButtonText.No != null) SetDlgItemText(wParam, ID_BUT_NO, this.ButtonText.No);
            if (this.ButtonText.OK != null) SetDlgItemText(wParam, ID_BUT_OK, this.ButtonText.OK);
            if (this.ButtonText.Retry != null) SetDlgItemText(wParam, ID_BUT_RETRY, this.ButtonText.Retry);
            if (this.ButtonText.Yes != null) SetDlgItemText(wParam, ID_BUT_YES, this.ButtonText.Yes);

            EndHook();
        }

        return CallNextHookEx(this.hHook, nCode, wParam, lParam);
    }

    /// <summary>
    /// フックを終了します。何回呼んでもOKです。
    /// </summary>
    void EndHook()
    {
        if (this.hHook != IntPtr.Zero)
        {
            UnhookWindowsHookEx(this.hHook);
            this.hHook = IntPtr.Zero;
        }
    }

    #region Win32API
    const int WH_CBT = 5;
    const int HCBT_ACTIVATE = 5;

    const int ID_BUT_OK = 1;
    const int ID_BUT_CANCEL = 2;
    const int ID_BUT_ABORT = 3;
    const int ID_BUT_RETRY = 4;
    const int ID_BUT_IGNORE = 5;
    const int ID_BUT_YES = 6;
    const int ID_BUT_NO = 7;

    private delegate IntPtr HOOKPROC(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowsHookEx(int idHook, HOOKPROC lpfn, IntPtr hInstance, IntPtr threadId);

    [DllImport("user32.dll")]
    private static extern bool UnhookWindowsHookEx(IntPtr hHook);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetCurrentThreadId();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool SetDlgItemText(IntPtr hWnd, int nIDDlgItem, string lpString);
    #endregion
}
#endif