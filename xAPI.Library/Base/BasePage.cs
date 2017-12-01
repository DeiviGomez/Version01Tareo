
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace xAPI.Library.Base
{
    public class BasePage : System.Web.UI.Page
    {
        //public void Alert(string message)
        //{
        //    ClientScript.RegisterStartupScript(typeof(Page), "alerta", @"<script type='text/javascript'>jAlert('" + message +"', 'Alert');</script>", false);
        //}        
        //public void Alert(string message,string function)
        //{
        //    ClientScript.RegisterStartupScript(typeof(Page), 
        //        "alerta", @"<script type='text/javascript'>jAlert('" + message + "', 'Alert'," + function + ");</script>", false);
        //}        
        
        //public static void Alert(string message,ref Page  page){
        //    page.ClientScript.RegisterStartupScript(typeof(Page), "alerta", @"<script type='text/javascript'>jAlert('" + message + "', 'Alert');</script>", false);
        //}

        //public void JQueryDateTimePicker(String StartDate, String EndDate)
        //{
        //    String jscript = " var dates = $('input[id$=" + StartDate + "],input[id$="+EndDate+"]').datepicker({" +
        //     "defaultDate: \"+1w\","+
        //     "changeMonth: true,"+
        //     "changeYear: true,"+
        //     "onSelect: function (selectedDate) {"+

        //         "var option = $(\"input[id$=" + StartDate + "]\").attr('id') == this.id ? \"minDate\" : \"maxDate\"," +
        //            "instance = $(this).data(\"datepicker\"),"+
        //            "date = $.datepicker.parseDate("+
        //                "instance.settings.dateFormat ||"+
        //                "$.datepicker._defaults.dateFormat,"+
        //                "selectedDate, instance.settings);"+
        //         "dates.not(this).datepicker(\"option\", option, date);}});";

        //    ClientScript.RegisterStartupScript(typeof(Page), "date", @"<script type='text/javascript'>"+jscript+"</script>", false);
        //}

        //public void SendDownload(String filePath)
        //{
        //    try
        //    {
        //        FileInfo file = new FileInfo(filePath);
        //        if (file.Exists)
        //        {
        //            Response.Clear();
        //            Response.ClearHeaders();
        //            Response.ClearContent();
        //            Response.AddHeader("content-disposition", "attachment; filename=" + Path.GetFileName(filePath));
        //            Response.AddHeader("Content-Type", "application/Excel");
        //            //Response.ContentType = "application/vnd.xls";
        //            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("Content-Length", file.Length.ToString());
        //            Response.WriteFile(file.FullName);
        //            Response.Flush();
        //            file.Delete();
        //            Response.End();
        //            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        //        }                
        //    }
        //    catch (Exception ex)
        //    {
                
        //    }            
        //}
    }
}
