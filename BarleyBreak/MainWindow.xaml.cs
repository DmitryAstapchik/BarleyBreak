using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BarleyBreak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FrameworkElement currentElement;
        int currentRow;
        int currentColumn;
        int emptyRow = 3;
        int emptyColumn = 3;
        int previousIndex = 0;
        int ticks = 0;
        bool moving = false;
        Random random = new Random();
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            MixButtons();
            DataContext = this;
            timer.Interval = new TimeSpan(100000);
            timer.Tick += Timer_Tick;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (moving == false)
            {
                currentElement = (FrameworkElement)e.OriginalSource;
                currentRow = Grid.GetRow(currentElement);
                currentColumn = Grid.GetColumn(currentElement);
                if (emptyRow == currentRow && emptyColumn + 1 == currentColumn || 
                    emptyRow == currentRow && emptyColumn - 1 == currentColumn || 
                    emptyRow - 1 == currentRow && emptyColumn == currentColumn || 
                    emptyRow + 1 == currentRow && emptyColumn == currentColumn)
                {
                    StartSwap();
                }
            }
        }

        private void MenuItem_Mix_Click(object sender, RoutedEventArgs e)
        {
            MixButtons();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            double offsetWidth = currentElement.ActualWidth / 10;
            double offsetHeight = currentElement.ActualHeight / 10;
            if (emptyRow == currentRow && emptyColumn + 1 == currentColumn)
            {
                currentElement.Margin = new Thickness(currentElement.Margin.Left - offsetWidth, currentElement.Margin.Top, currentElement.Margin.Right + offsetWidth, currentElement.Margin.Bottom);
            }
            if (emptyRow == currentRow && emptyColumn - 1 == currentColumn)
            {
                currentElement.Margin = new Thickness(currentElement.Margin.Left + offsetWidth, currentElement.Margin.Top, currentElement.Margin.Right - offsetWidth, currentElement.Margin.Bottom);
            }
            if (emptyRow + 1 == currentRow && emptyColumn == currentColumn)
            {
                currentElement.Margin = new Thickness(currentElement.Margin.Left, currentElement.Margin.Top - offsetHeight, currentElement.Margin.Right, currentElement.Margin.Bottom + offsetHeight);
            }
            if (emptyRow - 1 == currentRow && emptyColumn == currentColumn)
            {
                currentElement.Margin = new Thickness(currentElement.Margin.Left, currentElement.Margin.Top + offsetHeight, currentElement.Margin.Right, currentElement.Margin.Bottom - offsetHeight);
            }
            ticks++;
            if (ticks == 10)
            {
                StopSwap();
            }
        }

        private void MixButtons()
        {
            UIElement[] buttons = new UIElement[15] { b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15 };
            for (int j = 0; j < 100; j++)
            {
                UIElement[] cross = new UIElement[4];
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (emptyRow == Grid.GetRow(buttons[i]) && emptyColumn - 1 == Grid.GetColumn(buttons[i]))
                    {
                        cross[0] = buttons[i];
                    }
                    if (emptyRow == Grid.GetRow(buttons[i]) && emptyColumn + 1 == Grid.GetColumn(buttons[i]))
                    {
                        cross[1] = buttons[i];
                    }
                    if (emptyRow - 1 == Grid.GetRow(buttons[i]) && emptyColumn == Grid.GetColumn(buttons[i]))
                    {
                        cross[2] = buttons[i];
                    }
                    if (emptyRow + 1 == Grid.GetRow(buttons[i]) && emptyColumn == Grid.GetColumn(buttons[i]))
                    {
                        cross[3] = buttons[i];
                    }
                }
                int index = random.Next(4);
                while (cross[index] == null)
                {
                    index = random.Next(4);
                }
                while (previousIndex == 0 && index == 1 ||
                       previousIndex == 1 && index == 0 ||
                       previousIndex == 2 && index == 3 ||
                       previousIndex == 3 && index == 2)
                {
                    index = random.Next(4);
                    while (cross[index] == null)
                    {
                        index = random.Next(4);
                    }
                }
                previousIndex = index;
                currentRow = Grid.GetRow(cross[index]);
                currentColumn = Grid.GetColumn(cross[index]);
                Grid.SetRow(cross[index], emptyRow);
                Grid.SetColumn(cross[index], emptyColumn);
                emptyRow = currentRow;
                emptyColumn = currentColumn;
            }
        }

        private void StartSwap()
        {
            ticks = 0;
            timer.Start();
            moving = true;
        }

        private void StopSwap()
        {
            timer.Stop();
            Grid.SetRow(currentElement, emptyRow);
            Grid.SetColumn(currentElement, emptyColumn);
            int swapRow = currentRow;
            int swapColumn = currentColumn;
            currentRow = emptyRow;
            currentColumn = emptyColumn;
            emptyRow = swapRow;
            emptyColumn = swapColumn;
            currentElement.Margin = new Thickness(0, 0, 0, 0);
            moving = false;
            CheckForWin();
        }

        private void CheckForWin()
        {
            if (Grid.GetRow(b1) == 0 && Grid.GetColumn(b1) == 0 &&
                Grid.GetRow(b2) == 0 && Grid.GetColumn(b2) == 1 &&
                Grid.GetRow(b3) == 0 && Grid.GetColumn(b3) == 2 &&
                Grid.GetRow(b4) == 0 && Grid.GetColumn(b4) == 3 &&
                Grid.GetRow(b5) == 1 && Grid.GetColumn(b5) == 0 &&
                Grid.GetRow(b6) == 1 && Grid.GetColumn(b6) == 1 &&
                Grid.GetRow(b7) == 1 && Grid.GetColumn(b7) == 2 &&
                Grid.GetRow(b8) == 1 && Grid.GetColumn(b8) == 3 &&
                Grid.GetRow(b9) == 2 && Grid.GetColumn(b9) == 0 &&
                Grid.GetRow(b10) == 2 && Grid.GetColumn(b10) == 1 &&
                Grid.GetRow(b11) == 2 && Grid.GetColumn(b11) == 2 &&
                Grid.GetRow(b12) == 2 && Grid.GetColumn(b12) == 3 &&
                Grid.GetRow(b13) == 3 && Grid.GetColumn(b13) == 0 &&
                Grid.GetRow(b14) == 3 && Grid.GetColumn(b14) == 1 &&
                Grid.GetRow(b15) == 3 && Grid.GetColumn(b15) == 2)
            {
                MessageBox.Show("Вы выиграли!");
            }
        }
    }
}
