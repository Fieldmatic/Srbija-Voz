﻿using SrbijaVoz.dataGridRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SrbijaVoz.managerWindows
{
    /// <summary>
    /// Interaction logic for LinePage.xaml
    /// </summary>
    public partial class LinePage : Page
    {
        public LinePage(List<LineRecord> lineRecords)
        {
            InitializeComponent();
            LineDataGrid.ItemsSource = lineRecords;
        }
    }
}
