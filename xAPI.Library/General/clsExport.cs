using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using xAPI.Library.Base;

namespace xAPI.Library.General
{
    public class clsExport
    {

        public static clsExcel ExcelExport(DataTable dt, string fileName, string resourcePath)
        {
            if (!Directory.Exists(resourcePath))
                Directory.CreateDirectory(resourcePath);
            clsExcel objExcel = new clsExcel();
            return objExcel;
        }
        public static clsCsv CSVExport(DataTable dt, string fileName, string resourcePath)
        {
            if (!Directory.Exists(resourcePath))
                Directory.CreateDirectory(resourcePath);
            clsCsv objCSV = new clsCsv();
            return objCSV;
        }






        /// <summary>
        /// CREATE AND SAVE PDF FILE
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public static bool iTextSharpCreatePdfFile(DataTable dt, string fileName,float[] columnsWidth,HttpResponse Response)
        {
            bool isSuccess = false;
            try
            {
             
                Document document = new Document();
                MemoryStream mStream = new MemoryStream();
                //PdfWriter writer = PdfWriter.GetInstance(document, new FileStream((resourcePath + "\\" + fileName), FileMode.Create));
                PdfWriter writer = PdfWriter.GetInstance(document, mStream);
                document.Open();
                iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 11, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font font4 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 11);

                PdfPTable table = new PdfPTable(dt.Columns.Count);
                PdfPRow row = null;
                //float[] columnsWidth = new float[] { 4f, 4f, 4f, 4f, 4f, 4f };

                table.SetWidths(columnsWidth);
                table.WidthPercentage = 100;

                //int iCol = 0;
                //string colname = "";
                //PdfPCell cell = new PdfPCell(new Phrase("Products"));
                //cell.Colspan = dt[0].Columns.Count;

                foreach (DataColumn c in dt.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(c.ColumnName, font5));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                foreach (DataRow r in dt.Rows)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(r[0].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[1].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[2].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[3].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[4].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[5].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);
                }
                document.Add(table);
                document.Close();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                Response.Clear();
                Response.BinaryWrite(mStream.ToArray());
                Response.End();

                isSuccess = true;
            }
            catch (Exception ex) {  }

            return isSuccess;
        }

        public static bool iTextSharpCreatePdfFileVC(DataTable dt, DataTable dtFooter, string fileName, float[] columnsWidth, HttpResponse Response)
        {
            bool isSuccess = false;
            try
            {

                Document document = new Document();
                MemoryStream mStream = new MemoryStream();
                //PdfWriter writer = PdfWriter.GetInstance(document, new FileStream((resourcePath + "\\" + fileName), FileMode.Create));
                PdfWriter writer = PdfWriter.GetInstance(document, mStream);
                document.Open();
                iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 11, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font font4 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 11);

                PdfPTable table = new PdfPTable(dt.Columns.Count);
                PdfPRow row = null;

                PdfPTable tableF = new PdfPTable(dtFooter.Columns.Count);
                PdfPRow rowF = null;
                //float[] columnsWidth = new float[] { 4f, 4f, 4f, 4f, 4f, 4f };

                table.SetWidths(columnsWidth);
                table.WidthPercentage = 100;

                float[] columnsWidthF = new float[] { 4f, 2f };
                tableF.SetWidths(columnsWidthF);
                tableF.WidthPercentage = 100;

                //int iCol = 0;
                //string colname = "";
                //PdfPCell cell = new PdfPCell(new Phrase("Products"));
                //cell.Colspan = dt[0].Columns.Count;

                foreach (DataColumn c in dt.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(c.ColumnName, font5));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                foreach (DataRow r in dt.Rows)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(r[0].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[1].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[2].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[3].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[4].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[5].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);
                }

                foreach (DataRow f in dtFooter.Rows)
                {
                    PdfPCell cell = new PdfPCell(new Phrase("Total: "+f[0].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tableF.AddCell(cell);
                    cell = new PdfPCell(new Phrase(f[1].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tableF.AddCell(cell);
                }
                document.Add(table);
                document.Add(tableF);
                document.Close();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                Response.Clear();
                Response.BinaryWrite(mStream.ToArray());
                Response.End();

                isSuccess = true;
            }
            catch (Exception ex) { }

            return isSuccess;
        }

        public static bool iTextSharpCreatePdfFileVRA(DataTable dt, string fileName, float[] columnsWidth, HttpResponse Response)
        {
            bool isSuccess = false;
            try
            {

                Document document = new Document();
                MemoryStream mStream = new MemoryStream();
                //PdfWriter writer = PdfWriter.GetInstance(document, new FileStream((resourcePath + "\\" + fileName), FileMode.Create));
                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                PdfWriter writer = PdfWriter.GetInstance(document, mStream);
                document.Open();
                iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font font4 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 9);

                PdfPTable table = new PdfPTable(dt.Columns.Count);
                PdfPRow row = null;
                //float[] columnsWidth = new float[] { 4f, 4f, 4f, 4f, 4f, 4f };

                table.SetWidths(columnsWidth);
                table.WidthPercentage = 100;
                
                //int iCol = 0;
                //string colname = "";
                //PdfPCell cell = new PdfPCell(new Phrase("Products"));
                //cell.Colspan = dt[0].Columns.Count;

                foreach (DataColumn c in dt.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(c.ColumnName, font5));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                foreach (DataRow r in dt.Rows)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(r[0].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[1].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[2].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[3].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[4].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[5].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(r[6].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(r[7].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(r[8].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(r[9].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }
                document.Add(table);
                document.Close();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                Response.Clear();
                Response.BinaryWrite(mStream.ToArray());
                Response.End();

                isSuccess = true;
            }
            catch (Exception ex) { }

            return isSuccess;
        }

        //by gm
        public static void ExportPDF_v2(DataTable myDt,String fileName, HttpResponse Response,String [] colum=null)
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
            try
            {
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();
           

                Font font8 = FontFactory.GetFont("Times", 5);
                Font fontTitle = FontFactory.GetFont("Times", 10,new BaseColor(132,127,131));
                Font fontbold = FontFactory.GetFont("Times-Bold", 4);
                DataTable dt = myDt;
                DataColumn dc = new DataColumn();
                if (dt != null)
                {
                    //Craete instance of the pdf table and set the number of column in that table  
                    PdfPTable PdfTable = new PdfPTable(dt.Columns.Count);
                    PdfTable.DefaultCell.Padding = 2;
                    
                    PdfTable.WidthPercentage = 100; // percentage

                    PdfTable.DefaultCell.BorderWidth = 0;
                    PdfTable.DefaultCell.BorderColor = new BaseColor(255, 255, 255);
                    PdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    PdfTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                   int alignText=Element.ALIGN_LEFT;
                    PdfPCell PdfPCell = null;
                    PdfPCell cell = new PdfPCell(new Phrase(new Chunk(dt.TableName,fontTitle)));
                    
                    if (colum != null)
                    {
                        cell.Colspan = colum.Length;
                    }
                    else {
                         cell.Colspan = dt.Columns.Count;
                    }
                    cell.PaddingBottom = 10;
                    cell.Border = 0;
                   
                    cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //cell.BackgroundColor = new BaseColor(238,238,238);
                    //cell.BorderColorBottom = new BaseColor(0, 0, 0);
                    //cell.BorderWidthBottom = 2;
                    PdfTable.AddCell(cell);
                    if (colum == null)
                    {
                        foreach (DataColumn column in dt.Columns)
                        {
                            
                            PdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            PdfTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            PdfTable.DefaultCell.BackgroundColor = new BaseColor(238, 238, 238);
                            PdfTable.DefaultCell.BorderColorBottom = new BaseColor(0, 0, 0);
                            PdfTable.DefaultCell.BorderWidthBottom = 2;
                            //PdfTable.DefaultCell.BorderColorLeft = new BaseColor(255, 255, 255);
                            PdfTable.DefaultCell.BorderWidthLeft = 1;
                            //PdfTable.DefaultCell.BorderColorRight = new BaseColor(255, 255, 255);
                            PdfTable.DefaultCell.BorderWidthRight = 1;
                            PdfTable.DefaultCell.BorderWidthRight = 1;
                            //PdfTable.DefaultCell.BorderColorTop = new BaseColor(255, 255, 255);
                            PdfTable.AddCell(new Phrase(new Chunk(column.ColumnName, fontbold)));

                        }
                    }
                    else {
                        foreach (string column in colum)
                        {
                            PdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            PdfTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            PdfTable.DefaultCell.BackgroundColor = new BaseColor(238, 238, 238);
                            PdfTable.DefaultCell.BorderColorBottom = new BaseColor(0, 0, 0);
                            PdfTable.DefaultCell.BorderWidthBottom = 2;
                            //PdfTable.DefaultCell.BorderColorLeft = new BaseColor(255, 255, 255);
                            PdfTable.DefaultCell.BorderWidthLeft = 1;
                            //PdfTable.DefaultCell.BorderColorRight = new BaseColor(255, 255, 255);
                            PdfTable.DefaultCell.BorderWidthRight = 1;
                            //PdfTable.DefaultCell.BorderColorTop = new BaseColor(255, 255, 255);
                            PdfTable.DefaultCell.PaddingBottom = 4;
                            PdfTable.AddCell(new Phrase(new Chunk(column.ToString(), fontbold)));
                            


                        }    
                    }
                    
                    for (int rows = 0; rows < dt.Rows.Count; rows++)
                    {
                        for (int column = 0; column < dt.Columns.Count; column++)
                        {
                            PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font8)));
                            //"1287543.0" will return false for a long
                            long number1 = 0;
                            bool canConvert = long.TryParse(dt.Rows[rows][column].ToString(), out number1);
                            if (canConvert)
                            {
                                alignText = Element.ALIGN_RIGHT;
                            }
                            else {
                                alignText = Element.ALIGN_CENTER;
                            }
                            PdfPCell.BorderWidth = 0;
                            
                            PdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            PdfPCell.HorizontalAlignment = alignText;
                            PdfTable.AddCell(PdfPCell);
                        }
                    }
                    //PdfTable.SpacingBefore = 15f; // Give some space after the text or it may overlap the table            
                    pdfDoc.Add(PdfTable); // add pdf table to the document   
                }
                pdfDoc.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename="+fileName);
                System.Web.HttpContext.Current.Response.Write(pdfDoc);
                Response.Flush();
                Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();  
            }
            catch (DocumentException de)
            {
                System.Web.HttpContext.Current.Response.Write(de.Message);
            }
            catch (IOException ioEx)
            {
                System.Web.HttpContext.Current.Response.Write(ioEx.Message);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
        }

    }
}
