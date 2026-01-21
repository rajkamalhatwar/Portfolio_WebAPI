using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Service_Providers
{
    //public class TableToList
    //{
    //    public static List<T> ConvertDataTable<T>(DataTable dt)
    //    {
    //        List<T> data = new List<T>();
    //        foreach (DataRow row in dt.Rows)
    //        {
    //            T item = GetItem<T>(row);
    //            data.Add(item);
    //        }
    //        return data;
    //    }
    //    private static T GetItem<T>(DataRow dr)
    //    {
    //        Type temp = typeof(T);
    //        T obj = Activator.CreateInstance<T>();

    //        foreach (DataColumn column in dr.Table.Columns)
    //        {
    //            foreach (PropertyInfo pro in temp.GetProperties())
    //            {
    //                if (pro.Name == column.ColumnName)
    //                    pro.SetValue(obj, dr[column.ColumnName], null);
    //                else
    //                    continue;
    //            }
    //        }
    //        return obj;
    //    }
    //}

    public static class TableToList
    {
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                data.Add(GetItem<T>(row));
            }

            return data;
        }

        private static T GetItem<T>(DataRow row)
        {
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in row.Table.Columns)
            {
                var property = typeof(T).GetProperty(column.ColumnName);

                if (property == null || !property.CanWrite)
                    continue;

                object value = row[column];

                // ✅ CRITICAL FIX
                if (value == DBNull.Value)
                {
                    property.SetValue(obj, null);
                    continue;
                }

                Type targetType = Nullable.GetUnderlyingType(property.PropertyType)
                                  ?? property.PropertyType;

                property.SetValue(obj, Convert.ChangeType(value, targetType));
            }

            return obj;
        }
    }
}
