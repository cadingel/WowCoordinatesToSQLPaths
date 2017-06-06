using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using WowCoordinatesToSQLPaths.Model;
using NLua;
using System;
using System.Collections;
using LsonLib;
using System.IO;

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

        private List<CreatureMovementRow> _creatureMovementRows = new List<CreatureMovementRow>();
        private List<CreatureMovementTemplateRow> _creatureMovementTemplateRows = new List<CreatureMovementTemplateRow>();
        private List<ScriptWaypointRow> _scriptWaypointRows = new List<ScriptWaypointRow>();

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
            openFileDialog.Filter = ("Lua File|*.lua");

            if (openFileDialog.ShowDialog() == true)
            {
                currentlyLoadedCoordinatesFile.Content = coordinatesFilePath = openFileDialog.FileName;
                NameHereGood();
                CheckTableType();
            }
            
        }

        private void NameHereGood()
        {
            var reading = LsonVars.Parse(File.ReadAllText(coordinatesFilePath));
            var coordintaes = reading["CoordinatesList"];
            _creatureMovementRows.Clear();
            _creatureMovementTemplateRows.Clear();
            _scriptWaypointRows.Clear();

            for (int i = 0; i < coordintaes.Count; i++)
            {
                var row = coordintaes[i];

                if ((_isGuidChecked || _isEntryChecked) && !_isScriptWaypointPath)
                {
                    if (_isGuidChecked)
                    {
                        CreatureMovementRow creatureMovementRow = new CreatureMovementRow();
                        creatureMovementRow.Id = int.Parse(guidEntry.Text);
                        creatureMovementRow.Point = i;
                        creatureMovementRow.PositionX = (float)row["positionX"].GetDecimalLenient();
                        creatureMovementRow.PositionY = (float)row["positionY"].GetDecimalLenient();
                        creatureMovementRow.PositionZ = (float)row["positionZ"].GetDecimalLenient();
                        creatureMovementRow.Orientation = (float)row["orientation"].GetDecimalLenient();

                        _creatureMovementRows.Add(creatureMovementRow);
                    }
                    else
                    {
                        CreatureMovementTemplateRow creatureMovementTemplateRow = new CreatureMovementTemplateRow();
                        creatureMovementTemplateRow.Entry = int.Parse(guidEntry.Text);
                        creatureMovementTemplateRow.Point = i;
                        creatureMovementTemplateRow.PositionX = (float)row["positionX"].GetDecimalLenient();
                        creatureMovementTemplateRow.PositionY = (float)row["positionY"].GetDecimalLenient();
                        creatureMovementTemplateRow.PositionZ = (float)row["positionZ"].GetDecimalLenient();
                        creatureMovementTemplateRow.Orientation = (float)row["orientation"].GetDecimalLenient();

                        _creatureMovementTemplateRows.Add(creatureMovementTemplateRow);
                    }
                }
                else
                {
                    ScriptWaypointRow scriptWaypointRow = new ScriptWaypointRow();
                    scriptWaypointRow.Entry = int.Parse(guidEntry.Text);
                    scriptWaypointRow.PointId = i;
                    scriptWaypointRow.LocationX = (float)row["positionX"].GetDecimalLenient();
                    scriptWaypointRow.LocationY = (float)row["positionY"].GetDecimalLenient();
                    scriptWaypointRow.LocationZ = (float)row["positionZ"].GetDecimalLenient();

                    _scriptWaypointRows.Add(scriptWaypointRow);
                }
            }


        }
        private void CheckTableType()
        {
            if ((_isGuidChecked || _isEntryChecked) && !_isScriptWaypointPath)
            {
                if (_isGuidChecked)
                {
                    Waypoints = CollectionViewSource.GetDefaultView(_creatureMovementRows);

                }
                else
                {
                    Waypoints = CollectionViewSource.GetDefaultView(_creatureMovementTemplateRows);
                }
            }
            else
            {
                Waypoints = CollectionViewSource.GetDefaultView(_scriptWaypointRows);
            }

            Waypoints.Refresh();
            dataGrid.ItemsSource = Waypoints;
        }

        private void saveWaypoints_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
