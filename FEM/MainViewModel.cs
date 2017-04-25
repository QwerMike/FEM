using OxyPlot;
using OxyPlot.Series;

namespace FEM
{
    class MainViewModel
    {
        public MainViewModel()
        {
            this.MyModel = new PlotModel();
            MyModel.Series.Add(new AreaSeries());
        }

        public PlotModel MyModel { get; private set; }
    }
}
