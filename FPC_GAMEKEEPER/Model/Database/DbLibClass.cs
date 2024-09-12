using FPC.Model.Database;
using DbClassLibrary.Model.Sturcure;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using System;
using FPC.Model.Structure;
using FPC.Model.Structure.Trans;
using System.Runtime.InteropServices.ComTypes;
using FPC.Model;
using FirebirdSql.Data.FirebirdClient;
using FPC.Model.Structure.Receipt;
using FPC.Helper;
using System.IO;
using System.Text;
using System.Globalization;
using System.Data.SqlClient;
using System.Collections;


namespace DbClassLibrary
{
    public class DbLibClass
    {
        log4net.ILog log = FPC.Model.Logger.Logger.Get(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ThreadUtility _tu;
        


        public DbLibClass(ThreadUtility tu)
        { 
            _tu = tu != null ? tu : new ThreadUtility();
            
        }

        private DbContentResult GetDataSetFromQueryFirebird(FbConnection connection, string query)
        {
            DbContentResult dbContentResult = new DbContentResult();
            RequestContentResult requestContentResult = new RequestContentResult();
            dbContentResult.RequestContentResult = requestContentResult;

            // Создаем DataSet
            DataSet dataSet = new DataSet();

            // Создаем подключение
            //using (FbConnection connection = new FbConnection(connectionString))
            {
                try
                {
                    // Открываем подключение
                    //connection.Open();

                    Console.WriteLine("Подключение установлено успешно.");

                    // Создаем DataAdapter
                    FbDataAdapter dataAdapter = new FbDataAdapter(query, connection);

                    // Заполняем DataSet
                    // Заполняем DataSet
                    dataAdapter.Fill(dataSet, "JoinedData");

                    Console.WriteLine("Данные успешно загружены в DataSet.");

                    dbContentResult.DataSet = dataSet;
                    dbContentResult.RequestContentResult.StatusCode = 200;
                }
                catch (Exception ex)
                {
                    log.Debug($"{_tu.GetCurrentMethod()}|GetDataSetFromQuery|ex" + ex.ToString());

                    dbContentResult.RequestContentResult.StatusCode = 500;
                    dbContentResult.RequestContentResult.Content = ex.Message;
                }
                return dbContentResult;
            }
        }

        public List<Transaction> GetTransactionListFirebirdSql(SqlConnection connection, ModuleSettings moduleSettings, DateTime _appStartTime)
        {
            log.Debug($"{_tu.GetCurrentMethod()}|GetTransactionListFirebirdSql|started");

            List<Transaction> tranList = new List<Transaction>();

            // Получить список id уже распечатанных транзакций
            string alreadyPrinted = Functions.GetPrintedIds();

            // Получает строку запроса списка транзакций
            string query = GeTransactionListQuery(moduleSettings.startDate, alreadyPrinted, _appStartTime, moduleSettings);

            DateTime x1 = DateTime.Now;

            // Выполняем запрос списка транзакций
            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Transaction tran = new Transaction();

                            tran.trans_oid = reader["trans_oid"].ToString();
                            tran.trans_date = reader["trans_date"].ToString();
                            //tran.user_id = reader["user_oid"].ToString();
                            //tran.fiscal_receipt_printed = reader["fiscal_receipt_printed"].ToString();

                            tranList.Add(tran);
                        }
                    }
                }

                log.Debug($"{_tu.GetCurrentMethod()}|GetTransactionListFirebirdSql|Successfully retrieved transactions");
            }
            catch (Exception ex)
            {
                log.Error($"{_tu.GetCurrentMethod()}|Error: " + ex.ToString());
            }

            DateTime x2 = DateTime.Now;
            TimeSpan difference = x2 - x1;
            var y = difference.TotalMilliseconds;

            return tranList;
        }



        private string GeTransactionListQuery(string startDate, string alreadyPrinted, DateTime _appStartTime, ModuleSettings moduleSettings)
        {
            log.Debug($"{_tu.GetCurrentMethod()}|GeTransactionListQuery|started");
            string query = "";
            string dbName = moduleSettings.dbName != null ? moduleSettings.dbName : "gkArcade";
            string projectName = "SPACE";

            // Get the current date in the required format (yyyy-MM-dd)

           

            string currentDate = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            string _appStartTimeStr = _appStartTime.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss");

            if (alreadyPrinted.Length > 0)
            {
                alreadyPrinted = "AND [ads_oid] NOT IN (" + alreadyPrinted + ")";
            }

            if (projectName == "CINEGUARD")
            {
                query = @"
                    SELECT TOP (1000) 
                           [trans_oid]
                          ,[trans_date]
                          ,[user_oid]
                          ,[trans_inprocess]
                      FROM [CineGuardDB].[dbo].[Trans]
                      WHERE 
                        [trans_date] >= CONVERT(datetime, '{startDate}', 120)
                        AND [trans_inprocess] = 0
                        AND [isAdminBooking] = 0
                        AND [return_amount] IS NULL
                      ORDER BY [trans_date] ASC"
                ;
            }

            if (projectName == "SPACE")
            {
                query = @"
                    SELECT TOP 1000 
                         [ads_oid] AS trans_oid
                        ,[pers_oid] AS user_oid
                        ,[ads_date] 
                        ,[ads_enddate] AS trans_date
                        ,[ads_isclosed]
                        ,[check_no]
                        ,[check_printno]
                    FROM [{dbName}].[dbo].[Adisyon]
                    WHERE 
                          [ads_isclosed] = 1
                          AND [ads_enddate] >= CONVERT(datetime, '{_appStartTime}', 120)
                          AND [ads_enddate] <= GETDATE()
                          AND [ads_enddate] >= DATEADD(minute, -3, GETDATE())
                          AND [cust_discount] = 0
                          AND [check_printno] > 0
                          {alreadyPrinted} 
                    ORDER BY [ads_date]
                "
                ;
            }

            query = query.Replace("{startDate}", startDate);
            query = query.Replace("{currentDate}",  currentDate);
            query = query.Replace("{_appStartTime}", _appStartTimeStr);
            query = query.Replace("{alreadyPrinted}", alreadyPrinted);
            query = query.Replace("{dbName}", dbName);

            //log.Debug($"{_tu.GetCurrentMethod()}|GeTransactionListQuery|query=" + query);
            return query;
        }


        private DbContentResult GetDataSetFromQueryMssql(string CONNECTION_STRING, string query)
        {
            log.Debug($"{_tu.GetCurrentMethod()}|GetDataSetFromQuery|stated");
            DbContentResult dbContentResult = new DbContentResult();
            RequestContentResult requestContentResult = new RequestContentResult();
            dbContentResult.RequestContentResult = requestContentResult;
            try
            {
               
                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(CONNECTION_STRING);
                System.Data.SqlClient.SqlDataAdapter da;
                DataTable dt = new DataTable();
                conn.Open();
                da = new System.Data.SqlClient.SqlDataAdapter(query, conn);
                System.Data.SqlClient.SqlCommandBuilder cBuilder = new System.Data.SqlClient.SqlCommandBuilder(da);
                dt = new DataTable();
                DataSet dataSet = new DataSet();
                da.Fill(dataSet);
                conn.Close();
                dbContentResult.DataSet = dataSet;
                dbContentResult.RequestContentResult.StatusCode = 200;
                return dbContentResult;
            }
            catch (System.Exception ex)
            {
                log.Debug($"{_tu.GetCurrentMethod()}|GetDataSetFromQuery|ex"+ ex.ToString());
                dbContentResult.RequestContentResult.StatusCode = 500;
                dbContentResult.RequestContentResult.Content = ex.Message;
                return dbContentResult;
            }

        }

        private string GetUserListQuery(string threadId, string startTimeStamp)
        {
            string query = @"
                SELECT 
				      U.user_oid
				    , U.user_name_surename
			    FROM 
                    [CineGuardDB].[dbo].[Users] AS U 
				ORDER BY
				    U.[user_name_surename]"
                ;
           
            return query;
        }

        // Прлучение продуктов
        internal List<Product> GetProductsListFirebirdSql(SqlConnection connection, ModuleSettings moduleSettings, Transaction transaction, ref long receiptSum, ref long receiptCash, ref string tenderName, ref int itemType, ref long item_serviceprice, ref long item_serviceRate)
        {
            List<Product> productList = new List<Product>();

            // Получение строки запроса получения продуктов
            string query = GeProductListQuery(transaction.trans_oid, moduleSettings);

            DateTime x1 = DateTime.Now;


            string projectName = "SPACE";

            // Выполнение запроса и чтение данных
            using (SqlDataReader reader = ExecuteReader(connection, query))
            {
                while (reader.Read())
                {
                    Product product = new Product();

                    if (projectName == "CINEGUARD")
                    {
                        product.name = reader["r_item"].ToString();
                        product.price = (long)(Convert.ToDecimal(reader["ticket_price"].ToString().Replace(",", ".")) * 100);
                        product.quantity = 1; 
                        product.commodity = moduleSettings.commodity;
                        product.vatCode = moduleSettings.vatCode;
                        product.sum = (long)(Convert.ToDecimal(reader["ticket_price"].ToString().Replace(",", ".")) * 100);
                        productList.Add(product);
                        itemType = 1;
                        receiptSum += product.sum;
                        receiptCash += product.sum;
                        tenderName = "Наличные";
                       
                    }


                    if (projectName == "SPACE")
                    {
                        product.name = reader["item_name"].ToString();
                        product.price = (long)(Convert.ToDecimal(reader["item_price"].ToString().Replace(",", ".")) * 100);
                        product.quantity = Math.Round(Convert.ToDecimal(reader["item_qty"].ToString().Replace(",", ".")), 3);
                        product.commodity = moduleSettings.commodity;
                        product.vatCode = moduleSettings.vatCode;
                        product.sum = (long) (Convert.ToDecimal(reader["total"].ToString().Replace(",", ".")) * 100);
                        productList.Add(product);
                        itemType = 1;
                        receiptSum += product.sum;
                        receiptCash += product.sum;
                        tenderName = "Наличные";

                        decimal item_service = Convert.ToDecimal(reader["item_serviceprice"].ToString().Replace(",", "."));

                        item_serviceprice += (long) ((item_service * 100) * product.quantity);

                        if (item_serviceprice > 0)
                        {
                            item_serviceRate = (long) ((item_service / Convert.ToDecimal(product.sum / 100))  * 100);
                        }
                        

                    }
                }
            }

            DateTime x2 = DateTime.Now;
            TimeSpan difference = x2 - x1;
            var y = difference.TotalMilliseconds;

            return productList;
        }

        private string GeProductListQuery(string trans_oid, ModuleSettings moduleSettings)
        {
            string query = "";
            string projectName = "SPACE";
            string dbName = moduleSettings.dbName != null ? moduleSettings.dbName : "gkArcade";

            if (projectName == "CINEGUARD")
            {
                query = @"
                    SELECT 
                        TD.trans_oid,
                        F.film_name,
                        CASE WHEN F.is_3D = 1 THEN '3D' ELSE '2D' END AS movie_format, 	
                        CONCAT(CONVERT(VARCHAR(11), PG.prog_date,120),FORMAT(CONVERT(datetime, S.sess_start,  120), 'HH:mm')) as sess_start,
                        Sal.salon_name,
                        C.chair_row,
                        C.chair_num,
                        TD.ticket_price,
                        PR.pricing_name,
                        CONCAT(F.film_name, ' (', CASE WHEN F.is_3D = 1 THEN '3D' ELSE '2D' END, ')\n ', 
                               CONCAT(CONVERT(VARCHAR(11), PG.prog_date,120),FORMAT(CONVERT(datetime, S.sess_start,  120), 'HH:mm')), ',\n зал:', 
                               Sal.salon_name, '\n, ряд:', C.chair_row, ', место:', C.chair_num, ' \n(', PR.pricing_name, ')' ) AS r_item
                    FROM 
                        [CineGuardDB].[dbo].[TransDetails] TD
                    JOIN 
                        [CineGuardDB].[dbo].[Trans] T ON TD.trans_oid = T.trans_oid
                    JOIN 
                        [CineGuardDB].[dbo].[Sessions] S ON T.sess_oid = S.sess_oid
                    LEFT JOIN 
                        [CineGuardDB].[dbo].[Films] F ON S.film_oid = F.film_oid
                    JOIN 
                        [CineGuardDB].[dbo].[Salon] Sal ON T.salon_oid = Sal.salon_oid
                    JOIN 
                        [CineGuardDB].[dbo].[Chair] C ON TD.chair_oid = C.chair_oid
                    JOIN 
                        [CineGuardDB].[dbo].[Pricing] PR ON PR.pricing_oid = TD.pricing_oid
                    JOIN 
                        [CineGuardDB].[dbo].[Program] PG ON PG.prog_oid = S.prog_oid

                    WHERE 
                        TD.trans_oid = '{trans_oid}'
	                
                    ORDER BY 
                        S.sess_start, TD.trans_oid;
"
               ;
            }

            if (projectName == "SPACE")
            {

                query = @"
                    SELECT TOP 1000 
                        A.[ads_oid] AS trans_oid
                        ,A.[pers_oid]
                        ,A.[ads_date]
	                    ,A.[ads_enddate]
                        ,A.[ads_isclosed]
                        ,A.[check_no]
                        ,A.[check_printno]
      
	                    ,AD.[detail_oid]
                        ,AD.[item_oid]
      
                        ,AD.[item_price]
                        ,AD.[item_qty]
	                    ,AD.[item_price] * AD.[item_qty] AS total  
                        ,AD.[item_discount]
                        ,AD.[item_nds]
                        ,AD.[item_serviceprice]
                        ,AD.[item_iscanceled]
                        ,AD.[item_isordered]
                        ,AD.[item_isikram]
                        ,AD.[item_info]
                        ,AD.[depm_oid]
                        ,AD.[ads_order]
                        ,AD.[person_id]
                        ,AD.[order_requested]
                        ,AD.[order_delivered]
                        ,AD.[user_oid]
                        ,AD.[is_extra]

	                    , I.[item_name] 

                    FROM [{dbName}].[dbo].[Adisyon] AS A
                    JOIN [{dbName}].[dbo].[AdisyonDetails] AS AD ON AD.[adisyon_oid] = A.[ads_oid]
                    JOIN [{dbName}].[dbo].[Items] AS I ON I.[item_oid] = AD.[item_oid]

                    WHERE [ads_oid] = '{trans_oid}'
                    AND A.[ads_isclosed] = 1
                    AND AD.[item_iscanceled] = 0

                    ORDER BY [ads_date] DESC;
"
               ;
            }

            query = query.Replace("{trans_oid}", trans_oid);
            query = query.Replace("{dbName}", dbName);

            return query;
        }

        private SqlDataReader ExecuteReader(SqlConnection connection, string query)
        {
            SqlCommand command = new SqlCommand(query, connection);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            return command.ExecuteReader();
        }
    }
}
