using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;

public class WinuserWrapper : IWindowManager
{
	[DllImport("user32.dll")] private static extern int GetSystemMetrics(int nIndex);
	[DllImport("user32.dll")] private static extern bool AnimateWindow(IntPtr hWnd, int time, int flags);
	[DllImport("kernel32.dll")] private static extern int GetLastError();
	[DllImport("user32.dll")] private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
	[DllImport("user32.dll")] public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);
	[DllImport("user32.dll")] public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

	public int CaptionHeight => GetSystemMetrics(4); //Высота шапки окна

	private static void TryAnimateWindow(IntPtr hWnd, int time, AnimationFlag flags)
	{
		if (!AnimateWindow(hWnd, time, (int)flags))
			GD.PrintErr($"Window animation error: {GetLastError()} Args: {hWnd} {time} {flags}");
	}
	public void HideWindow(IntPtr hWnd, int time) => TryAnimateWindow(hWnd, time, AnimationFlag.Hide | AnimationFlag.BlendCenter);
	public void ShowWindow(IntPtr hWnd, int time) => TryAnimateWindow(hWnd, time, AnimationFlag.Activate | AnimationFlag.BlendCenter);
	private enum AnimationFlag : int
	{
		Activate = 0x00020000,
		Hide = 0x00010000,
		// Это типа BLEND, а это CENTER и они битово ИЛИются
		BlendCenter = 0x00080000 | 0x00000010,
	}

	private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
	[Obsolete] public static void EnumerateWindows(Action<IntPtr> callback) => EnumWindows((hWnd, lParam) => { callback(hWnd); return true; }, IntPtr.Zero);
	[Obsolete] public static void SetWindowBorder(IntPtr hWnd)
	{
		const int GWL_STYLE = -16;
		const int WS_CAPTION = 0x00C00000;
		IntPtr style = GetWindowLong(hWnd, GWL_STYLE);
		new Task(() => SetWindowLong(hWnd, GWL_STYLE, (IntPtr)(style.ToInt64() ^ WS_CAPTION))).Start();
		GD.Print(hWnd);
    }
}