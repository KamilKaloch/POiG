namespace MiniTC
{
    using System.Windows.Controls;
    using System.IO;
    using System;
    using System.Linq;
    using System.Windows;

    public partial class PanelTC : UserControl, IPanelTC
    {
        string currentPath = null;
        string[] availableDrives = null;
        string[] availablePaths = null;

        public PanelTC()
        {
            InitializeComponent();
            availableDrives = Directory.GetLogicalDrives();
            Drives.ItemsSource = AvailableDrives;
            Drives.SelectedIndex = 0;
            availablePaths = GetContents();
            Path_tb.Text = CurrentPath;
            AvailablePaths_lb.ItemsSource = availablePaths;
        }

        public string CurrentPath
        {
            get => currentPath;
            set
            {
                currentPath = value;
            }
        }
        public string[] AvailableDrives
        {
            get => availableDrives;
            set
            {
                availableDrives = value;
            }
        }
        public string[] AvailablePaths
        {
            get => availablePaths;
            set
            {
                availablePaths = value;
            }
        }

        public string[] GetContents()
        {
            string[] temp1 = null;
            string[] temp2 = null;
            string[] temp3 = null;
            try
            {
                temp1 = Directory.GetDirectories(currentPath);
                for (int i = 0; i < temp1.Length; i++)
                {
                    temp1[i] = Path.GetFileName(temp1[i]);
                    temp1[i] = $"<D>{temp1[i]}";
                }

                temp2 = Directory.GetFiles(currentPath);
                for (int i = 0; i < temp2.Length; i++)
                {
                    temp2[i] = Path.GetFileName(temp2[i]);
                }

                int size = temp1.Length + temp2.Length;
                temp3 = null;
                if (!availableDrives.Contains(currentPath))
                {
                    temp3 = new string[size + 1];
                    temp3[0] = "...";
                    for(int i = 0; i <temp1.Length; i++)
                    {
                        temp3[i+1] = temp1[i];
                    }
                    for (int i = 0; i < temp2.Length; i++)
                    {
                        temp3[i + temp1.Length +1] = temp2[i];
                    }
                }
                else
                {
                    temp3 = new string[size];
                    for (int i = 0; i < temp1.Length; i++)
                    {
                        temp3[i] = temp1[i];
                    }
                    for (int i = 0; i < temp2.Length; i++)
                    {
                        temp3[i + temp1.Length] = temp2[i];
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SuMtHin wEnt wong ", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                currentPath = currentPath.Substring(0, currentPath.Length - 1);
                int index = currentPath.LastIndexOf("\\");
                currentPath = currentPath.Substring(0, index + 1);
                Path_tb.Text = currentPath;
                return GetContents();
            }
            return temp3;
        }

        private void Drives_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Drives.SelectedIndex == -1) return;
            currentPath = availableDrives[Drives.SelectedIndex];
            Path_tb.Text = currentPath;
            availablePaths = GetContents();
            AvailablePaths_lb.ItemsSource = availablePaths;
        }

        private void AvailablePaths_lb_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (AvailablePaths_lb.SelectedIndex == -1) return;
            string item = availablePaths[AvailablePaths_lb.SelectedIndex];
            if (item.StartsWith("<D>"))
            {
                currentPath += $"{item.Substring(3)}\\";
                Path_tb.Text = currentPath;
                availablePaths = GetContents();
                AvailablePaths_lb.ItemsSource = availablePaths;
                AvailablePaths_lb.SelectedIndex = -1;
            }
            else if (item == "...")
            {
                currentPath = currentPath.Substring(0, currentPath.Length - 1);
                int index = currentPath.LastIndexOf("\\") + 1;
                currentPath = currentPath.Substring(0, index);
                Path_tb.Text = currentPath;
                availablePaths = GetContents();
                AvailablePaths_lb.ItemsSource = availablePaths;
                AvailablePaths_lb.SelectedIndex = -1;
            }
        }

        private void AvailablePaths_lb_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (AvailablePaths_lb.SelectedIndex == -1) return;
            if (availablePaths[AvailablePaths_lb.SelectedIndex].StartsWith("<D>") || availablePaths[AvailablePaths_lb.SelectedIndex].Equals("...")) return;
            string path =currentPath + availablePaths[AvailablePaths_lb.SelectedIndex];
            System.Diagnostics.Process.Start(path);
        }
    }
}
