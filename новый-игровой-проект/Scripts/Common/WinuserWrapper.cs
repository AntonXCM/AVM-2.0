using System;
using System.Runtime.InteropServices;

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
		if(!AnimateWindow(hWnd, time, flags))
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
}