using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using UnityEngine;
using System.Runtime.InteropServices;

public class ProcessStarter : MonoBehaviour
{

    [DllImport("user32.dll")]
    static extern bool LockSetForegroundWindow(uint uLockCode);

    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

    [DllImport("user32.dll")]
    static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

    const int ALT = 0xA4;
    const int EXTENDEDKEY = 0x1;
    const int KEYUP = 0x2;


    private void Start()
    {
        //LockSetForegroundWindow(1);
        unityWindow = GetActiveWindow();
        //keybd_event((byte)ALT, 0x45, EXTENDEDKEY | 0, 0);
        //keybd_event((byte)ALT, 0x45, EXTENDEDKEY | KEYUP, 0);
        //SetForegroundWindow(unityWindow);
        StartCoroutine(Checker());
    }

    IEnumerator Checker()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            IntPtr activeWindow = GetActiveWindow();

            if (unityWindow != activeWindow)
            {
                SwitchToThisWindow(unityWindow, true);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartProcess();
        }
    }


    public string pathToApplication;
    public string applicationName;

    IntPtr unityWindow;

    [ContextMenu("Start Process")]
    void StartProcess()
    {
        //currentProcess.
        //unityWindow = GetActiveWindow();

        Process p = new Process();
        p.StartInfo = new ProcessStartInfo(applicationName);
        p.StartInfo.WorkingDirectory = pathToApplication;
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

        p.Start();

        //Application.focusChanged += Application_focusChanged;
        StartCoroutine(SetWindowFocusRoutine());

    }

    private void Application_focusChanged(bool obj)
    {
        IntPtr activeWindow = GetActiveWindow();

        if (unityWindow != activeWindow)
        {
            SwitchToThisWindow(unityWindow, true);
        }
    }

    IEnumerator SetWindowFocusRoutine()
    {
        yield return new WaitWhile(() => unityWindow == GetActiveWindow());

        //yield return new WaitForSeconds(3.0f);

        // Simulate alt press
        //keybd_event((byte)ALT, 0x45, EXTENDEDKEY | 0, 0);

        //// Simulate alt release
        //keybd_event((byte)ALT, 0x45, EXTENDEDKEY | KEYUP, 0);
        //SetForegroundWindow(unityWindow);


        IntPtr activeWindow = GetActiveWindow();
        if (unityWindow != activeWindow)
        {
            SwitchToThisWindow(unityWindow, true);
        }
    }
}
