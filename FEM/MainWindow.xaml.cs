using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Windows;

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
            if (String.IsNullOrEmpty(txtT.Text) ||
                String.IsNullOrEmpty(txtS.Text) ||
                String.IsNullOrEmpty(txtF.Text) ||
                String.IsNullOrEmpty(txtQ.Text) ||
                String.IsNullOrEmpty(txtN.Text))
            {
                MessageBox.Show("Input cannot be empty");
                return;
            }

            int n = 0;
            double q = 0;

            if (!int.TryParse(txtN.Text, out n) ||
                !double.TryParse(txtQ.Text, out q))
            {
                MessageBox.Show("Failed to parse numeric values");
                return;
            }

            List<double> coefs;
            try
            {
                coefs = FemSolve.Coeficients(
                    txtT.Text,
                    txtS.Text,
                    txtF.Text,
                    double.Parse(txtQ.Text),
                    n);
            }
            catch (FunctionParseException ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

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
            foreach (var ax in chart.Model.Axes)
                ax.Maximum = ax.Minimum = Double.NaN;
            chart.ResetAllAxes();
            chart.InvalidatePlot();
        }
    }
}
