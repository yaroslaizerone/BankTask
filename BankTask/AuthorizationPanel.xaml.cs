using BankTask.Module;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace BankTask
{
    public partial class AuthorizationPanel : Window
    {
        private string name;
        private string dohod;
        private string stavka;
        private string summa;
        private string srokkredita;
        private string summavkladas;
        private double sum;
        private double proc;
        public AuthorizationPanel(string names, string dohods, string stavkas, string summas, double srokkredits, double summavklada, double prochentStavka)
        {
            InitializeComponent();
            name = names;
            dohod = dohods;
            stavka = stavkas;
            summa = summas;
            sum = summavklada;
            proc = prochentStavka;
            srokkredita = Convert.ToString(srokkredits);
            summavkladas = Convert.ToDecimal(summavklada).ToString("#,##0 Руб.");
        }
        private readonly string TemplateFileName = @"C:\Users\kolpa\Downloads\Word.docx";//таков путь

        private void btn_voity_Click(object sender, RoutedEventArgs e)
        {
            string login = tb_login.Text;
            string password = tb_password.Text;
            Entities1 models = new Entities1();
            var authorization = models.User;
            var contract = models.Contract;
            var bank = models.BankAccount;
            Helper helperCreate = new Helper();

            var idcontract = contract.OrderByDescending(x => x.IDContract).First().IDContract;
            var contractid = Convert.ToString(idcontract + 1);

            try
            {
                var user = authorization.Where(x => x.Login == login && x.Password == password).FirstOrDefault();
                if (user != null)
                {
                    MessageBox.Show("Авторизация выполнена успешно");
                    int IDUser = models.User.Where(x => x.Login == login && x.Password == password).First().IDUser;
                    var bankaccount = bank.Where(x => x.IDUser == IDUser).First().NumberAccount;
                    var accountbank = Convert.ToString(bankaccount);
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
                    string formatted = result.ToString("yyyy-MM-dd");

                    Contract contractForBase = new Contract
                    {
                        IDContract = idcontract + 1,
                        NumberAccount = bankaccount,
                        IDUser = IDUser,
                        Amount = sum,
                        Period = diff,
                        ExpirationDate = formatted,
                        Percet = proc
                    };
                    Helper.getContext();
                    Helper.Create(contractForBase);

                    var wordApp = new Word.Application();//переменная для word
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


                        wordDocument.SaveAs2(@"C:\Users\kolpa\Downloads\Word1.docx");//сохроняем наш документ
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
                MessageBox.Show("Фатальная ошибка");
            }



            void ReplaceWordsStub(string stubToReplace, string text, Word.Document wordDocument)
            {
                var range = wordDocument.Content;//перменная для хранения данных документа
                range.Find.ClearFormatting();//метод сброса всех натстроек текста
                range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);//находим ключевые слова и заменяем их
            }
        }
    } 
}

