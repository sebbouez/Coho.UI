using System.Windows;
using System.Windows.Controls;

namespace Test
{
    /// <summary>
    /// Logique d'interaction pour Backstage.xaml
    /// </summary>
    public partial class Backstage : UserControl
    {
        public Backstage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Global.AppWindow.HideBackstageView();
        }
    }
}
