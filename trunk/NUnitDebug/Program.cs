using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Gui;
namespace NUnitDebug
{
    class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            return NUnit.Gui.AppEntry.Main(args);
        }
    }
}
