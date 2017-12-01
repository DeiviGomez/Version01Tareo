using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Globalization;
using System.Threading;
using System.Data;
using System.IO;
using System.Web;
using System.Security.AccessControl;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using xAPI.Library.Base;
using System.Drawing;

namespace xAPI.Library.General
{
    public class clsExcel
    {
        public void ExportExcel(System.Data.DataTable dt, String savePath)
        {
            if (dt == null || dt.Rows.Count == 0)
                return;
            Application xlApp = new Application();

            if (xlApp == null)
            {
                return;
            }

            CultureInfo CurrentCI = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Workbooks workbooks = xlApp.Workbooks;
            Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet worksheet = (Worksheet)workbook.Worksheets[1];
            Range range;
            long totalCount = dt.Rows.Count;
            long rowRead = 0;
            float percent = 0;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                range.Interior.ColorIndex = 15;
                range.Font.Bold = true;
            }
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = dt.Rows[r][i].ToString();
                }
                rowRead++;
                percent = ((float)(100 * rowRead)) / totalCount;
            }
            //Autoajustar columnas
            worksheet.Columns.AutoFit();
            //Abrir excel
            xlApp.Visible = false;

            //workbook.Saved = true;            
            //workbook.SaveCopyAs(savePath);  

            //workbook.SaveAs(savePath, XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);           

            workbook.Close(true, Type.Missing, Type.Missing);
            workbook = null;
            xlApp.Quit();
            xlApp = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();

        }


        //Fila limitada de la versión anterior de Excel por hoja
        const int rowLimit = 65000;

        private string getWorkbookTemplate()
        {
            var sb = new StringBuilder();
            sb.Append("<xml version>\r\n<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n");
            sb.Append(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n	xmlns:x=\"urn:schemas- microsoft-com:office:excel\"\r\n	xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">\r\n");
            sb.Append(" <Styles>\r\n <Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n <Alignment ss:Vertical=\"Bottom\"/>\r\n <Borders/>");
            sb.Append("\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>\r\n	<Protection/>\r\n </Style>\r\n	<Style ss:ID=\"BoldColumn\">\r\n <Font ");
            sb.Append("x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n <Interior ss:Color=\"#DFD8DF\" ss:Pattern=\"Solid\"/> </Style>\r\n <Style ss:ID=\"s62\">\r\n <NumberFormat");
            sb.Append(" ss:Format=\"@\"/>\r\n </Style>\r\n<Style ss:ID=\"s23\"><Interior ss:Color=\"#800080\" ss:Pattern=\"Solid\"/></Style>\r\n <Style ss:ID=\"Decimal\">\r\n <NumberFormat ss:Format=\"0.0000\"/>\r\n </Style>\r\n ");
            sb.Append("<Style ss:ID=\"Integer\">\r\n <NumberFormat ss:Format=\"0\"/>\r\n </Style>\r\n <Style ss:ID=\"DateLiteral\">\r\n <NumberFormat ");
            sb.Append("ss:Format=\"mm/dd/yyyy;@\"/>\r\n </Style>\r\n <Style ss:ID=\"s28\">\r\n");
            sb.Append("<Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Top\" ss:ReadingOrder=\"LeftToRight\" ss:WrapText=\"1\"/>\r\n");
            sb.Append("<Font x:CharSet=\"1\" ss:Size=\"9\" ss:Color=\"#808080\" ss:Underline=\"Single\"/>\r\n");
            sb.Append("<Interior ss:Color=\"#FFFFFF\" ss:Pattern=\"Solid\"/> </Style>\r\n</Styles>\r\n {0}</Workbook>");
            return sb.ToString();
        }

        private string replaceXmlChar(string input)
        {
            input = input.Replace("&", "&");
            input = input.Replace("<", "<");
            input = input.Replace(">", ">");
            input = input.Replace("\"", "\"");
            input = input.Replace("'", "'");
            return input;
        }

       private string getWorksheets(DataSet source)
        {
            var sw = new StringWriter();
            if (source == null || source.Tables.Count == 0)
            {
                sw.Write("<Worksheet ss:Name=\"Sheet1\"><Table><Row> <Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\"></Data> </Cell></Row></Table></Worksheet>");
                return sw.ToString();
            }
            foreach (System.Data.DataTable dt in source.Tables)
            {
                if (dt.Rows.Count == 0)
                    sw.Write("<Worksheet ss:Name=\"" + replaceXmlChar(dt.TableName) + "\"><Table><Row><Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\"></Data></Cell></Row></Table></Worksheet>");
                else
                {
                    //Escribe por cada fila
                    var sheetCount = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if ((i % rowLimit) == 0)
                        {
                            //Agrega etiquetas de cierre para la hoja anterior de la misma tabla de datos
                            if ((i / rowLimit) > sheetCount)
                            {
                                sw.Write("</Table></Worksheet>");
                                sheetCount = (i / rowLimit);
                            }
                            sw.Write("<Worksheet ss:Name=\"" + replaceXmlChar(dt.TableName) + (((i / rowLimit) == 0) ? "" : Convert.ToString(i / rowLimit)) + "\"><Table>");
                            //Escribir fila de la columna Nombre
                            foreach (DataColumn dc in dt.Columns)
                                sw.Write("<Column ss:AutoFitWidth=\"1\"/ ss:Width=\"100\">");
                            sw.Write("<Row>");
                            foreach (DataColumn dc in dt.Columns)
                                sw.Write(string.Format("<Cell ss:StyleID=\"BoldColumn\">	<Data ss:Type=\"String\">{0}</Data></Cell>", replaceXmlChar(dc.ColumnName)));
                            sw.Write("</Row>\r\n");
                        }
                        sw.Write("<Row>\r\n");
                        foreach (DataColumn dc in dt.Columns)
                            sw.Write(string.Format("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">{0}</Data></Cell>", replaceXmlChar(dt.Rows[i][dc.ColumnName].ToString())));
                        sw.Write("</Row>\r\n");
                    }
                    sw.Write("</Table></Worksheet>");
                }
            }

            return sw.ToString();
        }
        public string GetExcelXml(System.Data.DataTable dtInput, string filename)
        {
            var excelTemplate = getWorkbookTemplate();
            var ds = new DataSet();
            ds.Tables.Add(dtInput.Copy());
            var worksheets = getWorksheets(ds);
            var excelXml = string.Format(excelTemplate, worksheets);
            return excelXml;
        }

        public string GetExcelXml(DataSet dsInput, string filename)
        {
            var excelTemplate = getWorkbookTemplate();
            var worksheets = getWorksheets(dsInput);
            var excelXml = string.Format(excelTemplate, worksheets);
            return excelXml;
        }

        public void ToExcel(DataSet dsInput, string filename, HttpResponse response)
        {
            var excelXml = GetExcelXml(dsInput, filename);
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            //response.ContentType = "application/vnd.xls";
            // response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.AppendHeader("Content-disposition", "attachment; filename=" + Path.GetFileName(filename));
            response.Write(excelXml);
            response.Flush();
            response.End();
        }
        //public String ToExcel(System.Data.DataTable dtInput, string filename, HttpResponse response)
        //{
        //    var ds = new DataSet();
        //    ds.Tables.Add(dtInput.Copy());
        //    ToExcel(ds, filename, response);
        //}
        public String ToExcelOledb(System.Data.DataTable dtInput, string filename, HttpResponse response)
        {
            var ds = new DataSet();
            ds.Tables.Add(dtInput.Copy());
            // ToExcel(ds, filename, response);

            String msg = String.Empty;

            //var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            string directory = filename.Split('\\').First();
            //var directory = new DirectoryInfo(@"C:\");
            String filname = filename.Split('.').First();
            filname = filname.Split('\\').Last();
            String extension = filename.Split('.').Last();
            Int32 counter = 1;
            String newFullPath = String.Empty;
            if (File.Exists(directory.ToString() + "\\" + filname + "." + extension))
            {
                do
                {
                    String newFile = String.Format("{0}({1}).{2}", filname, counter, extension);
                    newFullPath = Path.Combine(directory.ToString() + "\\", newFile);
                    filename = newFullPath;
                    counter++;
                } while (File.Exists(newFullPath));
            }
            else
            {
                filename = String.Empty;
                filename = directory.ToString() + "\\" + filname + "." + extension;

            }

            System.Data.OleDb.OleDbConnection MyConnection = null;
            try
            {
                System.Data.OleDb.OleDbCommand myCommand = new System.Data.OleDb.OleDbCommand();
                string sql = null;
                String sqltable = null;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + filename + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                myCommand.Connection = MyConnection;
                String columt = String.Empty;
                sqltable = "CREATE TABLE [Sheet1]({0})";

                for (int j = 0; j < dtInput.Columns.Count; j++)
                {
                    columt += dtInput.Columns[j].ColumnName + " " + "varchar(250)";
                    if (j < dtInput.Columns.Count - 1)
                        columt += ",";

                }
                myCommand.CommandText = string.Format(sqltable, columt);
                myCommand.ExecuteNonQuery();

                sql = "Insert into [Sheet1]({0})values({1})";
                String column = String.Empty;
                String values = String.Empty;
                for (int j = 0; j < dtInput.Columns.Count; j++)
                {
                    column += dtInput.Columns[j].ColumnName;
                    if (j < dtInput.Columns.Count - 1)
                        column += ",";

                }
                for (int i = 0; i < dtInput.Rows.Count; i++)
                {
                    for (int j = 0; j < dtInput.Columns.Count; j++)
                    {
                        values += "'" + dtInput.Rows[i][j].ToString() + "'";
                        if (j < dtInput.Columns.Count - 1)
                            values += ",";
                    }

                    myCommand.CommandText = string.Format(sql, column, values);
                    myCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                //response.Redirect("www.google.com");
            }
            finally
            {
                MyConnection.Close();
                //  transmit(filename, response);
            }
            return msg;
        }

        public void ToExcelXL(System.Data.DataTable dtInput, string filename, HttpResponse response)
        {
            XLWorkbook workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add(dtInput.TableName);
            ws.Cell(2, 2).SetValue(dtInput.TableName.ToString());
            for (int j = 0; j < dtInput.Columns.Count; j++)
            {
                int c = j + 2;
                ws.Cell(3, c).SetValue(dtInput.Columns[j].ColumnName.ToString());
            }
            for (int i = 0; i < dtInput.Rows.Count; i++)
            {
                int r = i + 4;
                for (int j = 0; j < dtInput.Columns.Count; j++)
                {
                    int c = j + 2;
                    if (!dtInput.Rows[i][j].ToString().Equals("EMPTY"))
                        ws.Cell(r, c).SetValue(dtInput.Rows[i][j].ToString());
                }
            }
            var rngTable = ws.Range(2, 2, dtInput.Rows.Count + 3, dtInput.Columns.Count + 1);
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            var rngHeaders = ws.Range(3, 2, 3, dtInput.Columns.Count + 1);
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua;

            rngTable.Cell(1, 1).Style.Font.Bold = true;
            rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
            rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngTable.Row(1).Merge();
            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            ws.Columns(2, dtInput.Columns.Count + 1).AdjustToContents();

            // Prepare the response
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");

            //// Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();   

        }
        XLWorkbook workbook = new XLWorkbook();
      
        // start:to excel dataset export
        public void ToExcelDataSetXL(System.Data.DataSet dtTables, string filename, HttpResponse response)
        {
            char chsplit = '_';
            System.Data.DataTable dtInput = null;
            String [] strSheetName = filename.Split(chsplit);
            var ws = workbook.Worksheets.Add(strSheetName[0].ToString());
            for (int t = 0; t<dtTables.Tables.Count;t++ )
            {
                dtInput = null;
                dtInput = new System.Data.DataTable();
                dtInput = dtTables.Tables[t];
              
                    
                
                // le asigna el nombre de la tabla  la cabezera
                ws.Cell(2,2+3*t).SetValue(dtInput.TableName.ToString());
                // este for recorrer los nombres de las columnas y las agrega al excel como nombres de columnas
                for (int j = 0; j < dtInput.Columns.Count; j++)
                {
                    int c = j + 2;
                    ws.Cell(3, c + 3 * t).SetValue(dtInput.Columns[j].ColumnName.ToString());
                }

                // este for recorre y agrega los campos a cada columna 
                for (int i = 0; i < dtInput.Rows.Count; i++)
                {
                    int r = i + 4;
                    for (int j = 0; j < dtInput.Columns.Count; j++)
                    {
                        int c = j + 2;
                        if (!dtInput.Rows[i][j].ToString().Equals("EMPTY"))
                            ws.Cell(r, c + 3 * t).SetValue(dtInput.Rows[i][j].ToString());
                    }
                }
                // agrega estilos a los cuadros
                var rngTable = ws.Range(2, 2 + (3 * t), dtInput.Rows.Count + 3, dtInput.Columns.Count + 1 + (3 * t));
                rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                // range(filaInicial,Columna inicial,filaFinal,ColumnaFinal)
                var rngHeaders = ws.Range(3, 2 + (3 * t), 3, dtInput.Columns.Count + 1 + (3 * t));
                rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngHeaders.Style.Font.Bold = true;
                rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua;

                rngTable.Cell(1, 1).Style.Font.Bold = true;
                rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
                rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngTable.Row(1).Merge();
                rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                ws.Columns(2 + (3 * t), 3 + (3 * t)).AdjustToContents();
            }
           

            // Prepare the response
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");

            //// Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();

        }
        //end
        // export columns name of datatable for template data import
        public void ToExcelXLTV1(System.Data.DataTable dtInput, string filename, HttpResponse response)
        {
            XLWorkbook workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add(dtInput.TableName);
            for (int j = 0; j < dtInput.Columns.Count; j++)
            {
                int c = j+1 ;
                ws.Cell(1, c).SetValue(dtInput.Columns[j].ColumnName.ToString());
            }
          

            var rngHeaders = ws.Range(1, 1, 1, dtInput.Columns.Count);
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.LightGray;
            rngHeaders.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            rngHeaders.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            rngHeaders.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            rngHeaders.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            rngHeaders.Worksheet.RowHeight = 20;
            ws.Columns(1, dtInput.Columns.Count + 1).AdjustToContents();
            
            // Prepare the response
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");

            //// Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();

        }
        //end
        public void ToExcelXLVC(System.Data.DataTable dtInput, System.Data.DataTable dtFooter, string filename, HttpResponse response, Boolean isFooter)
        {
            XLWorkbook workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add(dtInput.TableName);
            ws.Cell(2, 2).SetValue(dtInput.TableName.ToString());
            for (int j = 0; j < dtInput.Columns.Count; j++)
            {
                int c = j + 2;
                ws.Cell(3, c).SetValue(dtInput.Columns[j].ColumnName.ToString());
            }
            for (int i = 0; i < dtInput.Rows.Count; i++)
            {
                int r = i + 4;
                for (int j = 0; j < dtInput.Columns.Count; j++)
                {
                    int c = j + 2;
                    if (!dtInput.Rows[i][j].ToString().Equals("EMPTY"))
                        ws.Cell(r, c).SetValue(dtInput.Rows[i][j].ToString());
                }

            }

            if (dtFooter.Rows.Count == 1)
            {
                ws.Cell(dtInput.Rows.Count + 4, 2).SetValue("Total: $" + dtFooter.Rows[0]["SUM_CONV"].ToString());
                ws.Cell(dtInput.Rows.Count + 4, dtInput.Columns.Count).SetValue(dtFooter.Rows[0]["SUM_CONVBONUS"].ToString());
            }
   


            //for (int ii = 1; ii < dtFooter.Rows.Count; ii++)
            //{
                //Int32 tt = 0;
                //for (int jj = 0; jj < dtFooter.Rows.Count; jj++)
                //{
                //    tt++;
                //    if (!dtFooter.Columns[jj].ToString().Equals("EMPTY"))
                //    {
                //        if (tt==1)ws.Cell(8, tt).SetValue("Total: $" + dtFooter.Rows[jj][0].ToString());
                //        else ws.Cell(8, tt).SetValue(dtFooter.Rows[jj][1].ToString());
                //    }
                //}
            //}
            
                
            
            var rngTable = ws.Range(2, 2, dtInput.Rows.Count + 4, dtInput.Columns.Count + 1);
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            var rngHeaders = ws.Range(3, 2, 3, dtInput.Columns.Count + 1);
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua;

            rngTable.Cell(1, 1).Style.Font.Bold = true;
            rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
            rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngTable.Row(1).Merge();
            //rngTable.Row(7).Merge();
            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            ws.Columns(2, dtInput.Columns.Count + 1).AdjustToContents();

            var rnhfooterTotal = ws.Range(dtInput.Rows.Count + 4, 2, dtInput.Rows.Count + 4, dtInput.Columns.Count - 1).Merge();
            rnhfooterTotal.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            var rnhfooter = ws.Range(dtInput.Rows.Count + 4, dtInput.Columns.Count, dtInput.Rows.Count + 4, dtInput.Columns.Count + 1).Merge();
            rnhfooter.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            // Prepare the response
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");

            //// Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();

        }

        public void ToExcelXLV2(List<string> sendTo, System.Data.DataTable dtInput, string filename, HttpResponse response, string path, string action)
        {
            string table_name = dtInput.TableName.ToString().Replace('/', ' ');
            XLWorkbook workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add(table_name);
            ws.Cell(2, 2).SetValue(dtInput.TableName.ToString());
            for (int j = 0; j < dtInput.Columns.Count; j++)
            {
                int c = j + 2;
                ws.Cell(3, c).SetValue(dtInput.Columns[j].ColumnName.ToString());
            }
            for (int i = 0; i < dtInput.Rows.Count; i++)
            {
                int r = i + 4;
                for (int j = 0; j < dtInput.Columns.Count; j++)
                {
                    int c = j + 2;
                    if (!dtInput.Rows[i][j].ToString().Equals("EMPTY"))
                        ws.Cell(r, c).SetValue(dtInput.Rows[i][j].ToString());
                }
            }
            var rngTable = ws.Range(2, 2, dtInput.Rows.Count + 3, dtInput.Columns.Count + 1);
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            var rngHeaders = ws.Range(3, 2, 3, dtInput.Columns.Count + 1);
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua;

            rngTable.Cell(1, 1).Style.Font.Bold = true;
            rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
            rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngTable.Row(1).Merge();
            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            ws.Columns(2, dtInput.Columns.Count + 1).AdjustToContents();

            if (!action.Equals(""))
            {
                workbook.SaveAs(path + filename);
                if (!action.Equals("RUN"))
                {//DESCOMENTAR AL QUIRTAR LA EMPRESA ASIGNADA
                    //clsSendEmail.SendEmailAsyncV2(sendTo, "This is a preview file", "Preview Order Export", path + filename, filename);
                }
               
            }
            else
            {
                // Prepare the response (download the file)
                HttpResponse httpResponse = response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");
                                
                //// Flush the workbook to the Response.OutputStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);  // for showing the file 
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
            }
        }


        /*Se puso para probar la exportación a Excel sino funciona borrarlo*/
        public void ToExcelXListDataTable(List<System.Data.DataTable> dtInput, string filename, HttpResponse response, string title)
        {
            XLWorkbook workbook = new XLWorkbook();
            //var ws = workbook.Worksheets.Add(dtInput.TableName);
            var ws = workbook.Worksheets.Add(title);
            ws.Cell(2, 2).SetValue(title);
            int contador = 0;
            for (int m = 0; m < dtInput.Count; m++)
            {
                //  ws.Cell(contador+2, 2).SetValue("probando");
                for (int j = 0; j < dtInput[m].Columns.Count; j++)
                {
                    int c = j + 2;
                    ws.Cell(contador + 3, c).SetValue(dtInput[m].Columns[j].ColumnName.ToString());
                }
                for (int i = 0; i < dtInput[m].Rows.Count; i++)
                {
                    int r = i + contador + 4;
                    for (int j = 0; j < dtInput[m].Columns.Count; j++)
                    {
                        int c = j + 2;
                        if (!dtInput[m].Rows[i][j].ToString().Equals("EMPTY"))
                            ws.Cell(r, c).SetValue(dtInput[m].Rows[i][j]);
                    }
                }
                var rngTable = ws.Range(contador + 2, 2, dtInput[m].Rows.Count + 3 + contador, dtInput[m].Columns.Count + 1);
                rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                var rngHeaders = ws.Range(contador + 3, 2, contador + 3, dtInput[m].Columns.Count + 1);
                rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngHeaders.Style.Font.Bold = true;
                rngHeaders.Style.Fill.BackgroundColor = XLColor.White;

                rngTable.Cell(1, 1).Style.Font.Bold = true;
                rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.White;
                rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngTable.Row(1).Merge();
                rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                ws.Columns(contador + 2, dtInput[m].Columns.Count + 1).AdjustToContents();

                contador = 3 + contador + dtInput[m].Rows.Count;


            }


            // Prepare the response
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");

            //// Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();

        }

        /*Aqui termina la función para Exportar en excel*/

        #region EXPORTAR EXCEL AFP
        public void ToExcelXListDatatableAFP(System.Data.DataTable dtInput, string filename, HttpResponse response, string title)
        {
            XLWorkbook workbook = new XLWorkbook();

            String FechaTotal = "";

            String fecha2 = Convert.ToString(DateTime.Now.ToString("MMMM"));
            String anho = Convert.ToString(DateTime.Now.Year);
            FechaTotal = fecha2 + " " + anho;
            if (dtInput.Rows.Count > 0)
            {
                DataRow row = dtInput.Rows[0];
                var ws = workbook.Worksheets.Add(title + " "+ FechaTotal);

                ws.Cell("A1").Value = "Modelo de archivo para declarar aportes en AFPnet a partir del 11 de abril de 2016. \r\n \r\n" + "Antes de usar elimine las filas 1,2 y 3, guarde y cierre el archivo.";
                ws.Range("A1:Q1").Row(1).Merge();
                ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("A1").Style.Font.Bold = true;
                ws.Cell("A1").Style.Font.FontSize = 12;
                ws.Cell("A1").Style.Font.FontName = "Arial";
                ws.Cell("A1").Style.Fill.BackgroundColor = XLColor.CelestialBlue;
                ws.Range("A1:Q1").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("1:1").Height = 79.00;

                #region cabecera
                ws.Cell("A2").Value = "Número de \r\n" + "Secuencia";
                ws.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("A2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("A2").Style.Font.Bold = true;
                ws.Cell("A2").Style.Font.FontSize = 9;
                ws.Cell("A2").Style.Font.FontName = "Arial";
                ws.Cell("A2").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("A2:A2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("A:A").Width = 11;

                ws.Cell("B2").Value = "CUSPP";
                ws.Cell("B2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("B2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("B2").Style.Font.Bold = true;
                ws.Cell("B2").Style.Font.FontSize = 9;
                ws.Cell("B2").Style.Font.FontName = "Arial";
                ws.Cell("B2").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("B2:B2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("B:B").Width = 15;

                ws.Cell("C2").Value = "Tipo de \r\n" + "documento de \r\n" + "identidad";
                ws.Cell("C2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("C2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("C2").Style.Font.Bold = true;
                ws.Cell("C2").Style.Font.FontSize = 9;
                ws.Cell("C2").Style.Font.FontName = "Arial";
                ws.Cell("C2").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("C2:C2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("C:C").Width = 15;

                ws.Cell("D2").Value = "Número de \r\n" + "documento de \r\n" + "identidad";
                ws.Cell("D2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("D2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("D2").Style.Font.Bold = true;
                ws.Cell("D2").Style.Font.FontSize = 9;
                ws.Cell("D2").Style.Font.FontName = "Arial";
                ws.Cell("D2").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("D2:D2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("D:D").Width = 18;

                ws.Cell("E2").Value = "Apellido Paterno";
                ws.Cell("E2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("E2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("E2").Style.Font.Bold = true;
                ws.Cell("E2").Style.Font.FontSize = 9;
                ws.Cell("E2").Style.Font.FontName = "Arial";
                ws.Cell("E2").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("E2:E2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("E:E").Width = 16;

                ws.Cell("F2").Value = "Apellido Materno";
                ws.Cell("F2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("F2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("F2").Style.Font.Bold = true;
                ws.Cell("F2").Style.Font.FontSize = 9;
                ws.Cell("F2").Style.Font.FontName = "Arial";
                ws.Cell("F2").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("F2:F2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("F:F").Width = 16;

                ws.Cell("G2").Value = "Nombres";
                ws.Cell("G2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("G2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("G2").Style.Font.Bold = true;
                ws.Cell("G2").Style.Font.FontSize = 9;
                ws.Cell("G2").Style.Font.FontName = "Arial";
                ws.Cell("G2").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("G2:G2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("G:G").Width = 20;

                ws.Cell("H2").Value = "Relación \r\n" + "Laboral";
                ws.Cell("H2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("H2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("H2").Style.Font.Bold = true;
                ws.Cell("H2").Style.Font.FontSize = 9;
                ws.Cell("H2").Style.Font.FontName = "Arial";
                ws.Cell("H2").Style.Fill.BackgroundColor = XLColor.Melon;
                ws.Range("H2:H2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("H:H").Width = 14;

                ws.Cell("I2").Value = "Inicio de RL";
                ws.Cell("I2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("I2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("I2").Style.Font.Bold = true;
                ws.Cell("I2").Style.Font.FontSize = 9;
                ws.Cell("I2").Style.Font.FontName = "Arial";
                ws.Cell("I2").Style.Fill.BackgroundColor = XLColor.Melon;
                ws.Range("I2:I2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("I:I").Width = 14;

                ws.Cell("J2").Value = "Cese de RL";
                ws.Cell("J2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("J2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("J2").Style.Font.Bold = true;
                ws.Cell("J2").Style.Font.FontSize = 9;
                ws.Cell("J2").Style.Font.FontName = "Arial";
                ws.Cell("J2").Style.Fill.BackgroundColor = XLColor.Melon;
                ws.Range("J2:J2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("J:J").Width = 14;

                ws.Cell("K2").Value = "Excepcion de Aportar";
                ws.Cell("K2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("K2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("K2").Style.Font.Bold = true;
                ws.Cell("K2").Style.Font.FontSize = 9;
                ws.Cell("K2").Style.Font.FontName = "Arial";
                ws.Cell("K2").Style.Fill.BackgroundColor = XLColor.Melon;
                ws.Range("K2:K2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("K:K").Width = 32;

                ws.Cell("L2").Value = "Remuneración \r\n" + "Asegurable";
                ws.Cell("L2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("L2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("L2").Style.Font.Bold = true;
                ws.Cell("L2").Style.Font.FontSize = 9;
                ws.Cell("L2").Style.Font.FontName = "Arial";
                ws.Cell("L2").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("L2:L2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("L:L").Width = 18;

                ws.Cell("M2").Value = "Aporte \r\n" + "voluntario del \r\n" + "afiliado con fin";
                ws.Cell("M2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("M2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("M2").Style.Font.Bold = true;
                ws.Cell("M2").Style.Font.FontSize = 9;
                ws.Cell("M2").Style.Font.FontName = "Arial";
                ws.Cell("M2").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("M2:M2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("M:M").Width = 15;

                ws.Cell("N2").Value = "Aporte \r\n" + "voluntario del \r\n" + "afiliado sin fin";
                ws.Cell("N2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("N2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("N2").Style.Font.Bold = true;
                ws.Cell("N2").Style.Font.FontSize = 9;
                ws.Cell("N2").Style.Font.FontName = "Arial";
                ws.Cell("N2").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("N2:N2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("N:N").Width = 15;

                ws.Cell("O2").Value = "Aporte voluntario del \r\n" + "empleador";
                ws.Cell("O2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("O2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("O2").Style.Font.Bold = true;
                ws.Cell("O2").Style.Font.FontSize = 9;
                ws.Cell("O2").Style.Font.FontName = "Arial";
                ws.Cell("O2").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("O2:O2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("O:O").Width = 23;

                ws.Cell("P2").Value = "Tipo de trabajo \r\n" + "o Rubro";
                ws.Cell("P2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("P2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("P2").Style.Font.Bold = true;
                ws.Cell("P2").Style.Font.FontSize = 9;
                ws.Cell("P2").Style.Font.FontName = "Arial";
                ws.Cell("P2").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("P2:P2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("P:P").Width = 23;

                ws.Cell("Q2").Value = "AFP\r\n" + "(Conviene\r\n" + "dejar en";
                ws.Cell("Q2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("Q2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("Q2").Style.Font.Bold = true;
                ws.Cell("Q2").Style.Font.FontSize = 9;
                ws.Cell("Q2").Style.Font.FontName = "Arial";
                ws.Cell("Q2").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("Q2:Q2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("2:2").Height = 37.00;
                ws.Columns("Q:Q").Width = 14;

                ws.Cell("A3").Value = "Dato Obligatorio. \r\nEs un número de referencia \r\npara \r\nubicar los \r\nposibles \r\nproblemas en los mensajes\r\n de validación";
                ws.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("A3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("A3").Style.Font.FontSize = 8;
                ws.Cell("A3").Style.Font.FontName = "Arial";
                ws.Cell("A3").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("A3:A3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("A:A").Width = 11;

                ws.Cell("B3").Value = "Dato referencial para \r\nverificación.";
                ws.Cell("B3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("B3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("B3").Style.Font.FontSize = 8;
                ws.Cell("B3").Style.Font.FontName = "Arial";
                ws.Cell("B3").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("B3:B3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("B:B").Width = 15;

                ws.Cell("C3").Value = "Dato Obligatorio. \r\nJunto con el número de documento de \r\nidentidad son los \r\ndatos con los que \r\nse identifica al \r\ntrabajador.   \r\n \r\n \r\n0: DNI \r\n1: Carnet de Extranjería \r\n2: Carnet Militar y Policial \r\n3: Libreta Adolecentes Trabajador \r\n4: Pasaporte \r\n5: Inexistente / Afilia";
                ws.Cell("C3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("C3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("C3").Style.Font.FontSize = 8;
                ws.Cell("C3").Style.Font.FontName = "Arial";
                ws.Cell("C3").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("C3:C3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("C:C").Width = 15;

                ws.Cell("D3").Value = "Dato Obligatorio.\r\nJunto con el Tipo  de\r\ndocumento de identidad este\r\ndato identifica al trabajador.\r\n\r\nLas celdas deben tener\r\nformato de texto para\r\nmantener los ceros a la\r\nizquierda\r\n\r\n" +
                                      "NO declarar más de una\r\nvez el mismo trabajador a\r\nmenos que tenga\r\ndistintos 'Tipos de\r\nTrabajo'  en el mismo\r\nmes. (ver columna P)";
                ws.Cell("D3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("D3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("D3").Style.Font.FontSize = 8;
                ws.Cell("D3").Style.Font.FontName = "Arial";
                ws.Cell("D3").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("D3:D3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("D:D").Width = 18;

                ws.Cell("E3").Value = "Dato referencial para \r\nverificación.";
                ws.Cell("E3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("E3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("E3").Style.Font.FontSize = 8;
                ws.Cell("E3").Style.Font.FontName = "Arial";
                ws.Cell("E3").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("E3:E3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("E:E").Width = 16;

                ws.Cell("F3").Value = "Dato referencial para \r\nverificación.";
                ws.Cell("F3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("F3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("F3").Style.Font.FontSize = 8;
                ws.Cell("F3").Style.Font.FontName = "Arial";
                ws.Cell("F3").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("F3:F3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("F:F").Width = 16;

                ws.Cell("G3").Value = "Dato referencial para \r\nverificación.";
                ws.Cell("G3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("G3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("G3").Style.Font.FontSize = 8;
                ws.Cell("G3").Style.Font.FontName = "Arial";
                ws.Cell("G3").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("G3:G3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("G:G").Width = 20;

                ws.Cell("H3").Value = "Dato Obligatorio \r\nIndica que existe la \r\nrelación laboral en el mes \r\nde devengue \r\n \r\nS: Sí \r\nN: No - usado para \r\npagos posteriores al \r\ncese de la Relación \r\nLaboral.";
                ws.Cell("H3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("H3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("H3").Style.Font.FontSize = 8;
                ws.Cell("H3").Style.Font.FontName = "Arial";
                ws.Cell("H3").Style.Fill.BackgroundColor = XLColor.Melon;
                ws.Range("H3:H3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("H:H").Width = 14;

                ws.Cell("I3").Value = "Dato Obligatorio\r\nIndica si la relación\r\nlaboral se inició en el mes de \r\ndevengue\r\n\r\nS: Sí\r\nN: No";
                ws.Cell("I3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("I3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("I3").Style.Font.FontSize = 8;
                ws.Cell("I3").Style.Font.FontName = "Arial";
                ws.Cell("I3").Style.Fill.BackgroundColor = XLColor.Melon;
                ws.Range("I3:I3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("I:I").Width = 14;

                ws.Cell("J3").Value = "Dato Obligatorio\r\nIndica si la relación\r\nlaboral terminó en el mes de \r\ndevengue\r\n\r\nS: Sí\r\nN: No";
                ws.Cell("J3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("J3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("J3").Style.Font.FontSize = 8;
                ws.Cell("J3").Style.Font.FontName = "Arial";
                ws.Cell("J3").Style.Fill.BackgroundColor = XLColor.Melon;
                ws.Range("J3:J3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("J:J").Width = 14;

                ws.Cell("K3").Value = "Indica que no corresponde aportar en el mes de\r\ndevengue por alguno de los motivos de \r\nexcepción previstos en las normas.\r\nSi no existe la excepción, este dato se debe dejar en blanco.\r\n\r\n" +
                                      "IMPORTANTE: Para declarar una excepción, el\r\nempleador debe estar en capacidad de\r\nsustentarla.\r\n\r\nVacío = Existe obligación de aportar.\r\nL = Licencia sin goce de haber.\r\n" +
                                      "U = Subsidio pagado directamente por ESSALUD al trabajador.\r\nJ = Afiliado pensionado por jubilación.\r\nI = Afiliado pensionado por invalidez.\r\nP = Aporte postergado por inicio de la Relación\r\n"+
                                      "Laboral posterior al cierre de planillas.\r\nO = Otro motivo por el cual el trabajador no percibió remuneración por el mes.";
                ws.Cell("K3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("K3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("K3").Style.Font.FontSize = 8;
                ws.Cell("K3").Style.Font.FontName = "Arial";
                ws.Cell("K3").Style.Fill.BackgroundColor = XLColor.Melon;
                ws.Range("K3:K3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("K:K").Width = 32;

                ws.Cell("L3").Value = "Dato Obligatorio. \r\n Es el monto de remuneración \r\n sobre el cual el sistema \r\n calculará los aportes \r\n siguientes aportes:\r\n-Aporte obligatorio \r\n- Prima de seguro \r\n-Comisión AFP\r\n \r\n" +
                                      "(Si es trabajo de riesgo) \r\n - Aporte obligatorio \r\n complementario \r\n del trabajador.\r\n- Aporte obligatorio \r\n complementario \r\n del empleador. \r\n \r\n Si no hay remuneración, \r\n debe informarse cero (0).";
                ws.Cell("L3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("L3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("L3").Style.Font.FontSize = 8;
                ws.Cell("L3").Style.Font.FontName = "Arial";
                ws.Cell("L3").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("L3:L3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("L:L").Width = 18;

                ws.Cell("M3").Value = "Si no hay aporte \r\nvoluntario con fin \r\nprevisional del afiliado, \r\ninformarse cero (0).";
                ws.Cell("M3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("M3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("M3").Style.Font.FontSize = 8;
                ws.Cell("M3").Style.Font.FontName = "Arial";
                ws.Cell("M3").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("M3:M3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("M:M").Width = 15;

                ws.Cell("N3").Value = "Si no hay aporte \r\nvoluntario sin fin \r\nprevisional del afiliado, \r\ninformarse cero (0).";
                ws.Cell("N3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("N3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("N3").Style.Font.FontSize = 8;
                ws.Cell("N3").Style.Font.FontName = "Arial";
                ws.Cell("N3").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("N3:N3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("N:N").Width = 15;

                ws.Cell("O3").Value = "Si no hay aporte voluntario del \r\nempleador, debe informarse cero (0).";
                ws.Cell("O3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("O3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("O3").Style.Font.FontSize = 8;
                ws.Cell("O3").Style.Font.FontName = "Arial";
                ws.Cell("O3").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("O3:O3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("O:O").Width = 23;

                ws.Cell("P3").Value = "Dato Obligatorio.\r\nN = Dependiente Normal \r\nC = Dependiente Contrucción \r\nM = Dependiente Minería \r\nP = Pesquero \r\n \r\nSi tiene al menos un trabajador \r\npesquero recuerde que debe cargar un \r\narchivo adicional con las semanas \r\ncontributivas";
                ws.Cell("P3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("P3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("P3").Style.Font.FontSize = 8;
                ws.Cell("P3").Style.Font.FontName = "Arial";
                ws.Cell("P3").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("P3:P3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("P:P").Width = 23;

                ws.Cell("Q3").Value = "Dato referencial.\r\nEl sistema determina la \r\nAFP a la que pertence \r\nel trabajador.\r\n \r\nConviene dejar en \r\nblanco. \r\n \r\nEl sistema dará un \r\nmensaje de \r\nadvertencia, indicando \r\nque la AFP informada \r\nno se encuentra \r\nregistrada y que se \r\nreemplaza por la que \r\nse encuentra en base \r\nde datos";
                ws.Cell("Q3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell("Q3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                ws.Cell("Q3").Style.Font.FontSize = 8;
                ws.Cell("Q3").Style.Font.FontName = "Arial";
                ws.Cell("Q3").Style.Fill.BackgroundColor = XLColor.LightYellow;
                ws.Range("Q3:Q3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Rows("3:3").Height = 258.80;
                ws.Columns("Q:Q").Width = 14;

                #endregion

                int i = 0;
                foreach (System.Data.DataRow item in dtInput.Rows)
                {
                    i++;

                    ws.Cell("A" + (3 + i)).Value = item["ORDEN"].ToString();
                    //ws.Cell("A" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("B" + (3 + i)).Value = item["CUSPP"].ToString();
                    //ws.Cell("B" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("C" + (3 + i)).Value = item["TIPO_DOCUMENTO"].ToString();
                    //ws.Cell("B" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("D" + (3 + i)).Value = item["NUMERO_DOCUMENTO"].ToString();
                    //ws.Cell("B" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("E" + (3 + i)).Value = item["APELLIDO_PATERNO"].ToString();
                    //ws.Cell("B" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("F" + (3 + i)).Value = item["APELLIDO_MATERNO"].ToString();
                    //ws.Cell("B" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("G" + (3 + i)).Value = item["NOMBRES"].ToString();
                    //ws.Cell("B" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("H" + (3 + i)).Value = item["RELACION_LABORAL"].ToString();
                    //ws.Cell("B" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("I" + (3 + i)).Value = item["INICIO_RL"].ToString();
                    //ws.Cell("B" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("J" + (3 + i)).Value = item["CESE_RL"].ToString();
                    //ws.Cell("B" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("K" + (3 + i)).Value = item["EXCEPCION"].ToString();
                    //ws.Cell("B" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("L" + (3 + i)).Value = item["REMUNERACION_ASEGURABLE"].ToString();
                    //ws.Cell("B" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("M" + (3 + i)).Value = item["APORTE_VOLUNTARIO_FIN"].ToString();
                    //ws.Cell("B" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("N" + (3 + i)).Value = item["APORTE_VOLUNTARIO_SIN_FIN"].ToString();
                    //ws.Cell("B" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("O" + (3 + i)).Value = item["APORTE_VOLUNTARIO_EMPLEADOR"].ToString();
                    //ws.Cell("B" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("P" + (3 + i)).Value = item["RUBRO"].ToString();
                    //ws.Cell("B" + (3 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                }

                // Prepare the response
                HttpResponse httpResponse = response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");

                //// Flush the workbook to the Response.OutputStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
            }
        }
        #endregion


        public void ToExcelReporteDetalleAFPTable(System.Data.DataTable dtInpunt, String filename, HttpResponse response, String title) {
            XLWorkbook workbook = new XLWorkbook();
            String FechaTotal = "";

            String fecha2 = Convert.ToString(DateTime.Now.ToString("MMMM"));
            String anho = Convert.ToString(DateTime.Now.Year);
            FechaTotal = fecha2 + " " + anho;
            if (dtInpunt.Rows.Count > 0) {
                DataRow row = dtInpunt.Rows[0];
                var ws = workbook.Worksheets.Add(title + " " + FechaTotal);

                #region CABECERA
                ws.Cell("B7").Value = "#";
                ws.Range("B7:B9").Column(1).Merge();
                ws.Cell("B7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("B7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("B7").Style.Font.Bold = true;
                ws.Range("B7:B9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Columns("B:B").Width = 4.14;

                //ws.Cell("B7").Value = "AFP";
                //ws.Range("B7:B9").Column(1).Merge();
                //ws.Cell("B7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                //ws.Cell("B7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                //ws.Cell("B7").Style.Font.Bold = true;
                //ws.Range("B7:B9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                //ws.Columns("B:B").Width = 20;

                ws.Cell("C7").Value = "APELLIDOS Y NOMBRES";
                ws.Range("C7:C9").Column(1).Merge();
                ws.Cell("C7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("C7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("C7").Style.Font.Bold = true;
                ws.Range("C7:C9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Columns("C:C").Width = 40;

                ws.Cell("D7").Value = "REMUNERACION\r\nASEGURABLE";
                ws.Range("D7:D9").Column(1).Merge();
                ws.Cell("D7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("D7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("D7").Style.Font.Bold = true;
                ws.Range("D7:D9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Columns("D:D").Width = 15;

                ws.Cell("E7").Value = "AFP";
                ws.Range("E7:K7").Row(1).Merge();
                ws.Cell("E7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("E7").Style.Font.Bold = true;
                ws.Range("E7:K7").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("E8").Value = "APORTE\r\nOBLIGATORIO";
                ws.Range("E8:E9").Column(1).Merge();
                ws.Cell("E8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("E8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("E8").Style.Font.Bold = true;
                ws.Range("E8:E9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Columns("E:E").Width = 15;

                ws.Cell("F8").Value = "APORTES \r\nCOMPLEMENTARIOS";
                ws.Range("F8:G8").Row(1).Merge();
                ws.Cell("F8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("F8").Style.Font.Bold = true;
                ws.Rows("8:8").Height = 27;
                ws.Range("F8:G8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("F9").Value = "TRABAJADOR";
                ws.Range("F9:F9").Column(1).Merge();
                ws.Cell("F9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("F9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("F9").Style.Font.Bold = true;
                ws.Range("F9:F9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Columns("F:F").Width = 15;

                ws.Cell("G9").Value = "EMPLEADOR";
                ws.Range("G9:G9").Column(1).Merge();
                ws.Cell("G9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("G9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("G9").Style.Font.Bold = true;
                ws.Range("G9:G9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Columns("G:G").Width = 15;

                ws.Cell("H8").Value = "APORTES \r\nVOLUNTARIOS";
                ws.Range("H8:I8").Row(1).Merge();
                ws.Cell("H8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("H8").Style.Font.Bold = true;
                ws.Rows("8:8").Height = 27;
                ws.Range("H8:H8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("H9").Value = "TRABAJADOR";
                ws.Range("H9:H9").Column(1).Merge();
                ws.Cell("H9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("H9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("H9").Style.Font.Bold = true;
                ws.Range("H9:H9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Columns("H:H").Width = 15;

                ws.Cell("I9").Value = "EMPLEADOR";
                ws.Range("I9:I9").Column(1).Merge();
                ws.Cell("I9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("I9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("I9").Style.Font.Bold = true;
                ws.Range("I9:I9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Columns("I:I").Width = 15;

                ws.Cell("J8").Value = "PRIMA DE\r\nSEGURO";
                ws.Range("J8:J9").Column(1).Merge();
                ws.Cell("J8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("J8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("J8").Style.Font.Bold = true;
                ws.Range("J8:J9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Columns("J:J").Width = 15;

                ws.Cell("K8").Value = "COMISION\r\nAFP";
                ws.Range("K8:K9").Column(1).Merge();
                ws.Cell("K8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("K8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("K8").Style.Font.Bold = true;
                ws.Range("K8:K9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Columns("K:K").Width = 15;

                ws.Cell("L7").Value = "TOTAL AFP";
                ws.Range("L7:L9").Column(1).Merge();
                ws.Cell("L7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("L7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("L7").Style.Font.Bold = true;
                ws.Range("L7:L9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Columns("L:L").Width = 15;

                ws.Cell("M7").Value = "ONP";
                ws.Range("M7:M9").Column(1).Merge();
                ws.Cell("M7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("M7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("M7").Style.Font.Bold = true;
                ws.Range("M7:M9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Columns("M:M").Width = 15;
                #endregion

                int i = 0;
                foreach (System.Data.DataRow item in dtInpunt.Rows)
                {
                    i++;
                    if (item["RegimenPensionId"].ToString() == "0")
                    {
                        ws.Cell("B" + (9 + i)).Value = item["NOMBRECOMPLETO"].ToString();
                        ws.Range("B" + (9 + i) + ":" + "C" + (9 + i)).Row(1).Merge();
                        ws.Cell("B" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell("B" + (9 + i)).Style.Font.Bold = true;
                        ws.Range("B" + (9 + i) + ":" + "C" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        ws.Range("D" + (9 + i) + ":" + "M" + (9 + i)).Row(1).Merge();
                        ws.Range("D" + (9 + i) + ":" + "M" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        
                    }
                    else if (item["RegimenPensionId"].ToString() == "1" || item["RegimenPensionId"].ToString() == "2")
                    {
                        ws.Cell("B" + (9 + i)).Value = item["NOMBRECOMPLETO"].ToString();
                        ws.Range("B" + (9 + i) + ":" + "C" + (9 + i)).Row(1).Merge();
                        ws.Cell("B" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell("B" + (9 + i)).Style.Font.Bold = true;
                        ws.Cell("B" + (9 + i)).Style.Fill.BackgroundColor = XLColor.LightYellow;
                        ws.Range("B" + (9 + i) + ":" + "C" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        

                        ws.Cell("D" + (9 + i)).Value = item["REMUNERACION"].ToString();
                        ws.Cell("D" + (9 + i)).Style.Font.Bold = true;
                        ws.Cell("D" + (9 + i)).Style.Fill.BackgroundColor = XLColor.LightYellow;
                        ws.Cell("D" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        ws.Cell("E" + (9 + i)).Value = item["APORTEOBLIGATORIO"].ToString();
                        ws.Cell("E" + (9 + i)).Style.Font.Bold = true;
                        ws.Cell("E" + (9 + i)).Style.Fill.BackgroundColor = XLColor.LightYellow;
                        ws.Cell("E" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        ws.Cell("F" + (9 + i)).Value = item["APORCOMPLETRABAJA"].ToString();
                        ws.Cell("F" + (9 + i)).Style.Font.Bold = true;
                        ws.Cell("F" + (9 + i)).Style.Fill.BackgroundColor = XLColor.LightYellow;
                        ws.Cell("F" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        ws.Cell("G" + (9 + i)).Value = item["APORTECOMPLEEMPLE"].ToString();
                        ws.Cell("G" + (9 + i)).Style.Font.Bold = true;
                        ws.Cell("G" + (9 + i)).Style.Fill.BackgroundColor = XLColor.LightYellow;
                        ws.Cell("G" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        ws.Cell("H" + (9 + i)).Value = item["APORTEVOLUNTRABAJA"].ToString();
                        ws.Cell("H" + (9 + i)).Style.Font.Bold = true;
                        ws.Cell("H" + (9 + i)).Style.Fill.BackgroundColor = XLColor.LightYellow;
                        ws.Cell("H" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        ws.Cell("I" + (9 + i)).Value = item["APORTEVOLUNEMPLE"].ToString();
                        ws.Cell("I" + (9 + i)).Style.Font.Bold = true;
                        ws.Cell("I" + (9 + i)).Style.Fill.BackgroundColor = XLColor.LightYellow;
                        ws.Cell("I" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        ws.Cell("J" + (9 + i)).Value = item["PRIMASEGURO"].ToString();
                        ws.Cell("J" + (9 + i)).Style.Font.Bold = true;
                        ws.Cell("J" + (9 + i)).Style.Fill.BackgroundColor = XLColor.LightYellow;
                        ws.Cell("J" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        ws.Cell("K" + (9 + i)).Value = item["COMISION"].ToString();
                        ws.Cell("K" + (9 + i)).Style.Font.Bold = true;
                        ws.Cell("K" + (9 + i)).Style.Fill.BackgroundColor = XLColor.LightYellow;
                        ws.Cell("K" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        ws.Cell("L" + (9 + i)).Value = item["TOTAL_AFP"].ToString();
                        ws.Cell("L" + (9 + i)).Style.Font.Bold = true;
                        ws.Cell("L" + (9 + i)).Style.Fill.BackgroundColor = XLColor.LightYellow;
                        ws.Cell("L" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        ws.Cell("M" + (9 + i)).Value = item["TOTAL_ONP"].ToString();
                        ws.Cell("M" + (9 + i)).Style.Font.Bold = true;
                        ws.Cell("M" + (9 + i)).Style.Fill.BackgroundColor = XLColor.LightYellow;
                        ws.Cell("M" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }else { 

                    ws.Cell("B" + (9 + i)).Value = item["ORDEN"].ToString();
                    ws.Cell("B" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    //ws.Cell("B" + (9 + i)).Value = item["DESCRIPCIONAFP"].ToString();
                    //ws.Cell("B" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("C" + (9 + i)).Value = item["NOMBRECOMPLETO"].ToString();
                    ws.Cell("C" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("D" + (9 + i)).Value = item["REMUNERACION"].ToString();
                    ws.Cell("D" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("E" + (9 + i)).Value = item["APORTEOBLIGATORIO"].ToString();
                    ws.Cell("E" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("F" + (9 + i)).Value = item["APORCOMPLETRABAJA"].ToString();
                    ws.Cell("F" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("G" + (9 + i)).Value = item["APORTECOMPLEEMPLE"].ToString();
                    ws.Cell("G" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("H" + (9 + i)).Value = item["APORTEVOLUNTRABAJA"].ToString();
                    ws.Cell("H" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("I" + (9 + i)).Value = item["APORTEVOLUNEMPLE"].ToString();
                    ws.Cell("I" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("J" + (9 + i)).Value = item["PRIMASEGURO"].ToString();
                    ws.Cell("J" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("K" + (9 + i)).Value = item["COMISION"].ToString();
                    ws.Cell("K" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("L" + (9 + i)).Value = item["TOTAL_AFP"].ToString();
                    ws.Cell("L" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("M" + (9 + i)).Value = item["TOTAL_ONP"].ToString();
                    ws.Cell("M" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }
                }

                // Prepare the response
                HttpResponse httpResponse = response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");

                //// Flush the workbook to the Response.OutputStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();

            }
        }


        /*EXPORTACION A EXCEL CON ID*/
        public void ToExcelXListDataTableID(System.Data.DataTable dtInput, string filename, HttpResponse response, string title)
        {
            XLWorkbook workbook = new XLWorkbook();
            String  fecha2 = "";


            if (dtInput.Rows.Count > 0)
            {
            
            DataRow row = dtInput.Rows[0];
          
            fecha2 = row["FechaCierreMes"].ToString() + " " + row["FechaCierreAnio"].ToString();

            var ws = workbook.Worksheets.Add(title + " " + fecha2);
    
            ws.Cell("A7").Value = "#";
            ws.Range("A7:A9").Column(1).Merge();
            ws.Cell("A7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A7").Style.Font.Bold = true;
            ws.Range("A7:A9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Columns("A:A").Width = 4.14;

            ws.Cell("B7").Value = "APELLIDOS Y NOMBRES";
            ws.Range("B7:B9").Column(1).Merge();
            ws.Cell("B7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("B7").Style.Font.Bold = true;
            ws.Range("B7:B9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Columns("B:B").Width = 30;

            ws.Cell("C7").Value = "DNI Nº";
            ws.Range("C7:C9").Column(1).Merge();
            ws.Cell("C7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("C7").Style.Font.Bold = true;
            ws.Range("C7:C9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("D7").Value = "CARGO U OCUPACIÓN";
            ws.Range("D7:D9").Column(1).Merge();
            ws.Cell("D7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("D7").Style.Font.Bold = true;
            ws.Range("D7:D9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Columns("D:D").Width = 21.86;

            ws.Cell("E7").Value = "DIAS Y HORAS LABORADAS";
            ws.Range("E7:G7").Row(1).Merge();
            ws.Cell("E7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("E7").Style.Font.Bold = true;
            ws.Range("E7:G7").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("E8").Value = "DIAS";
            ws.Range("E8:E9").Column(1).Merge();
            ws.Cell("E8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("E8").Style.Font.Bold = true;
            ws.Range("E8:E9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Columns("E:E").Width = 4.43;

            ws.Cell("F8").Value = "HORAS";
            ws.Range("F8:F9").Column(1).Merge();
            ws.Cell("F8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("F8").Style.Font.Bold = true;
            ws.Range("F8:F9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("G8").Value = "HORAS EXTRAS";
            ws.Range("G8:G9").Column(1).Merge();
            ws.Cell("G8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("G8").Style.Font.Bold = true;
            ws.Range("G8:G9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Cell("H7").Value = "SUELDO BÁSICO SEGÚN CONTRATO";
            ws.Range("H7:H9").Column(1).Merge();
            ws.Cell("H7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("H7").Style.Font.Bold = true;
            ws.Range("H7:H9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
           /* ws.Columns("D:D").Width = 21.86; */


            ws.Cell("I7").Value = "INGRESOS DEL TRABAJADOR";
            ws.Range("I7:M7").Row(1).Merge();
            ws.Cell("I7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("I7").Style.Font.Bold = true;
            ws.Range("I7:M7").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("I8").Value = "SUELDO BÁSICO";
            ws.Range("I8:I9").Column(1).Merge();
            ws.Cell("I8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("I8").Style.Font.Bold = true;
            ws.Range("I8:I9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("J8").Value = "ASIGNAC. FAMILIAR";
            ws.Range("J8:J9").Column(1).Merge();
            ws.Cell("J8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("J8").Style.Font.Bold = true;
            ws.Range("J8:J9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("K8").Value = "HORAS EXTRAS";
            ws.Range("K8:K9").Column(1).Merge();
            ws.Cell("K8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("K8").Style.Font.Bold = true;
            ws.Range("K8:K9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("L8").Value = "COMISIONES";
            ws.Range("L8:L9").Column(1).Merge();
            ws.Cell("L8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("L8").Style.Font.Bold = true;
            ws.Range("L8:L9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("M8").Value = "OTROS";
            ws.Range("M8:M9").Column(1).Merge();
            ws.Cell("M8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("M8").Style.Font.Bold = true;
            ws.Range("M8:M9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("N7").Value = "TOTAL REMUNERACIÓN BRUTA";
            ws.Range("N7:N9").Column(1).Merge();
            ws.Cell("N7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("N7").Style.Font.Bold = true;
            ws.Range("N7:N9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("O7").Value = "DESCUENTOS AL TRABAJADOR";
            ws.Range("O7:T7").Row(1).Merge();
            ws.Cell("O7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("O7").Style.Font.Bold = true;
            ws.Range("O7:T7").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("P8").Value = "AFP";
            ws.Range("P8:R8").Row(1).Merge();
            ws.Cell("P8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("P8").Style.Font.Bold = true;
            ws.Range("P8:R8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("P9").Value = "APORTE OBLIGATORIO";
            ws.Cell("P9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("P9").Style.Font.Bold = true;
            ws.Range("P9:P9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("Q9").Value = "COMISIÓN PORCENTUAL";
            ws.Cell("Q9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("Q9").Style.Font.Bold = true;
            ws.Range("Q9:Q9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("R9").Value = "PRIMA DE SEGURO";
            ws.Cell("R9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("R9").Style.Font.Bold = true;
            ws.Range("R9:R9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("O8").Value = "ONP";
            ws.Range("O8:O9").Column(1).Merge();
            ws.Cell("O8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("O8").Style.Font.Bold = true;
            ws.Range("O8:O9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Columns("O:O").Width = 6.71;


            ws.Cell("S8").Value = "RETENCIÓN RENTA 5A CATEGORIA";
            ws.Range("S8:S9").Column(1).Merge();
            ws.Cell("S8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("S8").Style.Font.Bold = true;
            ws.Range("S8:S9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Cell("T8").Value = "OTROS DESCUENTOS";
            ws.Range("T8:T9").Column(1).Merge();
            ws.Cell("T8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("T8").Style.Font.Bold = true;
            ws.Range("T8:T9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("U7").Value = "TOTAL DESCUENTOS";
            ws.Range("U7:U9").Column(1).Merge();
            ws.Cell("U7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("U7").Style.Font.Bold = true;
            ws.Range("U7:U9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("V7").Value = "REMUNERACIÓN NETA";
            ws.Range("V7:V9").Column(1).Merge();
            ws.Cell("V7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("V7").Style.Font.Bold = true;
            ws.Range("V7:V9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("W7").Value = "APORTES DEL EMPLEADOR";
            ws.Range("W7:Z7").Row(1).Merge();
            ws.Cell("W7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("W7").Style.Font.Bold = true;
            ws.Range("W7:Z7").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("W8").Value = "ESSALUD";
            ws.Range("W8:W9").Column(1).Merge();
            ws.Cell("W8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("W8").Style.Font.Bold = true;
            ws.Range("W8:W9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Cell("X8").Value = "EPS";
            ws.Range("X8:X9").Column(1).Merge();
            ws.Cell("X8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("X8").Style.Font.Bold = true;
            ws.Range("X8:X9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Cell("Y8").Value = "SCTR";
            ws.Range("Y8:Y9").Column(1).Merge();
            ws.Cell("Y8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("Y8").Style.Font.Bold = true;
            ws.Range("Y8:Y9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("Z8").Value = "TOTAL APORTES";
            ws.Range("Z8:Z9").Column(1).Merge();
            ws.Cell("Z8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("Z8").Style.Font.Bold = true;
            ws.Range("Z8:Z9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            int i = 0; double tdias = 0, tohoras = 0, textras = 0, sueltbasico = 0, tasignacion = 0, thorasextras = 0,
                              tcomiciones = 0, totros = 0,tremueneracionbruta = 0, tonp = 0, taporteobligatorio = 0, 
                              tcomisionporcentual = 0, tprima = 0, t5categoria = 0, totrosdescuentos = 0,
                              tremuneracionneta = 0, tesalud = 0, tepsd=0, tscrt = 0, taporte = 0, tdescuentos = 0, tsueldocontarto = 0; 

            string empresa = "", ruc = "", direccion = "", fecha = "", tdiasc = "", tohorasc = "", sueltbasicoc = "", tasignacionc = "", thorasextrasc = "",
                            tcomicionesc = "", totrosc = "", suelcontrato = "",
                            tremueneracionbrutac = "", tonpc = "", taporteobligatorioc = "", tcomisionporcentualc = "",
                            tprimac = "", t5categoriac = "", tremuneracionnetac = "", tesaludc = "",tsEPS="",
                            tscrtc = "", taportec = "", tdescuentosc = "", tfaltasc = "", ttardanzasc = "", tmontoprestamoc = "", tmontoademantoc = "",tDescEPS = "";
          
                var tiempo = ""; var tiempot = ""; 

            foreach (System.Data.DataRow item in dtInput.Rows)
            {
                i++;



                tdiasc = item["Dias"].ToString();
                tohorasc = item["Horas"].ToString();
                tiempo = item["HorasExtras"].ToString();
                suelcontrato = item["SueldoSegunContrato"].ToString();
                sueltbasicoc = item["SueldoBasico"].ToString();
                tasignacionc = item["ASIGNACIONFAMILIAR"].ToString();
                thorasextrasc = item["TotalHorasExtras"].ToString();
                tcomicionesc = item["COMISIONESODESTAJO"].ToString();
                totrosc = item["PrecioOtrosBeneficio"].ToString();
                tremueneracionbrutac = item["TotalRemuneracion"].ToString();
                tonpc = item["Onp"].ToString();
                taporteobligatorioc = item["AporteObligatorio"].ToString();
                tcomisionporcentualc = item["Comision"].ToString();
                tprimac = item["PrimaSeguro"].ToString();
                t5categoriac = item["Renta5taCategoria"].ToString();
                tfaltasc = item["TotalFaltas"].ToString();
                ttardanzasc = item["TotalTardanzas"].ToString();
                tmontoprestamoc = item["MontoPrestamo"].ToString();
                tmontoademantoc = item["MontoAdelanto"].ToString();
                tdescuentosc = item["TotalDescuento"].ToString();
                tremuneracionnetac = item["RemuneracionNeta"].ToString();
                tesaludc = item["Salud"].ToString();
                tscrtc = item["SCTR"].ToString();
                taportec = item["TotalAporte"].ToString();
                //tsEPS = item["PEPS"].ToString();
                tDescEPS = item["EpsDescuento"].ToString();


                if (tdiasc.Equals("")) tdiasc = "0";
                else tdiasc = item["Dias"].ToString();
                if (tohorasc.Equals("")) tohorasc = "0";
                else tohorasc = item["Horas"].ToString();
                if (tiempo.Equals("") || tiempo.Equals("00:00:00")) tiempo = "00:00:00";
                else tiempo = item["HorasExtras"].ToString();
                if (suelcontrato.Equals("")) suelcontrato = "0";
                else suelcontrato = item["SueldoSegunContrato"].ToString();
                if (sueltbasicoc.Equals("")) sueltbasicoc = "0";
                else sueltbasicoc = item["SueldoBasico"].ToString();
                if (tasignacionc.Equals("")) tasignacionc = "0";
                else tasignacionc = item["ASIGNACIONFAMILIAR"].ToString();
                if (thorasextrasc.Equals("")) thorasextrasc = "0";
                else thorasextrasc = item["TotalHorasExtras"].ToString();
                if (tcomicionesc.Equals("")) tcomicionesc = "0";
                else tcomicionesc = item["COMISIONESODESTAJO"].ToString();
                if (totrosc.Equals("")) totrosc = "0";
                else totrosc = item["PrecioOtrosBeneficio"].ToString();
                if (tremueneracionbrutac.Equals("")) tremueneracionbrutac = "0";
                else tremueneracionbrutac = item["TotalRemuneracion"].ToString();
                if (tonpc.Equals("")) tonpc = "0";
                else tonpc = item["Onp"].ToString();
                if (taporteobligatorioc.Equals("")) taporteobligatorioc = "0";
                else taporteobligatorioc = item["AporteObligatorio"].ToString();
                if (tcomisionporcentualc.Equals("")) tcomisionporcentualc = "0";
                else tcomisionporcentualc = item["Comision"].ToString();
                if (tprimac.Equals("")) tprimac = "0";
                else tprimac = item["PrimaSeguro"].ToString();
                if (t5categoriac.Equals("")) t5categoriac = "0";
                else t5categoriac = item["Renta5taCategoria"].ToString();
                if (tfaltasc.Equals("")) tfaltasc = "0";
                else tfaltasc = item["TotalFaltas"].ToString();
                if (ttardanzasc.Equals("")) ttardanzasc = "0";
                else ttardanzasc = item["TotalTardanzas"].ToString();
                if (tmontoprestamoc.Equals("")) tmontoprestamoc = "0";
                else tmontoprestamoc = item["MontoPrestamo"].ToString();
                if (tmontoademantoc.Equals("")) tmontoademantoc = "0";
                else tmontoademantoc = item["MontoAdelanto"].ToString();
                if (tdescuentosc.Equals("")) tdescuentosc = "0";
                else tdescuentosc = item["TotalDescuento"].ToString();
                if (tremuneracionnetac.Equals("")) tremuneracionnetac = "0";
                else tremuneracionnetac = item["RemuneracionNeta"].ToString();
                if (tesaludc.Equals("")) tesaludc = "0";
                else tesaludc = item["Salud"].ToString();
                if (tscrtc.Equals("")) tscrtc = "0";
                else tscrtc = item["SCTR"].ToString();
                if (taportec.Equals("")) taportec = "0";
                else taportec = item["TotalAporte"].ToString();
                if (tDescEPS.Equals("")) tDescEPS = "0";
                else tDescEPS = item["EpsDescuento"].ToString();


                    ws.Cell("A" + (9 + i)).Value = item["ORDEN"].ToString();
                    ws.Cell("A" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("B" + (9 + i)).Value = item["Apellido"].ToString() + " " + item["Nombre"].ToString();
                    ws.Cell("B" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("C" + (9 + i)).Value = item["NumeroDocumento"].ToString();
                    ws.Cell("C" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("D" + (9 + i)).Value = item["CargoUOcupacion"].ToString();
                    ws.Cell("D" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("D" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;



                    ws.Cell("E" + (9 + i)).Value = item["Dias"].ToString();
                    ws.Cell("E" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("F" + (9 + i)).Value = FormatoMontosSINCOMA2(item["Horas"].ToString());
                    ws.Cell("F" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("F" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("F" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("G" + (9 + i)).Value = item["HorasExtras"].ToString();
                    ws.Cell("G" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                    ws.Cell("H" + (9 + i)).Value = FormatoMontosSINCOMA2(item["SueldoSegunContrato"].ToString());
                    ws.Cell("H" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("H" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("H" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("I" + (9 + i)).Value = FormatoMontosSINCOMA2(item["SueldoBasico"].ToString());
                    ws.Cell("I" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("I" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("I" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("J" + (9 + i)).Value = FormatoMontosSINCOMA2(item["ASIGNACIONFAMILIAR"].ToString());
                    ws.Cell("J" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("J" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("J" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("K" + (9 + i)).Value = FormatoMontosSINCOMA2(item["TotalHorasExtras"].ToString());
                    ws.Cell("K" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("K" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("K" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("L" + (9 + i)).Value = FormatoMontosSINCOMA2(item["COMISIONESODESTAJO"].ToString());
                    ws.Cell("L" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("L" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("L" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("M" + (9 + i)).Value = FormatoMontosSINCOMA2(item["PrecioOtrosBeneficio"].ToString());
                    ws.Cell("M" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("M" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("M" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("N" + (9 + i)).Value = FormatoMontosSINCOMA2(item["TotalRemuneracion"].ToString());
                    ws.Cell("N" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("N" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("N" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("O" + (9 + i)).Value = FormatoMontosSINCOMA2(item["Onp"].ToString());
                    ws.Cell("O" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("O" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("O" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("P" + (9 + i)).Value = FormatoMontosSINCOMA2(item["AporteObligatorio"].ToString());
                    ws.Cell("P" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("P" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("P" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("Q" + (9 + i)).Value = FormatoMontosSINCOMA2(item["Comision"].ToString());
                    ws.Cell("Q" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("Q" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("Q" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("R" + (9 + i)).Value = FormatoMontosSINCOMA2(item["PrimaSeguro"].ToString());
                    ws.Cell("R" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("R" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("R" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("S" + (9 + i)).Value = FormatoMontosSINCOMA2(item["Renta5taCategoria"].ToString());
                    ws.Cell("S" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("S" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("S" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("T" + (9 + i)).Value = FormatoMontosSINCOMA2((Double.Parse(tfaltasc) + Double.Parse(ttardanzasc) +
                                          Double.Parse(tmontoprestamoc) + Double.Parse(tmontoademantoc) + Double.Parse(tDescEPS)).ToString());

                    ws.Cell("T" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("T" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Cell("T" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell("U" + (9 + i)).Value = FormatoMontosSINCOMA2(item["TotalDescuento"].ToString());
                    ws.Cell("U" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("U" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("U" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("V" + (9 + i)).Value = FormatoMontosSINCOMA2(item["RemuneracionNeta"].ToString());
                    ws.Cell("V" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("V" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("V" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("W" + (9 + i)).Value = FormatoMontosSINCOMA2(item["Salud"].ToString());
                    ws.Cell("W" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("W" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("W" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("X" + (9 + i)).Value = FormatoMontosSINCOMA2(item["EPS"].ToString());
                    ws.Cell("X" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("X" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("X" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("Y" + (9 + i)).Value = FormatoMontosSINCOMA2(item["SCTR"].ToString());
                    ws.Cell("Y" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("Y" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("Y" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    ws.Cell("Z" + (9 + i)).Value = FormatoMontosSINCOMA2(item["TotalAporte"].ToString());
                    ws.Cell("Z" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell("Z" + (9 + i)).SetDataType(XLCellValues.Text);
                    ws.Cell("Z" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;



                    tdias = tdias + CalculoTotales(tdiasc);
                    tohoras = tohoras + CalculoTotales(tohorasc);
                    tiempot = CalculoTiempo(tiempot, tiempo);
                    tsueldocontarto = tsueldocontarto + CalculoTotales(suelcontrato);
                    sueltbasico = sueltbasico + CalculoTotales(sueltbasicoc);
                    tasignacion = tasignacion + CalculoTotales(tasignacionc);
                    thorasextras = thorasextras + CalculoTotales(thorasextrasc);
                    tcomiciones = tcomiciones + CalculoTotales(tcomicionesc);
                    totros = totros + CalculoTotales(totrosc);
                    tremueneracionbruta = tremueneracionbruta + CalculoTotales(tremueneracionbrutac);
                    tonp = tonp + CalculoTotales(tonpc);
                    taporteobligatorio = taporteobligatorio + CalculoTotales(taporteobligatorioc);
                    tcomisionporcentual = tcomisionporcentual + CalculoTotales(tcomisionporcentualc);
                    tprima = tprima + CalculoTotales(tprimac);
                    t5categoria = t5categoria + CalculoTotales(t5categoriac);
                    totrosdescuentos = totrosdescuentos + CalculoTotales(tfaltasc) +
                         CalculoTotales(ttardanzasc) + CalculoTotales(tmontoprestamoc) +
                         +CalculoTotales(tmontoademantoc) + CalculoTotales(tDescEPS);
                    tdescuentos = tdescuentos + CalculoTotales(tdescuentosc);
                    tremuneracionneta = tremuneracionneta + CalculoTotales(tremuneracionnetac);
                    tesalud = tesalud + CalculoTotales(tesaludc);
                    //tepsd = tepsd + CalculoTotales(tsEPS);
                    tscrt = tscrt + CalculoTotales(tscrtc);
                    taporte = taporte + CalculoTotales(taportec);

                    empresa = item["RazonSocial"].ToString();
                    ruc = "RUC Nº : " + item["NumeroRuc"].ToString();
                    direccion = "DIRECCIÓN : " + item["Direccion"].ToString();
                    fecha = "CORRESPONDIENTE AL MES DE " + item["FechaCierreMes"].ToString() + " DEL " + item["FechaCierreAnio"].ToString();
                   

            }

           
            ws.Cell(1, 1).SetValue(empresa);
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(2, 1).SetValue(ruc);
            ws.Cell(2, 1).Style.Font.Bold = true;
            ws.Cell(3, 1).SetValue(direccion);
            ws.Cell("I4").Value = "PLANILLA DE REMUNERACIONES";
            ws.Cell("I4").Style.Font.Bold = true;
            ws.Cell("J5").Value = fecha;
            ws.Cell("J5").Style.Font.Bold = true;
            ws.Cell("J5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;    

            ws.Cell("A"+(10 + i)).Value = "TOTALES S/.";
            ws.Range("A" + (10 + i) + ":" + "D" + (10 + i)).Row(1).Merge();
            ws.Cell("A" + (10 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A" + (10 + i)).Style.Font.Bold = true;
            ws.Range("A" + (10 + i) + ":" + "D" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("E" + (10 + i)).Value = "" + FormatoMontos(tdias.ToString());
            ws.Range("E" + (10 + i) + ":" + "E" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("F" + (10 + i)).Value = "" + FormatoMontos(tohoras.ToString());
            ws.Range("F" + (10 + i) + ":" + "F" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("G" + (10 + i)).Value =tiempot;// FormatoMontos(textras.ToString());
            ws.Range("G" + (10 + i) + ":" + "G" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("H" + (10 + i)).Value = "" + FormatoMontos(tsueldocontarto.ToString());
            ws.Range("H" + (10 + i ) + ":" + "H" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("I" + (10 + i)).Value = "" + FormatoMontos(sueltbasico.ToString());
            ws.Range("I" + (10 + i ) + ":" + "I" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("J" + (10 + i)).Value = "" + FormatoMontos(tasignacion.ToString());
            ws.Range("J" + (10 + i ) + ":" + "J" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("K" + (10 + i)).Value = "" + FormatoMontos(thorasextras.ToString());
            ws.Range("K" + (10 + i ) + ":" + "K" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("L" + (10 + i)).Value = "" + FormatoMontos(tcomiciones.ToString());
            ws.Range("L" + (10 + i ) + ":" + "L" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("M" + (10 + i)).Value = "" + FormatoMontos(totros.ToString());
            ws.Range("M" + (10 + i ) + ":" + "M" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("N" + (10 + i)).Value = "" + FormatoMontos(tremueneracionbruta.ToString());
            ws.Range("N" + (10 + i ) + ":" + "N" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("O" + (10 + i)).Value = "" + FormatoMontos(tonp.ToString());
            ws.Range("O" + (10 + i ) + ":" + "O" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("P" + (10 + i)).Value = "" + FormatoMontos(taporteobligatorio.ToString());
            ws.Range("P" + (10 + i ) + ":" + "P" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("Q" + (10 + i)).Value = "" + FormatoMontos(tcomisionporcentual.ToString());
            ws.Range("Q" + (10 + i ) + ":" + "Q" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("R" + (10 + i)).Value = "" + FormatoMontos(tprima.ToString());
            ws.Range("R" + (10 + i ) + ":" + "R" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("S" + (10 + i)).Value = "" + FormatoMontos(t5categoria.ToString());
            ws.Range("S" + (10 + i ) + ":" + "S" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("T" + (10 + i)).Value = "" + FormatoMontos(totrosdescuentos.ToString());
            ws.Range("T" + (10 + i ) + ":" + "T" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("U" + (10 + i)).Value = "" + FormatoMontos(tdescuentos.ToString());
            ws.Range("U" + (10 + i ) + ":" + "U" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("V" + (10 + i)).Value = "" + FormatoMontos(tremuneracionneta.ToString());
            ws.Range("V" + (10 + i ) + ":" + "V" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("W" + (10 + i)).Value = "" + FormatoMontos(tesalud.ToString());
            ws.Range("W" + (10 + i ) + ":" + "W" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("X" + (10 + i)).Value = "" + FormatoMontos(tepsd.ToString());
            ws.Range("X" + (10 + i) + ":" + "X" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("Y" + (10 + i)).Value = "" + FormatoMontos(tscrt.ToString());
            ws.Range("Y" + (10 + i ) + ":" + "Y" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("Z" + (10 + i)).Value = "" + FormatoMontos(taporte.ToString());
            ws.Range("Z" + (10 + i ) + ":" + "Z" + (10 + i )).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Range("H" + (10 + i) + ":" + "Z" + (10 + i)).SetDataType(XLCellValues.Text);

            ws.Range("E" + (10 + i) + ":" + "F" + (10 + i)).SetDataType(XLCellValues.Text);

            ws.Range("E" + (10 + i) + ":" + "Z" + (10 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            ws.Range("E" + (10 + i) + ":" + "Z" + (10 + i)).Style.Font.Bold = true;

            ws.Range("A7:Y9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Range("A7" + ":" + "Z" + (10 + i)).Style.Font.FontSize = 9;


  
            ws.Range("A7:Z9").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


            ws.Range("E8:G9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;

            ws.Range("H7:H9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("I8:M9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("N7:N9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("P9:R9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("S8:T9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("U7:V9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("W8:Z9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;


            // Prepare the response
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");
        
            //// Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();
            }
        }

        private double CalculoTotales(String monto)
        {

            double totalmonto = 0;
            return totalmonto = totalmonto + Convert.ToDouble(monto);
        }


        /*Aqui termina la función para Exportar en excel*/



        public void ExportarExcelBoletaTrabajador(System.Data.DataTable dtInput, System.Data.DataTable dtbeneficios, String filename, HttpResponse response, string title, String inicoboletacelda)
        {
            String valorporcentajeopn = "", valorporcentajeafpfondo = "", valorporcentajcomision = "", valorporcentajeafpseguro = "",
                   /* onpmonto = "", aportemonto = "", montocomision = "", montoprimaseguro = "", montofaltas = "",
                    monto5ta = "", montotardanzas = "", montoprestamo = "", montoadelantos = "", montoessalud = "",
                    montocstr = "",*/
                    sueldocomputable = "",  montohorasextras="",montoFaltas="", totalRemuneracion="";

            XLWorkbook workbook = new XLWorkbook();



            var ws = workbook.Worksheets.Add(title);


            ws.Cell("E1").Value = "BOLETA DE PAGO";
            ws.Range("E1:H1").Row(1).Merge();
            ws.Cell("E1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("E1").Style.Font.Bold = true;
            ws.Cell("E1").Style.Font.FontSize = 14;

            ws.Cell("D2").Value = "ART. 19 DEL DECRETO SUPREMO N° 001-98-TR DEL 22-01-98 ";
            ws.Range("D2:I2").Row(1).Merge();
            ws.Cell("D2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("D2").Style.Font.Bold = true;
            ws.Cell("D2").Style.Font.FontSize = 9;


            ws.Cell("A5").Value = "DATOS DE LA EMPRESA";
            ws.Range("A5:C5").Row(1).Merge();
            ws.Cell("A5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A5").Style.Font.Bold = true;
            ws.Cell("A5").Style.Font.FontSize = 8;
            ws.Range("A5:C5").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Rows("6").Height = 5;

            ws.Cell("A7").Value = "RUC";
            ws.Range("A7:B7").Row(1).Merge();
            ws.Cell("A7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A7").Style.Font.Bold = true;
            ws.Cell("A7").Style.Font.FontSize = 8;
            ws.Range("A7:B7").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("C7").Value = "RAZON SOCIAL";
            ws.Range("C7:D7").Row(1).Merge();
            ws.Cell("C7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("C7").Style.Font.Bold = true;
            ws.Cell("C7").Style.Font.FontSize = 8;
            ws.Range("C7:D7").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("E7").Value = "RUBRO DE LA EMPRESA";
            ws.Range("E7:G7").Row(1).Merge();
            ws.Cell("E7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("E7").Style.Font.Bold = true;
            ws.Cell("E7").Style.Font.FontSize = 8;
            ws.Range("E7:G7").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("H7").Value = "DIRECCIÓN";
            ws.Range("H7:L7").Row(1).Merge();
            ws.Cell("H7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("H7").Style.Font.Bold = true;
            ws.Cell("H7").Style.Font.FontSize = 8;
            ws.Range("H7:L7").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Rows("9").Height = 5;

            ws.Cell("A10").Value = "DATOS DEL TRABAJADOR";
            ws.Range("A10:C10").Row(1).Merge();
            ws.Cell("A10").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A10").Style.Font.Bold = true;
            ws.Cell("A10").Style.Font.FontSize = 8;
            ws.Range("A10:C10").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Rows("11").Height = 5;

            ws.Cell("A12").Value = "CÓDIGO";
            ws.Cell("A12").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A12").Style.Font.Bold = true;
            ws.Cell("A12").Style.Font.FontSize = 8;
            ws.Range("A12:A12").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("B12").Value = "NOMBRES";
            ws.Range("B12:C12").Row(1).Merge();
            ws.Cell("B12").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("B12").Style.Font.Bold = true;
            ws.Cell("B12").Style.Font.FontSize = 8;
            ws.Range("B12:C12").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("D12").Value = "APELLIDOS";
            ws.Range("D12:E12").Row(1).Merge();
            ws.Cell("D12").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("D12").Style.Font.Bold = true;
            ws.Cell("D12").Style.Font.FontSize = 8;
            ws.Range("D12:E12").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("F12").Value = "D.N.I.";
            ws.Cell("F12").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("F12").Style.Font.Bold = true;
            ws.Cell("F12").Style.Font.FontSize = 8;
            ws.Range("F12:F12").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("G12").Value = "F. NAC.";
            ws.Cell("G12").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("G12").Style.Font.Bold = true;
            ws.Cell("G12").Style.Font.FontSize = 8;
            ws.Range("G12:G12").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("H12").Value = "HIJOS";
            ws.Cell("H12").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("H12").Style.Font.Bold = true;
            ws.Cell("H12").Style.Font.FontSize = 8;
            ws.Range("H12:H12").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Cell("I12").Value = "DIRECCIÓN";
            ws.Range("I12:L12").Row(1).Merge();
            ws.Cell("I12").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("I12").Style.Font.Bold = true;
            ws.Cell("I12").Style.Font.FontSize = 8;
            ws.Range("I12:L12").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Rows("14").Height = 7;


            ws.Cell("A15").Value = "DATOS DEL TRABAJADOR VINCULADOS A LA RELACIÓN LABORAL";
            ws.Range("A15:G15").Row(1).Merge();
            ws.Cell("A15").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A15").Style.Font.Bold = true;
            ws.Cell("A15").Style.Font.FontSize = 8;
            ws.Range("A15:G15").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Rows("16").Height = 7;


            ws.Cell("A17").Value = "CARGO";
            ws.Range("A17:B17").Row(1).Merge();
            ws.Cell("A17").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A17").Style.Font.Bold = true;
            ws.Cell("A17").Style.Font.FontSize = 8;
            ws.Range("A17:B17").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("C17").Value = "CATEGORÍA";
            ws.Cell("C17").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("C17").Style.Font.Bold = true;
            ws.Cell("C17").Style.Font.FontSize = 8;
            ws.Range("C17:C17").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("D17").Value = "PERIODIC.";
            ws.Cell("D17").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("D17").Style.Font.Bold = true;
            ws.Cell("D17").Style.Font.FontSize = 8;
            ws.Range("D17:D17").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("E17").Value = "ONP";
            ws.Cell("E17").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("E17").Style.Font.Bold = true;
            ws.Cell("E17").Style.Font.FontSize = 8;
            ws.Range("E17:E17").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("F17").Value = "A.F.P.";
            ws.Cell("F17").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("F17").Style.Font.Bold = true;
            ws.Cell("F17").Style.Font.FontSize = 8;
            ws.Range("F17:F17").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("G17").Value = "C.U.S.P.P.";
            ws.Cell("G17").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("G17").Style.Font.Bold = true;
            ws.Cell("G17").Style.Font.FontSize = 8;
            ws.Range("G17:G17").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("H17").Value = "F.ING.";
            ws.Cell("H17").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("H17").Style.Font.Bold = true;
            ws.Cell("H17").Style.Font.FontSize = 8;
            ws.Range("H17:H17").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Cell("I17").Value = "F.CESE";
            ws.Cell("I17").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("I17").Style.Font.Bold = true;
            ws.Cell("I17").Style.Font.FontSize = 8;
            ws.Range("I17:I17").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("J17").Value = "INI.VAC.";
            ws.Cell("J17").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("J17").Style.Font.Bold = true;
            ws.Cell("J17").Style.Font.FontSize = 8;
            ws.Range("J17:J17").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("K17").Value = "FIN.VAC.";
            ws.Cell("K17").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("K17").Style.Font.Bold = true;
            ws.Cell("K17").Style.Font.FontSize = 8;
            ws.Range("K17:K17").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("L17").Value = "DÍAS VAC.";
            ws.Cell("L17").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("L17").Style.Font.Bold = true;
            ws.Cell("L17").Style.Font.FontSize = 8;
            ws.Range("L17:L17").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Rows("19").Height = 5;

            ws.Cell("A20").Value = "DÍAS LABORADOS";
            ws.Range("A20:A21").Column(1).Merge();
            ws.Range("A20:A21").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("A20:A21").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("A20").Style.Font.Bold = true;
            ws.Cell("A20").Style.Font.FontSize = 8;
            ws.Range("A20:A21").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("B20").Value = "TOTAL HORAS LABORADAS";
            ws.Range("B20:B21").Column(1).Merge();
            ws.Range("B20:B21").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("B20:B21").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("B20").Style.Font.FontSize = 6;
            ws.Range("B20:B21").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("C20").Value = "HORAS EXTRAS";
            ws.Range("C20:C21").Column(1).Merge();
            ws.Range("C20:C21").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("C20:C21").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("C20").Style.Font.Bold = true;
            ws.Cell("C20").Style.Font.FontSize = 8;
            ws.Range("C20:C21").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("D20").Value = "DIAS NO LABORADOS";
            ws.Range("D20:D21").Column(1).Merge();
            ws.Cell("D20").Style.Font.Bold = true;
            ws.Cell("D20").Style.Font.FontSize = 8;
            ws.Range("D20:D21").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("D20:D21").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("D20:D21").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("F20").Value = "OTRO EMPLEADOR";
            ws.Range("F20:F22").Column(1).Merge();
            ws.Cell("F20").Style.Font.Bold = true;
            ws.Cell("F20").Style.Font.FontSize = 8;
            ws.Range("F20:F22").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("F20:F22").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("F20:F22").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("G20").Value = "IMPORTE REMUN.";
            ws.Range("G20:G21").Column(1).Merge();
            ws.Cell("G20").Style.Font.Bold = true;
            ws.Cell("G20").Style.Font.FontSize = 8;
            ws.Range("G20:G21").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("G20:G21").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("G20:G21").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


            ws.Cell("I20").Value = "FORMA DE PAGO";
            ws.Range("I20:L20").Row(1).Merge();
            ws.Cell("I20").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("I20").Style.Font.Bold = true;
            ws.Cell("I20").Style.Font.FontSize = 8;
            ws.Range("I20:L20").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Rows("23").Height = 9;


            ws.Cell("A24").Value = "REMUNERACIONES";
            ws.Range("A24:C24").Row(1).Merge();
            ws.Cell("A24").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A24").Style.Font.Bold = true;
            ws.Cell("A24").Style.Font.FontSize = 8;
            ws.Range("A24:C24").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("A24:C24").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


            ws.Cell("D24").Value = "REMUNERACIONES";
            ws.Range("D24:F24").Row(1).Merge();
            ws.Cell("D24").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("D24").Style.Font.Bold = true;
            ws.Cell("D24").Style.Font.FontSize = 8;
            ws.Range("D24:F24").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("D24:F24").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


            ws.Cell("G24").Value = "RETENCIONES / DESCUENTOS";
            ws.Range("G24:I24").Row(1).Merge();
            ws.Cell("G24").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("G24").Style.Font.Bold = true;
            ws.Cell("G24").Style.Font.FontSize = 8;
            ws.Range("G24:I24").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("G24:I24").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("J24").Value = "NETO A PAGAR";     
            ws.Cell("J24").Style.Font.Bold = true;
            ws.Cell("J24").Style.Font.FontSize = 8;
            ws.Range("J24:J24").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("J24:J24").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;

            ws.Cell("K24").Value = "APORTACIONES DEL EMPLEADOR";
            ws.Range("K24:L24").Row(1).Merge();
            ws.Cell("K24").Style.Font.Bold = true;
            ws.Cell("K24").Style.Font.FontSize = 8;
            ws.Range("K24:L24").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("K24:L24").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;

            ws.Cell("C41").Value = "EMPLEADOR";
            ws.Range("C41:E41").Row(1).Merge();
            ws.Cell("C41").Style.Font.Bold = true;
            ws.Cell("C41").Style.Font.FontSize = 8;
            ws.Range("C41:E41").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Range("C41:E41").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;

            ws.Cell("H41").Value = "TRABAJADOR";
            ws.Range("H41:J41").Row(1).Merge();
            ws.Cell("H41").Style.Font.Bold = true;
            ws.Cell("H41").Style.Font.FontSize = 8;
            ws.Range("H41:J41").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Range("H41:J41").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;


            ws.Cell("G25").Value = "AFP Fondo";
            ws.Cell("G25").Style.Font.FontSize = 8;
            ws.Cell("G26").Value = "AFP Comisión";
            ws.Cell("G26").Style.Font.FontSize = 8;
            ws.Cell("G27").Value = "AFP Seguro";
            ws.Cell("G27").Style.Font.FontSize = 8;
            ws.Cell("G28").Value = "ONP";
            ws.Cell("G28").Style.Font.FontSize = 8;
            //ws.Cell("G29").Value = "Faltas";
            //ws.Cell("G29").Style.Font.FontSize = 8;
            ws.Cell("G29").Value = "Retenciones 5ta";
            ws.Cell("G29").Style.Font.FontSize = 8;
            ws.Cell("G30").Value = "Tardanzas";
            ws.Cell("G30").Style.Font.FontSize = 8;
            ws.Cell("G31").Value = "Prestamos";
            ws.Cell("G31").Style.Font.FontSize = 8;
            ws.Cell("G32").Value = "Adelantos";
            ws.Cell("G32").Style.Font.FontSize = 8;
            ws.Cell("G33").Value = "Descuentos Eps";
            ws.Cell("G33").Style.Font.FontSize = 8;
            ws.Cell("K25").Value = "Essalud";
            ws.Cell("K25").Style.Font.FontSize = 8;
            ws.Cell("K26").Value = "EPS";
            ws.Cell("K26").Style.Font.FontSize = 8;
            ws.Cell("K27").Value = "S.C.T.R";
            ws.Cell("K27").Style.Font.FontSize = 8;


            if (dtInput.Rows.Count > 0)
            {
                DataRow row = dtInput.Rows[0];

                ws.Rows("8").Height = 21;

                ws.Cell("E3").Value = "MES DE " + row["FechaCierreMes"].ToString() + " " + row["FechaCierreAnio"].ToString();
                ws.Cell("E3").Style.Font.Bold = true;
                ws.Range("E3:H3").Row(1).Merge();
                ws.Cell("E3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("E3").Style.Font.FontSize = 10;

                ws.Cell("A8").Value = row["NumeroRuc"].ToString();
                ws.Range("A8:B8").Row(1).Merge();
                ws.Cell("A8").Style.Font.Bold = true;
                ws.Cell("A8").Style.Font.FontSize = 8;
                ws.Range("A8:B8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A8:B8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
                ws.Range("A8:B8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


                ws.Cell("C8").Value = row["RazonSocial"].ToString();
                ws.Range("C8:D8").Row(1).Merge();
                ws.Cell("C8").Style.Font.FontSize = 7;
                ws.Range("C8:D8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("C8:D8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
                ws.Range("C8:D8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("E8").Value = row["RubroEmpresa"].ToString();
                ws.Range("E8:G8").Row(1).Merge();
                ws.Cell("E8").Style.Font.FontSize = 7;
                ws.Range("E8:G8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("E8:G8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.General;
                ws.Range("E8:G8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("H8").Value = row["DireccionEmpresa"].ToString();
                ws.Range("H8:L8").Row(1).Merge();
                ws.Cell("H8").Style.Font.FontSize = 7;
                ws.Range("H8:L8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("H8:L8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
                ws.Range("H8:L8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Rows("13").Height = 26;

                ws.Cell("A13").Value = "";
                ws.Range("A13:A13").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                ws.Cell("B13").Value = row["Nombre"].ToString();
                ws.Range("B13:C13").Row(1).Merge();
                ws.Cell("B13").Style.Font.FontSize = 7;
                ws.Range("B13:C13").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("B13:C13").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("D13").Value = row["Apellido"].ToString();
                ws.Range("D13:E13").Row(1).Merge();
                ws.Cell("D13").Style.Font.FontSize = 7;
                ws.Range("D13:E13").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("D13:E13").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


                ws.Cell("F13").Value = row["DNI"].ToString();
                ws.Cell("F13").Style.Font.FontSize = 7;
                ws.Range("F13:F13").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("F13:F13").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
                ws.Range("F13:F13").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


                ws.Cell("G13").Value = row["FechaNacimiento"].ToString();
                ws.Cell("G13").Style.Font.FontSize = 7;
                ws.Range("G13:G13").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("G13:G13").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
                ws.Range("G13:G13").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


                ws.Cell("H13").Value = row["Hijos"].ToString();
                ws.Cell("H13").Style.Font.FontSize = 7;
                ws.Range("H13:H13").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("H13:H13").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
                ws.Range("H13:H13").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("I13").Value = row["DireccionTrabajador"].ToString();
                ws.Range("I13:L13").Row(1).Merge();
                ws.Cell("I13").Style.Font.FontSize = 7;
                ws.Range("I13:L13").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("I13:L13").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
                ws.Range("I13:L13").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("A18").Value = row["CargoUOcupacion"].ToString();
                ws.Range("A18:B18").Row(1).Merge();
                ws.Cell("A18").Style.Font.FontSize = 7;
                ws.Range("A18:B18").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A18:B18").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
                ws.Range("A18:B18").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("C18").Value = row["Categoria"].ToString();
                ws.Cell("C18").Style.Font.FontSize = 7;
                ws.Range("C18:C18").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("C18:C18").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("C18:C18").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("D18").Value = row["Periocidad"].ToString();
                ws.Cell("D18").Style.Font.FontSize = 7;
                ws.Range("D18:D18").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("D18:D18").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("D18:D18").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("E18").Value = row["ONP"].ToString();
                ws.Cell("E18").Style.Font.FontSize = 7;
                ws.Range("E18:E18").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("E18:E18").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
                ws.Range("E18:E18").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


                ws.Cell("F18").Value = row["AFP"].ToString();
                ws.Cell("F18").Style.Font.FontSize = 7;
                ws.Range("F18:F18").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("F18:F18").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
                ws.Range("F18:F18").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("G18").Value = row["CUSPP"].ToString();
                ws.Cell("G18").Style.Font.FontSize = 7;
                ws.Range("G18:G18").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("G18:G18").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
                ws.Range("G18:G18").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("H18").Value = row["Finigreso"].ToString();
                ws.Cell("H18").Style.Font.FontSize = 7;
                ws.Range("H18:H18").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("H18:H18").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("H18:H18").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("I18").Value = row["Fcese"].ToString();
                ws.Cell("I18").Style.Font.FontSize = 7;
                ws.Range("I18:I18").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("I18:I18").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("I18:I18").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("J18").Value = row["FechaInicioVacaciones"].ToString();
                ws.Cell("J18").Style.Font.FontSize = 7;
                ws.Range("J18:J18").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("J18:J18").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("J18:J18").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("K18").Value = row["FechaTerminoVacaciones"].ToString();
                ws.Cell("K18").Style.Font.FontSize = 7;
                ws.Range("K18:K18").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("K18:K18").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("K18:K18").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("L18").Value = row["DiasVacaciones"].ToString();
                ws.Cell("L18").Style.Font.FontSize = 7;
                ws.Range("L18:L18").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("L18:L18").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("L18:L18").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("A22").Value = row["Dias"].ToString();
                ws.Cell("A22").Style.Font.FontSize = 7;
                ws.Range("A22:A22").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A22:A22").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("A22:A22").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("B22").Value = row["Horas"].ToString();
                ws.Cell("B22").Style.Font.FontSize = 7;
                ws.Range("B22:B22").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("B22:B22").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("B22:B22").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("C22").Value = row["HorasExtras"].ToString();
                ws.Cell("C22").Style.Font.FontSize = 7;
                ws.Range("C22:C22").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("C22:C22").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("C22:C22").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell("D22").Value = row["DiasNoLaborados"].ToString();
                ws.Cell("D22").Style.Font.FontSize = 7;
                ws.Range("D22:D22").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range("D22:D22").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("D22:D22").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            
              
                ws.Range("G22:G22").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
  
     

                if (row["TIPOPAGO"].Equals("DEPÓSITO EN CUENTA"))
                {
                    ws.Cell("I21").Value = row["TIPOPAGO"].ToString();
                    ws.Cell("I21").Style.Font.FontSize = 7;
                    ws.Range("I21:J21").Row(1).Merge();
                    ws.Cell("I21").Style.Font.Bold = true;
                    ws.Range("I21:J21").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("I21:J21").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("I21:J21").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


                    ws.Cell("I22").Value = row["NUMEROCUENTA"].ToString();
                    ws.Cell("I22").Style.Font.FontSize = 7;
                    ws.Range("I22:J22").Row(1).Merge();
                    ws.Range("I22:J22").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("I22:J22").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
                    ws.Range("I22:J22").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    ws.Range("K21:L21").Row(1).Merge();
                    ws.Range("K21:L21").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("K22:L22").Row(1).Merge();
                    ws.Range("K22:L22").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                else
                {
                    ws.Cell("K21").Value = row["TIPOPAGO"].ToString();
                    ws.Cell("K21").Style.Font.FontSize = 7;
                    ws.Range("K21:L21").Row(1).Merge();
                    ws.Cell("K21").Style.Font.Bold = true;
                    ws.Range("K21:L21").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("K21:L21").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("K21:L21").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    ws.Range("K22:L22").Row(1).Merge();
                    ws.Range("K22:L22").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                    ws.Range("I22:J22").Row(1).Merge();
                    ws.Range("I21:J21").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    ws.Range("I22:J22").Row(1).Merge();
                    ws.Range("I22:J22").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }


               

                valorporcentajeafpfondo = row["AFPFONDO"].ToString();
                valorporcentajcomision = row["AFPCOMISION"].ToString();
                valorporcentajeafpseguro = row["AFPSEGURO"].ToString();
                valorporcentajeopn = row["PorcentajeONP"].ToString();


                if (valorporcentajeafpfondo.Equals(""))
                {
                    ws.Cell("H25").Value = row["AFPFONDO"].ToString();
                    ws.Cell("H25").Style.Font.FontSize = 8;
                }
                else
                {
                    ws.Cell("H25").Value = row["AFPFONDO"].ToString() + "%";
                    ws.Cell("H25").Style.Font.FontSize = 8;
                    ws.Cell("H25").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                }
                if (valorporcentajcomision.Equals(""))
                {
                    ws.Cell("H26").Value = row["AFPCOMISION"].ToString();
                    ws.Cell("H26").Style.Font.FontSize = 8;
         
                }
                else
                {
                    ws.Cell("H26").Value = row["AFPCOMISION"].ToString() + "%";
                    ws.Cell("H26").Style.Font.FontSize = 8;
                    ws.Cell("H26").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                }
                if (valorporcentajeafpseguro.Equals(""))
                {
                    ws.Cell("H27").Value = row["AFPSEGURO"].ToString();
                    ws.Cell("H27").Style.Font.FontSize = 8;
                }
                else
                {
                    ws.Cell("H27").Value = row["AFPSEGURO"].ToString()+"%";
                    ws.Cell("H27").Style.Font.FontSize = 8;
                    ws.Cell("H27").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                }

                if (valorporcentajeopn.Equals(""))
                {
                    ws.Cell("H28").Value = row["PorcentajeONP"].ToString();
                    ws.Cell("H28").Style.Font.FontSize = 8;
                }
                else
                {
                    ws.Cell("H28").Value = row["PorcentajeONP"].ToString() + "%";
                    ws.Cell("H28").Style.Font.FontSize = 8;
                    ws.Cell("H28").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
  
                }


                //onpmonto = row["ONPMonto"].ToString();
                //aportemonto = row["MAporteObligatorio"].ToString();
                //montocomision = row["MComision"].ToString();
                //montoprimaseguro = row["MPrimaSeguro"].ToString();
                ////montofaltas = row["Faltas"].ToString();
                //monto5ta = row["Renta5taCategoria"].ToString();
                //montotardanzas = row["Tardanzas"].ToString();
                //montoprestamo = row["Prestamos"].ToString();
                //montoadelantos = row["Adelanto"].ToString();
                //montoessalud = row["Essalud"].ToString();
                //montocstr = row["SCTR"].ToString();


                //if (onpmonto.Equals("")) onpmonto = "0";
                //else onpmonto = row["ONPMonto"].ToString();
                //if (aportemonto.Equals("")) aportemonto = "0";
                //else aportemonto = row["MAporteObligatorio"].ToString();
                //if (montocomision.Equals("")) montocomision = "0";
                //else montocomision = row["MComision"].ToString();
                //if (montoprimaseguro.Equals("")) montoprimaseguro = "0";
                //else montoprimaseguro = row["MPrimaSeguro"].ToString();
                ////if (montofaltas.Equals("")) montofaltas = "0";
                ////else montofaltas = row["Faltas"].ToString();
                //if (monto5ta.Equals("")) monto5ta = "0";
                //else monto5ta = row["Renta5taCategoria"].ToString();
                //if (montotardanzas.Equals("")) montotardanzas = "0";
                //else montotardanzas = row["Tardanzas"].ToString();
                //if (montoprestamo.Equals("")) montoprestamo = "0";
                //else montoprestamo = row["Prestamos"].ToString();
                //if (montoadelantos.Equals("")) montoadelantos = "0";
                //else montoadelantos = row["Adelanto"].ToString();
                //if (montoessalud.Equals("")) montoessalud = "0";
                //else montoessalud = row["Essalud"].ToString();
                //if (montocstr.Equals("")) montocstr = "0";
                //else montocstr = row["SCTR"].ToString();

                ws.Cell("I25").Value = FormatoMontosSINCOMA(row["MAporteObligatorio"].ToString());
                ws.Cell("I25").Style.Font.FontSize = 8;
                ws.Cell("I25").SetDataType(XLCellValues.Text);
                ws.Cell("I25").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                ws.Cell("I26").Value = FormatoMontosSINCOMA(row["MComision"].ToString());
                ws.Cell("I26").Style.Font.FontSize = 8;
                ws.Cell("I26").SetDataType(XLCellValues.Text);
                ws.Cell("I26").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                ws.Cell("I27").Value = FormatoMontosSINCOMA(row["MPrimaSeguro"].ToString());
                ws.Cell("I27").Style.Font.FontSize = 8;
                ws.Cell("I27").SetDataType(XLCellValues.Text);
                ws.Cell("I27").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                ws.Cell("I28").Value = FormatoMontosSINCOMA(row["ONPMonto"].ToString());
                ws.Cell("I28").Style.Font.FontSize = 8;
                ws.Cell("I28").SetDataType(XLCellValues.Text);
                ws.Cell("I28").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                //ws.Cell("I29").Value =FormatoMontosSINCOMA(row["Faltas"].ToString());
                //ws.Cell("I29").Style.Font.FontSize = 8;
                //ws.Cell("I29").SetDataType(XLCellValues.Text);
                //ws.Cell("I29").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                ws.Cell("I29").Value =FormatoMontosSINCOMA(row["Renta5taCategoria"].ToString());
                ws.Cell("I29").Style.Font.FontSize = 8;
                ws.Cell("I29").SetDataType(XLCellValues.Text);
                ws.Cell("I29").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                ws.Cell("I30").Value = FormatoMontosSINCOMA(row["Tardanzas"].ToString());
                ws.Cell("I30").Style.Font.FontSize = 8;
                ws.Cell("I30").SetDataType(XLCellValues.Text);
                ws.Cell("I30").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;


                ws.Cell("I31").Value = FormatoMontosSINCOMA(row["Prestamos"].ToString());
                ws.Cell("I31").Style.Font.FontSize = 8;
                ws.Cell("I31").SetDataType(XLCellValues.Text);
                ws.Cell("I31").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                ws.Cell("I32").Value = FormatoMontosSINCOMA(row["Adelanto"].ToString());
                ws.Cell("I32").Style.Font.FontSize = 8;
                ws.Cell("I32").SetDataType(XLCellValues.Text);
                ws.Cell("I32").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                ws.Cell("I33").Value = FormatoMontosSINCOMA(row["EpsDescuento"].ToString());
                ws.Cell("I33").Style.Font.FontSize = 8;
                ws.Cell("I33").SetDataType(XLCellValues.Text);
                ws.Cell("I33").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                ws.Cell("L25").Value =FormatoMontosSINCOMA(row["Essalud"].ToString());
                ws.Cell("L25").Style.Font.FontSize = 8;
                ws.Cell("L25").SetDataType(XLCellValues.Text);
                ws.Cell("L25").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;


                ws.Cell("L26").Value = FormatoMontosSINCOMA(row["EPS"].ToString());
                ws.Cell("L26").Style.Font.FontSize = 8;
                ws.Cell("L26").SetDataType(XLCellValues.Text);
                ws.Cell("L26").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                ws.Cell("L27").Value = FormatoMontosSINCOMA(row["SCTR"].ToString());
                ws.Cell("L27").Style.Font.FontSize = 8;
                ws.Cell("L27").SetDataType(XLCellValues.Text);
                ws.Cell("L27").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;


                ws.Cell("I36").Value =FormatoMontos(
                                        /*(Double.Parse(onpmonto) + Double.Parse(aportemonto) +
                                         Double.Parse(montocomision) + Double.Parse(montoprimaseguro) +
                                          Double.Parse(montofaltas) + Double.Parse(monto5ta) +
                                         Double.Parse(montotardanzas) + Double.Parse(montoprestamo)
                                         + Double.Parse(montoadelantos)
                                         ).ToString()*/
                                             row["TotalRetenciones"].ToString()
                                         );


                ws.Cell("J36").Value =  FormatoMontos(row["NetoPagar"].ToString());

                ws.Cell("L36").Value = FormatoMontos(
                                               /* (Double.Parse(montoessalud) + Double.Parse(montocstr)
                                                ).ToString()*/
                                                row["TotalAporte"].ToString()
                                            );

                ws.Cell("A25").Value = "Sueldo Computable";
                ws.Cell("A25").Style.Font.FontSize = 8;
                ws.Cell("C25").Value =FormatoMontosSINCOMA(row["SueldoComputable"].ToString());
                ws.Cell("C25").Style.Font.FontSize = 8;
                ws.Cell("C25").SetDataType(XLCellValues.Text);
                ws.Cell("C25").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;


                sueldocomputable = row["SueldoComputable"].ToString();
                montohorasextras = row["MontoHorasExtras"].ToString();
                montoFaltas = row["Faltas"].ToString();


                if (sueldocomputable.Equals("")) sueldocomputable = "0";
                else sueldocomputable = row["SueldoComputable"].ToString();
                if (montohorasextras.Equals("")) montohorasextras = "0";
                else montohorasextras = row["MontoHorasExtras"].ToString();
                if (montoFaltas.Equals("")) montoFaltas = "0";
                else montoFaltas = row["Faltas"].ToString();

                ws.Cell("A26").Value = "Monto Horas Extras";
                ws.Cell("A26").Style.Font.FontSize = 8;
                ws.Cell("C26").Value = FormatoMontosSINCOMA(montohorasextras);
                ws.Cell("C26").Style.Font.FontSize = 8;
                ws.Cell("C26").SetDataType(XLCellValues.Text);
                ws.Cell("C26").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;


                ws.Cell("A27").Value = "Monto Faltas";
                ws.Cell("A27").Style.Font.FontSize = 8;
                ws.Cell("C27").Value = Convert.ToDouble(montoFaltas) > 0 ? "-" + FormatoMontosSINCOMA(montoFaltas) : FormatoMontosSINCOMA(montoFaltas);
                ws.Cell("C27").Style.Font.FontSize = 8;
                ws.Cell("C27").SetDataType(XLCellValues.Text);
                ws.Cell("C27").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;


                //pie de pagina boleta

                ws.Cell("D42").Value = row["RepresentanteLegal"].ToString();
                ws.Cell("D42").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("D42").Style.Font.FontSize = 8;
                ws.Cell("I42").Value = row["Nombre"].ToString() + " " + row["Apellido"].ToString();
                ws.Cell("I42").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("I42").Style.Font.FontSize = 8;


                 totalRemuneracion = (((row["SueldoComputable"].ToString().Trim().Equals("") ? 0 : Convert.ToDouble((row["SueldoComputable"].ToString().Trim()))) -
                                              (row["Faltas"].ToString().Trim().Equals("") ? 0 : Convert.ToDouble((row["Faltas"].ToString().Trim())))) +
                                               (row["MontoHorasExtras"].ToString().Trim().Equals("") ? 0 : Convert.ToDouble((row["MontoHorasExtras"].ToString().Trim())))
                                              ).ToString();    



            }
            int i = 0; String montob = "", variable1 = "A", variable2 = "C", descripcionbeneficio=""; Double montobeneficiocalculo = 0;

            foreach (System.Data.DataRow item in dtbeneficios.Rows)
            {
                i++;

                if (i == 9)
                {
                    variable1 = "D"; variable2 = "F"; i = -2;
                }

                montob =item["MONTOBENEFICIO"].ToString();

                if (montob.Equals("")) montob = "0";
                else montob = item["MONTOBENEFICIO"].ToString();

          
                descripcionbeneficio = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((item["DESCRIPCIONBENEFICIOS"].ToString()).ToLower());

                ws.Cell(variable1 + (27 + i)).Value = descripcionbeneficio;
                ws.Cell(variable1 + (27 + i)).Style.Font.FontSize = 8;

                ws.Cell(variable2 + (27 + i)).Value = FormatoMontosSINCOMA(item["MONTOBENEFICIO"].ToString());
                ws.Cell(variable2 + (27 + i)).Style.Font.FontSize = 8;
                ws.Cell(variable2 + (27 + i)).SetDataType(XLCellValues.Text);
                ws.Cell(variable2 + (27 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;


               montobeneficiocalculo = montobeneficiocalculo + CalculoTotales(montob);

            

            }



            ws.Cell("F36").Value = FormatoMontosSINREDONDEO((montobeneficiocalculo + Double.Parse(totalRemuneracion) /*+ Double.Parse(montohorasextras)*/).ToString());
         

            //bordes de los detalles de la boleta

            ws.Range("A25:B35").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("C25:C35").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("D25:E35").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("F25:F35").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("G25:H35").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("I25:I35").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("J25:J35").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("K25:K35").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("L25:L35").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            //FECHA 
            ws.Rows("37").Height = 5;

            Int32 mes = DateTime.Now.Month;
            Int32 anio = DateTime.Now.Year;

            String[] meses = new String[]{"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"};

            ws.Cell("A38").Value = "Trujillo, " + meses[mes-1] + " del " + anio;
            ws.Cell("A38").Style.Font.FontSize = 8;
            
          

            //BORDES TOTALES

            ws.Cell("A36").Value = "TOTAL REMUNERACIONES";
            ws.Range("A36:E36").Row(1).Merge();
            ws.Cell("A36").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A36").Style.Font.Bold = true;
            ws.Cell("A36").Style.Font.FontSize = 8;
            ws.Range("A36:E36").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("A36:E36").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Range("F36:F36").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("F36").Style.Font.Bold = true;
            ws.Cell("F36").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            ws.Cell("F36").Style.Font.FontSize = 9;
            ws.Cell("F36").SetDataType(XLCellValues.Text);

            ws.Cell("G36").Value = "TOTAL RETENCIONES";
            ws.Range("G36:H36").Row(1).Merge();
            ws.Cell("G36").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("G36").Style.Font.Bold = true;
            ws.Cell("G36").Style.Font.FontSize = 8;
            ws.Range("G36:H36").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("G36:H36").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Range("I36:I36").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("I36").Style.Font.Bold = true;
            ws.Cell("I36").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            ws.Cell("I36").Style.Font.FontSize = 9;
            ws.Cell("I36").SetDataType(XLCellValues.Text);

            ws.Range("J36:J36").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("J36").Style.Font.Bold = true;
            ws.Cell("J36").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            ws.Cell("J36").Style.Font.FontSize = 9;
            ws.Cell("J36").SetDataType(XLCellValues.Text);

            ws.Cell("K36").Value = "TOTAL APORTACIÓN";
            ws.Range("K36:K36").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("K36:K36").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("K36").Style.Font.FontSize = 6;
            ws.Range("K36:K36").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Range("L36:L36").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("L36").Style.Font.Bold = true;
            ws.Cell("L36").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            ws.Cell("L36").Style.Font.FontSize = 9;
            ws.Cell("L36").SetDataType(XLCellValues.Text);





                // Prepare the response
                HttpResponse httpResponse = response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");

                //// Flush the workbook to the Response.OutputStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
            }



        #region  Exportacion_Excel_Gratificacion_Mes_por_deivi_gomez
        public void ToExce_Gratificacion_Mes(System.Data.DataTable dtInput, string filename, HttpResponse response, string title)
        {
            XLWorkbook workbook = new XLWorkbook();
            String fecha2 = "";
            var ws = workbook.Worksheets.Add(title);


            ws.Cell("A7").Value = "#";
            ws.Range("A7:A8").Column(1).Merge();
            ws.Cell("A7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("A7:A8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Columns("A:A").Width = 4.14;

            ws.Cell("B7").Value = "Datos del Trabajador";
            ws.Range("B7:E7").Row(1).Merge();
            ws.Cell("B7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("B7:E7").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("B8").Value = "Apellidos y Nombres";
            ws.Range("B8:B8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("B8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Columns("B:B").Width = 35;

            ws.Cell("C8").Value = "Nº DNI";
            ws.Range("C8:C8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("C8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("D8").Value = "Cargo u Ocupación";
            ws.Range("D8:D8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("D8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Columns("D:D").Width = 31.86;

            ws.Cell("E8").Value = "Fecha de Ingreso";
            ws.Range("E8:E8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("E8:E8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("E8:E8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("F7").Value = "Periodo Computable";
            ws.Range("F7:G7").Row(1).Merge();
            ws.Range("F7:G7").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("F7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("F8").Value = "Mes";
            ws.Range("F8:F8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("F8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("G8").Value = "Día";
            ws.Range("G8:G8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("G8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("H7").Value = "Inasistencias sin goce de haber";
            ws.Range("H7:H8").Column(1).Merge();
            ws.Range("H7:H8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("H7:H8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("H7:H8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("I7").Value = "Remuneración según contrato";
            ws.Range("I7:I8").Column(1).Merge();
            ws.Range("I7:I8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("I7:I8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("I7:I8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("J7").Value = "Ingresos del Trabajador";
            ws.Range("J7:N7").Row(1).Merge();
            ws.Range("J7:N7").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("J7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("J8").Value = "Remuneración Básico";
            ws.Range("J8:J8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("J8:J8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("J8:J8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("k8").Value = "Asignación Familiar";
            ws.Range("k8:k8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("k8:k8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("k8:k8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


            ws.Cell("L8").Value = "Movilidad Supeditada a la Asistencia";
            ws.Range("L8:L8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("L8:L8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("L8:L8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("M8").Value = "Comisiones";
            ws.Range("M8:M8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("M8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("N8").Value = "Otros";
            ws.Range("N8:N8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("N8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


            ws.Cell("O7").Value = "Remuneración Computable Gratificación";
            ws.Range("O7:O8").Column(1).Merge();
            ws.Range("O7:O8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("O7:O8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("O7:O8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


            ws.Cell("P7").Value = "Gratificación por Pagar";
            ws.Range("P7:P8").Column(1).Merge();
            ws.Range("P7:P8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Cell("Q7").Value = "Bonificación Extraordinaria";
            ws.Range("Q7:Q8").Column(1).Merge();
            ws.Range("Q7:Q8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            //-----
            ws.Cell("R7").Value = "EPS";
            ws.Range("R7:R8").Column(1).Merge();
            ws.Range("R7:R8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("S7").Value = "Total Pagar";
            ws.Range("S7:S8").Column(1).Merge();
            ws.Range("S7:S8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Range("A7:S8").Style.Font.Bold = true;
            ws.Range("O7:S8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("A7:S8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


            //ws.Cell("R7").Value = "Total Pagar";
            //ws.Range("R7:R8").Column(1).Merge();
            //ws.Range("R7:R8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            //ws.Range("A7:R8").Style.Font.Bold = true;
            //ws.Range("O7:R8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            //ws.Range("A7:R8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            int i = -1;
            double Meses = 0, Dias = 0, Inasistencias = 0, RemuneracionBasica = 0, AsigFamiliar = 0, Movilidad = 0, Comisiones = 0,
                      Otros = 0, RemuneracionComputable = 0, GratiPagar = 0, BonifiExtraordinaria = 0,
                      TotalPagar = 0, RemuneracionContrato = 0, PEPS=0;


            if (dtInput.Rows.Count > 0)
            {
                DataRow row = dtInput.Rows[0];

                ws.Cell("A1").Value = row["RazonSocial"].ToString();
                ws.Cell("A1").Style.Font.Bold = true;
                ws.Cell("A2").Value = "RUC Nº : " + row["NumeroRuc"].ToString();
                ws.Cell("A2").Style.Font.Bold = true;
                ws.Cell("A3").Value = "DIRECCIÓN : " + row["Direccion"].ToString();
                ws.Cell("A3").Style.Font.FontSize = 9;
                ws.Cell("A4").Value = "ACTIVIDAD: " + row["Actividad"].ToString();
                ws.Cell("A4").Style.Font.FontSize = 9;


                ws.Cell("G4").Value = "PLANILLA DE GRATIFICACIÓN";
                ws.Cell("G4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("G4").Style.Font.Bold = true;
                ws.Cell("G4").Style.Font.FontSize = 14;

                ws.Cell("G5").Value = "CORRESPONDIENTE AL MES DE " + (Convert.ToInt32(row["Mes"].ToString()) <= 7 ? "Julio" : "Diciembre") + " DEL " + row["Anio"].ToString();
                ws.Cell("G5").Style.Font.Bold = true;
                ws.Cell("G5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("G5").Style.Font.FontSize = 11;



            }



            foreach (System.Data.DataRow item in dtInput.Rows)
            {
                i++;

                ws.Cell("A" + (9 + i)).Value = item["Id"].ToString();
                ws.Cell("A" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("B" + (9 + i)).Value = item["Apellido"].ToString() + " " + item["Nombre"].ToString();
                ws.Cell("B" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                ws.Cell("C" + (9 + i)).Value = item["NumeroDocumento"].ToString();
                ws.Cell("C" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                ws.Cell("D" + (9 + i)).Value = item["Ocupacion"].ToString();
                ws.Cell("D" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell("D" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
                

                ws.Cell("E" + (9 + i)).Value = item["ContartoInicio"].ToString();
                ws.Cell("E" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("F" + (9 + i)).Value = item["Meses"].ToString();
                ws.Cell("F" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("G" + (9 + i)).Value = (item["Dias"].ToString().Equals("0") ? " " : item["Dias"].ToString());
                ws.Cell("G" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("H" + (9 + i)).Value = (item["Inasistencias"].ToString().Equals("0") ? " " : item["Inasistencias"].ToString());
                ws.Cell("H" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("I" + (9 + i)).Value = (item["RemuneracionContrato"].ToString().Equals("0,00") ? " " : FormatoMontosSINCOMA(item["RemuneracionContrato"].ToString()));
                ws.Cell("I" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("j" + (9 + i)).Value = (item["RemuneracionBasica"].ToString().Equals("0,00") ? " " : FormatoMontosSINCOMA(item["RemuneracionBasica"].ToString()));
                ws.Cell("j" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("k" + (9 + i)).Value = (item["AsigFamiliar"].ToString().Equals("0,00") ? "" : FormatoMontosSINCOMA(item["AsigFamiliar"].ToString()));
                ws.Cell("k" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("L" + (9 + i)).Value = (item["Movilidad"].ToString().Equals("0,00") ? "" : FormatoMontosSINCOMA(item["Movilidad"].ToString()));
                ws.Cell("L" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("M" + (9 + i)).Value = (item["Comisiones"].ToString().Equals("0,00") ? "" : FormatoMontosSINCOMA(item["Comisiones"].ToString()));
                ws.Cell("M" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("N" + (9 + i)).Value = (item["Otros"].ToString().Equals("0,00") ? "" : FormatoMontosSINCOMA(item["Otros"].ToString()));
                ws.Cell("N" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("O" + (9 + i)).Value = (item["RemuneracionComputable"].ToString().Equals("0,00") ? "" : FormatoMontosSINCOMA(item["RemuneracionComputable"].ToString()));
                ws.Cell("O" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("P" + (9 + i)).Value = (item["GratiPagar"].ToString().Equals("0,00") ? "" : FormatoMontosSINCOMA(item["GratiPagar"].ToString()));
                ws.Cell("P" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("Q" + (9 + i)).Value = (item["BonifiExtraordinaria"].ToString().Equals("0,00") ? "" : FormatoMontosSINCOMA(item["BonifiExtraordinaria"].ToString()));
                ws.Cell("Q" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("R" + (9 + i)).Value = (item["BonificacionEPS"].ToString().Equals("0,00") ? "" : FormatoMontosSINCOMA(item["BonificacionEPS"].ToString()));
                ws.Cell("R" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                ws.Cell("S" + (9 + i)).Value = (item["TotalPagar"].ToString().Equals("0,00") ? "" : FormatoMontosSINCOMA(item["TotalPagar"].ToString()));
                ws.Cell("S" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                //ws.Cell("R" + (9 + i)).Value = (item["TotalPagar"].ToString().Equals("0,00") ? "" : FormatoMontosSINCOMA(item["TotalPagar"].ToString()));
                //ws.Cell("R" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;




                Meses = Meses + ConvertirStringADouble(item["Meses"].ToString());
                Dias = Dias + (item["Dias"].ToString().Equals(" ") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["Dias"].ToString()));
                Inasistencias = Inasistencias + (item["Inasistencias"].ToString().Equals(" ") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["Inasistencias"].ToString()));
                RemuneracionContrato = RemuneracionContrato + (item["RemuneracionContrato"].ToString().Equals(" ") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["RemuneracionContrato"].ToString()));
                RemuneracionBasica = RemuneracionBasica + (item["RemuneracionBasica"].ToString().Equals(" ") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["RemuneracionBasica"].ToString()));
                AsigFamiliar = AsigFamiliar + (item["AsigFamiliar"].ToString().Equals(" ") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["AsigFamiliar"].ToString()));
                Movilidad = Movilidad + (item["Movilidad"].ToString().Equals(" ") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["Movilidad"].ToString()));
                Comisiones = Comisiones + (item["Comisiones"].ToString().Equals(" ") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["Comisiones"].ToString()));
                Otros = Otros + (item["Otros"].ToString().Equals(" ") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["Otros"].ToString()));
                RemuneracionComputable = RemuneracionComputable + (item["RemuneracionComputable"].ToString().Equals(" ") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["RemuneracionComputable"].ToString()));
                GratiPagar = GratiPagar + (item["GratiPagar"].ToString().Equals(" ") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["GratiPagar"].ToString()));
                BonifiExtraordinaria = BonifiExtraordinaria + (item["BonifiExtraordinaria"].ToString().Equals(" ") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["BonifiExtraordinaria"].ToString()));

                PEPS = PEPS + (item["BonificacionEPS"].ToString().Equals(" ") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["BonificacionEPS"].ToString()));

                TotalPagar = TotalPagar + (item["TotalPagar"].ToString().Equals(" ") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["TotalPagar"].ToString()));




            }


            ws.Cell("A" + (10 + i)).Value = "TOTALES S/.";
            ws.Range("A" + (10 + i) + ":" + "E" + (10 + i)).Row(1).Merge();
            ws.Range("A" + (10 + i) + ":" + "E" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("A" + (10 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("F" + (10 + i)).Value = FormatoMontosTotales(Meses.ToString());
            ws.Range("F" + (10 + i) + ":" + "F" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("G" + (10 + i)).Value = FormatoMontosTotales(Dias.ToString());
            ws.Range("G" + (10 + i) + ":" + "G" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("H" + (10 + i)).Value = FormatoMontosTotales(Inasistencias.ToString());
            ws.Range("H" + (10 + i) + ":" + "H" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("I" + (10 + i)).Value = FormatoMontosTotales(RemuneracionContrato.ToString());
            ws.Range("I" + (10 + i) + ":" + "I" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("J" + (10 + i)).Value = FormatoMontosTotales(RemuneracionBasica.ToString());
            ws.Range("J" + (10 + i) + ":" + "J" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("K" + (10 + i)).Value = FormatoMontosTotales(AsigFamiliar.ToString());
            ws.Range("K" + (10 + i) + ":" + "K" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("L" + (10 + i)).Value = FormatoMontosTotales(Movilidad.ToString());
            ws.Range("L" + (10 + i) + ":" + "L" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("M" + (10 + i)).Value = FormatoMontosTotales(Comisiones.ToString());
            ws.Range("M" + (10 + i) + ":" + "M" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("N" + (10 + i)).Value = FormatoMontosTotales(Otros.ToString());
            ws.Range("N" + (10 + i) + ":" + "N" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Cell("O" + (10 + i)).Value = FormatoMontosTotales(RemuneracionComputable.ToString());
            ws.Range("O" + (10 + i) + ":" + "O" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Cell("P" + (10 + i)).Value = FormatoMontosTotales(GratiPagar.ToString());
            ws.Range("P" + (10 + i) + ":" + "P" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Cell("Q" + (10 + i)).Value = FormatoMontosTotales(BonifiExtraordinaria.ToString());
            ws.Range("Q" + (10 + i) + ":" + "Q" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Cell("R" + (10 + i)).Value = FormatoMontosTotales(PEPS.ToString());
            ws.Range("R" + (10 + i) + ":" + "R" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("S" + (10 + i)).Value = FormatoMontosTotales(TotalPagar.ToString());
            ws.Range("S" + (10 + i) + ":" + "S" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            //ws.Cell("R" + (10 + i)).Value = FormatoMontosTotales(TotalPagar.ToString());
            //ws.Range("R" + (10 + i) + ":" + "R" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


           
            ws.Range("A" + (10 + i) + ":" + "R" + (10 + i)).Style.Font.Bold = true;
            ws.Range("F" + (10 + i) + ":" + "Q" + (10 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("I9:"+"Q" + (9 + i)).SetDataType(XLCellValues.Text);
            ws.Range("A9:" + "Q" + (9 + i)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Range("F" + (10 + i) +":" + "Q" + (10 + i)).SetDataType(XLCellValues.Text);
            ws.Range("I9:" + "Q" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            ws.Columns("E:E").Width = 11;
            ws.Columns("D:D").Width = 22.57;

            // Prepare the response
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");

            //// Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();

        }

        #endregion



        #region  Exportacion_Excel_Claculo_Quinta_Categoria_POR_deivi_gomez
        public void ToExce_Quinta_Categoria(System.Data.DataTable dtInput, string filename, HttpResponse response, string title)
        {
            XLWorkbook workbook = new XLWorkbook();
            String fecha2 = "";
            var ws = workbook.Worksheets.Add(title);


            ws.Cell("A7").Value = "#";
            ws.Range("A7:A8").Column(1).Merge();
            ws.Range("A7:A8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("A7:A8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Range("A7:A8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Columns("A:A").Width = 4.14;

            ws.Cell("B7").Value = "Apellidos y Nombres";
            ws.Range("B7:B8").Column(1).Merge();
            ws.Range("B7:B8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("B7:B8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Columns("B:B").Width = 35;

            ws.Cell("C7").Value = "Nº DNI";
            ws.Range("C7:C8").Column(1).Merge();
            ws.Range("C7:C8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("C7:C8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("D7").Value = "Cargo u Ocupación";
            ws.Range("D7:D8").Column(1).Merge();
            ws.Range("D7:D8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("D7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("D7:D8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Columns("D:D").Width = 31.86;

            ws.Cell("E7").Value = "Fecha de Ingreso";
            ws.Range("E7:E8").Column(1).Merge();
            ws.Range("E7:E8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("E7:E8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("E7:E8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Columns("E:E").Width = 10.50;


            ws.Cell("F7").Value = "Remuneración Previa";
            ws.Range("F7:F8").Column(1).Merge();
            ws.Range("F7:F8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("F7:F8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("F7:F8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


            ws.Cell("G7").Value = "Remuneración Mensual";
            ws.Range("G7:G8").Column(1).Merge();
            ws.Range("G7:G8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("G7:G8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("G7:G8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


            ws.Cell("H7").Value = "Factor Multiplicante";
            ws.Range("H7:H8").Column(1).Merge();
            ws.Range("H7:H8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("H7:H8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("H7:H8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("I7").Value = "Remuneración anual proyectada";
            ws.Range("I7:I8").Column(1).Merge();
            ws.Range("I7:I8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("I7:I8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("I7:I8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


            ws.Cell("J7").Value = "Otras remuneraciones anual proyectada";
            ws.Range("J7:J8").Column(1).Merge();
            ws.Range("J7:J8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("J7:J88").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("J7:J8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("K7").Value = "Total Ingresos";
            ws.Range("K7:K8").Column(1).Merge();
            ws.Range("K7:K8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("K7:K8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("K7:K8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("L7").Value = "Monto deducible";
            ws.Range("L7:L8").Column(1).Merge();
            ws.Range("L7:L8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("L7:L8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("L7:L8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


            ws.Cell("M7").Value = "Monto Imponible  (Total renta 5C)";
            ws.Range("M7:M8").Column(1).Merge();
            ws.Range("M7:M8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("M7:M8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("M7:M8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("N7").Value = "Impuesto a la venta anual";
            ws.Range("N7:N8").Column(1).Merge();
            ws.Range("N7:N8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("N7:N8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("N7:N8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("O7").Value = "Retenciones Previas";
            ws.Range("O7:O8").Column(1).Merge();
            ws.Range("O7:O8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("O7:O8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("O7:O8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("P7").Value = "Renta imponible a retener";
            ws.Range("P7:P8").Column(1).Merge();
            ws.Range("P7:P8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("P7:P8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("P7:P8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Cell("Q7").Value = "Importe a retener";
            ws.Range("Q7:Q8").Column(1).Merge();
            ws.Range("Q7:Q8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("Q7:Q8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;
            ws.Range("Q7:Q8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Rows("8").Height = 42;

            int i = -1;
            double RemAnualPrev = 0, RemunMes = 0, FactorMultipl = 0, RemAnual = 0, GratifVirtuales = 0, TotalIngresos = 0, MontoDeduc = 0,
                             MontoImponible = 0, ImpuestoAnual = 0, PagosCuentaPrev = 0, Saldo = 0,
                             Dscto5taCatg = 0;



            if (dtInput.Rows.Count > 0)
            {
                DataRow row = dtInput.Rows[0];

                ws.Cell("A1").Value = row["RazonSocial"].ToString();
                ws.Cell("A1").Style.Font.Bold = true;
                ws.Cell("A2").Value = "RUC Nº : " + row["NumeroRuc"].ToString();
                ws.Cell("A2").Style.Font.Bold = true;
                ws.Cell("A3").Value = "DIRECCIÓN : " + row["Direccion"].ToString();
                ws.Cell("A3").Style.Font.FontSize = 9;

                ws.Cell("G4").Value = "PLANILLA RENTA 5TA CATEGORIA";
                ws.Cell("G4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("G4").Style.Font.Bold = true;
                ws.Cell("G4").Style.Font.FontSize = 14;

                ws.Cell("G5").Value = "CORRESPONDIENTE AL MES DE " + row["Mes"].ToString() + " DEL " + row["Anio"].ToString();
                ws.Cell("G5").Style.Font.Bold = true;
                ws.Cell("G5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("G5").Style.Font.FontSize = 11;



            }



            foreach (System.Data.DataRow item in dtInput.Rows)
            {
                i++;

                ws.Cell("A" + (9 + i)).Value = item["ORDEN"].ToString();
                ws.Cell("A" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("B" + (9 + i)).Value = item["Apellido"].ToString() + " " + item["Nombre"].ToString();
                ws.Cell("B" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                ws.Cell("C" + (9 + i)).Value = item["NumeroDocumento"].ToString();
                ws.Cell("C" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                ws.Cell("D" + (9 + i)).Value = item["Descripcion"].ToString();
                ws.Cell("D" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell("D" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Distributed;


                ws.Cell("E" + (9 + i)).Value = item["ContratoInicio"].ToString();
                ws.Cell("E" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                ws.Cell("F" + (9 + i)).Value = (item["RemAnualPrev"].ToString().Equals("0,0000") ? " " : FormatoMontosSINCOMA(item["RemAnualPrev"].ToString()));
                ws.Cell("F" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("G" + (9 + i)).Value = (item["RemunMes"].ToString().Equals("0,0000") ? " " : FormatoMontosSINCOMA(item["RemunMes"].ToString()));
                ws.Cell("G" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("H" + (9 + i)).Value = (item["FactorMultipl"].ToString().Equals("0,0000") ? "" : FormatoMontosSINCOMA(item["FactorMultipl"].ToString()));
                ws.Cell("H" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("I" + (9 + i)).Value = (item["RemAnual"].ToString().Equals("0,0000") ? "" : FormatoMontosSINCOMA(item["RemAnual"].ToString()));
                ws.Cell("I" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("J" + (9 + i)).Value = (item["GratifVirtuales"].ToString().Equals("0,0000") ? "" : FormatoMontosSINCOMA(item["GratifVirtuales"].ToString()));
                ws.Cell("J" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("K" + (9 + i)).Value = (item["TotalIngresos"].ToString().Equals("0,0000") ? "" : FormatoMontosSINCOMA(item["TotalIngresos"].ToString()));
                ws.Cell("K" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("L" + (9 + i)).Value = (item["MontoDeduc"].ToString().Equals("0,0000") ? "" : FormatoMontosSINCOMA(item["MontoDeduc"].ToString()));
                ws.Cell("L" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("M" + (9 + i)).Value = (item["MontoImponible"].ToString().Equals("0,0000") ? "" : FormatoMontosSINCOMA(item["MontoImponible"].ToString()));
                ws.Cell("M" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("N" + (9 + i)).Value = (item["ImpuestoAnual"].ToString().Equals("0,0000") ? "" : FormatoMontosSINCOMA(item["ImpuestoAnual"].ToString()));
                ws.Cell("N" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("O" + (9 + i)).Value = (item["PagosCuentaPrev"].ToString().Equals("0,0000") ? "" : FormatoMontosSINCOMA(item["PagosCuentaPrev"].ToString()));
                ws.Cell("O" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                ws.Cell("P" + (9 + i)).Value = (item["Saldo"].ToString().Equals("0,00") ? "" : FormatoMontosSINCOMA(item["Saldo"].ToString()));
                ws.Cell("P" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Cell("Q" + (9 + i)).Value = (item["Dscto5taCatg"].ToString().Equals("0,00") ? "" : FormatoMontosSINCOMA(item["Dscto5taCatg"].ToString()));
                ws.Cell("Q" + (9 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                RemAnualPrev = RemAnualPrev + (item["RemAnualPrev"].ToString().Equals("") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["RemAnualPrev"].ToString()));
                RemunMes = RemunMes + (item["RemunMes"].ToString().Equals("") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["RemunMes"].ToString()));
                FactorMultipl = FactorMultipl + (item["FactorMultipl"].ToString().Equals("") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["FactorMultipl"].ToString()));
                RemAnual = RemAnual + (item["RemAnual"].ToString().Equals("") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["RemAnual"].ToString()));
                GratifVirtuales = GratifVirtuales + (item["GratifVirtuales"].ToString().Equals("") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["GratifVirtuales"].ToString()));
                TotalIngresos = TotalIngresos + (item["TotalIngresos"].ToString().Equals("") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["TotalIngresos"].ToString()));
                MontoDeduc = MontoDeduc + (item["MontoDeduc"].ToString().Equals("") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["MontoDeduc"].ToString()));
                MontoImponible = MontoImponible + (item["MontoImponible"].ToString().Equals("") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["MontoImponible"].ToString()));
                ImpuestoAnual = ImpuestoAnual + (item["ImpuestoAnual"].ToString().Equals("") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["ImpuestoAnual"].ToString()));
                PagosCuentaPrev = PagosCuentaPrev + (item["PagosCuentaPrev"].ToString().Equals("") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["PagosCuentaPrev"].ToString()));
                Saldo = Saldo + (item["Saldo"].ToString().Equals(" ") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["Saldo"].ToString()));
                Dscto5taCatg = Dscto5taCatg + (item["Dscto5taCatg"].ToString().Equals("") ? ConvertirStringADouble("0") : ConvertirStringADouble(item["Dscto5taCatg"].ToString()));
       
            }


            ws.Cell("A" + (10 + i)).Value = "TOTALES S/.";
            ws.Range("A" + (10 + i) + ":" + "E" + (10 + i)).Row(1).Merge();
            ws.Range("A" + (10 + i) + ":" + "E" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("A" + (10 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("F" + (10 + i)).Value = FormatoMontosTotales(RemAnualPrev.ToString());
            ws.Range("F" + (10 + i) + ":" + "F" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("G" + (10 + i)).Value = FormatoMontosTotales(RemunMes.ToString());
            ws.Range("G" + (10 + i) + ":" + "G" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("H" + (10 + i)).Value = FormatoMontosTotales(FactorMultipl.ToString());
            ws.Range("H" + (10 + i) + ":" + "H" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("I" + (10 + i)).Value = FormatoMontosTotales(RemAnual.ToString());
            ws.Range("I" + (10 + i) + ":" + "I" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("J" + (10 + i)).Value = FormatoMontosTotales(GratifVirtuales.ToString());
            ws.Range("J" + (10 + i) + ":" + "J" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("K" + (10 + i)).Value = FormatoMontosTotales(TotalIngresos.ToString());
            ws.Range("K" + (10 + i) + ":" + "K" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("L" + (10 + i)).Value = FormatoMontosTotales(MontoDeduc.ToString());
            ws.Range("L" + (10 + i) + ":" + "L" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("M" + (10 + i)).Value = FormatoMontosTotales(MontoImponible.ToString());
            ws.Range("M" + (10 + i) + ":" + "M" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Cell("N" + (10 + i)).Value = FormatoMontosTotales(ImpuestoAnual.ToString());
            ws.Range("N" + (10 + i) + ":" + "N" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("O" + (10 + i)).Value = FormatoMontosTotales(PagosCuentaPrev.ToString());
            ws.Range("O" + (10 + i) + ":" + "O" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


            ws.Cell("P" + (10 + i)).Value = FormatoMontosTotales(Saldo.ToString());
            ws.Range("P" + (10 + i) + ":" + "P" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("Q" + (10 + i)).Value = FormatoMontosTotales(Dscto5taCatg.ToString());
            ws.Range("Q" + (10 + i) + ":" + "Q" + (10 + i)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



            ws.Range("A" + (10 + i) + ":" + "Q" + (10 + i)).Style.Font.Bold = true;
            ws.Range("A7"  + ":" + "Q8" ).Style.Font.Bold = true;
            ws.Range("F" + (10 + i) + ":" + "Q" + (10 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("F9:" + "Q" + (9 + i)).SetDataType(XLCellValues.Text);
            ws.Range("A9:" + "Q" + (9 + i)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Range("F" + (10 + i) + ":" + "Q" + (10 + i)).SetDataType(XLCellValues.Text);
            ws.Range("F9:" + "Q" + (9 + i)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            ws.Range("A9:" + "Q" + (9 + i)).Style.Font.FontSize = 10;


            // Prepare the response
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");

            //// Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();

        }

        #endregion




        private String FormatoMontos(String monto)
        {
      
            String totalmonto = "", converstring = ""; Double convertmontod = 0;
            if (monto.Equals(""))
            {
                monto = "0";
            }

            convertmontod = Math.Round(Convert.ToDouble(monto), 1);
            converstring = convertmontod.ToString();

            return totalmonto = Double.Parse(converstring).ToString("0,0.00", CultureInfo.InvariantCulture); 

        }

        private String FormatoMontosSINCOMA(String monto)
        {
            String totalmonto = "";
            if (monto.Equals(""))
            {
                monto = "0";
            }

            return totalmonto = Double.Parse(monto).ToString("0.00", CultureInfo.InvariantCulture); 
        }

        private static String FormatoMontosSINCOMA2(String monto)
        {
            String totalmonto = "";
            if (monto.Equals("") || monto.Equals("0,00")||monto.Equals("0"))
            {
                return monto="";
            }
            else
            {
                return totalmonto = Double.Parse(monto).ToString("0.00", CultureInfo.InvariantCulture);
            }
        }

        private String FormatoMontosSINREDONDEO(String monto)
        {

            String totalmonto = "";
            if (monto.Equals(""))
            {
                monto = "0";
            }
            return totalmonto = Double.Parse(monto).ToString("0,0.00", CultureInfo.InvariantCulture);
        }


        private String CalculoTiempo(String valor, String valo2)
        {

            if (valor.Equals(""))
            {
                valor = "00:00:00";
            }
            if (valo2.Equals(""))
            {
                valo2 = "00:00:00";
            }

            var TimeSpan1 = new TimeSpan(Convert.ToInt32(valor.Split(':')[0]), Convert.ToInt32(valor.Split(':')[1]), Convert.ToInt32(valor.Split(':')[2]));
            var TimeSpan2 = new TimeSpan(Convert.ToInt32(valo2.Split(':')[0]), Convert.ToInt32(valo2.Split(':')[1]), Convert.ToInt32(valo2.Split(':')[2]));
            String Time3 = String.Format("{0}:{1}:{2}", Math.Truncate((TimeSpan1 + TimeSpan2).TotalHours).ToString("00"), (TimeSpan1 + TimeSpan2).Minutes.ToString("00"), (TimeSpan1 + TimeSpan2).Seconds.ToString("00"));
            return Time3;
        }

        private static double ConvertirStringADouble(String monto)
        {
            double totalmonto = 0;
            return totalmonto = Convert.ToDouble(monto);
        }

        private String FormatoMontosTotales(String monto)
        {
            String totalmonto = "", converstring = ""; Double convertmontod = 0;
            convertmontod = Math.Round(Convert.ToDouble(monto), 1);
            converstring = convertmontod.ToString();
            return totalmonto = Double.Parse(converstring).ToString("0,0.00", CultureInfo.InvariantCulture);

        }

    
    }







   //}
}