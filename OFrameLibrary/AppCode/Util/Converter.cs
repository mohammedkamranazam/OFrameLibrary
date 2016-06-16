using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace OFrameLibrary.Util
{
    public static class Converter
    {
        public static DataTable ConvertTo<T>(IList<T> list)
        {
            var table = CreateTable<T>();

            var entityType = typeof(T);

            var properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                var row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    var item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public static IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            var rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        public static T CreateItem<T>(DataRow row)
        {
            var obj = default(T);

            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    var prop = obj.GetType().GetProperty(column.ColumnName);

                    var value = row[column.ColumnName];
                    prop.SetValue(obj, value, null);
                }
            }

            return obj;
        }

        public static DataTable CreateTable<T>()
        {
            var entityType = typeof(T);

            var table = new DataTable(entityType.Name);

            var properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }
    }
}