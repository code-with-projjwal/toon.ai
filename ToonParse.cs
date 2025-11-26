using System;
using System.Data;

namespace Toon.AI
{
    public class ToonParse
    {
        private static string tokenize(string token)
        {
            const string escape_token = "\\",
                         quote = "\"",
                         ln = "\n",
                         ret = "\r";

            string encloser_beg = quote,
                   encloser_end = quote;

            if (token.Contains(escape_token))
                token = token.Replace(escape_token, escape_token + escape_token);

            if (token.Contains(quote))
                token = token.Replace(quote, escape_token+quote);

            if (token.Contains(ln))
                token = token.Replace(ln, escape_token + "n"); 
            
            if (token.Contains(ret))
                token = token.Replace(ret, escape_token + "r");

            if (
               token.Contains(escape_token) ||
               token.Contains("[") ||
               token.Contains("]") ||
               token.Contains("{") ||
               token.Contains("}") ||
               token.Contains(",") ||
               token.Contains(":")
              )
                token = encloser_beg + token + encloser_end;

            return token;
        }
        public static string FromDataTable(DataTable input)
        {
            string toon = "";
            string col_seperator = ",";
            string ln_seperator = "\n";

            if (input == null || input.Rows.Count == 0)
                return toon;

            string tableToken = string.IsNullOrEmpty(input.TableName)?"table":input.TableName;
            toon += tokenize(tableToken) + "[" + input.Rows.Count.ToString() + "]";

            string column = "", columns= "";
            int colCount = input.Columns.Count;
            foreach (DataColumn col in input.Columns)
            {
                column = col.ColumnName;
                column = string.IsNullOrEmpty(column) ? "undefined" : column;
                column = tokenize(column);
                columns += (String.IsNullOrEmpty(columns) ? "" : col_seperator) + column;
            }

            toon += "{"+columns+"}:"+ln_seperator;

            string cell, line, csv = "";
            foreach (DataRow row in input.Rows)
            {
                line = "";
                for (int col = 0; col < colCount; col++)
                {
                    cell = (row[col] == DBNull.Value || row[col] == null) ? "":  row[col].ToString();
                    cell = tokenize(cell);
                    line+= (String.IsNullOrEmpty(line) ? "" : col_seperator) + cell;
                }
                csv += (String.IsNullOrEmpty(csv) ? "" : ln_seperator) + "  " +line;
            }

            toon += csv;

            return toon;
        }
    }
}
