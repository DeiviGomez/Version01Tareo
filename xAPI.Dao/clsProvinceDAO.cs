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

    public class clsProvinceDAO
    {
        #region Singleton
        private static readonly clsProvinceDAO _instance = new clsProvinceDAO();
        public static clsProvinceDAO Instance
        {
            get { return clsProvinceDAO._instance; }
        }
        #endregion

        #region Metodos
        public DataTable ListProvinceByDepartament(ref BaseEntity objBase, int DepartId)
        {
            DataTable dt = null;

            try
            {
                using (var objCommand = new SqlCommand("sp_xP_Listar_Provincia_Por_Departamento", clsConnection.GetConnection()))
                {
                    objCommand.CommandType = CommandType.StoredProcedure;
                    objCommand.Parameters.AddWithValue("@iddepa", DepartId);

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
                dt = null;
                objBase.Errors.Add(new BaseEntity.ListError() { Error = ex, MessageClient = ex.Message });
            }

            return dt;
        }

        public DataTable ListProvince(ref BaseEntity objBase)
        {
            DataTable dt = null;

            try
            {
                using (var objCommand = new SqlCommand("sp_xP_Listar_Provincia", clsConnection.GetConnection()))
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
