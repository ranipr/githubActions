using GiveawayHistorianScheduler.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveawayHistorianScheduler
{
    public class HistorianScheduler : IHistorianScheduler
    {
        private string SqlServerConstr;
        private string SpName;
        private IConfig config;

        public HistorianScheduler(IConfig config)
        {
            this.SqlServerConstr = config.SqlServerConstr;
            this.SpName = config.SpName;
            this.config = config;
        }

        /// <summary>
        /// To call the stored procedures "HistorianSp" to move historical data for ETL performance enhancement.
        /// </summary>
        /// <returns></returns>

        public void call_HistorianSP()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(SqlServerConstr))
                {
                    using (SqlCommand cmd = new SqlCommand(SpName, con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 1800;
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error while calling stored Procedure StackTrace: {ex.StackTrace} Message : {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occured call_AlertsToHistoricalSP :{ex.StackTrace} Message : {ex.Message}");
            }

        }
    }
}
