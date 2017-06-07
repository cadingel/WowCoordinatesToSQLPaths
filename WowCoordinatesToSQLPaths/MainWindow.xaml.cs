using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using WowCoordinatesToSQLPaths.Model;
using System;
using LsonLib;
using System.IO;
using System.Windows.Controls;

namespace WowCoordinatesToSQLPaths
{
    public partial class MainWindow : Window
    {
        public ICollectionView Waypoints { get; set; }

        private bool _isGuidChecked = true;
        private bool _isEntryChecked = false;
        private bool _isOneDirectionalPath = true;
        private bool _isBiDirectionalPath = false;
        private bool _isScriptWaypointPath = false;
        private bool _orientationChecked = false;
        private int _guidEntryValue = 123;
        private int _startingPoint = 1;

        private List<CreatureMovementRow> _creatureMovementRows = new List<CreatureMovementRow>();
        private List<CreatureMovementTemplateRow> _creatureMovementTemplateRows = new List<CreatureMovementTemplateRow>();
        private List<ScriptWaypointRow> _scriptWaypointRows = new List<ScriptWaypointRow>();
        private List<CreatureMovementRow> _creatureMovementRowsTwoWays = new List<CreatureMovementRow>();
        private List<CreatureMovementTemplateRow> _creatureMovementTemplateRowsTwoWays = new List<CreatureMovementTemplateRow>();

        private string coordinatesFilePath = "";
    
        public MainWindow()
        {
            InitializeComponent();
            InitContent();
            DataContext = this;

            dataGrid.CellEditEnding += dataGrid_CellEditEnding;

            guidEntry.GotFocus += RemoveText;
            guidEntry.LostFocus += AddText;
        }

        private void InitContent()
        {
            Waypoints = CollectionViewSource.GetDefaultView(_creatureMovementRows);
            UpdateDataGrid();
        } 

        private void guidRadButton_Checked(object sender, RoutedEventArgs e)
        {
            _isGuidChecked = true;
            _isEntryChecked = false;
            _isScriptWaypointPath = false;
            _isOneDirectionalPath = true;
            
            if (oneDirectional != null)
            {
                oneDirectional.IsChecked = true;
            }

            UpdateDataGrid();
        }

        private void entryRadButton_Checked(object sender, RoutedEventArgs e)
        {
            _isEntryChecked = true;
            _isGuidChecked = false;

            UpdateDataGrid();
        }

        private void oneDirectional_Checked(object sender, RoutedEventArgs e)
        {
            _isOneDirectionalPath = true;
            _isBiDirectionalPath = false;
            _isScriptWaypointPath = false;

            UpdateDataGrid();
        }

        private void biDirectional_Checked(object sender, RoutedEventArgs e)
        {
            _isBiDirectionalPath = true;
            _isOneDirectionalPath = false;
            _isScriptWaypointPath = false;

            UpdateDataGrid();
        }

        private void script_Checked(object sender, RoutedEventArgs e)
        {
            _isScriptWaypointPath = true;
            _isOneDirectionalPath = false;
            _isBiDirectionalPath = false;
            _isGuidChecked = false;
            _isEntryChecked = true;

            if (entryRadButton != null)
            {
                entryRadButton.IsChecked = true;
            }

            UpdateDataGrid();
        }

        private void getCoordinatesFiles_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = ("Lua Files (*.lua)|*.lua" /*|CSV Files (*.csv)|*.csv*/);

            if (openFileDialog.ShowDialog() == true)
            {
                currentlyLoadedCoordinatesFile.Content = coordinatesFilePath = openFileDialog.FileName;
                CreateListsForDataGrid();
                UpdateDataGrid();
                saveWaypoints.IsEnabled = true;
            }
        }

        private void CreateListsForDataGrid()
        {
            if (coordinatesFilePath.EndsWith(".lua"))
            {
                var reading = LsonVars.Parse(File.ReadAllText(coordinatesFilePath));
                var coordintaes = reading["CoordinatesList"];
                _creatureMovementRows.Clear();
                _creatureMovementTemplateRows.Clear();
                _scriptWaypointRows.Clear();

                for (int i = int.Parse(startingPoint.Text); i < coordintaes.Count; i++)
                {
                    var row = coordintaes[i];
                    var creatureMovementRow = new CreatureMovementRow();
                    var creatureMovementTemplateRow = new CreatureMovementTemplateRow();
                    var scriptWaypointRow = new ScriptWaypointRow();

                    creatureMovementRow.Id = int.Parse(guidEntry.Text);
                    creatureMovementRow.Point = i;
                    creatureMovementRow.PositionX = (float)row["positionX"].GetDecimalLenient();
                    creatureMovementRow.PositionY = (float)row["positionY"].GetDecimalLenient();
                    creatureMovementRow.PositionZ = (float)row["positionZ"].GetDecimalLenient();
                    creatureMovementRow.Orientation = _orientationChecked ? (float)row["orientation"].GetDecimalLenient() : 0;

                    _creatureMovementRows.Add(creatureMovementRow);

                    creatureMovementTemplateRow.Entry = int.Parse(guidEntry.Text);
                    creatureMovementTemplateRow.Point = i;
                    creatureMovementTemplateRow.PositionX = (float)row["positionX"].GetDecimalLenient();
                    creatureMovementTemplateRow.PositionY = (float)row["positionY"].GetDecimalLenient();
                    creatureMovementTemplateRow.PositionZ = (float)row["positionZ"].GetDecimalLenient();
                    creatureMovementTemplateRow.Orientation = _orientationChecked ? (float)row["orientation"].GetDecimalLenient() : 0;

                    _creatureMovementTemplateRows.Add(creatureMovementTemplateRow);

                    scriptWaypointRow.Entry = int.Parse(guidEntry.Text);
                    scriptWaypointRow.PointId = i;
                    scriptWaypointRow.LocationX = (float)row["positionX"].GetDecimalLenient();
                    scriptWaypointRow.LocationY = (float)row["positionY"].GetDecimalLenient();
                    scriptWaypointRow.LocationZ = (float)row["positionZ"].GetDecimalLenient();

                    _scriptWaypointRows.Add(scriptWaypointRow);
                }
            }
            else
            {

            }
            
            var copyCreatureMovementRows = new List<CreatureMovementRow>();
            var copyCreatureMovementTemplateRows = new List<CreatureMovementTemplateRow>();

            foreach (var row in _creatureMovementRows)
            {
                _creatureMovementRowsTwoWays.Add(row.Clone() as CreatureMovementRow);
                copyCreatureMovementRows.Add(row.Clone() as CreatureMovementRow);
            }

            foreach (var row in _creatureMovementTemplateRows)
            {
                _creatureMovementTemplateRowsTwoWays.Add(row.Clone() as CreatureMovementTemplateRow);
                copyCreatureMovementTemplateRows.Add(row.Clone() as CreatureMovementTemplateRow);
            }

            copyCreatureMovementRows.Reverse();
            copyCreatureMovementTemplateRows.Reverse();

            var creatureMovement = copyCreatureMovementRows.ToArray();
            var creatureMovementTemplate = copyCreatureMovementTemplateRows.ToArray();

            for (int i = _creatureMovementRows.Count, j = 1; i < _creatureMovementRows.Count * 2 - 2; i++, j++)
            {
                var tempCreatureMovementRow = creatureMovement[j].Clone() as CreatureMovementRow;
                tempCreatureMovementRow.Point = i;
                _creatureMovementRowsTwoWays.Add(tempCreatureMovementRow);

                var tempCreatureMovementTemplateRow = creatureMovementTemplate[j].Clone() as CreatureMovementTemplateRow;
                tempCreatureMovementTemplateRow.Point = i;
                _creatureMovementTemplateRowsTwoWays.Add(tempCreatureMovementTemplateRow);
            }
        }

        private void UpdateDataGrid()
        {
            if ((_isGuidChecked || _isEntryChecked) && !_isScriptWaypointPath)
            {
                if (_isGuidChecked)
                {
                    if (_isOneDirectionalPath)
                    {
                        Waypoints = CollectionViewSource.GetDefaultView(_creatureMovementRows);
                    }
                    else
                    {
                        Waypoints = CollectionViewSource.GetDefaultView(_creatureMovementRowsTwoWays);
                    }
                }
                else
                {
                    if (_isOneDirectionalPath)
                    {
                        Waypoints = CollectionViewSource.GetDefaultView(_creatureMovementTemplateRows);
                    }
                    else
                    {
                        Waypoints = CollectionViewSource.GetDefaultView(_creatureMovementTemplateRowsTwoWays);
                    }
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

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if ((_isGuidChecked || _isEntryChecked) && !_isScriptWaypointPath)
            {
                if (_isGuidChecked)
                {
                    if (_isOneDirectionalPath)
                    {
                        foreach (var row in _creatureMovementRows)
                        {
                            if (e.Row.GetIndex() == row.Point)
                            {
                                if (e.Column.Header.ToString().CompareTo("Id") == 0)
                                {
                                    row.Id = Int32.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Point") == 0)
                                {
                                    row.Point = e.Row.GetIndex();
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("PositionX") == 0)
                                {
                                    row.PositionX = float.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("PositionY") == 0)
                                {
                                    row.PositionY = float.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("PositionZ") == 0)
                                {
                                    row.PositionZ = float.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("WaitTime") == 0)
                                {
                                    row.WaitTime = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("ScriptId") == 0)
                                {
                                    row.ScriptId = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId1") == 0)
                                {
                                    row.TextId1 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId2") == 0)
                                {
                                    row.TextId2 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId3") == 0)
                                {
                                    row.TextId3 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId5") == 0)
                                {
                                    row.TextId4 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId5") == 0)
                                {
                                    row.TextId5 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Emote") == 0)
                                {
                                    row.Emote = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Spell") == 0)
                                {
                                    row.Spell = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Orientation") == 0)
                                {
                                    row.Orientation = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Model1") == 0)
                                {
                                    row.Model1 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Model2") == 0)
                                {
                                    row.Model2 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var row in _creatureMovementRowsTwoWays)
                        {
                            if (e.Row.GetIndex() == row.Point)
                            {
                                if (e.Column.Header.ToString().CompareTo("Id") == 0)
                                {
                                    row.Id = Int32.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Point") == 0)
                                {
                                    row.Point = e.Row.GetIndex();
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("PositionX") == 0)
                                {
                                    row.PositionX = float.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("PositionY") == 0)
                                {
                                    row.PositionY = float.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("PositionZ") == 0)
                                {
                                    row.PositionZ = float.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("WaitTime") == 0)
                                {
                                    row.WaitTime = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("ScriptId") == 0)
                                {
                                    row.ScriptId = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId1") == 0)
                                {
                                    row.TextId1 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId2") == 0)
                                {
                                    row.TextId2 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId3") == 0)
                                {
                                    row.TextId3 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId5") == 0)
                                {
                                    row.TextId4 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId5") == 0)
                                {
                                    row.TextId5 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Emote") == 0)
                                {
                                    row.Emote = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Spell") == 0)
                                {
                                    row.Spell = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Orientation") == 0)
                                {
                                    row.Orientation = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Model1") == 0)
                                {
                                    row.Model1 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Model2") == 0)
                                {
                                    row.Model2 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (_isOneDirectionalPath)
                    {
                        foreach (var row in _creatureMovementTemplateRows)
                        {
                            if (e.Row.GetIndex() == row.Point)
                            {
                                if (e.Column.Header.ToString().CompareTo("Id") == 0)
                                {
                                    row.Entry = Int32.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Point") == 0)
                                {
                                    row.Point = e.Row.GetIndex();
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("PositionX") == 0)
                                {
                                    row.PositionX = float.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("PositionY") == 0)
                                {
                                    row.PositionY = float.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("PositionZ") == 0)
                                {
                                    row.PositionZ = float.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("WaitTime") == 0)
                                {
                                    row.WaitTime = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("ScriptId") == 0)
                                {
                                    row.ScriptId = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId1") == 0)
                                {
                                    row.TextId1 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId2") == 0)
                                {
                                    row.TextId2 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId3") == 0)
                                {
                                    row.TextId3 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId5") == 0)
                                {
                                    row.TextId4 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId5") == 0)
                                {
                                    row.TextId5 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Emote") == 0)
                                {
                                    row.Emote = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Spell") == 0)
                                {
                                    row.Spell = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Orientation") == 0)
                                {
                                    row.Orientation = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Model1") == 0)
                                {
                                    row.Model1 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Model2") == 0)
                                {
                                    row.Model2 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var row in _creatureMovementTemplateRowsTwoWays)
                        {
                            if (e.Row.GetIndex() == row.Point)
                            {
                                if (e.Column.Header.ToString().CompareTo("Id") == 0)
                                {
                                    row.Entry = Int32.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Point") == 0)
                                {
                                    row.Point = e.Row.GetIndex();
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("PositionX") == 0)
                                {
                                    row.PositionX = float.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("PositionY") == 0)
                                {
                                    row.PositionY = float.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("PositionZ") == 0)
                                {
                                    row.PositionZ = float.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("WaitTime") == 0)
                                {
                                    row.WaitTime = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("ScriptId") == 0)
                                {
                                    row.ScriptId = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId1") == 0)
                                {
                                    row.TextId1 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId2") == 0)
                                {
                                    row.TextId2 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId3") == 0)
                                {
                                    row.TextId3 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId5") == 0)
                                {
                                    row.TextId4 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("TextId5") == 0)
                                {
                                    row.TextId5 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Emote") == 0)
                                {
                                    row.Emote = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Spell") == 0)
                                {
                                    row.Spell = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Orientation") == 0)
                                {
                                    row.Orientation = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Model1") == 0)
                                {
                                    row.Model1 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                                else if (e.Column.Header.ToString().CompareTo("Model2") == 0)
                                {
                                    row.Model2 = int.Parse((e.EditingElement as TextBox).Text);
                                    break;
                                }
                            }
                        }
                    }
                    
                }
            }
            else
            {
                foreach (var row in _scriptWaypointRows)
                {
                    if (e.Row.GetIndex() == row.PointId)
                    {
                        if (e.Column.Header.ToString().CompareTo("Entry") == 0)
                        {
                            row.Entry = Int32.Parse((e.EditingElement as TextBox).Text);
                            break;
                        }
                        else if (e.Column.Header.ToString().CompareTo("PointId") == 0)
                        {
                            row.PointId = Int32.Parse((e.EditingElement as TextBox).Text);
                            break;
                        }
                        else if (e.Column.Header.ToString().CompareTo("LocationX") == 0)
                        {
                            row.LocationX = float.Parse((e.EditingElement as TextBox).Text);
                            break;
                        }
                        else if (e.Column.Header.ToString().CompareTo("LocationY") == 0)
                        {
                            row.LocationY = float.Parse((e.EditingElement as TextBox).Text);
                            break;
                        }
                        else if (e.Column.Header.ToString().CompareTo("LocationZ") == 0)
                        {
                            row.LocationZ = float.Parse((e.EditingElement as TextBox).Text);
                            break;
                        }
                        else if (e.Column.Header.ToString().CompareTo("WaitTime") == 0)
                        {
                            row.WaitTime = int.Parse((e.EditingElement as TextBox).Text);
                            break;
                        }
                        else if (e.Column.Header.ToString().CompareTo("Comment") == 0)
                        {
                            row.Comment = (e.EditingElement as TextBox).Text;
                            break;
                        }
                    }
                }
            }
        }

        private void guidEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (guidEntry != null && guidEntry.Text != "")
            {
                if (getCoordinatesFiles != null) getCoordinatesFiles.IsEnabled = true;
            }
        }

        private void RemoveText(object sender, EventArgs e)
        {
            if (guidEntry != null) guidEntry.Text = "";
        }

        private void AddText(object sender, EventArgs e)
        {
            if (guidEntry != null && String.IsNullOrWhiteSpace(guidEntry.Text))
            {
                guidEntry.Text = "guid or entry";
            }
        }

        private void orientation_Click(object sender, RoutedEventArgs e)
        {
            if (orientation != null)
            {
                _orientationChecked = orientation.IsChecked == true ? true : false;
            }
        }
    }
}
