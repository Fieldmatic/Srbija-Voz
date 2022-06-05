using SrbijaVoz.database;
using SrbijaVoz.dataGridRecord;
using SrbijaVoz.model;
using System.Collections.Generic;
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
        private List<TrainRecord> Trains;
        private TrainRecord? Train;
        public delegate Point GetPosition(IInputElement element);
        private int rowIndex = -1;

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
            TrainDataGrid.ItemsSource = Trains;
            this.Train = null;
        }

        private List<TrainRecord> getTrainGridData()
        {
            List<TrainRecord> trainRecordData = new();
            foreach (Train train in Database.Trains) trainRecordData.Add(new TrainRecord(train));
            return trainRecordData;
        }


        private void TrainDataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            rowIndex = GetCurrentRowIndex(e.GetPosition);
            TrainDataGrid.SelectedIndex = rowIndex;
            if (Trains[rowIndex] is not TrainRecord selectedEmp)
                return;
            var dataObject = new DataObject();
            dataObject.SetData("TrainRecord", selectedEmp);
            DragDrop.DoDragDrop(TrainDataGrid, dataObject, DragDropEffects.Move);

        }

        private void AddTrain_Drop(object sender, DragEventArgs e)
        {
            this.Train = e.Data.GetData("TrainRecord") as TrainRecord;
            Add_Train_Event(sender, e);
              
        }

        private void Add_Train_Event(object sender, RoutedEventArgs e)
        {
            AddTrainWindow addTrain;
            if (this.Train is not null)
            {
                addTrain = new(this.Train, Database.Trains);
            }
            else
            {
                addTrain = new(Database.Trains);
            }
            RefreshTrainsListEvent += new RefreshTrains(InitializeTrains); // event initialization
            addTrain.AddTrain = RefreshTrainsListEvent;
            addTrain.Show();
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
