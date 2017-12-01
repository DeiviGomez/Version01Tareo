using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace xAPI.Library.Base
{
    [Serializable]
    public class BaseId
    {

        public Int32 Id { get; set; }
        public Int16 Status { get; set; }
        public Int16 Action { get; set; }
        public Int32 Type { get; set; } // solo aplica para prestamos y adelantos por estar  en una sola lista(dos tablas)

    }
        //[Serializable]
        //public class tBaseIdList : List<BaseId>, IEnumerable<SqlDataRecord>
        //{
        //    IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        //    {
        //        SqlDataRecord ret = new SqlDataRecord(
        //            new SqlMetaData("ID", SqlDbType.Int),
        //            new SqlMetaData("STATUS", SqlDbType.SmallInt),
        //            new SqlMetaData("ACTION", SqlDbType.SmallInt),
        //            new SqlMetaData("TYPE", SqlDbType.SmallInt)

        //            );
        //        foreach (BaseId data in this)
        //        {
        //            ret.SetInt32(0, data.Id);
        //            ret.SetInt16(1, data.Status);
        //            ret.SetInt16(2, data.Action);
        //            ret.SetInt32(2, data.Type);

        //            yield return ret;
        //        }
        //    }
        //}

    
    
}
