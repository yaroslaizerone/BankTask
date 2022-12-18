using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace BankTask
{
    public partial class DepositCalculatorPage : Window
    {
        public DepositCalculatorPage()
        {
            InitializeComponent();
        }

        private void bt_compare_Click(object sender, RoutedEventArgs e)
        {
            //Подготовка данных к расчетам
            var stabilityValue = tbl_stab_result.Text;
            var optimalValue = tbl_opt_result.Text;
            var standartValue = tbl_standart_result.Text;

            double srok = Convert.ToDouble(tb_srok.Text);
            var totalSumma = Convert.ToDouble(tb_sum.Text);
            double stavkastab = 0.08;
            double stavkaoptimal = 0.05;
            double stavkastandart = 0.06;
            try
            {
                double stabotvet = totalSumma * Math.Pow((1 + stavkastab / 365), srok);
                double stabotvet1 = totalSumma * Math.Pow((1 + stavkaoptimal / 365), srok);
                double stabotvet2 = totalSumma * Math.Pow((1 + stavkastandart / 365), srok);
                ComparisonOfDepositsPage form = new ComparisonOfDepositsPage(stabilityValue, optimalValue, standartValue, stabotvet, stabotvet1, stabotvet2, srok, totalSumma);
                form.Show();
            }
            catch
            {
                MessageBox.Show("Error");
            }

        }

        private void sl_sum_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            NumberFormatInfo nfi = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalDigits = 0 };
            //параметр n отвечает за точность получаемого значения *.00 
            tb_sum.Text = ((Slider)sender).Value.ToString("n", nfi);
            valueCheange();
        }
        private void sl_srok_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            NumberFormatInfo nfi = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalDigits = 0 };
            //параметр n отвечает за точность получаемого значения *.00
            tb_srok.Text = ((Slider)sender).Value.ToString("n", nfi);
            valueCheange();
        }

        private void sl_popoln_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            NumberFormatInfo nfi = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalDigits = 0 };
            //параметр n отвечает за точность получаемого значения *.00
            tb_popoln.Text = ((Slider)sender).Value.ToString("n", nfi);
            valueCheange();
        }

        void valueCheange()
        {
            try
            {
                double summa = Convert.ToDouble(tb_sum.Text);
                double srok = Convert.ToDouble(tb_srok.Text);
                double popoln = Convert.ToDouble(tb_popoln.Text);
                double stavkaStability = 0.08;
                double stavkaOptimal = 0.05;
                double stavkaStandart = 0.06;
                double stabilitySumma = summa * stavkaStability * srok / 365;
                double optimalSumma = summa * stavkaOptimal * srok / 365;
                double standartSumma = summa * stavkaStandart * srok / 365;

                tbl_stab_result.Text = Convert.ToDecimal(stabilitySumma).ToString("#,##0 Руб.");
                tbl_opt_result.Text = Convert.ToDecimal(optimalSumma).ToString("#,##0 Руб.");
                tbl_standart_result.Text = Convert.ToDecimal(standartSumma).ToString("#,##0 Руб.");
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }
    }
}
