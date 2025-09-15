using System;
using System.Threading.Tasks;

using Godot;

public partial class OnTopWindow : Window
{
	Vector2I defaultSize;
	Window mainWindow;
	[Export] int animationTime = 300;
	IntPtr Handle => new(DisplayServer.WindowGetNativeHandle(DisplayServer.HandleType.WindowHandle, GetWindowId()));
	bool opened = true;
	public override void _Notification(int what)
	{
		switch(what)
		{
			case (int)NotificationApplicationFocusIn:
			ShowAnim();
			break;
			case (int)NotificationApplicationFocusOut:
			HideAnim();
			break;
		}
	}

	public override void _Ready()
	{
		defaultSize = Size;
		// Resize();
		// ZoomCalculator.OnZoomChanged += Resize;
		Position = new Vector2I(0, WinuserWrapper.GetSystemMetrics(4));
		mainWindow = GetTree().Root;
		mainWindow.FocusExited += ChangeVisibility;
	}
	void ChangeVisibility()
	{
		if(mainWindow.Mode == ModeEnum.Minimized) HideAnim();
		else if(mainWindow.HasFocus()) ShowAnim();
	}
	// void Resize() => Size = (Vector2I)(defaultSize * ZoomCalculator.ZoomVector);
	async void HideAnim(bool check = true)
	{
		if(check && !opened) return;
		opened = false;
		Borderless = false;
		WinuserWrapper.AnimateWindow(Handle, animationTime, WinuserWrapper.AnimationFlag.Hide | WinuserWrapper.AnimationFlag.Center | WinuserWrapper.AnimationFlag.Blend);
		await Task.Delay(animationTime);
		if(opened) ShowAnim(false);
	}
	async void ShowAnim(bool check = true)
	{
		if(check && opened) return;
		opened = true;
		WinuserWrapper.AnimateWindow(Handle, animationTime, WinuserWrapper.AnimationFlag.Activate | WinuserWrapper.AnimationFlag.Center | WinuserWrapper.AnimationFlag.Blend);
		await Task.Delay(animationTime);
		Borderless = true;
		if(!opened) HideAnim(false);
	}
}
