using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace FEM
{
    class MainViewModel
    {
        public MainViewModel()
        {
            this.MyModel = new PlotModel();

            MyModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                AbsoluteMinimum = 0,
                AbsoluteMaximum = 1,
                IsPanEnabled = true,
                IsZoomEnabled = false
            });

            MyModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                IsPanEnabled = true,
                IsZoomEnabled = false
            });

            //MyModel.Series.Add(new AreaSeries());
        }

        public PlotModel MyModel { get; private set; }
    }
}
