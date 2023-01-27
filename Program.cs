﻿using System;
using System.Windows.Forms;
using Checkers.UI;

namespace Checkers;

internal static class Program
{
    
  [STAThread]
  private static void Main()
  {
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new MainMenuForm());
  }
}