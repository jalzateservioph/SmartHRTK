using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace TKProcessor.Common
{
    public class ExcelFileHandler
    {
        public static DataSet ImportFile(string filename)
        {
            try
            {
                DataSet ds = null;

                // Open the Excel file using ClosedXML.
                // Keep in mind the Excel file cannot be open when trying to read it
                using (XLWorkbook workBook = new XLWorkbook(filename))
                {
                    ds = new DataSet(Path.GetFileNameWithoutExtension(filename));

                    for (int worksheetCounter = 1; worksheetCounter <= workBook.Worksheets.Count; worksheetCounter++)
                    {
                        //Read the first Sheet from Excel file.
                        IXLWorksheet workSheet = workBook.Worksheet(worksheetCounter);

                        //Create a new DataTable.
                        DataTable dt = new DataTable();

                        //Loop through the Worksheet rows.
                        bool firstRow = true;
                        foreach (IXLRow row in workSheet.Rows())
                        {
                            //Use the first row to add columns to DataTable.
                            if (firstRow)
                            {
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dt.Columns.Add(cell.Value.ToString().Replace("/", " "));
                                }

                                firstRow = false;
                            }
                            else
                            {
                                if (row.FirstCellUsed() == null)
                                    continue;

                                //Add rows to DataTable.
                                dt.Rows.Add();
                                int i = 0;


                                foreach (IXLCell cell in row.Cells(1, dt.Columns.Count))
                                {
                                    dt.Rows[dt.Rows.Count - 1][i] = cell.Value?.ToString() ?? "";

                                    i++;
                                }
                            }
                        }

                        ds.Tables.Add(dt);
                    }

                    return ds;
                }
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                    throw new Exception(ex.Message + " at  ExcelReader.ImportExceltoDatatable(..)");
                else
                    throw new Exception("There was a problem importing the file. Please verify the file's data and try again.");
            }
        }

        public static DataTable Import(string filename)
        {
            try
            {
                // Open the Excel file using ClosedXML.
                // Keep in mind the Excel file cannot be open when trying to read it
                using (XLWorkbook workBook = new XLWorkbook(filename))
                {
                    //Read the first Sheet from Excel file.
                    IXLWorksheet workSheet = workBook.Worksheet(1);

                    //Create a new DataTable.
                    DataTable dt = new DataTable();

                    //Loop through the Worksheet rows.
                    bool firstRow = true;
                    foreach (IXLRow row in workSheet.Rows())
                    {
                        //Use the first row to add columns to DataTable.
                        if (firstRow)
                        {
                            foreach (IXLCell cell in row.Cells())
                            {
                                dt.Columns.Add(cell.Value.ToString().Replace("/", " "));
                            }

                            firstRow = false;
                        }
                        else
                        {
                            if (row.FirstCellUsed() == null)
                                continue;

                            //Add rows to DataTable.
                            dt.Rows.Add();
                            int i = 0;


                            foreach (IXLCell cell in row.Cells(1, dt.Columns.Count))
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell.Value?.ToString() ?? "";

                                i++;
                            }
                        }
                    }

                    return dt;
                }
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                    throw new Exception(ex.Message + " at  ExcelReader.ImportExceltoDatatable(..)");
                else
                    throw new Exception("There was a problem importing the file. Please verify the file's data and try again.");
            }
        }

        public static void Export(string filename, DataTable table)
        {
            table.TableName = "Sheet1";

            XLWorkbook w = new XLWorkbook();

            w.AddWorksheet(table);

            w.SaveAs(filename);
        }

        public static void Export(string filename, IEnumerable<string> columns)
        {
            var dt = new DataTable();

            foreach (var column in columns)
                dt.Columns.Add(column);

            Export(filename, dt);
        }
    }
}
