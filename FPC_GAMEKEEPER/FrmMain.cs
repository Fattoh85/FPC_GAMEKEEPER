using FPC.Helper;
using FPC.Model;
using FPC.Model.Database;
using FPC.Model.Enum;
using FPC.Model.Structure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using DbClassLibrary;
using FPC.Model.Crypto;
using Org.BouncyCastle.Crypto;
using FPC.Model.Structure.Receipt;
using System.Security.Cryptography;
using System.Reflection;



namespace FPC
{
    public partial class FrmMain : Form
    {
        //log4net.ILog log = FPC.Model.Logger.Logger.Get(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ThreadUtility _tu;
        private static readonly ILog log = LogManager.GetLogger(typeof(FrmMain));
        string filePath = "Files/sign.txt";

        Functions fn = new Functions(null);
        ModuleSettings _moduleSettings;
        User _user;
        public DateTime _appStartTime;
        private static Mutex mutex = null;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnOpenShift_Click(object sender, EventArgs e)
        {
           
        }

        private void btnDeviceStatus_Click(object sender, EventArgs e)
        {
            
        }

        


        private async void RequestAndDisplay(string url, string postData)
        {
            rtbResponse.Text = $"{DateTime.Now} REQUEST:\n{url}\n{postData}";
            ContentResult cr = await fn.SendRestfulRequest(url, postData);
            rtbResponse.Text = rtbResponse.Text + $"\n\n\n{DateTime.Now} RESPONSE:\nStatuscode:{cr.Statuscode}\n\nContent:\n{cr.Content}";
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
          


            try
            {
                // Получаем сборку текущего исполняемого файла
                Assembly assembly = Assembly.GetExecutingAssembly();

                // Получаем версию сборки
                Version version = assembly.GetName().Version;

                this.Text = "Fiscal Printer Module GAME KEEPER v" + version.ToString();


                string mbr = SystemInfoLib.GetMotherboardSn();
                string prc = SystemInfoLib.GetProcessorId();
                string hdd = SystemInfoLib.GetMacAndHdd();

                log.Debug($"Form1_Load|mbr={mbr}");
                log.Debug($"Form1_Load|prc={prc}");
                log.Debug($"Form1_Load|mhd={hdd}");

                cmbKey.Items.Add(mbr);
                cmbKey.Items.Add(prc);
                cmbKey.Items.Add(hdd);



                CheckAppIsRun(version.ToString());

                // Путь к файлу конфигурации JSON
                string configFileContent = await fn.GetFileContent("projects.json");

                log.Debug($"Form1_Load|configFileContent={configFileContent}");

                _moduleSettings = JsonConvert.DeserializeObject<ModuleSettings>(configFileContent);

                log.Debug($"Form1_Load|_moduleSettings={JsonConvert.SerializeObject(_moduleSettings)}");
            }
            catch (Exception ex) {
                log.Debug($"Form1_Load|ex={ex}");
            }

           

            LicenseLoad();
        }

        static void CheckAppIsRun(string version)
        {
            string mutexName = $"FPC_v{version.ToString()}p";
            //const string mutexName = "FPC";

            // Попытка создания мьютекса
            mutex = new Mutex(true, mutexName, out bool createdNew);

            // Если мьютекс уже существует, приложение закрывается
            if (!createdNew)
            {
                MessageBox.Show("Приложение уже запущено!");
                Application.Exit();
                return;
            }

            

            // Освобождение мьютекса после завершения работы приложения
            GC.KeepAlive(mutex);
        }



        public async void Stmf()
        {
            timer2.Interval = 30000;
            log.Debug($"Form1_Load|App started");

            DateTime _appStartTime = DateTime.Now;

            fn.CreateDirectoryWithCurrentDate();

           

            

            try
            {
                 _appStartTime = _moduleSettings.flibber ? DateTime.Now.AddDays(-1) : DateTime.Now;
            }
            catch (Exception ex) {
                 _appStartTime =  DateTime.Now;
                log.Error($"Form1_Load|flibber not set");
            }

           

            Functions.tranCheckDelay = _moduleSettings.tranCheckDelay;
            Functions.printerCheckDelay = _moduleSettings.printerCheckDelay;

            UpdateUsers();

            // Вызываем функцию в отдельном потоке
            Task myTask = Task.Run(() => fn.CheckNewTransactionToPrint(_moduleSettings, _appStartTime));

            // Вызываем функцию в отдельном потоке
            Task myTask2 = Task.Run(() => fn.CheckDeviceStatuses(_moduleSettings));
        }
       

       

       

        private void CmbPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUsers();
        }

        

        private void CmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            _user = cmbUsers.SelectedItem as User;
        }

        private void btnCloseShift_Click(object sender, EventArgs e)
        {
            
        }

        private void btnGetLastFD_Click(object sender, EventArgs e)
        {
            
        }

        private void btnGetFDByNumber_Click(object sender, EventArgs e)
        {
            
        }

        private void btnGetXReport_Click(object sender, EventArgs e)
        {
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Functions.WsStates.Count > 0)
                {
                    lsbWsStatuses.Items.Clear();
                    foreach (var ws in Functions.WsStates)
                    {
                        lsbWsStatuses.Items.Add($"{DateTime.Now}  {ws.Value}  ({ws.Key.Ip}) ");
                    }
                }
            }
            catch
            { 
            
            }
            
        }

        private void cmbUsers_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            _user = cmbUsers.SelectedItem as User;
        }

        private void UpdateUsers()
        {
            
            if (_moduleSettings != null)
            {
                cmbUsers.DisplayMember = "Name";  // Отображаемое свойство
                cmbUsers.ValueMember = "Name";    // Значение (может быть Name)
                cmbUsers.DataSource = _moduleSettings.Users;

                cmbUsers.SelectedIndexChanged += CmbUsers_SelectedIndexChanged;
                if (_moduleSettings.Users != null && _moduleSettings.Users.Count > 0)
                {
                    _user = _moduleSettings.Users[0]; // Set _user to the first user by default
                }
                else
                {
                    _user = null;
                }
            }
            else
            {
                cmbUsers.DataSource = null;
                _user = null;
            }
        }

        private void btnDrawer_Click(object sender, EventArgs e)
        {
           
            if (_moduleSettings.status == true)
            {
                var url = _moduleSettings.Ip + "/api/openDrawer";
                var postData = fn.GetPostdata(RequestType.OPEN_DRAWER, false);
                RequestAndDisplay(url, postData);
            }
            
        }


        #region KEY
        public static string ReadFileContent(string filePath)
        {
            try
            {
                return File.Exists(filePath) ? File.ReadAllText(filePath) : null;
            }
            catch (Exception ex)
            {
                log.Error($"Error reading file: {ex.Message}");
                return null;
            }
        }

       

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

       

        public void Shmf()
        {
            panelMain.Visible = true;
            panelKey.Visible = false;
            Stmf();
        }

        private bool LicenseLoad(bool loadSettings = true)
        {
            /*panelKey.Visible = true;
            panelMain.Visible = false;*/

            string strToSign_MacAndHdd = SystemInfoLib.GetMacAndHdd();
            string strToSign_MotherboardSn = SystemInfoLib.GetMotherboardSn();
            string strToSign_ProcessorId = SystemInfoLib.GetProcessorId();

            cmbKey.Text = strToSign_MotherboardSn;

            string fileContent = ReadFileContent(filePath);

            if (fileContent != null)
            {
                if (fn.Vsval(strToSign_MacAndHdd, fileContent) || fn.Vsval(strToSign_MotherboardSn, fileContent) || fn.Vsval(strToSign_ProcessorId, fileContent))
                {
                    if (loadSettings) {
                        Shmf();
                    } 
                    return true;
                }
            }
            return false;
        }

       


        #endregion

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (LicenseLoad(false) == false)
            {
                MessageBox.Show("Ошибка лицензии");
                Application.Exit();
            }    
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string fileContent = ReadFileContent(filePath);

            string strToSign_MacAndHdd = SystemInfoLib.GetMacAndHdd();
            string strToSign_MotherboardSn = SystemInfoLib.GetMotherboardSn();
            string strToSign_ProcessorId = SystemInfoLib.GetProcessorId();

            string sign = fileContent != null ? fileContent : rtbKey.Text;

            if (fn.Vsval(strToSign_MacAndHdd, sign) || fn.Vsval(strToSign_MotherboardSn, sign) || fn.Vsval(strToSign_ProcessorId, sign))
            {
                if (string.IsNullOrEmpty(fileContent))
                {
                    fn.SaveFileContent(filePath, rtbKey.Text);
                }

                Shmf();
            }
            else
            {
                MessageBox.Show("Не верная подпись");
            }
        }

        private void panelKey_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnPaste_Click_1(object sender, EventArgs e)
        {
            rtbKey.Text = Clipboard.GetText();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Clipboard.SetText(cmbKey.Text);
        }

        private void btnDeviceStatus_Click_1(object sender, EventArgs e)
        {
            if (_moduleSettings.status == true)
            {
                var url = fn.GetUrl(_moduleSettings.Ip, RequestType.DEVICE_STATUS);
                string postData = fn.GetPostdata(RequestType.DEVICE_STATUS, true);
                RequestAndDisplay(url, postData);
            }
        }

        private void btnOpenShift_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show($"Вы действительно хотите открыть смену для {_user.Name}  ?", "Открытие смены", MessageBoxButtons.YesNo); ;
            if (dialogResult == DialogResult.Yes)
            {
                if (_moduleSettings.status == true)
                {
                    var url = fn.GetUrl(_moduleSettings.Ip, RequestType.OPEN_SHIFT);
                    var postData = fn.GetPostdata(RequestType.OPEN_SHIFT, true);
                    RequestAndDisplay(url, postData);
                }

            }
        }

        private void btnCloseShift_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show($"Вы действительно хотите закрыть смену для {_user.Name} ?", "Закрытие смены", MessageBoxButtons.YesNo); ;
            if (dialogResult == DialogResult.Yes)
            {
                if (_moduleSettings.status == true)
                {
                    var url = fn.GetUrl(_moduleSettings.Ip, RequestType.CLOSE_SHIFT);
                    string postData = fn.GetPostdata(RequestType.CLOSE_SHIFT, true);

                    RequestAndDisplay(url, postData);
                }

            }
        }

        private void btnGetLastFD_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show($"Вы действительно хотите отправить на для печать последний фискальный документ?", "Печать последнего фискального документа", MessageBoxButtons.YesNo); ;
            if (dialogResult == DialogResult.Yes)
            {
                if (_moduleSettings.status == true)
                {
                    var url = fn.GetUrl(_moduleSettings.Ip, RequestType.PRINT_LAST_FD);
                    string postData = fn.GetPostdata(RequestType.PRINT_LAST_FD, true);
                    RequestAndDisplay(url, postData);
                }
            }
        }

        private void btnGetXReport_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show($"Вы действительно хотите отправить на для печать X-отчет ?", "Печать отчета", MessageBoxButtons.YesNo); ;
            if (dialogResult == DialogResult.Yes)
            {
                if (_moduleSettings.status == true)
                {
                    var url = _moduleSettings.Ip + "/api/getXReport";
                    var postData = fn.GetPostdata(RequestType.GET_X_REPORT, true);
                    RequestAndDisplay(url, postData);
                }
            }
        }

        private void btnGetFDByNumber_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show($"Вы действительно хотите отправить на для печать фискальный документ с  номером: {txtFdNumber.Text} ?", "Печать фискального документа", MessageBoxButtons.YesNo); ;
            if (dialogResult == DialogResult.Yes)
            {
                if (_moduleSettings.status == true)
                {
                    var url = fn.GetUrl(_moduleSettings.Ip, RequestType.PRINT_FD_BY_NUMBER);
                    var postData = fn.GetPostdata(RequestType.PRINT_FD_BY_NUMBER, true, null, txtFdNumber.Text);
                    RequestAndDisplay(url, postData);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmSettings frmSettings = new FrmSettings();
            frmSettings.Show();
        }
    }
}
