using System.Windows;

namespace BankTask
{
    /// <summary>
    /// Логика взаимодействия для IncomePage.xaml
    /// </summary>
    public partial class IncomePage : Window
    {
        public IncomePage()
        {
            InitializeComponent();
        }

        private void bt_income_Click(object sender, RoutedEventArgs e)
        {
            DepositCalculatorPage form = new DepositCalculatorPage();
            form.Show();
        }
    }
}
