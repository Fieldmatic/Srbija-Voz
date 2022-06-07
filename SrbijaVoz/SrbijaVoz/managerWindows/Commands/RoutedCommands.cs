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

        public static readonly RoutedUICommand AddNewItem = new RoutedUICommand(
            "Add new item",
            "AddItem",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand EditItem = new RoutedUICommand(
            "Edit item",
            "EditItem",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.E, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand DeleteItem = new RoutedUICommand(
            "Delete item",
            "DeleteItem",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Delete, ModifierKeys.Shift)
            }
        );
    }
}
