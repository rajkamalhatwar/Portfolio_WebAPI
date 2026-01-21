using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public  class SqlHelper
    {

        private string _constr = string.Empty;      //connection string

        public SqlHelper(string connectionstring)
        {
            _constr = connectionstring;
        }
        public SqlHelper()
        {

        }

        public string Getcon()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration _configuration = builder.Build();


            string connectionstring = _configuration.GetValue<string>("ConnectionStrings:MainConnectionstring");
            //   string connectionstring = "Data Source=40.80.85.212;Initial Catalog=db_RFCoreMvcTest;User ID=db_RFCoreMvcTest;Password=12dveergsd@#bfdg;";
            return connectionstring;


            //return "Data Source=172.16.0.12;Initial Catalog=DEV_DB_ONLINEREG;User ID=ONLINEREG;Password=Ok$bul9vhern!0zt;";
        }


 

        public object ExecuteNonQuerySP(String query, SqlParameter[] parameters, bool flag)
        {
            object? retval = null;
            using (SqlConnection cnn = new SqlConnection(Getcon()))
            {
                using (SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    cmd.CommandTimeout = 2000;
                    if (query.StartsWith("INSERT") | query.StartsWith("insert") | query.StartsWith("UPDATE") | query.StartsWith("update") | query.StartsWith("DELETE") | query.StartsWith("delete"))
                        cmd.CommandType = CommandType.Text;
                    else
                        cmd.CommandType = CommandType.StoredProcedure;

                    int i;
                    for (i = 0; i < parameters.Length; i++)
                        cmd.Parameters.Add(parameters[i]);

                    try
                    {
                        cnn.Open();
                        retval = cmd.ExecuteNonQuery();

                        if (flag == true)
                            retval = cmd.Parameters[parameters.Length - 1].Value;
                    }
                    catch (Exception ex)
                    {
                        retval = null;

                    }
                    finally
                    {
                        if (cnn.State == ConnectionState.Open) cnn.Close();
                    }

                }
            }
            return retval;
        }

        public DataSet ExecuteDataSetSP(String query, SqlParameter[] parameters)
        {
            DataSet? ds = null;
            try
            {
                using (SqlConnection cnn = new SqlConnection(Getcon()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, cnn))
                    {
                        if (query.StartsWith("SELECT") | query.StartsWith("select") | query.StartsWith("INSERT") | query.StartsWith("insert") | query.StartsWith("UPDATE") | query.StartsWith("update") | query.StartsWith("DELETE") | query.StartsWith("delete"))
                            cmd.CommandType = CommandType.Text;
                        else
                            cmd.CommandType = CommandType.StoredProcedure;

                        int i;
                        for (i = 0; i < parameters.Length; i++)
                            cmd.Parameters.Add(parameters[i]);

                        using (SqlDataAdapter da = new SqlDataAdapter())
                        {
                            da.SelectCommand = cmd;
                            cmd.CommandTimeout = 180;
                            ds = new DataSet();
                            da.Fill(ds);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ds = null;

            }
            return ds;
        }

        public SqlDataReader ExecuteDataReaderSP(String query, SqlParameter[] parameters)
        {
            SqlDataReader? dr = null;
            try
            {
                SqlConnection cnn = new SqlConnection(Getcon());
                SqlCommand cmd = new SqlCommand(query, cnn);
                if (query.StartsWith("SELECT") | query.StartsWith("select"))
                    cmd.CommandType = CommandType.Text;
                else
                    cmd.CommandType = CommandType.StoredProcedure;

                int i;
                for (i = 0; i < parameters.Length; i++)
                    cmd.Parameters.Add(parameters[i]);

                cnn.Open();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch (Exception ex)
            {
                dr = null;

                throw;
            }
        }
        public SqlDataReader ExecuteReaderSP(String query, SqlParameter[] parameters)
        {
            SqlDataReader? dr = null;
            try
            {
                SqlConnection cnn = new SqlConnection(Getcon());
                SqlCommand cmd = new SqlCommand(query, cnn);
                if (query.StartsWith("SELECT") | query.StartsWith("select"))
                    cmd.CommandType = CommandType.Text;
                else
                    cmd.CommandType = CommandType.StoredProcedure;

                int i;
                for (i = 0; i < parameters.Length; i++)
                    cmd.Parameters.Add(parameters[i]);

                cnn.Open();
                cmd.CommandTimeout = 180;
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                dr = null;

            }
            return dr;
        }
        public Object ExecuteScalarSP(String query, SqlParameter[] parameters)
        {
            Object retval;
            using (SqlConnection cnn = new SqlConnection(Getcon()))
            {
                using (SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    if (query.StartsWith("SELECT") | query.StartsWith("select"))
                        cmd.CommandType = CommandType.Text;
                    else
                        cmd.CommandType = CommandType.StoredProcedure;

                    int i;
                    for (i = 0; i < parameters.Length; i++)
                        cmd.Parameters.Add(parameters[i]);


                    try
                    {
                        cnn.Open();
                        retval = cmd.ExecuteScalar();

                    }
                    catch (Exception ex)
                    {
                        retval = null;

                    }
                    finally
                    {
                        if (cnn.State == ConnectionState.Open) cnn.Close();
                    }
                }
            }
            return retval;
        }

        //public DataSet ExecuteReaderSP(String query, SqlParameter[] parameters)
        //{
        //    DataSet ds = null;
        //    //SqlDataReader dr = null;
        //    try
        //    {
        //        SqlConnection cnn = new SqlConnection(Getcon());
        //        SqlCommand cmd = new SqlCommand(query, cnn);
        //        if (query.StartsWith("SELECT") | query.StartsWith("select"))
        //            cmd.CommandType = CommandType.Text;
        //        else
        //            cmd.CommandType = CommandType.StoredProcedure;

        //        int i;
        //        for (i = 0; i < parameters.Length; i++)
        //            cmd.Parameters.Add(parameters[i]);

        //        cnn.Open();
        //        //dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = cmd;
        //        ds = new DataSet();
        //        da.Fill(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        ds = null;

        //    }
        //    return ds;
        //}


        public DataSet ExecuteDataSet(String query)
        {
            DataSet? ds = null;
            try
            {
                using (SqlConnection cnn = new SqlConnection(Getcon()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, cnn))
                    {
                        if (query.StartsWith("SELECT") | query.StartsWith("select"))
                            cmd.CommandType = CommandType.Text;
                        else
                            cmd.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        cmd.CommandTimeout = 180;
                        ds = new DataSet();
                        da.Fill(ds);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPSTBNI99ER21212So89652FT";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string Decrypt(string cipherText)
        {
            // cipherText = cipherText.Trim();
            cipherText = cipherText.Replace(" ", "+");

            string EncryptionKey = "MAKV2SPSTBNI99ER21212So89652FT";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }
}
