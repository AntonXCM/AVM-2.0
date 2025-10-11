using System;
using System.Runtime.InteropServices;

public static class WindowManagerAPI
{
    private static IWindowManager manager;
    public static bool ForceNative { get; private set; } = true;
    static WindowManagerAPI()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            manager = new WinuserWrapper();
        else
            ForceNative = false;
    } 
    public static int CaptionHeight => manager.CaptionHeight;
    public static void HideWindow(IntPtr hWnd, int time) => manager.HideWindow(hWnd, time);
    public static void ShowWindow(IntPtr hWnd, int time) => manager.ShowWindow(hWnd, time);
}

public interface IWindowManager
{
    int CaptionHeight { get; }
    void HideWindow(IntPtr hWnd, int time);
    void ShowWindow(IntPtr hWnd, int time);
}