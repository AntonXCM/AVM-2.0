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
		Activate = 0x00020000, //Активирует окно. Не используйте это значение с AW_HIDE.
		Blend = 0x00080000, //Использует эффект исчезания. Этот флаг можно использовать только в том случае, если hwnd является окном верхнего уровня.
		Center = 0x00000010, //Делает окно сворачиваться вовнутрь, если используется AW_HIDE, или разворачиваться наружу, если AW_HIDE не используется. Различные флаги направления не оказывают никакого влияния.
		Hide = 0x00010000, //Скрывает окно. По умолчанию отображается окно .
		HOR_POSITIVE = 0x00000001, //Анимирует окно слева направо. Этот флаг можно использовать с анимацией наката или слайда. Он игнорируется при использовании с AW_CENTER или AW_BLEND.
		HOR_NEGATIVE = 0x00000002, //Анимирует окно справа налево. Этот флаг можно использовать с анимацией наката или слайда. Он игнорируется при использовании с AW_CENTER или AW_BLEND.
		SLIDE = 0x00040000, //Использует анимацию слайдов. По умолчанию используется анимация наката. Этот флаг игнорируется при использовании с AW_CENTER.
		VER_POSITIVE = 0x00000004, //Анимирует окно сверху вниз. Этот флаг можно использовать с анимацией наката или слайда. Он игнорируется при использовании с AW_CENTER или AW_BLEND.
		VER_NEGATIVE = 0x00000008 //Анимирует окно снизу вверх. Этот флаг можно использовать с анимацией наката или слайда. Он игнорируется при использовании с AW_CENTER или AW_BLEND.
	}
}