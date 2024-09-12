
using FirebirdSql.Data.FirebirdClient;
using FPC.Model;
using FPC.Model.Crypto;
using FPC.Model.Database;
using FPC.Model.Enum;
using FPC.Model.Structure;
using FPC.Model.Structure.Receipt;
using FPC.Model.Structure.Receipt.Responce;
using FPC.Model.Structure.Trans;
using log4net;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using User = FPC.Model.Structure.User;


namespace FPC.Helper
{
    internal class Functions
    {
        log4net.ILog log = FPC.Model.Logger.Logger.Get(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ThreadUtility _tu;
        DbClassLibrary.DbLibClass lib = new DbClassLibrary.DbLibClass(null);
        public static int tranCheckDelay = 1000;
        public static int printerCheckDelay = 1000;
        string filePath = "Files/sign.txt";

        public Functions(ThreadUtility tu)
        {
            log = LogManager.GetLogger(typeof(Functions));
            _tu = tu!=null ?  tu : new ThreadUtility();
        }


        public static Dictionary<ModuleSettings, string> WsStates = new Dictionary<ModuleSettings, string>();

        internal async Task<string> GetFileContent(string filename)
        {
            log.Debug($"{_tu.GetCurrentMethod()}|started");

            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Files\" + filename;
            
            log.Debug($"{_tu.GetCurrentMethod()}|filePath={filePath}");

            try
            {
                // Read text from file
                string jsonString = File.ReadAllText(filePath);
                log.Error($"{_tu.GetCurrentMethod()}|jsonString={jsonString}");
                return jsonString;

            }
            catch (FileNotFoundException)
            {
                log.Error($"{_tu.GetCurrentMethod()}|File {filePath} not found.");
            }
            catch (Exception ex)
            {
                log.Error($"{_tu.GetCurrentMethod()}|An error occurred: {ex.Message}");
            }

            return "";
        }

        internal bool SaveRatesInLocalFile(string json, string filename)
        {
            try
            {
                // Получаем путь к текущей директории + путь к файлу
                string filePath = AppDomain.CurrentDomain.BaseDirectory + $"/Files/{filename}.txt";

                // Создаем или перезаписываем файл
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Записываем JSON в файл
                    writer.WriteLine(json);
                }

                //_log.Debug($"{_tu.GetCurrentMethod()}|Данные успешно сохранены в файл: {filePath}");
                return true;
            }
            catch (Exception ex)
            {
                //_log.Debug($"{_tu.GetCurrentMethod()}|Ошибка при сохранении данных в файл: {ex.Message}");
                return false;
            }
        }


        public async Task<ContentResult> SendRestfulRequest(string url, string postData)
        {
            log.Debug($"{_tu.GetCurrentMethod()}|{_tu.GetCurrentMethod()}|SendRestfulRequest|url:" + url + "|postData:" + postData);

            ContentResult contentResult = new ContentResult();

            if (url.Length > 0)
            {
                using (var client = new HttpClient())
                {
                    // Set the timeout for the HttpClient
                    client.Timeout = TimeSpan.FromSeconds(10); // Adjust the timeout value as needed

                    var content = new StringContent(postData, Encoding.UTF8, "application/json");

                    try
                    {
                        var response = await client.PostAsync(url, content);
                        response.EnsureSuccessStatusCode();
                        var responseBody = await response.Content.ReadAsStringAsync();
                        contentResult.Content = responseBody;
                        contentResult.Statuscode = response.StatusCode;

                        log.Debug($"{_tu.GetCurrentMethod()}|url:" + url + "|postData:" + postData + "|responseBody:" + responseBody);
                        return contentResult;
                    }
                    catch (HttpRequestException e)
                    {
                        contentResult.Content = $"Request error: {e.Message}";
                        contentResult.Statuscode = HttpStatusCode.InternalServerError;

                        log.Debug($"{_tu.GetCurrentMethod()}|url:" + url + "|postData:" + postData + "|ex:" + e.ToString());
                        return contentResult;
                    }
                    catch (TaskCanceledException e) when (e.CancellationToken == default)
                    {
                        contentResult.Content = "Request timed out.";
                        contentResult.Statuscode = HttpStatusCode.RequestTimeout;

                        log.Debug($"{_tu.GetCurrentMethod()}|url:" + url + "|postData:" + postData + "|ex:" + e.ToString());
                        return contentResult;
                    }
                }
            }
            else
            {
                log.Debug($"{_tu.GetCurrentMethod()}|invalid url:");
                return contentResult;
            }

            
        }


        // Task проверки транзакций
        internal async void CheckNewTransactionToPrint(ModuleSettings moduleSettings, DateTime _appStartTime)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(moduleSettings.connectionString))
                {
                    connection.Open();

                    while (true)
                    {
                        _tu = new ThreadUtility();

                        log.Debug($"{_tu.GetCurrentMethod()}|stated");

                        // Получение списка трназакций
                        List<Transaction> transactions = lib.GetTransactionListFirebirdSql(connection, moduleSettings, _appStartTime);

                        log.Debug($"{_tu.GetCurrentMethod()}|Транзвкций:{transactions.Count}");


                        foreach (Transaction transaction in transactions)
                        {
                            try
                            {
                                long receiptSum = 0;
                                long receiptCash = 0;
                                string tenderName = "";
                                int itemType = 1;
                                long item_serviceprice = 0;
                                long item_serviceRate = 0;

                                // Получение списка продуктов
                                List<Product> products = lib.GetProductsListFirebirdSql(connection, moduleSettings, transaction, ref receiptSum, ref receiptCash, ref tenderName, ref itemType, ref item_serviceprice, ref item_serviceRate);

                                log.Debug($"{_tu.GetCurrentMethod()}|tran={transaction.trans_oid}|Продуктов:{products.Count}|Отправка на принтер");

                                if (item_serviceprice > 0)
                                {
                                    Product serviceFee = GetServiseFee(transaction, moduleSettings, item_serviceprice, item_serviceRate);
                                    products.Add(serviceFee);

                                    receiptSum += item_serviceprice;
                                    receiptCash += item_serviceprice;
                                }

                                // Отправка на принтер чека
                                bool print_resp = await SendFormReceipt(moduleSettings, transaction, products, receiptSum, receiptCash, tenderName, itemType);

                                if (print_resp)
                                {
                                    log.Debug($"{_tu.GetCurrentMethod()}|tran={transaction.trans_oid}|Распечатан, сохранение статуса в базе");

                                    // Открытие денежного ящика 
                                    OpenDrawer(moduleSettings);

                                    log.Debug($"{_tu.GetCurrentMethod()}|tran={transaction.trans_oid}|finished=");
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Debug($"{_tu.GetCurrentMethod()}|" + ex.ToString());
                                if (ex.ToString().Contains("FirebirdSql.Data.FirebirdClient.FbException"))
                                {
                                    try
                                    {
                                        connection.Open();
                                    }
                                    catch (Exception exReconect)
                                    {
                                        log.Debug($"{_tu.GetCurrentMethod()}|exReconect=" + exReconect.ToString());
                                        MessageBox.Show("Ошибка получения данных. Перезапустите модуль");
                                        Application.Exit();
                                    }
                                }
                            }

                            // Сохранить id  в знак успешной печати
                            bool dbFiscalPrintedStatus = CreateFile(transaction.trans_oid);

                            if (dbFiscalPrintedStatus)
                            {
                                log.Debug($"{_tu.GetCurrentMethod()}|tran={transaction.trans_oid}|Cохранен в базе");
                            }
                            else
                            {
                                log.Debug($"{_tu.GetCurrentMethod()}|tran={transaction.trans_oid}|Не сохранен в базе");
                                MessageBox.Show("ошибка сохранения файла");
                                
                                Application.Exit();
                            }
                        }

                        log.Debug($"{_tu.GetCurrentMethod()}|finished|waiting...");
                        Task.Delay(tranCheckDelay * 1000).Wait();
                    }
                }
            }
            catch (Exception e)
            {
                log.Debug($"{_tu.GetCurrentMethod()}|ex={e}");
            }
        }

        private Product GetServiseFee(Transaction transaction, ModuleSettings moduleSettings, long item_serviceprice, long item_serviceRate)
        {
            Product serviceFee = new Product();

            serviceFee.quantity = 1;
            serviceFee.name = "\n\n----------------------------------------------\nОбслуживание: " + item_serviceRate + "%";
            serviceFee.price = item_serviceprice;
            serviceFee.quantity = 1;
            serviceFee.commodity = moduleSettings.commodity;
            serviceFee.vatCode = moduleSettings.vatCode;
            serviceFee.sum = serviceFee.price;

            return serviceFee;
        }

        private async void OpenDrawer(ModuleSettings moduleSettings)
        {
            var url = moduleSettings.Ip + "/api/openDrawer";
            var postData = GetPostdata(RequestType.OPEN_DRAWER, false);

            ContentResult cr = await SendRestfulRequest(url, postData);
        }

        internal async void CheckDeviceStatuses(ModuleSettings moduleSettings)
        {
            //log.Debug($"{_tu.GetCurrentMethod()}|_moduleSettings={JsonConvert.SerializeObject(moduleSettings)}");
            while (true)
            {
                WsStates.Clear();
                try
                {
                    string print_status = await CheckStatusDevice(moduleSettings);
                    WsStates.Add(moduleSettings, print_status);
                }
                catch (Exception ex)
                {
                    log.Error($"{_tu.GetCurrentMethod()}|CheckDeviceStatuses|" + ex.ToString());
                }

                Task.Delay(printerCheckDelay * 1000).Wait();
            }
        }

        private async Task<string> CheckStatusDevice(ModuleSettings moduleSettings)
        {
            log.Error($"{_tu.GetCurrentMethod()}|started|");

            string url = GetUrl(moduleSettings.Ip, RequestType.DEVICE_STATUS);
            log.Error($"{_tu.GetCurrentMethod()}|moduleSettings.Ip={moduleSettings.Ip}|url={url}|");
            string postData = GetPostdata(RequestType.DEVICE_STATUS, false);
            ContentResult cr = await SendRestfulRequest(url, postData);

            if (cr.Statuscode == HttpStatusCode.OK)
            {
                return "Доступен";
            }
            else
            {
                return "Не доступен";
            }
        }

        private async Task<bool> SendFormReceipt(ModuleSettings moduleSettings, Transaction transaction, List<Product> products,  long receiptSum, long receiptCash, string tenderName, int itemType)
        {
            


            if (products.Count > 0)
            {
                Taxes taxes = GetTaxes(receiptSum, moduleSettings);

                string operationType = GetOperationType(itemType);


                var url = moduleSettings.Ip + "/api/formReceipt";
                var postData = "";
                if (tenderName == "Наличные")
                {
                    postData = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        formCode = "RECEIPT",
                        ffdVersion = "VER_1",
                        operationType = operationType,
                        taxType = "GENERAL",
                        consumerContacts = "",
                        customMessage = "",
                        shouldPrintSlip = true,
                        products = products,
                        receiptSum = (long)receiptSum,
                        receiptCash = (long)receiptCash,
                        receiptNonCash = 0,
                        cashChangeAmount = (long)receiptCash - (long)receiptSum,
                        /*bankRRN = "",
                        bankCard = "",
                        bankAuthCode = "",
                        bankCardName = "",
                        bankResult = "",*/
                        taxes = taxes
                    });
                }
                else
                {
                    postData = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        formCode = "RECEIPT",
                        ffdVersion = "VER_1",
                        operationType = "INCOME",
                        taxType = "GENERAL",
                        consumerContacts = "",
                        customMessage = "",
                        shouldPrintSlip = true,
                        products = products,
                        receiptSum = (long)receiptSum,
                        receiptCash = 0,
                        receiptNonCash = (long)receiptSum,
                        cashChangeAmount = 0,
                        //bankRRN = null,
                        //bankCard = "",
                        bankAuthCode = "0",
                        bankCardName = "",
                        bankResult = "OK",
                        taxes = taxes
                    });
                }

                

                ContentResult cr = await SendRestfulRequest(url, postData);

                if (cr.Statuscode == HttpStatusCode.OK)
                {
                    ReceiptResponce receiptResponce = JsonConvert.DeserializeObject<ReceiptResponce>(cr.Content);


                    if (receiptResponce.rc == "SUCCESS")
                    {
                        log.Debug($"{_tu.GetCurrentMethod()}|tran={transaction}|Получен успешный ответ от принтера");
                        return true;
                    }

                    if (receiptResponce.rc == "SHIFT_TOO_LONG")
                    {
                        log.Debug($"{_tu.GetCurrentMethod()}|tran={transaction}|SHIFT_TOO_LONG| Прошло 24 часа с момента олткрытия смены | Закрываем смену");

                        var urlCloseShift = GetUrl(moduleSettings.Ip, RequestType.CLOSE_SHIFT);
                        string postDataCloseShift = GetPostdata(RequestType.CLOSE_SHIFT, true);
                        ContentResult crCloseShift = await SendRestfulRequest(urlCloseShift, postDataCloseShift);
                        ReceiptResponce receiptResponceCloseShift = JsonConvert.DeserializeObject<ReceiptResponce>(crCloseShift.Content);

                        if (receiptResponceCloseShift.rc == "SUCCESS")
                        {
                            log.Debug($"{_tu.GetCurrentMethod()}|tran={transaction}|SHIFT_TOO_LONG| Смена закрыта | Открываем смену");

                            var urlOpenShift = GetUrl(moduleSettings.Ip, RequestType.CLOSE_SHIFT);
                            string postDataOpenShift = GetPostdata(RequestType.CLOSE_SHIFT, true);
                            ContentResult crOpenShift = await SendRestfulRequest(urlOpenShift, postDataOpenShift);
                            ReceiptResponce receiptResponceOpenShift = JsonConvert.DeserializeObject<ReceiptResponce>(crOpenShift.Content);

                            if (receiptResponceOpenShift.rc == "SUCCESS")
                            {
                                log.Debug($"{_tu.GetCurrentMethod()}|tran={transaction}|SHIFT_TOO_LONG| Смена открыта | Печатаем чек");
                                
                                cr = await SendRestfulRequest(url, postData);
                                receiptResponce = JsonConvert.DeserializeObject<ReceiptResponce>(cr.Content);

                                if (receiptResponce.rc == "SUCCESS")
                                {
                                    log.Debug($"{_tu.GetCurrentMethod()}|tran={transaction}|Получен успешный ответ от принтера");
                                    return true;
                                }
                            }
                        }
                    }

                    if (receiptResponce.rc == "SHIFT_MUST_BE_OPENED")
                    {
                        log.Debug($"{_tu.GetCurrentMethod()}|tran={transaction}|SHIFT_MUST_BE_OPENED| Смена должна быть открыта | Открываем смену");

                        var urlOpenShift = GetUrl(moduleSettings.Ip, RequestType.OPEN_SHIFT);
                        string postDataOpenShift = GetPostdata(RequestType.OPEN_SHIFT, true);
                        ContentResult crOpenShift = await SendRestfulRequest(urlOpenShift, postDataOpenShift);
                        ReceiptResponce receiptResponceOpenShift = JsonConvert.DeserializeObject<ReceiptResponce>(crOpenShift.Content);

                        if (receiptResponceOpenShift.rc == "SUCCESS")
                        {
                            log.Debug($"{_tu.GetCurrentMethod()}|tran={transaction}|SHIFT_MUST_BE_OPENED| Смена открыта | Печатаем чек");

                            cr = await SendRestfulRequest(url, postData);
                            receiptResponce = JsonConvert.DeserializeObject<ReceiptResponce>(cr.Content);

                            if (receiptResponce.rc == "SUCCESS")
                            {
                                log.Debug($"{_tu.GetCurrentMethod()}|tran={transaction}|Получен успешный ответ от принтера");
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                log.Debug($"{_tu.GetCurrentMethod()}|tran={transaction}|Нет информации о товаре по id транзакции");
            }

            return false;
        }

        public bool Vsval(string strToSign, string signatureString)
        {
            if (signatureString != null)
            {
                byte[] signatureBytes = Convert.FromBase64String(signatureString);
                AsymmetricKeyParameter publicKey = PemKeyManager.LoadKeyPair(null, null);

                return StationIdentVerifier.VerifyStationIdent(strToSign, signatureBytes, publicKey);
            }
            else
            {
                return false;
            }

        }


        private string GetOperationType(int itemType)
        {
            switch (itemType)
            {
                case 1: return "INCOME";
                case 3: return "REVERT_INCOME";
            }
            return "INCOME";
        }

        private Taxes GetTaxes(long receiptSum, ModuleSettings moduleSettings)
        {
            log.Debug($"{_tu.GetCurrentMethod()}|vatCode={moduleSettings.vatCode}|receiptSum={receiptSum}|");

            Taxes taxes = new Taxes();
            Vat vat = new Vat();

            taxes.vats = new List<Vat>();

            vat.vatSum = GetVatSumByVatCode(moduleSettings, receiptSum);
            vat.vatCode = moduleSettings.vatCode;

            log.Debug($"{_tu.GetCurrentMethod()}| vat.vatSum={vat.vatSum}|vat.vatCode={vat.vatCode}|");

            taxes.vats.Add(vat);

            log.Debug($"{_tu.GetCurrentMethod()}| taxes={JsonConvert.SerializeObject(taxes)}|moduleSettings={JsonConvert.SerializeObject(moduleSettings)}|");

            try
            {
                if (LicenseCheck() != true)
                {
                    log.Debug($"{_tu.GetCurrentMethod()}|receiptSum={receiptSum}|Ex 348");
                    Application.Exit();
                }
            }
            catch (Exception e)
            {
                log.Debug($"{_tu.GetCurrentMethod()}|receiptSum={receiptSum}|349 Ex:{e}");
            }


            return taxes;
        }

        private long GetVatSumByVatCode(ModuleSettings moduleSettings, long receiptSum)
        {
            log.Debug($"{_tu.GetCurrentMethod()}| receiptSum={receiptSum}|");

            foreach (var item in moduleSettings.vatCodeList)
            {
                if (item.key == moduleSettings.vatCode)
                {
                    log.Debug($"{_tu.GetCurrentMethod()}| code={moduleSettings.vatCode}|item.value={item.value}|receiptSum={receiptSum}|retuen={Convert.ToInt32(receiptSum * (item.value / 100)).ToString()}|");
                    
                    return Convert.ToInt32(receiptSum * (item.value/100));
                }
            }
            log.Debug($"{_tu.GetCurrentMethod()}|{moduleSettings.vatCode} не найден в списке");
            return 0;
        }
        private bool LicenseCheck()
        {
            string strToSign_MacAndHdd = SystemInfoLib.GetMacAndHdd();
            string strToSign_MotherboardSn = SystemInfoLib.GetMotherboardSn();

            string fileContent = ReadFileContent(filePath);

            if (fileContent != null)
            {
                if (Vsval(strToSign_MacAndHdd, fileContent) || Vsval(strToSign_MotherboardSn, fileContent))
                {
                    return true;
                }
            }
            return false;
        }


        private decimal GetProdoctTotalAmount(List<Product> products)
        {
            decimal receiptSum = 0;
            foreach (var product in products)
            {
                receiptSum += Convert.ToDecimal(product.sum);
            }

            return receiptSum;
        }

        internal string GetPostdata(RequestType device_status, bool shouldPrintSlip, string _userName = "", string fdNumber = "")
        {
            switch (device_status)
            {
                case RequestType.DEVICE_STATUS:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        formCode = "DEVICE_STATUS",
                        shouldPrintSlip = shouldPrintSlip
                    });

                case RequestType.CLOSE_SHIFT:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        formCode = "CLOSE_SHIFT",
                        ffdVersion = "VER_1",
                        shouldPrintSlip = true,
                        cashier = _userName,
                        kktVersion = "1",
                    });

                case RequestType.PRINT_LAST_FD:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        formCode = "PRINT_LAST_FD",
                        ffdVersion = "VER_1",
                        shouldPrintSlip = true,

                    });

                case RequestType.PRINT_FD_BY_NUMBER:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        formCode = "PRINT_FD_BY_NUMBER",
                        ffdVersion = "VER_1",
                        shouldPrintSlip = true,
                        fdNumber = fdNumber,
                    });

                case RequestType.GET_X_REPORT:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        formCode = "GET_X_REPORT",
                        ffdVersion = "VER_1",
                        shouldPrintSlip = true,
                    });
                case RequestType.OPEN_SHIFT:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        formCode = "OPEN_SHIFT",
                        ffdVersion = "VER_1",
                        shouldPrintSlip = true,
                        cashier = _userName,
                        kktVersion = "1",
                    });

                case RequestType.OPEN_DRAWER:
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        formCode = "OPEN_DRAWER",
                        onTimeout = 500,
                        offTimeout = 100,
                        onQuantity = 1,
                    });

            }

            return "";

        }


        public bool SaveFileContent(string filePath, string content)
        {
            try
            {
                File.WriteAllText(filePath, content);
                log.Debug("File saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                log.Error($"Error saving file: {ex.Message}");
                return false;
            }
        }


        internal string GetUrl(string host, RequestType requestType)
        {
            log.Error($"{_tu.GetCurrentMethod()}|host={host}|requestType={requestType.ToString()}|");
            switch (requestType)
            {
                case RequestType.OPEN_SHIFT:            return host + "/api/openShift";
                case RequestType.DEVICE_STATUS:         return host + "/api/deviceStatus";
                case RequestType.CLOSE_SHIFT:           return host + "/api/closeShift";
                case RequestType.PRINT_LAST_FD:         return host + "/api/getLastFD";
                case RequestType.PRINT_FD_BY_NUMBER:    return host + "/api/getFDByNumber";
            }
            return "";
        }

        public void CreateDirectoryWithCurrentDate()
        {

            string basePath = AppDomain.CurrentDomain.BaseDirectory + @"Files\Data\";
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string fullPath = Path.Combine(basePath, date);

            try
            {
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                    Console.WriteLine($"Папка успешно создана: {fullPath}");
                }
                else
                {
                    Console.WriteLine($"Папка уже существует: {fullPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка при создании папки: {ex.Message}");
            }
        }


        public static string GetPrintedIds()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Files\Data\" + DateTime.Now.ToString("yyyy-MM-dd");
            List<string> fileNames = GetFileNamesInDirectory(path);

            // Форматируем имена файлов с одинарными кавычками
            List<string> formattedFileNames = fileNames.Select(name => $"'{name}'").ToList();

            // Объединяем форматированные имена файлов в строку, разделяя их запятыми
            string fileNamesCommaSeparated = string.Join(", ", formattedFileNames);

            return fileNamesCommaSeparated;
        }

        public static List<string> GetFileNamesInDirectory(string directoryPath)
        {
            List<string> fileNames = new List<string>();

            try
            {
                if (Directory.Exists(directoryPath))
                {
                    string[] files = Directory.GetFiles(directoryPath);
                    foreach (string file in files)
                    {
                        fileNames.Add(Path.GetFileName(file));
                    }
                }
                else
                {
                    Console.WriteLine($"Путь не существует: {directoryPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка при чтении файлов: {ex.Message}");
            }

            return fileNames;
        }


        public bool CreateFile(string fileName)
        {
            string folderPath = AppDomain.CurrentDomain.BaseDirectory + @"Files\Data\" + DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                // Ensure the folder exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Combine the folder path and file name to get the full file path
                string filePath = Path.Combine(folderPath, fileName);

                // Create the file
                using (FileStream fs = File.Create(filePath))
                {
                    // Optionally write some content to the file
                    byte[] content = new UTF8Encoding(true).GetBytes("File created successfully.");
                    fs.Write(content, 0, content.Length);
                }

                Console.WriteLine("File created successfully at: " + filePath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

       

       

        public string ReadFileContent(string filePath)
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







    }






}
