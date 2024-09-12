using DbClassLibrary.Model;
using DbClassLibrary.Model.Sturcure;
using System.Data;

namespace DbClassLibrary
{
    public class DbLibClass
    {





        public List<User> GetUserListMsSql(string CONNECTION_STRING, string startTimeStamp)
        {
            List<User> userlist = new List<User>();
            string query = GetUserListQuery(CONNECTION_STRING, startTimeStamp);
            DbContentResult dbContentResult = GetDataSetFromQuery(CONNECTION_STRING, query);
            if (dbContentResult.RequestContentResult.StatusCode == 200)
            {
                for (int i = 0; i < dbContentResult.DataSet.Tables[0].Rows.Count; i++)
                {
                    User sessionInfo = new User();

                    sessionInfo.user_id = dbContentResult.DataSet.Tables[0].Rows[i]["user_oid"].ToString();
                    sessionInfo.user_name = Convert.ToDateTime(dbContentResult.DataSet.Tables[0].Rows[i]["user_name_surename"].ToString());


                    userlist.Add(sessionInfo);
                }
            }

            return userlist;
        }

        private DbContentResult GetDataSetFromQuery(string CONNECTION_STRING, string query)
        {
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
            catch (Exception ex)
            {
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
    }
}
