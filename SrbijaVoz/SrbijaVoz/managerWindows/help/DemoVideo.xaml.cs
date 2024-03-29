﻿using System;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SrbijaVoz.managerWindows.help
{
    /// <summary>
    /// Interaction logic for DemoVideo.xaml
    /// </summary>
    public partial class DemoVideo : Window
    {
		public DemoVideo(String Demo)
		{
			InitializeComponent();

			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += timer_Tick;
			timer.Start();
			mePlayer.Source = new Uri(Demo, UriKind.Relative);
			CenterWindowOnScreen();
		}

		void timer_Tick(object sender, EventArgs e)
		{
			if (mePlayer.Source != null)
			{
				if (mePlayer.NaturalDuration.HasTimeSpan)
					lblStatus.Content = String.Format("{0} / {1}", mePlayer.Position.ToString(@"mm\:ss"), mePlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
			}
			else
				lblStatus.Content = "No file selected...";
		}

		private void btnPlay_Click(object sender, RoutedEventArgs e)
		{
			mePlayer.Play();
		}

		private void btnPause_Click(object sender, RoutedEventArgs e)
		{
			mePlayer.Pause();
		}

		private void btnStop_Click(object sender, RoutedEventArgs e)
		{
			mePlayer.Stop();
		}

		private void CenterWindowOnScreen()
		{
			double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
			double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
			double windowWidth = this.Width;
			double windowHeight = this.Height;
			this.Left = (screenWidth / 2) - (windowWidth / 2);
			this.Top = (screenHeight / 2) - (windowHeight / 2);
		}

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
			this.Close();
        }
    }
}
