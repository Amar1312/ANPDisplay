using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowMaxAndMin : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    const int SW_SHOWMINIMIZED = 2; //{minimize, activate}
    const int SW_SHOWMAXIMIZED = 3;//Maximize
    const int SW_SHOWRESTORE = 1;//Restore
    public void OnClickMinimize()
    { //minimize   
        ShowWindow(GetForegroundWindow(), SW_SHOWMINIMIZED);
    }

    public void OnClickMaximize()
    {
        //Maximize
        ShowWindow(GetForegroundWindow(), SW_SHOWMAXIMIZED);
    }

    public void OnClickRestore()
    {
        //Restore
        ShowWindow(GetForegroundWindow(), SW_SHOWRESTORE);
    }

    public void OnApplicationClose()
    {
        Application.Quit();
    }

    //Test
}
