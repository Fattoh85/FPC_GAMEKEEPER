using FPC.Helper;
using FPC.Model.Enum;
using FPC.Model;
using FPC.Model.Structure;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FPC
{
    public partial class FrmSettings : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FrmMain));
        string filePath = "Files/sign.txt";
        Functions fn = new Functions(null);
        ModuleSettings _moduleSettings;

        public FrmSettings()
        {
            InitializeComponent();
        }

        private async void FrmSettings_Load(object sender, EventArgs e)
        {
            // Путь к файлу конфигурации JSON
            string configFileContent = await fn.GetFileContent("projects.json");

            log.Debug($"Form1_Load|configFileContent={configFileContent}");

            _moduleSettings = JsonConvert.DeserializeObject<ModuleSettings>(configFileContent);

            log.Debug($"Form1_Load|_moduleSettings={JsonConvert.SerializeObject(_moduleSettings)}");

            
            txtChTranDelay.Text = _moduleSettings.tranCheckDelay.ToString();
            txtChecStateDelay.Text = _moduleSettings.printerCheckDelay.ToString();
            txtPrinterPath.Text = _moduleSettings.Ip;

            // Assuming moduleSettings is your deserialized object
            foreach (var vat in _moduleSettings.vatCodeList)
            {
                comboBoxVatCodes.Items.Add(vat.key);
            }

            // Preselect the first item (optional)
            if (comboBoxVatCodes.Items.Count > 0)
            {
                comboBoxVatCodes.SelectedIndex = 0;
            }

            // Populate the second combobox with fixed options
            comboBoxCommodity.Items.AddRange(new string[] { "GOODS", "SERVICE", "JOB", "ADVANCE" });
            comboBoxCommodity.Text = _moduleSettings.commodity;
            comboBoxCommodity.SelectedItem = _moduleSettings.commodity; // Set the initial selection
            comboBoxVatCodes.SelectedItem = _moduleSettings.vatCode;


            lblDevStatus.Text = await CheckStatusDevice(_moduleSettings);
            Task<bool> dbConnected = ChekcDbConnection(_moduleSettings.connectionString);

            
        }

        private async Task<string> CheckStatusDevice(ModuleSettings moduleSettings)
        {
            

            string url = fn.GetUrl(moduleSettings.Ip, RequestType.DEVICE_STATUS);
            
            string postData = fn.GetPostdata(RequestType.DEVICE_STATUS, false);
            ContentResult cr = await fn.SendRestfulRequest(url, postData);

            if (cr.Statuscode == HttpStatusCode.OK)
            {
                return "Доступен";
            }
            else
            {
                return "Не доступен";
            }
        }


        private void txtChTranDelay_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetConnStr();
        }

        public  async void GetConnStr()
        {
            // Открываем диалоговое окно выбора папки
            using (var folderDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    string folderPath = folderDialog.SelectedPath;
                    string filePath = Path.Combine(folderPath, "IdeaBlade.ibconfig");

                    // Проверяем существование файла
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            // Загружаем XML файл
                            XDocument doc = XDocument.Load(filePath);

                            // Извлекаем данные из тега <connection>
                            var connectionElement = doc.Descendants("rdbKey").FirstOrDefault()?.Element("connection");
                            if (connectionElement != null)
                            {
                                string connectionString = connectionElement.Value;

                                _moduleSettings.connectionString = connectionString;

                                MessageBox.Show("Connection String: " + connectionString);

                                lblTimeDiff.Text = "Подождите...";
                                lblDbConState.Text = "Подождите...";

                                bool dbConnected = await ChekcDbConnection(connectionString);

                                
                            }
                            else
                            {
                                MessageBox.Show("Тег <connection> не найден.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при чтении файла: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Файл IdeaBlade.ibconfig не найден в выбранной папке.");
                    }
                }
                else
                {
                    Console.WriteLine("Выбор папки отменён.");
                }
            }

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            //Console.ReadKey();
        }

        private async Task<bool> ChekcDbConnection(string connectionString)
        {
            try
            {
                lblTimeDiff.Text = "Идет соединение...";
                lblDbConState.Text = "Идет соединение...";

                // Используем SqlConnectionStringBuilder для разбора строки подключения
                var builder = new SqlConnectionStringBuilder(connectionString);

                // Извлекаем значения
                string dataSource = builder.DataSource;
                string initialCatalog = builder.InitialCatalog;

                // Извлекаем название базы данных
                string databaseName = ExtractDatabaseNameFromConnectionString(connectionString);
                if (!string.IsNullOrEmpty(databaseName))
                {
                    txtDbName.Text = databaseName;
                }
                else
                {
                    MessageBox.Show("Не удалось извлечь название базы данных.");
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Получаем время с сервера MSSQL
                    SqlCommand command = new SqlCommand("SELECT GETDATE()", connection);
                    DateTime serverTime = (DateTime)command.ExecuteScalar();

                    // Получаем текущее время на локальной машине
                    DateTime localTime = DateTime.Now;

                    // Сравниваем время на сервере и локальной машине
                    TimeSpan timeDifference = localTime - serverTime;

                    // Выводим разницу во времени в минутах
                    int differenceInMinutes = (int)timeDifference.TotalMinutes;

                    MessageBox.Show($"Разница во времени: {differenceInMinutes} минут");

                    lblTimeDiff.Text = $"{serverTime} (сервер) | {localTime} (локально)";


                    MessageBox.Show($"Соединение установлено: {dataSource} | {initialCatalog} ");

                    lblDbConState.Text = $"Соединение установлено: {dataSource} | {initialCatalog}";
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка " + ex.Message);
                lblDbConState.Text = $"Ошибка {ex.Message}";
                lblTimeDiff.Text = Text = $"Ошибка {ex.Message}";
                return false;
            }
        }

        // Метод для извлечения названия базы данных
        static string ExtractDatabaseNameFromConnectionString(string connectionString)
        {
            // Разбиваем строку подключения на части и ищем элемент с "Initial Catalog="
            var parts = connectionString.Split(';');
            foreach (var part in parts)
            {
                if (part.Trim().StartsWith("Initial Catalog=", StringComparison.OrdinalIgnoreCase) || part.Trim().StartsWith("database=", StringComparison.OrdinalIgnoreCase)  )
                {
                    return part.Split('=')[1].Trim();
                }
            }
            return null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string filePath = "Files/projects.json";

            _moduleSettings.tranCheckDelay      = Convert.ToInt32(txtChTranDelay.Text);
            _moduleSettings.printerCheckDelay   = Convert.ToInt32(txtChecStateDelay.Text);
            _moduleSettings.Ip                  = txtPrinterPath.Text;
            _moduleSettings.dbName              = txtDbName.Text;

            _moduleSettings.vatCode             = comboBoxVatCodes.Text;

            foreach (var item in _moduleSettings.vatCodeList)
            {
                if (item.key == comboBoxVatCodes.Text)
                {
                    item.value = Convert.ToInt32(Convert.ToDecimal(txtNdsValue.Text));
                }
            }

            _moduleSettings.commodity           = comboBoxCommodity.Text;



            string content = JsonConvert.SerializeObject(_moduleSettings, Formatting.Indented);

            if (fn.SaveFileContent(filePath, content))
            {
                MessageBox.Show($"Сохранен");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxVatCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedVatCode = comboBoxVatCodes.SelectedItem.ToString();

            // Find the corresponding value in the vatCodeList
            var selectedVat = _moduleSettings.vatCodeList.FirstOrDefault(v => v.key == selectedVatCode);

            if (selectedVat != null)
            {
                txtNdsValue.Text = selectedVat.value.ToString();
            }
        }

        private void comboBoxCommodity_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCommodity = comboBoxCommodity.SelectedItem.ToString();
            _moduleSettings.commodity = selectedCommodity; // Optionally store the selection in your data
        }

        private async void label1_Click(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            lblDevStatus.Text = "Идет соединение";


            lblDevStatus.Text = await CheckStatusDevice(_moduleSettings);




        }
    }
}
