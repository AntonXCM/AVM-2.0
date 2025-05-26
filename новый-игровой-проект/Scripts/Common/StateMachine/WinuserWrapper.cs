using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;

public static class WinuserWrapper
{
	[DllImport("user32.dll")]
	public static extern int GetSystemMetrics(int nIndex);

	[DllImport("user32.dll")]
	public static extern bool AnimateWindow(IntPtr hWnd, int time, int flags);
	[DllImport("kernel32.dll")]
	public static extern int GetLastError();
	public static void TryAnimateWindow(IntPtr hWnd, int time, int flags)
	{
		if (!AnimateWindow(hWnd, time, flags))
			GD.PrintErr($"Window animation error: {GetLastError()} Args: {hWnd} {time} {flags}");
	}
	public static void AnimateWindow(IntPtr hWnd, int time, AnimationFlag flags) => TryAnimateWindow(hWnd, time, (int)flags);
	public enum AnimationFlag : int
	{
		Activate = 0x00020000, //���������� ����. �� ����������� ��� �������� � AW_HIDE.
		Blend = 0x00080000, //���������� ������ ���������. ���� ���� ����� ������������ ������ � ��� ������, ���� hwnd �������� ����� �������� ������.
		Center = 0x00000010, //������ ���� ������������� ��������, ���� ������������ AW_HIDE, ��� ��������������� ������, ���� AW_HIDE �� ������������. ��������� ����� ����������� �� ��������� �������� �������.
		Hide = 0x00010000, //�������� ����. �� ��������� ������������ ���� .
		HOR_POSITIVE = 0x00000001, //��������� ���� ����� �������. ���� ���� ����� ������������ � ��������� ������ ��� ������. �� ������������ ��� ������������� � AW_CENTER ��� AW_BLEND.
		HOR_NEGATIVE = 0x00000002, //��������� ���� ������ ������. ���� ���� ����� ������������ � ��������� ������ ��� ������. �� ������������ ��� ������������� � AW_CENTER ��� AW_BLEND.
		SLIDE = 0x00040000, //���������� �������� �������. �� ��������� ������������ �������� ������. ���� ���� ������������ ��� ������������� � AW_CENTER.
		VER_POSITIVE = 0x00000004, //��������� ���� ������ ����. ���� ���� ����� ������������ � ��������� ������ ��� ������. �� ������������ ��� ������������� � AW_CENTER ��� AW_BLEND.
		VER_NEGATIVE = 0x00000008 //��������� ���� ����� �����. ���� ���� ����� ������������ � ��������� ������ ��� ������. �� ������������ ��� ������������� � AW_CENTER ��� AW_BLEND.
	}
	private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

	[DllImport("user32.dll")]
	private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
	public static void EnumerateWindows(Action<IntPtr> callback) => EnumWindows((hWnd, lParam) => { callback(hWnd); return true; }, IntPtr.Zero);

	[DllImport("user32.dll")]
	public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

	[DllImport("user32.dll")]
	public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

	public static void SetWindowBorder(IntPtr hWnd, bool state)
	{
		const int GWL_STYLE = -16;
		const int WS_CAPTION = 0x00C00000;
		IntPtr style = GetWindowLong(hWnd, GWL_STYLE);
		new Task(() => SetWindowLong(hWnd, GWL_STYLE, (IntPtr)(style.ToInt64() ^ WS_CAPTION))).Start();
		GD.Print(hWnd);
    }

}