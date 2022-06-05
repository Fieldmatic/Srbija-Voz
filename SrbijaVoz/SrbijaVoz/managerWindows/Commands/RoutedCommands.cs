using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SrbijaVoz.managerWindows.Commands
{
    public static class RoutedCommands
    {

        public static readonly RoutedUICommand AddLine = new RoutedUICommand(
            "Add new line",
            "AddLine",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Control)
            }
            );
    }
}
