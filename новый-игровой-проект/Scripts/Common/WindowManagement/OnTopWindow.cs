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

	public override void _Ready()
	{
		if (!WindowManagerAPI.ForceNative) return;
		Hide();
		ForceNative = true;
		Show();
		defaultSize = Size;
		// Resize();
		// ZoomCalculator.OnZoomChanged += Resize;
		Position = new Vector2I(0, WindowManagerAPI.CaptionHeight);
		mainWindow = GetTree().Root;
		mainWindow.FocusExited += ChangeVisibility;
	}

    public override void _Notification(int what)
	{
		if (!WindowManagerAPI.ForceNative) return;
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
	void ChangeVisibility()
	{
		if(mainWindow.Mode == ModeEnum.Minimized) HideAnim();
		else if(mainWindow.HasFocus()) ShowAnim();
	}
	// void Resize() => Size = (Vector2I)(defaultSize * ZoomCalculator.ZoomVector);
	async void HideAnim()
	{
		if(!opened) return;
		opened = false;
		Borderless = false;
		
		WindowManagerAPI.HideWindow(Handle, animationTime);
		await Task.Delay(animationTime);
		if(opened) ShowAnim();
	}
	async void ShowAnim()
	{
		if(opened) return;
		opened = true;
		WindowManagerAPI.ShowWindow(Handle, animationTime);
		await Task.Delay(animationTime);
		Borderless = true;
		if(!opened) HideAnim();
	}
}
