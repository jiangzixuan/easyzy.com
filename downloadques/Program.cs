using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTools.DB.MSSQL;

namespace downloadques
{
    class Program
    {
        const string SOURCE_DB_CONNSTR = "Data Source=36.110.49.104;Initial Catalog=Zxxk_Question;User ID=jqzydb;Password=kw3s4V5-9o$cr^The96yp;Pooling=True;Max Pool Size=512;";
        const string DEST_DB_CONNSTR = "Data Source=47.94.100.66;Database=easyzy_home;User ID=zyadmin-jiang;Password=jyq911818!;SslMode=None";
        static void Main(string[] args)
        {
            
            DataTable dt = GetGroups();
            for (int i =0; i<dt.Rows.Count; i++)
            {
                DataTable dtData = GetGroupData(int.Parse(dt.Rows[i][0].ToString()), int.Parse(dt.Rows[i][1].ToString()), int.Parse(dt.Rows[i][2].ToString()));

            }
        }

        private static DataTable GetGroups()
        {
            DataTable dt = DBHelper.GetDataTable(SOURCE_DB_CONNSTR,
                "select courseid, (case ParentQuesID when 0 then 0 else 1 end) pid, paperyear from mdm_ques group by courseid,(case ParentQuesID when 0 then 0 else 1 end), paperyear order by courseid, paperyear"
                );
            return dt;
        }

        private static DataTable GetGroupData(int courseId, int pId, int paperYear)
        {
            string sql = "select * from MDM_Ques where CourseId = @CourseId and PaperYear = @PaperYear ";
            if (pId == 0)
            {
                sql += "and ParentQuesID = 0";
            }
            else
            {
                sql += "and ParentQuesID <> 0";
            }
            DataTable dt = DBHelper.GetDataTable(SOURCE_DB_CONNSTR,
                sql,
                "@CourseId".ToIntInPara(courseId),
                "@PaperYear".ToIntInPara(paperYear)
                );
            return dt;
        }

        private static void InsertGroupData(DataTable dt)
        {
            object o = MySqlHelper. ExecuteScalar(DEST_DB_CONNSTR,
                "insert into T_Questions(Id, CourseId, TypeId, TypeName) values (null, @UserId, @RUserId, @CreateDate); select last_insert_id();"
                );
            return o == null ? 0 : int.Parse(o.ToString());
        }
    }
}
