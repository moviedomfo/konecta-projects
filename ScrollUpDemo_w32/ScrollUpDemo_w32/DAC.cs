using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrollUpDemo_w32
{
    public class CaseCommenDAC
    {
        static int top = 5;
        public static CaseCommentList search(int start_id)
        {
            CaseCommentBE item = null;
            CaseCommentList list = new CaseCommentList();
            //select top (@TOP) * from mc.CaseComment where casecommentid >= @start_id
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EpironConnectionString"].ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand() { Connection = cnn, CommandType = System.Data.CommandType.Text })
            {

                cmd.CommandText = string.Format("select top({0}) * from mc.CaseComment where casecommentid > {1} order by CaseCommentId asc", top.ToString(), start_id);
                cnn.Open();



                using (IDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        item = new CaseCommentBE();

                        //cue_nombre sar_nombre car_nombre usuariowindows dominio
                        item.CaseCommentId = Convert.ToInt32(reader["CaseCommentId"]);
                        item.CaseCommentText = reader["CaseCommentText"].ToString();
                        item.CaseId = Convert.ToInt32(reader["CaseId"]);
                        item.CreatedRowOrigenLog = Convert.ToDateTime(reader["CreatedRowOrigenLog"]);
                        item.CaseCommentCreated = Convert.ToDateTime(reader["CaseCommentCreated"]);
                        list.Add(item);
                    }
                }



            }
            //Invertir la lista
             
            CaseCommentList list2 = new CaseCommentList();
            //list2.AddRange(list.OrderByDescending(p => p.CaseCommentId));
            list2.AddRange(list.OrderBy(p => p.CaseCommentId));
            return list2;

        }
    }

}

        
    

