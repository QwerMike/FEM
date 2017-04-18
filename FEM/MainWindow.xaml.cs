using OxyPlot.Series;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FEM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            var n = int.Parse(txtN.Text);
            var coefs = FemSolve.Coeficients(
                txtT.Text,
                txtS.Text,
                txtF.Text,
                double.Parse(txtQ.Text),
                n);

            updateChart(coefs);
        }

        private void updateChart(List<double> coefs)
        {
            chart.Model.Series.Clear();
            LineSeries series = new LineSeries();
            int n = coefs.Count - 1;
            for (int i = 0; i < n + 1; i++)
            {
                series.Points.Add(new OxyPlot.DataPoint(i / (double)n, coefs[i]));
            }
            chart.Model.Series.Add(series);
            chart.InvalidatePlot();
        }
    }
}
