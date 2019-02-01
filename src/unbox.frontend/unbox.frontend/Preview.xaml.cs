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
using System.Windows.Shapes;
using x.common.WPF.Tools;

namespace unbox.frontend
{
    /// <summary>
    /// Interaction logic for Preview.xaml
    /// </summary>
    public partial class Preview : Window
    {
        public Preview()
        {
            InitializeComponent();
            var workArea = WindowTools.GetPrimaryScreen().WorkingArea;
            Left = WindowTools.ConvertPixelsToDiPixels(workArea.Right) - Width;
            Top = WindowTools.ConvertPixelsToDiPixels(workArea.Top);
            Height = WindowTools.ConvertPixelsToDiPixels(workArea.Height);
        }

        public Action<DateTime> ShowToast { get; set; }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.A)
            {
                var timeslot = ((MainViewModel) DataContext).TimeSlots.FirstOrDefault();
                if(timeslot != null)
                {
                    ShowToast?.Invoke(timeslot.ActualStartTime);
                };
            }
        }
    }
}
