using SrbijaVoz.database;
using SrbijaVoz.dataGridRecord;
using SrbijaVoz.managerWindows.help;
using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace SrbijaVoz.managerWindows
{
    public partial class TrainPage : Page
    {
        private readonly Database Database;
        public ObservableCollection<TrainRecord> Trains = new ObservableCollection<TrainRecord>();
        private TrainRecord? Train;

        public delegate Point GetPosition(IInputElement element);
        private int selectedRowIndex = -1;

        public delegate void RefreshTrains();
        public event RefreshTrains RefreshTrainsListEvent;

        public TrainPage(Database database)
        {
            InitializeComponent();
            this.Database = database;
            InitializeTrains();
            
        }

        private void InitializeTrains()
        {
            this.Trains = getTrainGridData();
            this.TrainDataGrid.DataContext = this.Trains;
            this.Train = null;
        }

        private ObservableCollection<TrainRecord> getTrainGridData()
        {
            ObservableCollection<TrainRecord> trainRecordData = new();
            foreach (Train train in this.Database.Trains) trainRecordData.Add(new TrainRecord(train));
            return trainRecordData;
        }


        private void AddTrain_Drop(object sender, DragEventArgs e)
        {
            this.Train = e.Data.GetData("TrainRecord") as TrainRecord;
            Add_Train_Event(sender, e);
              
        }

        private void UpdateTrain_Drop(object sender, DragEventArgs e)
        {
            this.Train = e.Data.GetData("TrainRecord") as TrainRecord;
            AddTrainWindow updateTrainWindow = new(this.Train, Database, "update");
            RefreshTrainsListEvent += new RefreshTrains(InitializeTrains); // event initialization
            updateTrainWindow.UpdateTrain = RefreshTrainsListEvent;
            updateTrainWindow.Show();

        }

        private void DeleteTrain_Drop(object sender, DragEventArgs e)
        {
            this.Train = e.Data.GetData("TrainRecord") as TrainRecord;
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da obrišete voz " + this.Train.Name + "?", "Brisanje voza", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if ( result == MessageBoxResult.Yes)
            {
                Train trainForDelete = Database.Trains.Find(item => item.Id == this.Train.Id);
                Database.Trains.Remove(trainForDelete);
                this.TrainDataGrid.DataContext = null;
                InitializeTrains();
            }
            
        }

        private void Add_Train_Event(object sender, RoutedEventArgs e)
        {
            AddTrainWindow addTrain;
            if (this.Train is not null)
            {
                addTrain = new(this.Train, Database, "add");
                this.Train = null;
            }
            else
            {
                addTrain = new(Database, "add");
            }
            RefreshTrainsListEvent += new RefreshTrains(InitializeTrains); // event initialization
            addTrain.UpdateTrain = RefreshTrainsListEvent;
            addTrain.Show();
        }

        private void TrainDataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedRowIndex = GetCurrentRowIndex(e.GetPosition);
            if (selectedRowIndex < 0)
            {
                return;
            }
            TrainDataGrid.SelectedIndex = selectedRowIndex;
            if (Trains[selectedRowIndex] is not TrainRecord selectedEmp)
                return;
            var dataObject = new DataObject();
            dataObject.SetData("TrainRecord", selectedEmp);
            DragDrop.DoDragDrop(TrainDataGrid, dataObject, DragDropEffects.Move);
         

        }

        private void playDemo(object sender, RoutedEventArgs e)
        {
            DemoVideo m = new DemoVideo(@"../../../demo/Stanice.mp4");
            m.ShowDialog();
        }

        private int GetCurrentRowIndex(GetPosition pos)
        {
            int curIndex = -1;
            for (int i = 0; i < TrainDataGrid.Items.Count; i++)
            {
                DataGridRow itm = GetRowItem(i);
                if (GetMouseTargetRow(itm, pos))
                {
                    curIndex = i;
                    break;
                }
            }
            return curIndex;
        }

        private static bool GetMouseTargetRow(Visual theTarget, GetPosition position)
        {
            Rect rect = VisualTreeHelper.GetDescendantBounds(theTarget);
            Point point = position((IInputElement)theTarget);
            return rect.Contains(point);
        }

        private DataGridRow GetRowItem(int index)
        {
            if (TrainDataGrid.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;
            return TrainDataGrid.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
        }
    }
}
