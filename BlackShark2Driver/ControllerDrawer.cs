using System;
using System.Collections.Generic;
using XOutput.Devices.XInput;

namespace BlackShark2Driver
{
    public static class ControllerDrawer
    {
        public static void Print(Dictionary<XInputTypes, double> keys)
        {
			Console.SetCursorPosition(0, 1);
			
            int jx = (int)Math.Floor(5 * keys[XInputTypes.LX]);
            int jy = (int)Math.Floor(5 - 5 * keys[XInputTypes.LY]);
			
			jx = jx ==5?4:jx;
			jy = jy == 5?4:jy;
			
            for (int y = 0; y < 5; y++)
            {
                Console.Write("  ");
                for(int x = 0;x < 5; x++) Console.Write(x == jx && y == jy ? '●' : '□');
                if(y == 0)
                {
                    Console.Write("    |         ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = (int)keys[XInputTypes.Y] == 1 ? ConsoleColor.White : Console.BackgroundColor;
                    Console.Write('Y');
                    Console.ResetColor();
                    Console.Write("         |    ┌--┐");
                }
                else if(y == 1)
                {
                    Console.Write("    |                   |    ├");
                    Console.BackgroundColor = (int)keys[XInputTypes.L1] == 1 ? ConsoleColor.White : Console.BackgroundColor;
                    Console.Write("LB");
                    Console.ResetColor();
                    Console.Write('┤');
                }
                else if(y == 2)
                {
                    Console.Write("    |    ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.BackgroundColor = (int)keys[XInputTypes.X] == 1 ? ConsoleColor.White : Console.BackgroundColor;
                    Console.Write('X');
                    Console.ResetColor();
                    Console.Write("         ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = (int)keys[XInputTypes.B] == 1 ? ConsoleColor.White : Console.BackgroundColor;
                    Console.Write('B');
                    Console.ResetColor();
                    Console.Write("    |    ├--┤");
                }
                else if(y == 3)
                {
                    Console.Write("    |                   |    ├");
                    Console.BackgroundColor = (int)keys[XInputTypes.L3] == 1 ? ConsoleColor.White : Console.BackgroundColor;
                    Console.Write("LT");
                    Console.ResetColor();
                    Console.Write('┤');
                }
                else
                {
                    Console.Write("    |         ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = (int)keys[XInputTypes.A] == 1 ? ConsoleColor.White : Console.BackgroundColor;
                    Console.Write('A');
                    Console.ResetColor();
                    Console.Write("         |    └--┘");
                }
				Console.Write('\n');
            }
        }
    }
}
