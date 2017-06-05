using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using WowCoordinatesToSQLPaths.Model;

namespace WowCoordinatesToSQLPaths
{
    public partial class MainWindow : Window
    {
        public ICollectionView Waypoints { get; set; }

        private bool _isGuidChecked;
        private bool _isEntryChecked;
        private bool _isOneDirectionalPath;
        private bool _isBiDirectionalPath;
        private bool _isScriptWaypointPath;
        private int _guidEntryValue;
        private int _startingPoint;


        private string coordinatesFilePath = "";

        public MainWindow()
        {
            InitializeComponent();
            InitContent();
            DataContext = this;
        }

        private void InitContent()
        {
            _isGuidChecked = true;
            _isEntryChecked = false;
            _isOneDirectionalPath = true;
            _isBiDirectionalPath = false;
            _isScriptWaypointPath = false;

            _guidEntryValue = 0;
            _startingPoint = 0;

            var waypoints = new List<CreatureMovementRow> { new CreatureMovementRow
                {
                    Id = _guidEntryValue,
                    Point = _startingPoint,
                    PositionX = 0,
                    PositionY = 0,
                    PositionZ = 0,
                    WaitTime = 0,
                    ScriptId = 0,
                    TextId1 = 0,
                    TextId2 = 0,
                    TextId3 = 0,
                    TextId4 = 0,
                    TextId5 = 0,
                    Emote = 0,
                    Spell = 0,
                    Orientation = 0,
                    Model1 = 0,
                    Model2 = 0
                }
            };

            Waypoints = CollectionViewSource.GetDefaultView(waypoints);
        } 

        private void guidRadButton_Checked(object sender, RoutedEventArgs e)
        {
            _isGuidChecked = true;
            _isEntryChecked = false;
        }

        private void entryRadButton_Checked(object sender, RoutedEventArgs e)
        {
            _isEntryChecked = true;
            _isGuidChecked = false;
        }

        private void oneDirectional_Checked(object sender, RoutedEventArgs e)
        {
            _isOneDirectionalPath = true;
            _isBiDirectionalPath = false;
            _isScriptWaypointPath = false;
        }

        private void biDirectional_Checked(object sender, RoutedEventArgs e)
        {
            _isBiDirectionalPath = true;
            _isOneDirectionalPath = false;
            _isScriptWaypointPath = false;
        }

        private void script_Checked(object sender, RoutedEventArgs e)
        {
            _isScriptWaypointPath = true;
            _isOneDirectionalPath = false;
            _isBiDirectionalPath = false;
        }

        private void getCoordinatesFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                currentlyLoadedCoordinatesFile.Content = coordinatesFilePath = openFileDialog.FileName;
            }
        }

        private void saveWaypoints_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
