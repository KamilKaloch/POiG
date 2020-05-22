namespace MiniTC
{
    using System.Windows;
    using System.IO;
    using System;
    using System.Linq;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Left.AvailablePaths_lb.SelectedIndex == -1) return;
            string leftFile = Path.Combine(Left.CurrentPath, Left.AvailablePaths[Left.AvailablePaths_lb.SelectedIndex]);
            string rightFile = Path.Combine(Right.CurrentPath, Left.AvailablePaths[Left.AvailablePaths_lb.SelectedIndex]);

            try
            {
                if (Right.AvailablePaths.Contains(Left.AvailablePaths[Left.AvailablePaths_lb.SelectedIndex]))
                {
                    MessageBoxResult overwrite = MessageBoxResult.None;
                    while (overwrite == MessageBoxResult.None)
                    {
                        overwrite = MessageBox.Show($"This will overwrite the hell out of this file\n {Left.AvailablePaths[Left.AvailablePaths_lb.SelectedIndex]}.\n Proceed?", "OVERWRITE", MessageBoxButton.OKCancel, MessageBoxImage.Hand, MessageBoxResult.No);
                    }
                    if (overwrite == MessageBoxResult.OK)
                    {
                        File.Copy(leftFile, rightFile, true);
                        Right.AvailablePaths_lb.ItemsSource = Right.GetContents();
                        return;
                    }
                    return;
                }
                File.Copy(leftFile, rightFile, true);
                Right.AvailablePaths_lb.ItemsSource = Right.GetContents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SuMtHin wEnt wong ", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                Left.AvailablePaths_lb.ItemsSource = Left.GetContents();
                Right.AvailablePaths_lb.ItemsSource = Right.GetContents();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show("Thank You for using, have a nice day", "Tanks ferry much ", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
            MessageBox.Show("Danke schon", "Tanks ferry much ", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
            MessageBox.Show("See ya", "Tanks ferry much ", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
            MessageBox.Show("Aufwiedersehen", "Tanks ferry much ", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
            MessageBox.Show("Still here?, I said have a nice day!!", "Tanks ferry much ", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
            MessageBox.Show("That means GO AWAY", "Tanks ferry much ", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
            MessageBox.Show("LEAVE ME ALONE", "Tanks ferry much ", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
            MessageBox.Show("Use ALT + F4 maybe?", "Tanks ferry much ", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
            MessageBox.Show("...", "Tanks ferry much ", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
            MessageBox.Show("...", "Tanks ferry much ", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
            MessageBox.Show("...", "Tanks ferry much ", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
            MessageBox.Show("Fine I\'ll do it myself ^-^", "Tanks ferry much ", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
        }
    }
}
