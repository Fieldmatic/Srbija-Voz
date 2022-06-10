using SrbijaVoz.database;
using SrbijaVoz.dataGridRecord;
using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace SrbijaVoz.managerWindows
{
    /// <summary>
    /// Interaction logic for AddTrainWindow.xaml
    /// </summary>
    public partial class AddTrainWindow : Window
    {
        private List<Train> Trains;
        private TrainRecord selectedTrain;
        private readonly String operationType;
        private List<Wagon> selectedWagons = new();
        private int WagonNumber = 0;

        public Delegate UpdateTrain;

        public AddTrainWindow(TrainRecord Train, List<Train> trains, String operationType)
        {
            InitializeComponent();
            
            this.Trains = trains;
            this.selectedTrain = Train;
            this.operationType = operationType;
            SetVariableWindowInfos();
            List<Wagon> Wagons = new List<Wagon>(Trains.Find(item => item.Id == this.selectedTrain.Id).Wagons);
            this.selectedWagons = Wagons;
            DrawWagons();
            Name.Text = Train.Name;
            seatsPriceIClass.Text = Train.FirstClassPrice.ToString();
            seatsPriceIIClass.Text = Train.SecondClassPrice.ToString();

        }

        public AddTrainWindow(List<Train> trains, String operationType)
        {
            //za dodavanje voza
            InitializeComponent();
            this.Trains = trains;
            this.operationType = operationType;
            SetVariableWindowInfos();
            ClearAllFields();

        }

        private void DrawWagons()
        {
            WagonsPanel.Children.Clear();
            int numberOfRows = selectedWagons.Count;
            for (int i = 0; i < numberOfRows; i++)
            {
                Wagon wagon = selectedWagons[i];
                GroupBox groupBox = new GroupBox();
                groupBox.Width = 170;

                //header panel
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.VerticalAlignment = VerticalAlignment.Center;
                stackPanel.HorizontalAlignment = HorizontalAlignment.Center;
                TextBlock headerText = new TextBlock();
                headerText.Foreground = Brushes.White;
                headerText.Text = "Broj sedišta - Vagon " + wagon.Number;
                headerText.Name = "_" + wagon.Number; 
                headerText.FontWeight = FontWeights.Medium;
                stackPanel.Children.Add(headerText);

                var closeIcon = new MaterialDesignThemes.Wpf.PackIcon();
                closeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.CloseCircle;
                closeIcon.Foreground = Brushes.Red;
                closeIcon.Width = 19;
                closeIcon.Height = 19;
                closeIcon.Cursor = Cursors.Hand;
                closeIcon.MouseDown += DeleteWagon_Event;
                Thickness thickness = closeIcon.Margin;
                thickness.Right = 0;
                thickness.Left = 8;
                closeIcon.Margin = thickness;
                stackPanel.Children.Add(closeIcon);
                groupBox.Header = stackPanel;


                Grid.SetRow(groupBox, 1);
                Grid.SetColumn(groupBox, 1);
                Thickness margin = groupBox.Margin;
                margin.Right = 10;
                groupBox.Margin = margin;

                Grid groupBoxGrid = new Grid();
                groupBoxGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                RowDefinition row1 = new RowDefinition();
                RowDefinition row2 = new RowDefinition();
                row1.Height = new GridLength(1, GridUnitType.Star);
                row2.Height = new GridLength(1, GridUnitType.Star);
                groupBoxGrid.RowDefinitions.Add(row1);
                groupBoxGrid.RowDefinitions.Add(row2);


                TextBox textBox1 = new TextBox();
                textBox1.Height = 48;
                textBox1.FontSize = 18;
                textBox1.Name = "_1" + wagon.Number.ToString();
                textBox1.Text = wagon.getNumberOfSeatsClass(SeatClass.I).ToString();


                TextBox textBox2 = new TextBox();
                textBox2.Height = 48;
                textBox2.FontSize = 18;
                textBox2.Name = "_2" + wagon.Number.ToString(); 
                textBox2.Text = wagon.getNumberOfSeatsClass(SeatClass.II).ToString();

                groupBoxGrid.Children.Add(textBox1);
                groupBoxGrid.Children.Add(textBox2);
                Grid.SetRow(textBox1, 0);
                Grid.SetRow(textBox2, 1);

                groupBox.Content = groupBoxGrid;

                WagonsPanel.Children.Add(groupBox);
            }

        }

        private List<Wagon> getAllWagons()
        {
            List<Wagon> wagons = new List<Wagon>();
            UIElementCollection panel = WagonsPanel.Children;
    
            for (int i = 0; i < selectedWagons.Count; i++)
            {
                GroupBox groupBox = (GroupBox)panel[i];
                Grid groupBoxGrid = (Grid)groupBox.Content;
                UIElementCollection children = groupBoxGrid.Children;
                TextBox seatsIClass = (TextBox)children[0];
                TextBox seatsIIClass = (TextBox)children[1];

                Wagon wagon = new Wagon();
                wagon.Seats = GetWagonSeats(Int32.Parse(seatsIClass.Text), Int32.Parse(seatsIIClass.Text));
                wagon.Number = selectedWagons[i].Number;
                wagons.Add(wagon);
            }
            return wagons;
        }


        private void UpdateOrAddTrain_Click(object sender, RoutedEventArgs e)
        {
            if (this.operationType == "add")
            {
                AddTrain_Click(sender, e);
            } else
            {
                UpdateTrain_Click(sender, e);
            }
        }

        private void UpdateTrain_Click(object sender, RoutedEventArgs e)
        {
            Train trainForUpdate = Trains.Find(item => item.Id == this.selectedTrain.Id);
            trainForUpdate.Name = Name.Text;
            trainForUpdate.PricesPerMinute = GetTrainSeatsPrice();
            trainForUpdate.Wagons = getAllWagons();
            UpdateTrain.DynamicInvoke();
            this.Close();
        }

        private void AddTrain_Click(object sender, RoutedEventArgs e)
        {
            Train newTrain = new Train();
            newTrain.Name = Name.Text;
            newTrain.PricesPerMinute = GetTrainSeatsPrice();
            newTrain.Id = this.Trains.Last().Id + 1;
            newTrain.Wagons = getAllWagons();
            this.Trains.Add(newTrain);
            UpdateTrain.DynamicInvoke();
            this.Close();
        }

        private List<Seat> GetWagonSeats(int seatsIClass, int seatsIIClass)
        {
            List<Seat> seats = new List<Seat>();
            for (int i = 1; i <= seatsIClass; i++)
            {
                seats.Add(new Seat(i, SeatClass.I));
            }

            for (int i = 1; i <= seatsIIClass; i++)
            {
                seats.Add(new Seat(i, SeatClass.II));
            }
            return seats;

        }

        private Dictionary<SeatClass, int> GetTrainSeatsPrice()
        {
            Dictionary<SeatClass, int> seatsPrice = new Dictionary<SeatClass, int>();
            seatsPrice.Add(SeatClass.I, Int32.Parse(seatsPriceIClass.Text));
            seatsPrice.Add(SeatClass.II, Int32.Parse(seatsPriceIIClass.Text));
            return seatsPrice;
        }

        private void SetVariableWindowInfos()
        {
            if (this.operationType == "add")
            {
                updateButton.Content = "Dodaj voz";
                Title = "Dodavanje voza";
            }
            else if (this.operationType == "update")
            {
                updateButton.Content = "Izmeni voz";
                Title = "Izmena voza";
            }
        }

        private void ClearAllFields()
        {
            Name.Text = "";
            seatsPriceIClass.Text = "";
            seatsPriceIIClass.Text = "";
            ClearWagonInputs();
        }

        private void ClearWagonInputs()
        {
            seatsNumIClass.Text = "";
            seatsNumIIClass.Text = "";
        }

        private void AddWagon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WagonNumber++;
            Wagon newWagon = new Wagon();
            newWagon.Seats = GetWagonSeats(Int32.Parse(seatsNumIClass.Text), Int32.Parse(seatsNumIIClass.Text));
            if (selectedTrain == null)
            {
                if (selectedWagons.Count == 0)
                {
                    newWagon.Number = 1;
                } else
                {
                    Wagon lastWagon = new Wagon(selectedWagons.Last());
                    newWagon.Number = lastWagon.Number + 1;
                }
            }
            else
                newWagon.Number = selectedWagons.Last().Number + 1;
            selectedWagons = getAllWagons();
            selectedWagons.Add(newWagon);
            ClearWagonInputs();
            DrawWagons();
            
        }

        private void ReplaceWagonsNumber(int deletedWagonNumber)
        {
            //kada se vagon obrise moram da prilagodim brojeve vagona
            foreach(Wagon wagon in selectedWagons)
            {
                if (wagon.Number > deletedWagonNumber)
                {
                    wagon.Number = wagon.Number - 1;
                }
            }
        }

        private void DeleteWagon_Event(object sender, MouseButtonEventArgs e)
        {
            StackPanel panel = (StackPanel) VisualTreeHelper.GetParent((MaterialDesignThemes.Wpf.PackIcon)sender);
            TextBlock textBlock = (TextBlock) panel.Children[0];
            int wagonNumber = Int32.Parse(textBlock.Name.Replace("_", ""));

            Wagon deleteWagon = selectedWagons.FirstOrDefault(w => w.Number == wagonNumber);
            if (deleteWagon != null)
            {
                ReplaceWagonsNumber(deleteWagon.Number);
                selectedWagons.Remove(deleteWagon);
            }
            DrawWagons();

        }

    }

}
