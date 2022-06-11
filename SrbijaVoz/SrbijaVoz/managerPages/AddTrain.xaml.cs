using SrbijaVoz.database;
using SrbijaVoz.dataGridRecord;
using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SrbijaVoz.managerWindows
{
    /// <summary>
    /// Interaction logic for AddTrainWindow.xaml
    /// </summary>
    public partial class AddTrainWindow : Window
    {
        private List<Train> Trains;
        private Database database;
        private TrainRecord selectedTrain;
        private readonly String operationType;
        private List<Wagon> selectedWagons = new();

        public Delegate UpdateTrain;

        public AddTrainWindow(TrainRecord Train, Database database, String operationType)
        {
            InitializeComponent();
            this.database = database;
            this.Trains = database.Trains;
            this.selectedTrain = Train;
            this.operationType = operationType;
            SetVariableWindowInfos();
            List<Wagon> Wagons = new List<Wagon>((Trains.Find(item => item.Id == this.selectedTrain.Id).Wagons).Select(w => new Wagon(w)));
            this.selectedWagons = Wagons;
            DrawWagons();
            Name.Text = Train.Name;
            seatsPriceIClass.Text = Train.FirstClassPrice.ToString();
            seatsPriceIIClass.Text = Train.SecondClassPrice.ToString();

        }

        public AddTrainWindow(Database database, String operationType)
        {
            //za dodavanje voza
            InitializeComponent();
            this.database = database;
            this.Trains = database.Trains;
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
            selectedWagons = selectedWagons.OrderBy(wagon => wagon.Number).ToList();

            for (int i = 0; i < selectedWagons.Count; i++)
            {
                GroupBox groupBox = (GroupBox)panel[i];
                Grid groupBoxGrid = (Grid)groupBox.Content;
                UIElementCollection children = groupBoxGrid.Children;
                TextBox seatsIClass = (TextBox)children[0];
                TextBox seatsIIClass = (TextBox)children[1];

                Wagon wagon = new Wagon();
                if (Int32.TryParse(seatsIClass.Text, out int seatsIClassInt) && Int32.TryParse(seatsIIClass.Text, out int seatsIIClassInt))
                {
                    wagon.Number = selectedWagons[i].Number;
                    wagon.Seats = GetWagonSeats(seatsIClassInt, seatsIIClassInt, wagon.Number);
                    wagons.Add(wagon);
                    selectedWagons[wagon.Number - 1] = wagon;
                }
                else return null;
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
            try
            {
                Train trainForUpdate = Trains.Find(item => item.Id == this.selectedTrain.Id);
                if (Name.Text == "") throw new ArgumentException();
                trainForUpdate.Name = Name.Text;
                List<Wagon> allWagons = getAllWagons();

                if (Int32.TryParse(seatsPriceIClass.Text, out int seatsPriceIClassInt) && Int32.TryParse(seatsPriceIIClass.Text, out int seatsPriceIIClassInt) && allWagons is not null)
                {
                    trainForUpdate.PricesPerMinute = GetTrainSeatsPrice(seatsPriceIClassInt, seatsPriceIIClassInt);
                    trainForUpdate.Wagons = getAllWagons();
                    foreach(LineSchedule ls in database.LineSchedules)
                    {
                        if (ls.Train.Id == this.selectedTrain.Id)
                            ls.Train = trainForUpdate;
                    }
                    UpdateTrain.DynamicInvoke();
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Za cenu i broj sedišta dozvoljeni su samo brojevi. Molim vas, pokušajte ponovo.", "Pogrešan unos", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Naziv vagona je obavezno polje. Molim vas, pokušajte ponovo. ", "Pogrešan unos", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }

        private void AddTrain_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Train newTrain = new Train();
                if (Name.Text == "") throw new ArgumentException();
                newTrain.Name = Name.Text;
                List<Wagon> allWagons = getAllWagons();

                if (Int32.TryParse(seatsPriceIClass.Text, out int seatsPriceIClassInt) && Int32.TryParse(seatsPriceIIClass.Text, out int seatsPriceIIClassInt) && allWagons is not null)
                {
                    newTrain.PricesPerMinute = GetTrainSeatsPrice(seatsPriceIClassInt, seatsPriceIIClassInt);
                    newTrain.Id = this.Trains.Last().Id + 1;
                    newTrain.Wagons = allWagons;
                    this.Trains.Add(newTrain);
                    UpdateTrain.DynamicInvoke();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Za cenu i broj sedišta dozvoljeni su samo brojevi.", "Pogrešan unos", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Naziv vagona je obavezno polje. Molim vas, pokušajte ponovo. ", "Pogrešan unos", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }

        private List<Seat> GetWagonSeats(int seatsIClass, int seatsIIClass, int wagonNumber)
        {
            int lastSeatNumber = 1;
            if (wagonNumber != 1) lastSeatNumber = selectedWagons.Where(w => w.Number < wagonNumber).Last().Seats.Last().Number + 1; 
            List<Seat> seats = new List<Seat>();
            for (int i = lastSeatNumber; i < lastSeatNumber + seatsIClass; i++)
            {
                seats.Add(new Seat(i, SeatClass.I));
            }

            for (int i = 0; i < seatsIIClass; i++)
            {
                seats.Add(new Seat(lastSeatNumber + seatsIClass + i, SeatClass.II));
            }
            return seats;

        }

        private Dictionary<SeatClass, int> GetTrainSeatsPrice(int seatsIClass, int seatsIIClass)
        {

            Dictionary<SeatClass, int> seatsPrice = new Dictionary<SeatClass, int>();
            seatsPrice.Add(SeatClass.I, seatsIClass);
            seatsPrice.Add(SeatClass.II, seatsIIClass);
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
            try
            {
                Wagon newWagon = new Wagon();
                List<Wagon> existingWagons = getAllWagons();
                if (Int32.TryParse(seatsNumIClass.Text, out int seatsIClassInt) && Int32.TryParse(seatsNumIIClass.Text, out int seatsIIClassInt) && existingWagons is not null)
                {
                    if (selectedWagons.Count == 0)
                    {
                        newWagon.Number = 1;
                    }
                    else
                    {
                        Wagon lastWagon = new Wagon(selectedWagons.Last());
                        newWagon.Number = lastWagon.Number + 1;
                    }
                    newWagon.Seats = GetWagonSeats(seatsIClassInt, seatsIIClassInt, newWagon.Number);
                    selectedWagons = existingWagons;
                    selectedWagons.Add(newWagon);
                    ClearWagonInputs();
                    DrawWagons();
                }
                else
                {
                    MessageBox.Show("Za broj sedišta vagona dozvoljeni su samo brojevi. Molim vas, pokušajte ponovo. ", "Pogrešan unos", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Niste uneli broj sedišta za oba razreda vagona. Molim vas, pokušajte ponovo. ", "Pogrešan unos", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
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
