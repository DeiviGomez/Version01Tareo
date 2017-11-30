using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using xAPI.Library.Base;
using xAPI.Library.Connection;
namespace xAPI.Dao
{
    public class clsDistrictDAO
    {
        #region Singleton
        private static readonly clsDistrictDAO _instance = new clsDistrictDAO();
        public static clsDistrictDAO Instance
        {
            get { return clsDistrictDAO._instance; }
        }
        #endregion

        #region Metodos
        public DataTable ListDistrictByProvince(ref BaseEntity objBase, int ProvId)
        {
            DataTable dt = null;

            try
            {
                using (var objCommand = new SqlCommand("sp_xP_Listar_Distritos_Por_Provincia", clsConnection.GetConnection()))
                {
                    objCommand.CommandType = CommandType.StoredProcedure;
                    objCommand.Parameters.AddWithValue("@idprov", ProvId);

                    using (var objDataReader = objCommand.ExecuteReader())
                    {
                        if (objDataReader.HasRows)
                        {
                            dt = new DataTable();
                            dt.Load(objDataReader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objBase.Errors.Add(new BaseEntity.ListError() { Error = ex, MessageClient = ex.Message });
            }

            return dt;
        }

        public DataTable ListDistrict(ref BaseEntity objBase)
        {
            DataTable dt = null;

            try
            {
                using (var objCommand = new SqlCommand("sp_xP_Listar_Distrito", clsConnection.GetConnection()))
                {
                    objCommand.CommandType = CommandType.StoredProcedure;

                    using (var objDataReader = objCommand.ExecuteReader())
                    {
                        if (objDataReader.HasRows)
                        {
                            dt = new DataTable();
                            dt.Load(objDataReader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objBase.Errors.Add(new BaseEntity.ListError() { Error = ex, MessageClient = ex.Message });
            }

            return dt;
        }

        #endregion
    }
}
