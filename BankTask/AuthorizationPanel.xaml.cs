using BankTask.Module;
using System;
using System.Linq;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace BankTask
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AuthorizationPanel : Window
    {
        private string name;
        private string dohod;
        private string stavka;
        private string summa;
        private string srokkredita;
        private string summavkladas;
        public AuthorizationPanel(string names, string dohods, string stavkas, string summas, double srokkredits, double summavklada)
        {
            InitializeComponent();
            name = names;
            dohod = dohods;
            stavka = stavkas;
            summa = summas;
            srokkredita = Convert.ToString(srokkredits);
            summavkladas = Convert.ToDecimal(summavklada).ToString("#,##0 Руб.");          
        }
        private readonly string TemplateFileName = @"D:\Download\Word.docx";//таков путь

        private void btn_voity_Click(object sender, RoutedEventArgs e)
        {
            string login = tb_login.Text;
            string password = tb_password.Text;
            Entities model = new Entities();
            var authorization = model.User;
            var contract = model.Contract;
            var bank = model.BankAccount;
            Helper helperCreate = new Helper();

            var idcontract = contract.OrderByDescending(x => x.IDContract).First().IDContract;
            var contractid = Convert.ToString(idcontract + 1);

            var bankaccount = bank.OrderByDescending(x => x.NumberAccount).First().NumberAccount;
            var accountbank = Convert.ToString(bankaccount + 1);

            try
            {
                var user = authorization.Where(x => x.Login == login && x.Password == password).FirstOrDefault();
                if (user != null)
                {
                    string surname = user.Surname;
                    string nameuser = user.Name;
                    string patronymic = user.Patronymic;
                    string seriespasport = user.Series;
                    string numberpassport = user.Number;
                    string passportotdel = user.Issued;
                    string address = user.Adress;
                    string birth = Convert.ToString(user.DateOfBirth);
                    string email = user.E_Mail;
                    string mapbirth = user.PlaceOfBirth;
                    string date = DateTime.Now.ToString("dd");
                    string month = DateTime.Now.ToString("MM");
                    string year = DateTime.Now.ToString("yyyy");

                    DateTime d1 = DateTime.Now;
                    int diff = Convert.ToInt32(srokkredita);
                    DateTime result = d1.AddDays(diff);
                    string formatted = result.ToString("dd-MM-yyyy");


                    MessageBox.Show("Авторизация выполнена успешно");

                    var wordApp = new Word.Application();//переменная для word
                    wordApp.Visible = false;//word скрыт
                    try
                    {
                        var wordDocument = wordApp.Documents.Open(TemplateFileName);//переменная для хранения нашего документа

                        //Вставка вмето специальных выражений в нашем файле
                        ReplaceWordsStub("{date}", date, wordDocument);
                        ReplaceWordsStub("{month}", month, wordDocument);
                        ReplaceWordsStub("{year}", year, wordDocument);

                        ReplaceWordsStub("{contractid}", contractid, wordDocument);

                        ReplaceWordsStub("{accountbank}", accountbank, wordDocument);

                        ReplaceWordsStub("{dateend}", formatted, wordDocument);

                        ReplaceWordsStub("{birth}", birth, wordDocument);

                        ReplaceWordsStub("{srokkredita}", srokkredita, wordDocument);

                        ReplaceWordsStub("{name}", name, wordDocument);

                        ReplaceWordsStub("{surname}", surname, wordDocument);

                        ReplaceWordsStub("{nameuser}", nameuser, wordDocument);

                        ReplaceWordsStub("{patronymic}", patronymic, wordDocument);

                        ReplaceWordsStub("{surname}", surname, wordDocument);

                        ReplaceWordsStub("{nameuser}", nameuser, wordDocument);

                        ReplaceWordsStub("{patronymic}", patronymic, wordDocument);

                        ReplaceWordsStub("{seriespasport}", seriespasport, wordDocument);

                        ReplaceWordsStub("{numberpassport}", numberpassport, wordDocument);

                        ReplaceWordsStub("{passportotdel}", passportotdel, wordDocument);

                        ReplaceWordsStub("{address}", address, wordDocument);

                        ReplaceWordsStub("{email}", email, wordDocument);

                        ReplaceWordsStub("{mapbirth}", mapbirth, wordDocument);

                        ReplaceWordsStub("{kafedra}", dohod, wordDocument);

                        ReplaceWordsStub("{stavka}", stavka, wordDocument);

                        ReplaceWordsStub("{groupe}", summavkladas, wordDocument);


                        wordDocument.SaveAs2(@"D:\Download\Word1.docx");//сохроняем наш документ
                        wordDocument.Close();//закрываем документ


                    }
                    catch
                    {
                        MessageBox.Show("Произошла ошибка!!!");//окно ошибки
                    }
                    finally
                    {
                        wordApp.Quit();//закрываем word
                    }
                }
                else
                {
                    MessageBox.Show("Кооректно введите логин и пароль");
                }
            }
            catch
            {
                MessageBox.Show("Фотальная ошибка");
            }
            /*try
            {
                Contract objectContract = new Contract { IDContract = Convert.ToInt32(contractid), NumberAccount = Convert.ToInt32(accountbank), IDUser = helperCreate.GetLastIDStaff(), E_mail = emailUser, Сonfirmed = "F", ID_Staff = id_staff };
                helperCreate.CreateUser(objectContract);
            }
            catch
            {
                MessageBox.Show("Не удалось записать данные в базу");
            }*/
        }
        /// <summary>
        /// Метод замены ключевых слов на данные
        /// </summary>
        /// <param name="stubToReplace">Ключевые слова</param>
        /// <param name="text">Текст, который заменяет ключевые слова</param>
        /// <param name="wordDocument">Наш документ</param>
        private void ReplaceWordsStub(string stubToReplace, string text, Word.Document wordDocument)
        {
            var range = wordDocument.Content;//перменная для хранения данных документа
            range.Find.ClearFormatting();//метод сброса всех натстроек текста
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);//находим ключевые слова и заменяем их
        }
    }
}

