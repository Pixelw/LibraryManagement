using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Chapter12_winform.utils {
    public class ExcelUtils {
        public static void ExportToExcel(DataTable dataTable, string fileName, bool isOpen = false) {
            var lines = new List<string>();
            string[] columnNames = dataTable.Columns
                .Cast<DataColumn>()
                .Select(column => column.ColumnName)
                .ToArray();
            var header = string.Join(",", columnNames);
            lines.Add(header);
            var valueLines = dataTable.AsEnumerable()
                .Select(row => string.Join(",", row.ItemArray));
            lines.AddRange(valueLines);
            File.WriteAllLines(fileName, lines, Encoding.UTF8);
            if (isOpen)
                Process.Start(fileName);
        }
    }
}