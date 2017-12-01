using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Library.Base;
using System.Net;
using System.IO;
using System.Reflection;
using System.Web;
using System.Text.RegularExpressions;
using Microsoft.Web.Administration;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;
using System.Management;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using xAPI.Library.Security;
using System.Globalization;

//using System.DirectoryServices;


namespace xAPI.Library.General
{
    public static class clsUtilities
    {

        static Regex regularExpressionEmail = new Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");
        

        #region ValidationCard
        public static Boolean CardValidation(String card)
        {
            Boolean validate = false;
            card = card.Trim();
            try
            {
                Int32 card_len = card.Length; /* card length */
                int count; /* a counter */
                int weight; /* weight to apply to digit being checked */
                int sum; /* sum of weights */
                int digit; /* digit being checked */
                Int32 mod;
                weight = 2;
                sum = 0;
                for (count = card_len - 2; count >= 0; count = count - 1)
                {
                    digit = weight * (Convert.ToInt32(card[count].ToString()));
                    sum = sum + (digit / 10) + (digit % 10);
                    if (weight == 2)
                        weight = 1;
                    else
                        weight = 2;
                }
                mod = (10 - sum % 10) % 10;
                if (Convert.ToInt32(card[card.Length - 1].ToString()) == mod)
                    validate = true;
                else
                    validate = false;
            }
            catch (Exception)
            {

                validate = false;
            }

            return validate;
        }
        #endregion
        #region Captcha

        /// <summary>
        /// Generate a random number.
        /// </summary>
        /// <returns>String of six random digits.</returns>
        public static string GenerateRandomCode()
        {
            Random random = new Random();
            string s = "";
            for (int i = 0; i < 6; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        public static string GetValueResourceType(string contentType)
        {
            string resp = string.Empty;
            switch (contentType)
            {
                case ("image/gif"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Image);
                    break;
                case ("image/jpeg"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Image);
                    break;
                case ("image/png"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Image);
                    break;
                case ("video/mpeg"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Video);
                    break;
                case ("video/mp4"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Video);
                    break;
                case ("video/quicktime"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Video);
                    break;
                case ("video/x-ms-wmv"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Video);
                    break;
                case ("video/x-flv"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Video);
                    break;
                case ("audio/mp4"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Audio);
                    break;
                case ("audio/mpeg"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Audio);
                    break;
                case ("audio/x-ms-wma"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Audio);
                    break;
                case ("audio/mp3"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Audio);
                    break;
                case ("application/msword"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Document);
                    break;
                case ("application/vnd.openxmlformats-officedocument.wordprocessingml.document"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Document);
                    break;
                case ("application/vnd.ms-excel"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Document);
                    break;
                case ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Document);
                    break;
                case ("application/pdf"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Document);
                    break;
                case ("application/x-pdf"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Document);
                    break;
                case ("application/vnd.ms-powerpoint"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Document);
                    break;
                case ("application/vnd.openxmlformats-officedocument.presentationml.presentation"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Document);
                    break;
                case ("application/application/vnd.openxmlformats-officedocument.presentationml.slideshow"):
                    resp = clsEnum.GetStringValue(EnumMimeType.Document);
                    break;
                default:
                    break;
            }
            return resp;
        }

        public static Boolean ValidateMimeType(Int32 type, String name)
        {
            Boolean isCorrect = false;
            switch (type)
            {
                case (Int32)EnumMimeType.Image:
                    {
                        if (name == "image/gif" || name == "image/jpeg" || name == "image/png")
                            isCorrect = true;
                        break;
                    }
                case (Int32)EnumMimeType.Video:
                    {

                        if (name == "video/mpeg" || name == "video/mp4" ||
                            name == "video/quicktime" || name == "video/x-ms-wmv" || name == "video/x-flv")
                            isCorrect = true;
                        break;
                    }
                case (Int32)EnumMimeType.Audio:
                    {
                        if (name == "audio/mp4" || name == "audio/mpeg" ||
                            name == "audio/x-ms-wma" || name == "audio/mp3")
                            isCorrect = true;
                        break;
                    }
                case (Int32)EnumMimeType.Document:
                    {
                        if (name == "application/msword" || name == "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                            || name == "application/vnd.ms-excel" || name == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                            || name == "application/pdf" || name == "application/x-pdf")
                            isCorrect = true;
                        break;
                    }

                case (Int32)EnumMimeType.Presentation:
                    {
                        if (name == "application/vnd.ms-powerpoint" ||
                            name == "application/vnd.openxmlformats-officedocument.presentationml.presentation" ||
                            name == "application/vnd.openxmlformats-officedocument.presentationml.slideshow")
                            isCorrect = true;
                        break;
                    }
            }
            return isCorrect;
        }
        #endregion
        #region Application
        public static String CreateApplication(ref BaseEntity Base, string SiteName, string Name, string PhysicalPath, string appPool)
        {

            //            To add an application to a site, use the following syntax:

            //appcmd add app /site.name: string /path: string /physicalPath: string

            //The variable site.namestring is the name of the Web site to which you want to add the application. The variable pathstring is the virtual path of the application, such as /application, and physicalPathstring is the physical path of the application's content in the file system.

            //For example, to add an application named marketing to a site named contoso, with content at c:\application, type the following at the command prompt, and then press ENTER:

            //appcmd add app /site.name: contoso /path:/ marketing /physicalPath:c:\ application

            //String command2 =
            //    "cd c:\\windows\\system32\\inetsrv & appcmd add vdir /app.name:\"" +
            //    SiteName + "/\" /path:/" + Name + " /physicalPath:" + PhysicalPath;

            //cd c:\\windows\\system32\\inetsrv & appcmd add app /site.name:xReplicate /path:/marketing /physicalPath:C:\FolderPages\xReplicate



            //cd c:\\windows\\system32\\inetsrv & appcmd add app /site.name:xReplicate /path:/zchula /physicalPath:C:\FolderPages\xReplicate /applicationPool:xReplicate

            String message = "";
            String command =
                "cd c:\\windows\\system32\\inetsrv & appcmd add app /site.name:" +
                SiteName.Trim() + " /path:/" + Name.Trim() + " /physicalPath:" + PhysicalPath.Trim() + " /applicationPool:" + appPool.Trim();
            // "cd c:\\windows\\system32\\inetsrv & appcmd add app /site.name:" +
            //SiteName.Trim() + " /path:/" + Name.Trim() + " /physicalPath:" + PhysicalPath.Trim()+" & "+
            //"appcmd set app /app.name:" + SiteName.Trim() + "/" + Name.Trim() + " /applicationPool:xReplicate";            
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.WaitForExit();
                message = proc.StandardOutput.ReadToEnd().Substring(0, 5);// +"       " + proc.ExitCode;
            }

            catch (System.Exception objException)
            {
                Base.Errors.Add(new BaseEntity.ListError(objException, "An error occurred while creating and Application"));
                message = command + objException.Message;
            }
            return message;
        }
        #endregion
        #region Strings

        public static string TruncateAtWord(string input, int length)
        {
            if (input == null || input.Length < length)
                return input;
            int iNextSpace = input.LastIndexOf(" ", length);
            return string.Format("{0}...", input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
        }

        #endregion
        #region SessionStateServerSharedHelper
        public static class SessionStateServerSharedHelper
        {
            public static void ChangeAppDomainAppId(string name)
            {
                FieldInfo runtimeInfo = typeof(HttpRuntime).GetField("_theRuntime", BindingFlags.Static | BindingFlags.NonPublic);
                if (runtimeInfo == null) return;
                HttpRuntime theRuntime = (HttpRuntime)runtimeInfo.GetValue(null);
                if (theRuntime == null) return;
                FieldInfo appNameInfo = typeof(HttpRuntime).GetField("_appDomainAppId", BindingFlags.Instance | BindingFlags.NonPublic);
                if (appNameInfo == null) return;
                var appName = (String)appNameInfo.GetValue(theRuntime);
                if (appName != "applicationName") appNameInfo.SetValue(theRuntime, name);
            }
        }
        #endregion
        #region Callweb
        public static Boolean CallWeb(string soap)
        {
            Boolean isCorrect = false;
            try
            {
                String url = "";

#if DEBUG
                url = "http://localhost/AspNetDating8/Register.aspx/NewRegister";
#else
                    url = "http://trubizoffice.xirectss.com/AspNetDating8/Register.aspx/NewRegister";
#endif


                BaseEntity entity = new BaseEntity();
                //clsUsers objUser = xLogicf.Instance.MyApps_GetAppAccessCredentials(ref entity, BaseSession.Instance.SsDistributor.ID, 5);
                if (url != "")
                {

                    //string soap = "{ user:\"" + clsEncryption.Encrypt(objUser.Username) + "\",password:\"" + objUser.Password + "\" }";

                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    req.ContentType = "application/json; charset=utf-8";
                    req.Accept = "json";
                    req.Method = "POST";

                    using (Stream stm = req.GetRequestStream())
                    {
                        using (StreamWriter stmw = new StreamWriter(stm))
                        {
                            stmw.Write(soap);
                        }
                    }

                    HttpWebResponse response = (HttpWebResponse)req.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        isCorrect = true;
                    }
                }

            }
            catch (System.Exception ex)
            {
                //throw;
            }

            return isCorrect;
        }
        #endregion

        #region ValidateRegex

        public static String ValidateExpression(String value, List<EnumRegex> regexs, EnumRegexLength length = EnumRegexLength.None, Boolean Required = true, Int32? min = null, Int32? max = null)
        {
            Boolean isMatch = false;
            String message = String.Empty;
            if (String.IsNullOrEmpty(value) && Required)
            {
                //return "{0} field is required" + ValidateAndReplace((String)HttpContext.GetGlobalResourceObject("Messages", "VALIDATION_EXPRESSION_REQUIRED"), "field is required");
                return "{0} field is required";
            }

            switch (length)
            {
                case EnumRegexLength.None:
                    isMatch = true;
                    break;
                case EnumRegexLength.Exact:
                    if (min != null)
                        isMatch = value.Length == min ? true : false;
                    break;
                case EnumRegexLength.AtLeast:
                    if (min != null)
                        isMatch = value.Length >= min ? true : false;
                    break;
                case EnumRegexLength.Between:
                    if (min != null && max != null)
                        isMatch = (value.Length >= min && value.Length <= max) ? true : false;
                    break;
                case EnumRegexLength.Max:
                    if (min != null)
                        isMatch = value.Length <= min ? true : false;
                    break;
            }

            if (isMatch)
            {
                if (regexs!=null)
                foreach (EnumRegex regex in regexs)
                {
                    if (!Regex.IsMatch(value, regex.GetStringValue()))
                        message = regex.GetMessageValue();
                    if (!String.IsNullOrEmpty(message))
                        break;
                }
            }
            else
            {
                message = "{0}\'s field length invalid";
            }

            if (!Required && value != null && value.Length == 0)
            {
                message = String.Empty;
            }

            return message;
        }

        public static Boolean ValidateExpression2(String value, List<EnumRegex> regexs, EnumRegexLength length = EnumRegexLength.None, Boolean Required = true, Int32? min = null, Int32? max = null)
        {
            Boolean isMatch = false;
            String message = String.Empty;
            if (String.IsNullOrEmpty(value) && Required)
            {
                return false;
            }

            switch (length)
            {
                case EnumRegexLength.None:
                    isMatch = true;
                    break;
                case EnumRegexLength.Exact:
                    if (min != null)
                        isMatch = value.Length == min ? true : false;
                    break;
                case EnumRegexLength.AtLeast:
                    if (min != null)
                        isMatch = value.Length >= min ? true : false;
                    break;
                case EnumRegexLength.Between:
                    if (min != null && max != null)
                        isMatch = (value.Length >= min && value.Length <= max) ? true : false;
                    break;
                case EnumRegexLength.Max:
                    if (min != null)
                        isMatch = value.Length <= min ? true : false;
                    break;
            }

            if (isMatch)
            {
                if (regexs != null)
                    foreach (EnumRegex regex in regexs)
                    {
                        if (!Regex.IsMatch(value, regex.GetStringValue()))
                            message = regex.GetMessageValue();
                        if (!String.IsNullOrEmpty(message))
                        {
                            isMatch = false;
                            break;
                        }
                    }
            }
            else
            {
                message = "{0}\'s field length invalid";
            }

            if (!Required && value != null && value.Length == 0)
            {
                message = String.Empty;
            }

            return isMatch;
        }
        
        public static Boolean ValidateControl(ref String message, Control control, List<EnumRegex> regexs, String field = "", Boolean Required = true, EnumRegexLength length = EnumRegexLength.None, Int32? min = null, Int32? max = null)
        {
            Boolean IsCorrect = false;
            try
            {


                String Value = String.Empty;
                String Field = String.Empty;
                if (control is TextBox)
                {
                    Value = (control as TextBox).Text ?? String.Empty;

                    Field = (control as TextBox).ToolTip ?? String.Empty;
                }
                else
                    if (control is DropDownList)
                    {
                        Value = (control as DropDownList).SelectedValue ?? String.Empty;
                        Field = (control as DropDownList).ToolTip ?? String.Empty;
                    }
                    else
                        if (control is HiddenField)
                        {

                            Value = (control as HiddenField).Value ?? String.Empty;
                            Field = field;

                        }

                /*
                 Agregar progesivamente mas controles
                 */
                String resp = ValidateExpression(Value, regexs, length, Required, min, max);
                if (String.IsNullOrEmpty(resp))
                    IsCorrect = true;
                else
                {
                    message = String.Format(resp, Field);
                    if (!(control is HiddenField)) /*Agregar los controles que no tengan focus*/
                        control.Focus();
                }
            }
            catch (Exception)
            {
                message = "An error occurred while validating the fields";
                IsCorrect = false;
            }
            return IsCorrect;
        }
        public static Boolean ValidateAuthKey_Jixiti(String apikey,String method,String authkey)
        {
            Boolean isCorrect = false;
            List<String> lstKey = new List<String>();
            DateTime date = DateTime.Now;
            List<String> lstTime = new List<String>();
            
            String concat = apikey+"."+"/api/"+method.ToLower()+".";
            String encrypt="";
            for (int i = 10; i > 0; i--)
            {
                encrypt = Security.clsEncryption.Encrypt_Sha1(concat + date.AddMinutes(-i).ToString("yyyyMMddHHmm"),false);
                if (encrypt.Equals(authkey))
                {
                    isCorrect = true;
                    break;
                }
            }
      


            //if(String.IsNullOrEmpty(authkey))
            //    isCorrect = false;
            return true;
        }
        public static Boolean ValidateParam(ref String message,  String text, List<EnumRegex> regexs, Boolean Required = true, EnumRegexLength length = EnumRegexLength.None, Int32? min = null, Int32? max = null)
        {
            Boolean IsCorrect = false;
            try
            {


                String Value = text;
                String Field = String.Empty;
                //if (control is TextBox)
                //{
                //    Value = (control as TextBox).Text ?? String.Empty;

                //    Field = (control as TextBox).ToolTip ?? String.Empty;
                //}
                //else
                //    if (control is DropDownList)
                //    {
                //        Value = (control as DropDownList).SelectedValue ?? String.Empty;
                //        Field = (control as DropDownList).ToolTip ?? String.Empty;
                //    }
                //    else
                //        if (control is HiddenField)
                //        {

                //            Value = (control as HiddenField).Value ?? String.Empty;
                //            Field = field;

                //        }

                /*
                 Agregar progesivamente mas controles
                 */
                String resp = ValidateExpression(Value, regexs, length, Required, min, max);
                if (String.IsNullOrEmpty(resp))
                    IsCorrect = true;
                else
                {
                    message = String.Format(resp, Field);
                    //if (!(control is HiddenField)) /*Agregar los controles que no tengan focus*/
                    //    control.Focus();
                }
            }
            catch (Exception)
            {
                message = "An error occurred while validating the fields";
                IsCorrect = false;
            }
            return IsCorrect;
        }
        /// <summary>
        /// Sobrecargar para Controles: TextBox, DropDownList
        /// </summary>
        /// <param name="message">mensaje para usuario<OUTPUT></param>
        /// <param name="control">Tipo de Control , Esta sobrecarga es para TextBox, DropDownList</param>
        /// <param name="regex">Expresion regular de Validacion</param>
        /// <param name="Required">"True" si es parametro es requerido, si es Opcional "False"</param>
        /// <param name="length">Para validar cantidad de caracteres : None (No tiene validacion de cantidad) ,Exact,AtLeast,Between,Max</param>
        /// <param name="min">Cantidad minima de caracteres</param>
        /// <param name="max">Cantidad Maxima de Caracteres</param>
        /// <returns>True si es correcto la validacion </returns>
        public static Boolean ValidateString(ref String message, Control control, EnumRegex regex, Boolean Required = true, EnumRegexLength length = EnumRegexLength.None, Int32? min = null, Int32? max = null)
        {

            return ValidateControl(ref  message, control, new List<EnumRegex>() { regex }, "", Required, length, min, max);
        }


 
        /// 
        /// <summary>
        /// Sobrecargar para Controles: HiddenFIeld
        /// </summary>
        /// <param name="message">mensaje para usuario<OUTPUT></param>
        /// <param name="control">Tipo de Control , Esta sobrecarga es para HiddenFIeld</param>
        /// <param name="regex">Expresion regular de Validacion</param>
        /// <param name="field">Mensaje para mostrar ejm: "Search Distributor"</param>
        /// <param name="Required">"True" si es parametro es requerido, si es Opcional "False"</param>
        /// <param name="length">Para validar cantidad de caracteres : None (No tiene validacion de cantidad) ,Exact,AtLeast,Between,Max</param>
        /// <param name="min">Cantidad minima de caracteres</param>
        /// <param name="max">Cantidad Maxima de Caracteres</param>
        /// <returns>True si es correcto la validacion </returns>
        public static Boolean ValidateString(ref String message, Control control, EnumRegex regex = EnumRegex.None, String field = "", Boolean Required = true, EnumRegexLength length = EnumRegexLength.None, Int32? min = null, Int32? max = null)
        {

            return ValidateControl(ref  message, control, new List<EnumRegex>() { regex }, field, Required, length, min, max);
        }
        /// <summary>
        /// Sobrecargar para Controles: TextBox, DropDownList
        /// </summary>
        /// <param name="message">mensaje para usuario<OUTPUT></param>
        /// <param name="control">Tipo de Control , Esta sobrecarga es para TextBox, DropDownList</param>
        /// <param name="regexs">LISTA Expresion regular de Validacion</param>
        /// <param name="Required">"True" si es parametro es requerido, si es Opcional "False"</param>
        /// <param name="length">Para validar cantidad de caracteres : None (No tiene validacion de cantidad) ,Exact,AtLeast,Between,Max</param>
        /// <param name="min">Cantidad minima de caracteres</param>
        /// <param name="max">Cantidad Maxima de Caracteres</param>
        /// <returns>True si es correcto la validacion </returns>
        public static Boolean ValidateString(ref String message, Control control, List<EnumRegex> regexs, Boolean Required = true, EnumRegexLength length = EnumRegexLength.None, Int32? min = null, Int32? max = null)
        {

            return ValidateControl(ref  message, control, regexs, "", Required, length, min, max);
        }
        /// 
        /// <summary>
        /// Sobrecargar para Controles: HiddenFIeld
        /// </summary>
        /// <param name="message">mensaje para usuario<OUTPUT></param>
        /// <param name="control">Tipo de Control , Esta sobrecarga es para HiddenFIeld</param>
        /// <param name="regexs">LISTA Expresion regular de Validacion</param>
        /// <param name="field">Mensaje para mostrar ejm: "Search Distributor"</param>
        /// <param name="Required">"True" si es parametro es requerido, si es Opcional "False"</param>
        /// <param name="length">Para validar cantidad de caracteres : None (No tiene validacion de cantidad) ,Exact,AtLeast,Between,Max</param>
        /// <param name="min">Cantidad minima de caracteres</param>
        /// <param name="max">Cantidad Maxima de Caracteres</param>
        /// <returns>True si es correcto la validacion </returns>
        public static Boolean ValidateString(ref String message, Control control, List<EnumRegex> regexs, String field = "", Boolean Required = true, EnumRegexLength length = EnumRegexLength.None, Int32? min = null, Int32? max = null)
        {

            return ValidateControl(ref  message, control, regexs, field, Required, length, min, max);
        }






        public static Boolean ValidateString(String str, EnumRegex regex, Boolean Required = true, EnumRegexLength length = EnumRegexLength.None, Int32? min = null, Int32? max = null)
        {

            return ValidateExpression2(str, new List<EnumRegex>() { regex }, length, Required,min, max);
        }
        #endregion

        /*public static string ResolveUrl(string originalUrl)
        {
            if (originalUrl == null)
                return null;

            // *** Absolute path - just return
            if (originalUrl.IndexOf("://") != -1)
                return originalUrl;

            // *** Fix up image path for ~ root app dir directory
            if (originalUrl.StartsWith("~"))
            {
                string newUrl = "";
                if (HttpContext.Current != null)
                    newUrl = HttpContext.Current.Request.ApplicationPath +
                          originalUrl.Substring(1).Replace("//", "/");
                else
                    // *** Not context: assume current directory is the base directory
                    throw new ArgumentException("Invalid URL: Relative URL not allowed.");

                // *** Just to be sure fix up any double slashes
                return newUrl;
            }

            return originalUrl;
        }*/
        #region Regular Expression
        /// <summary>
        /// Replace malicious code by empty
        /// </summary>
        /// <param name="input">string</param>
        /// <returns>string</returns>
        public static string stripHTMLTags(string input)
        {
            return Regex.Replace(
                input,
                "(<(?<t>script|object|applet|embbed|frameset|iframe|form|textarea)(\\s+.*?)?>.*?</\\k<t>>)"
                + "|(<(script|object|applet|embbed|frameset|iframe|form|input|button|textarea)(\\s+.*?)?/?>)"
                + "|((?<=<\\w+)((?:\\s+)((?:on\\w+=((\"[^\"]*\")|('[^']*')|(.*?)))|(?<a>(?!on)\\w+=((\"[^\"]*\")|('[^']*')|(.*?)))))*(?=/?>))",
            match =>
            {
                if (!match.Groups["a"].Success)
                {
                    return string.Empty;
                }

                var attributesBuilder = new StringBuilder();

                foreach (Capture capture in match.Groups["a"].Captures)
                {
                    attributesBuilder.Append(' ');
                    attributesBuilder.Append(capture.Value);
                }

                return attributesBuilder.ToString();
            },
            RegexOptions.IgnoreCase
                | RegexOptions.Multiline
                | RegexOptions.ExplicitCapture
                | RegexOptions.CultureInvariant
                | RegexOptions.Compiled
        );
        }

        public static string stripBadHtmlImg2(string input)
        {
            return Regex.Replace(
               input,
               "<img[^>]* src=\"([^\"]*)\"[^>]*>",
           match =>
           {               
               if (match.Groups[0].Success)
               {
                   //string a = match.Groups[1].ToString();
                   //string[] parts = a.Split('.');
                   //parts[parts.Length - 1] = parts[parts.Length - 1].ToLower();
                   //if (parts[parts.Length - 1] != "jpg" && parts[parts.Length - 1] != "png" && parts[parts.Length-1]!="gif")
                   //{
                       
                   //}
                   bool band = false;
                   input = checkForImg(match.Groups[0].ToString(), ref band);

                   return input;
               }
               else
               {
                   return match.Groups[0].ToString();
               }

               /*var attributesBuilder = new StringBuilder();

               foreach (Capture capture in match.Groups[0].Captures)
               {
                   if (capture.Value.ToLower() == "jpg" && capture.Value.ToLower() != "png" && capture.Value.ToLower() != "gif")
                   {
                       attributesBuilder.Append(' ');
                       attributesBuilder.Append(capture.Value);
                   }
               }*/

               //return attributesBuilder.ToString();
           },
           RegexOptions.IgnoreCase
               | RegexOptions.Multiline
               | RegexOptions.ExplicitCapture
               | RegexOptions.CultureInvariant
               | RegexOptions.Compiled
            );
        }

        public static string stripBadHtmlImg(string input)
        {
            bool band = false;
            if (input.Contains("<img") && !band)
            {
                input = checkForImg( input, ref band);
            }
            return input;
        }

        private static string checkForImg(string p, ref bool band)
        {
            char[] a = new char[p.Length];
            a = p.ToCharArray();
            int ini = -1;
            ini = buscarInicioImg(a,p.Length);
            int fin = 0;
            if (ini > -1)
            {
                fin = buscarFinImg(a, ini, p.Length);
            }
            if (ini != -1 && fin != 0)
            {
                p = removerCodigoMalicioso(p, ini, fin, ref band);
            }
            return p;
        }

        private static string removerCodigoMalicioso(string p, int ini, int fin, ref bool band)
        {
            string img = p.Substring(ini+4, fin-(ini+4));
            if (img.Contains("src="))
            {
                int iniS = -1;
                iniS = buscarInicioSrc(img, img.Length);
                int finS = 0;
                if (iniS != -1)
                {
                    finS = buscarFinSrc(img, iniS, img.Length);
                }
                if (iniS != -1 && finS != 0)
                {
                    img = removerArchivoMalicioso(img, iniS, finS, ref band);
                    if (band)
                    {
                        p = p.Remove(ini, fin - ini+1);
                    }
                }
            }
            else
            {
                band = true;
            }
            return p;
        }

        private static string removerArchivoMalicioso(string img, int iniS, int finS, ref bool band)
        {
            string resp = img.Substring(iniS+1, finS-(iniS)-1);
            //Path.GetExtension();
            string[] file = resp.Split('.');
            file[file.Length-1].ToLower();
            if (file[file.Length - 1] != "jpg" && file[file.Length - 1] != "png" && file[file.Length - 1] != "gif")
            {
                band = true;
            }
            return resp;
        }

        private static int buscarFinSrc(string img, int iniS, int size)
        {
            int ind = 0;
            char[] a = new char[size];
            a = img.ToCharArray();
            for (int i = iniS+1; i < size; i++)
            {
                if (a[i] == '"')
                {
                    ind = i;
                    break;
                }
            }
            return ind;
        }

        private static int buscarInicioSrc(string img, int size)
        {
            int ind = 0;
            char[] a = new char[size];
            a = img.ToCharArray();
            for (int i = 0; i < size - 4; i++)
            {
                if (a[i] == 's' && a[i + 1] == 'r' && a[i + 2] == 'c' && a[i + 3] == '=' && a[i+4] == '"')
                {
                    ind = i+4;
                    break;
                }
            }
            return ind;
        }

        private static int buscarFinImg(char[] a, int ini, int size)
        {
            int ind = 0;
            for (int i = ini; i < size; i++)
            {
                if (a[i] == '>')
                {
                    ind = i;
                    break;
                }
            }
            return ind;
        }

        private static int buscarInicioImg(char[] a, int size)
        {
            int ind = 0;
            for (int i = 0; i < size-3; i++)
            {
                if (a[i]=='<' && a[i+1]=='i' && a[i+2]=='m' && a[i+3]=='g')
                {
                    ind = i;
                    break;
                }
            }
            return ind;
        }
        #endregion

        #region SubDomain-Binding a Site
        /// <summary>
        /// This method is used to create subdomain to xreplicate-ASEA
        /// </summary>
        /// <param name="newSubDomain">For exa mple "sue" to make a subdomain:"sue.namedomain.com"</param>
        /// <param name="actualSubDomain">An existing sub Domain</param>   
        /// <param name="namedomain">For example "namedomain.com"</param>
        /// <param name="nameSite">This is a site in server</param>
        /// <returns>A message</returns>
        public static string UpdateSubDomainBinding(string newSubDomain, string actualSubDomain, string namedomain_live, string namedomain_team, string nameSite_live,  string nameSite_team)
        {
            string resp = string.Empty;
            string mess = string.Empty;
            
            if (!string.IsNullOrEmpty(newSubDomain))
            {
                ServerManager serverManager = new ServerManager();
                if (serverManager.Sites != null)
                {
                    var esponceApp_live = serverManager.Sites.FirstOrDefault(x => x.Name == nameSite_live);//find site
                    var esponceApp_team = serverManager.Sites.FirstOrDefault(x => x.Name == nameSite_team);//find site
                    if (esponceApp_live != null && esponceApp_team != null)
                    {
                        ///create parameters to binding LIVE
                        BindingCollection bindingcollection_live = esponceApp_live.Bindings;
                        Binding binding_live = bindingcollection_live.CreateElement("binding");
                        binding_live["protocol"] = "http";
                        string domainUser_live = string.Format("{0}.{1}", newSubDomain, namedomain_live);
                        binding_live["bindingInformation"] = string.Format(@"{0}:{1}:{2}", "*", "80", domainUser_live);

                        ///create parameters to binding TEAM
                        BindingCollection bindingcollection_team = esponceApp_team.Bindings;
                        Binding binding_team = bindingcollection_team.CreateElement("binding");
                        binding_team["protocol"] = "http";
                        string domainUser_team = string.Format("{0}.{1}", newSubDomain, namedomain_team);
                        binding_team["bindingInformation"] = string.Format(@"{0}:{1}:{2}", "*", "80", domainUser_team);
                        
                        ///
                        int indexbinding = -1;
                        int indexOldbinding = -1;
                        
                            string oldDomainuser_Live = string.Format("{0}.{1}", actualSubDomain, namedomain_live);
                            string oldDomainuser_Team = string.Format("{0}.{1}", actualSubDomain, namedomain_team);
                            Boolean add = false;
                            try
                            {
                                Binding objBind_live = esponceApp_live.Bindings.FirstOrDefault(x => x.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", oldDomainuser_Live)));
                                if (objBind_live != null)
                                {
                                    indexOldbinding = esponceApp_live.Bindings.IndexOf(objBind_live);
                                    if (indexOldbinding == -1)
                                    {
                                        resp = "This subdomain doesn't exists";
                                    }
                                    else
                                    {
                                        //if (indexbinding != indexOldbinding && indexOldbinding != -1)
                                        //{
                                        esponceApp_live.Bindings.RemoveAt(indexOldbinding);
                                        bindingcollection_live.Add(binding_live);
                              
                                        add = true;
                                     
                                        //}
                                    }


                                }
                                else {
                                    resp = "This subdomain doesn't exists";
                                }
                               
                            }
                            catch (Exception)
                            {
                                resp = "This new subdomain exists";
                            }
                            if (add)
                            {
                                add=false;
                                try
                                {
                                    Binding objBind_team = esponceApp_team.Bindings.FirstOrDefault(x => x.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", oldDomainuser_Team)));
                                    if (objBind_team != null)
                                    {
                                        indexOldbinding = esponceApp_team.Bindings.IndexOf(objBind_team);
                                        if (indexOldbinding == -1)
                                        {
                                            resp = "This subdomain doesn't exists";
                                        }
                                        else
                                        {
                                            //if (indexbinding != indexOldbinding && indexOldbinding != -1)
                                            //{
                                            esponceApp_team.Bindings.RemoveAt(indexOldbinding);
                                            bindingcollection_team.Add(binding_team);
                                            add=true;
                                            //}
                                        }


                                    }
                                    else
                                    {
                                        resp = "This subdomain doesn't exists";
                                    }

                                }
                                catch (Exception)
                                {
                                    resp = "This new subdomain exists";
                                }
                            
                            
                            }
                            if (add)
                            {
                               

                                try
                                {
                                    serverManager.CommitChanges();
                                    resp = "Received";
                                }
                                catch (Exception ex)
                                { 
                                   // resp = "There is another call in progress, try again later.";
                                    mess = ex.Message.ToString() + "Val(" + actualSubDomain.ToString() + "," + newSubDomain.ToString() + ")";
                                }
                            }

                            //if (indexbinding != -1)
                            //{
                            //    resp = "This subdomain exists";
                            //}
                            //else
                            //{
                            //    if (indexbinding != indexOldbinding && indexOldbinding != -1)
                            //    {
                            //        esponceApp.Bindings.RemoveAt(indexOldbinding);
                            //        bindingcollection.Add(binding);
                            //        serverManager.CommitChanges();
                            //        resp = "Received";
                            //    }
                            //}
                        
                    }
                    else
                    {
                        resp = "This site doesn't exist in your server";
                    }
                }
                else
                {
                    resp = "There is not any site in our server";
                }
            }
            else
            {
                resp = "Type a username";
            }
            try
            {
                clsUtilities.RegisterFailDomain(actualSubDomain, newSubDomain, "check-UpdateSubDomainBinding", resp + " | " + mess);
            }
            catch (Exception)
            {


            }
            return resp;
        }

        public static string AddSubDomainBinding(string newSubDomain_id, string newSubDomain_user, string namedomain_live, string namedomain_team, string nameSite_live, string nameSite_team)
        {
            string resp = string.Empty;
            string mess = string.Empty;
            if (!string.IsNullOrEmpty(newSubDomain_id) && !string.IsNullOrEmpty(newSubDomain_user))
            {
                ServerManager serverManager = new ServerManager();
                if (serverManager.Sites != null)
                {
                    var esponceApp_live = serverManager.Sites.FirstOrDefault(x => x.Name == nameSite_live);//find site
                    var esponceApp_Team = serverManager.Sites.FirstOrDefault(x => x.Name == nameSite_team);//find site_team
                    if (esponceApp_live != null && esponceApp_Team != null)
                    {
                        ///create parameters to binding
                        BindingCollection bindingcollection_live = esponceApp_live.Bindings;
                        BindingCollection bindingcollection_team = esponceApp_Team.Bindings;
                        //Bindigs Live
                        Binding binding_id_live = bindingcollection_live.CreateElement("binding");
                        binding_id_live["protocol"] = "http";
                        string domainUser_id_live = string.Format("{0}.{1}", newSubDomain_id, namedomain_live);
                        binding_id_live["bindingInformation"] = string.Format(@"{0}:{1}:{2}", "*", "80", domainUser_id_live);

                        Binding binding_name_live = bindingcollection_live.CreateElement("binding");
                        binding_name_live["protocol"] = "http";
                        string domainUser_name_live = string.Format("{0}.{1}", newSubDomain_user, namedomain_live);
                        binding_name_live["bindingInformation"] = string.Format(@"{0}:{1}:{2}", "*", "80", domainUser_name_live);
                        //Bindigs Team
                        Binding binding_id_team = bindingcollection_team.CreateElement("binding");
                        binding_id_team["protocol"] = "http";
                        string domainUser_id_team = string.Format("{0}.{1}", newSubDomain_id, namedomain_team);
                        binding_id_team["bindingInformation"] = string.Format(@"{0}:{1}:{2}", "*", "80", domainUser_id_team);

                        Binding binding_name_team = bindingcollection_team.CreateElement("binding");
                        binding_name_team["protocol"] = "http";
                        string domainUser_name_team = string.Format("{0}.{1}", newSubDomain_user, namedomain_team);
                        binding_name_team["bindingInformation"] = string.Format(@"{0}:{1}:{2}", "*", "80", domainUser_name_team);

                        ///
                        int indexbinding = -1;
                        int indexOldbinding = -1;
                        //if (string.IsNullOrEmpty(actualSubDomain))//create a new subdomain
                        //{
                        //  Stopwatch timer = new Stopwatch();
                        ////  indexbinding = esponceApp.Bindings.IndexOf(esponceApp.Bindings.FirstOrDefault(x => x.BindingInformation.Equals(binding["bindingInformation"].ToString())));
                        //  timer.Start();
                        //  foreach (Binding item in esponceApp.Bindings)
                        //  {
                        //      if (item.BindingInformation == binding["bindingInformation"].ToString())
                        //      {
                        //          indexbinding = esponceApp.Bindings.IndexOf(item);
                        //          break;
                        //      }
                        //  }
                        //  timer.Stop();
                        //  long timeelapsed = timer.ElapsedTicks;
                        Boolean InsertId = true;
                        Boolean InsertUser = true;
                        try
                        {
                            bindingcollection_live.Add(binding_id_live); // al existir ya el dominio , salta al catch
                        }
                        catch (Exception)
                        {
                            InsertId = false;
                            resp = "Distributor Id already exists. Go to your business site on " + newSubDomain_id + "." + System.Web.Configuration.WebConfigurationManager.AppSettings["domain"];// "Error Bindings";
                        }

                        try
                        {
                            if (InsertId)
                                bindingcollection_team.Add(binding_id_team); // al existir ya el dominio , salta al catch
                        }
                        catch (Exception)
                        {
                            InsertId = false;
                            resp = "Distributor Id already exists. Go to your business site on " + newSubDomain_id + "." + System.Web.Configuration.WebConfigurationManager.AppSettings["domain_team"];// "Error Bindings";
                        }
                        if (!newSubDomain_id.Equals(newSubDomain_user))
                        {
                            try
                            {
                                bindingcollection_live.Add(binding_name_live); // al existir ya el dominio , salta al catch
                            }
                            catch (Exception)
                            {
                                InsertUser = false;
                                resp = "Username already exists. Go to your business site on " + newSubDomain_user + "." + System.Web.Configuration.WebConfigurationManager.AppSettings["domain"];// "Error Bindings";
                            }

                            try
                            {
                                if (InsertUser)
                                    bindingcollection_team.Add(binding_name_team); // al existir ya el dominio , salta al catch
                            }
                            catch (Exception)
                            {
                                InsertUser = false;
                                resp = "Username already exists. Go to your business site on " + newSubDomain_user + "." + System.Web.Configuration.WebConfigurationManager.AppSettings["domain_team"];// "Error Bindings";
                            }

                        }

                        if (InsertUser && InsertId)
                        {
                            //"Distributor Id already exists. Go to your business site on " + id.Trim() + "." + System.Web.Configuration.WebConfigurationManager.AppSettings["domain"];// "Error Bindings";
                            try
                            {
                                serverManager.CommitChanges(); // no llega a hacer commit
                                resp = "Received";
                            }
                            catch (Exception ex)
                            {
                                resp = "There is another call in progress, try again later.";
                                mess = ex.Message.ToString() + "Val(" + newSubDomain_id.ToString() + "," + newSubDomain_user.ToString() + ")";
                            }


                        }

                        //if (indexbinding != -1)
                        //{
                        //    resp = "This subdomain exists";
                        //}
                        //else
                        //{
                        //    bindingcollection.Add(binding);
                        //    serverManager.CommitChanges();
                        //    resp = "Received";
                        //}
                        //}

                    }
                    else
                    {
                        resp = "This site doesn't exist in your server";
                    }
                }
                else
                {
                    resp = "There is not any site in our server";
                }
            }
            else
            {
                resp = "Type a username";
            }
            try
            {
                clsUtilities.RegisterFailDomain(newSubDomain_id, newSubDomain_user, "check-AddSubDomainBinding", resp + " | " + mess);
            }
            catch (Exception)
            {


            }

            return resp;
        }
        

        //public static void RestartApp(String NameApp)
        //{
        //    string appPoolName = NameApp;
        //    string appPoolPath = @"IIS://" + System.Environment.MachineName + "/W3SVC/AppPools/" + appPoolName;
        //    try
        //    {
        //        DirectoryEntry w3svc = new DirectoryEntry(appPoolPath);
        //        w3svc.Invoke("Recycle", null);
        //        //w3svc.Invoke("Start", null);
        //        //status();
        //    }
        //    catch (Exception ex)
        //    {
        //        //Response.Write(ex.ToString());
        //    }
        //}
        public static Int32 DeleteAllBinding(string namedomain, string nameSite)
        {
            Int32 count = 0;
            try
            {


                ServerManager serverManager = new ServerManager();
                if (serverManager.Sites != null)
                {
                    var esponceApp = serverManager.Sites.FirstOrDefault(x => x.Name == nameSite);//find site
                    if (esponceApp != null)
                    {
                        ///create parameters to binding
                        BindingCollection bindingcollection = esponceApp.Bindings;
                        Binding binding = bindingcollection.CreateElement("binding");
                        try
                        {
                            Int32[] lstIndexDelete = new Int32[esponceApp.Bindings.Count];
                            //foreach (Binding item in esponceApp.Bindings)
                            //{


                            //    if (!item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "sue", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "becky", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "tami", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "verneytest", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "luis", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "tmartin", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "bwarberg", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "1135056852", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "test905can12", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "tm1219a", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "tamitestirl", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "tm124a", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "testfranceacct", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "tm301b", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "tm1218a", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "netherlandstest", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "tm1029live1", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "testslovenia123", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "Myspainaccount", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "wathne", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "tanjaanker", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "patossina", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "maitlands", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "jillparker", namedomain))) &&
                            //        !item.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", string.Format("{0}.{1}", "matthiasfritze", namedomain)))
                            //        )
                            //    {

                            //        lstIndexDelete[count] = esponceApp.Bindings.IndexOf(item);

                            //        //Generar una lista de index de los Bingings que se eliminaran
                            //    }
                            //    else {
                            //        lstIndexDelete[count] = -1;
                            //    }
                            //    count = count + 1;
                            //}
                            //count = 0;
                            if (lstIndexDelete != null && lstIndexDelete.Length > 0)
                            {
                                try
                                {
                                    //Recorrer la lista y eliminar los bingings
                                    //for (int i = 0; i < lstIndexDelete.Length; i++)
                                    //{
                                    //    //if (lstIndexDelete[i] > -1)
                                    //    //{
                                    //        esponceApp.Bindings.RemoveAt(lstIndexDelete[i]);
                                    //        count = count + 1;
                                    //    //}
                                    //}
                                    count = esponceApp.Bindings.Count;
                                    esponceApp.Bindings.Clear();
                                    serverManager.CommitChanges();
                                }
                                catch (Exception)
                                {
                                    serverManager.CommitChanges(); // si va al catch grabar las eliminaciones ya hechas

                                }


                            }
                        }
                        catch (Exception)
                        {


                        }
                       
                        //  
                     //  

                    }
                    else
                    {
                        // resp = "There is not any site in your server";
                    }
                }
                else
                {
                    // resp = "Type a subdomain";
                }
            }
            catch (Exception)
            {

            }
            return count;
        }
        public static void RegisterFailDomain(String ID, String subdomain, String Code, String Description)
        {
            try
            {
                string fic = System.Web.Configuration.WebConfigurationManager.AppSettings["fileLog"];
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fic, true);
                sw.WriteLine(String.Format("{0},{1},{2},{3},{4}", ID, subdomain, Code, Description, DateTime.Now.ToString()));
                sw.Close();
            }
            catch (Exception)
            {

                // throw;
            }
        }

        public static string DeleteBinding(string subdomain_id,string subdomain_user, string namedomain_live, string namedomain_team, string nameSite_live, string nameSite_team)
        {
            string resp = string.Empty;

            ServerManager serverManager = new ServerManager();
            if (serverManager.Sites != null)
            {
                var esponceApp_live = serverManager.Sites.FirstOrDefault(x => x.Name == nameSite_live);//find site
                var esponceApp_team = serverManager.Sites.FirstOrDefault(x => x.Name == nameSite_team);//find site
                if (esponceApp_live != null && esponceApp_team != null)
                {
                    int indexOldbinding = -1;
                    //live
                    string oldDomainid_live = string.Format("{0}.{1}", subdomain_id, namedomain_live);
                    string oldDomainuser_live = string.Format("{0}.{1}", subdomain_user, namedomain_live);
                    //team
                    string oldDomainid_team = string.Format("{0}.{1}", subdomain_id, namedomain_team);
                    string oldDomainuser_team = string.Format("{0}.{1}", subdomain_user, namedomain_team);
                    //foreach (Binding item in esponceApp.Bindings)
                    //{
                    //    if (item.BindingInformation == string.Format(@"{0}:{1}:{2}", "*", "80", oldDomainuser))
                    //    {
                    //        indexOldbinding = esponceApp.Bindings.IndexOf(item);
                    //        break;
                    //    }
                    //}

                    //if (indexOldbinding != -1)
                    //{
                    //    esponceApp.Bindings.RemoveAt(indexOldbinding);
                    //    serverManager.CommitChanges();
                    //    resp = "Received";
                    //}
                    //else
                    //    resp = "The binding doesn't exist in your site";


                    /**/
                    Boolean Deleteid_live = false;
                    Binding objBindid_live = esponceApp_live.Bindings.FirstOrDefault(x => x.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", oldDomainid_live)));
                    if (objBindid_live != null)
                    {
                        indexOldbinding = esponceApp_live.Bindings.IndexOf(objBindid_live);
                        if (indexOldbinding == -1)
                        {
                            resp = "The binding doesn't exist in your site";
                        }
                        else
                        {
                           
                            esponceApp_live.Bindings.RemoveAt(indexOldbinding);
                            Deleteid_live = true;
                        }
                    }
                    else
                    {
                        resp = "The binding doesn't exist in your site";
                    }
                    if (Deleteid_live)
                    {
                        Binding objBindid_teamasea = esponceApp_team.Bindings.FirstOrDefault(x => x.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", oldDomainid_team)));
                        if (objBindid_teamasea != null)
                        {
                            indexOldbinding = esponceApp_team.Bindings.IndexOf(objBindid_teamasea);
                            if (indexOldbinding == -1)
                            {
                                resp = "The binding doesn't exist in your site";
                            }
                            else
                            {

                                esponceApp_team.Bindings.RemoveAt(indexOldbinding);
                            }
                        }
                        else
                        {
                            resp = "The binding doesn't exist in your site";
                        }
                    }
                    Boolean Deleteuser_live=false;
                    if(!subdomain_user.Equals(subdomain_id))
                    {
                        Binding objBinduser_live = esponceApp_live.Bindings.FirstOrDefault(x => x.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", oldDomainuser_live)));
                    if (objBinduser_live != null)
                    {
                        indexOldbinding = esponceApp_live.Bindings.IndexOf(objBinduser_live);
                        if (indexOldbinding == -1)
                        {
                            resp = "The binding doesn't exist in your site";
                        }
                        else
                        {
                           
                            esponceApp_live.Bindings.RemoveAt(indexOldbinding);
                            Deleteuser_live = true;
                        }
                    }
                    else
                    {
                        resp = "The binding doesn't exist in your site";
                    }
                    if (Deleteuser_live)
                    {
                        Binding objBinduser_teamasea = esponceApp_team.Bindings.FirstOrDefault(x => x.BindingInformation.Equals(string.Format(@"{0}:{1}:{2}", "*", "80", oldDomainuser_team)));
                        if (objBinduser_teamasea != null)
                        {
                            indexOldbinding = esponceApp_team.Bindings.IndexOf(objBinduser_teamasea);
                            if (indexOldbinding == -1)
                            {
                                resp = "The binding doesn't exist in your site";
                            }
                            else
                            {

                                esponceApp_team.Bindings.RemoveAt(indexOldbinding);
                            }
                        }
                        else
                        {
                            resp = "The binding doesn't exist in your site";
                        }
                    }
                    
                    
                    }


                    if(Deleteuser_live || Deleteid_live)
                    {
                    try 
	                {	        
		                serverManager.CommitChanges();
                        resp = "Received";
	                }
	                catch (Exception ex)
	                {
		              resp= ex.Message.ToString()+ "Val(" + Deleteid_live.ToString() + "," + Deleteuser_live.ToString()+ ")"; 
		
	                }
                 
                    }

                  
                    /**/


                }
                else
                {
                    resp = "The binding doesn't exist in your site";
                }
            }
            else
            {
                resp = "The binding doesn't exist in your site";
            }
            try
            {
                  clsUtilities.RegisterFailDomain(subdomain_id, subdomain_user, "check-DeleteBindigs", resp );
            }
            catch (Exception)
            {
                
            }
            return resp;
        }

        #endregion

        #region Manipulate IPv4
        public static int[] ConvertStringIPToArrayInt(string[] ipArray)
        {
            int[] resp = new int[4];
            try
            {


                for (int i = 0; i < 4; i++)
                {
                    resp[i] = Convert.ToInt32(ipArray[i]);
                }
            }
            catch (Exception ex)
            {

                string a = ex.Message.ToString();
            }
            return resp;
        }

        public static int CompareIPgetInt(string ipinicio, string ipfin, string ip)
        {
            int band = int.MaxValue;
            string[] ipniniArray = ipinicio.Split('.');
            string[] ipfinArray = ipfin.Split('.');
            string[] ipArray = ip.Split('.');

            int[] ipArraySource = ConvertStringIPToArrayInt(ipArray);
            int[] ipBeginArray = ConvertStringIPToArrayInt(ipniniArray);
            int[] ipEndArray = ConvertStringIPToArrayInt(ipfinArray);

            if (ipArraySource[0] > ipBeginArray[0] && ipArraySource[0] < ipEndArray[0])
            {
                band = 0;
            }
            else
            {
                if (ipArraySource[0] == ipBeginArray[0] && ipArraySource[0] != ipEndArray[0])
                {
                    if (ipArraySource[1] > ipBeginArray[1])
                    {
                        band = 0;
                    }
                    else
                    {
                        if (ipArraySource[1] < ipBeginArray[1])
                        {
                            return -1;
                        }
                        if (ipArraySource[1] == ipBeginArray[1] && ipArraySource[1] != ipEndArray[1])
                        {
                            if (ipArraySource[2] > ipBeginArray[2])
                            {
                                band = 0;
                            }
                            else
                            {
                                if (ipArraySource[2] < ipBeginArray[2])
                                {
                                    return -1;
                                }
                                if (ipArraySource[2] == ipBeginArray[2] && ipArraySource[2] != ipEndArray[2])
                                {
                                    if (ipArraySource[3] > ipBeginArray[3])
                                    {
                                        band = 0;
                                    }
                                    else
                                    {
                                        if (ipArraySource[3] < ipBeginArray[3])
                                        {
                                            return -1;
                                        }
                                        if (ipArraySource[3] == ipBeginArray[3] || ipArraySource[3] == ipEndArray[3])
                                        {
                                            band = 0;
                                        }
                                        else
                                        {
                                            if (ipArraySource[3] < ipBeginArray[3])
                                            {
                                                return -1;
                                            }
                                            if (ipArraySource[3] > ipEndArray[3])
                                            {
                                                return 0;
                                            }
                                        }
                                    }
                                }
                                if (ipArraySource[2] == ipEndArray[2])
                                {
                                    if (ipArraySource[3] < ipEndArray[3])
                                    {
                                        band = 0;
                                    }
                                    else
                                    {
                                        if (ipArraySource[3] == ipBeginArray[3] || ipArraySource[3] == ipEndArray[3])
                                        {
                                            band = 0;
                                        }
                                        else
                                        {
                                            if (ipArraySource[3] < ipBeginArray[3])
                                            {
                                                return -1;
                                            }
                                            if (ipArraySource[3] > ipEndArray[3])
                                            {
                                                return 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (ipArraySource[1] == ipEndArray[1])
                        {
                            if (ipArraySource[2] < ipEndArray[2])
                            {
                                band = 0;
                            }
                            else
                            {
                                if (ipArraySource[2] > ipEndArray[2])
                                {
                                    return 1;
                                }
                                if (ipArraySource[2] == ipBeginArray[2] && ipArraySource[2] != ipEndArray[2])
                                {
                                    if (ipArraySource[3] > ipBeginArray[3])
                                    {
                                        band = 0;
                                    }
                                    else
                                    {
                                        if (ipArraySource[3] == ipBeginArray[3] || ipArraySource[3] == ipEndArray[3])
                                        {
                                            band = 0;
                                        }
                                        else
                                        {
                                            if (ipArraySource[3] < ipBeginArray[3])
                                            {
                                                return -1;
                                            }
                                            if (ipArraySource[3] > ipEndArray[3])
                                            {
                                                return 0;
                                            }
                                        }
                                    }
                                }
                                if (ipArraySource[2] == ipEndArray[2])
                                {
                                    if (ipArraySource[3] < ipEndArray[3])
                                    {
                                        band = 0;
                                    }
                                    else
                                    {
                                        if (ipArraySource[3] == ipBeginArray[3] || ipArraySource[3] == ipEndArray[3])
                                        {
                                            band = 0;
                                        }
                                        else
                                        {
                                            if (ipArraySource[3] < ipBeginArray[3])
                                            {
                                                return -1;
                                            }
                                            if (ipArraySource[3] > ipEndArray[3])
                                            {
                                                return 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (ipArraySource[0] < ipBeginArray[0])
                {
                    return -1;
                }
                if (ipArraySource[0] > ipEndArray[0])
                {
                    return 1;
                }
                if (ipArraySource[0] == ipEndArray[0])
                {
                    if (ipArraySource[1] > ipBeginArray[1] && ipArraySource[1] < ipEndArray[1])
                    {
                        band = 0;
                    }
                    else
                    {
                        if (ipArraySource[1] < ipBeginArray[1])
                        {
                            return -1;
                        }
                        if (ipArraySource[1] > ipEndArray[1])
                        {
                            return 1;
                        }
                        if (ipArraySource[1] == ipBeginArray[1] && ipArraySource[1] != ipEndArray[1])
                        {
                            if (ipArraySource[2] > ipBeginArray[2])
                            {
                                band = 0;
                            }
                            else
                            {
                                if (ipArraySource[2] < ipBeginArray[2])
                                {
                                    return -1;
                                }
                                if (ipArraySource[2] == ipBeginArray[2] && ipArraySource[2] != ipEndArray[2])
                                {
                                    if (ipArraySource[3] > ipBeginArray[3])
                                    {
                                        band = 0;
                                    }
                                    else
                                    {
                                        if (ipArraySource[3] == ipBeginArray[3] || ipArraySource[3] == ipEndArray[3])
                                        {
                                            band = 0;
                                        }
                                        else
                                        {
                                            if (ipArraySource[3] < ipBeginArray[3])
                                            {
                                                return -1;
                                            }
                                            if (ipArraySource[3] > ipEndArray[3])
                                            {
                                                return 0;
                                            }
                                        }
                                    }
                                }
                                if (ipArraySource[2] == ipEndArray[2])
                                {
                                    if (ipArraySource[3] < ipEndArray[3])
                                    {
                                        band = 0;
                                    }
                                    else
                                    {
                                        if (ipArraySource[3] == ipBeginArray[3] || ipArraySource[3] == ipEndArray[3])
                                        {
                                            band = 0;
                                        }
                                        else
                                        {
                                            if (ipArraySource[3] < ipBeginArray[3])
                                            {
                                                return -1;
                                            }
                                            if (ipArraySource[3] > ipEndArray[3])
                                            {
                                                return 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (ipArraySource[1] == ipEndArray[1])
                        {
                            if (ipArraySource[2] < ipEndArray[2] && ipArraySource[2] > ipBeginArray[2])
                            {
                                band = 0;
                            }
                            else
                            {
                                if (ipArraySource[2] > ipEndArray[2])
                                {
                                    return 1;
                                }
                                if (ipArraySource[2] < ipBeginArray[2])
                                {
                                    return -1;
                                }
                                if (ipArraySource[2] == ipBeginArray[2] && ipArraySource[2] != ipEndArray[2])
                                {
                                    if (ipArraySource[3] > ipBeginArray[3])
                                    {
                                        band = 0;
                                    }
                                    else
                                    {
                                        if (ipArraySource[3] == ipBeginArray[3] || ipArraySource[3] == ipEndArray[3])
                                        {
                                            band = 0;
                                        }
                                        else
                                        {
                                            if (ipArraySource[3] < ipBeginArray[3])
                                            {
                                                return -1;
                                            }
                                            if (ipArraySource[3] > ipEndArray[3])
                                            {
                                                return 0;
                                            }
                                        }
                                    }
                                }
                                if (ipArraySource[2] == ipEndArray[2])
                                {
                                    if (ipArraySource[3] < ipEndArray[3])
                                    {
                                        band = 0;
                                    }
                                    else
                                    {
                                        if (ipArraySource[3] == ipBeginArray[3] || ipArraySource[3] == ipEndArray[3])
                                        {
                                            band = 0;
                                        }
                                        else
                                        {
                                            if (ipArraySource[3] < ipBeginArray[3])
                                            {
                                                return -1;
                                            }
                                            if (ipArraySource[3] > ipEndArray[3])
                                            {
                                                return 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return band;
        }
        #endregion

       

        public static String GeneratePublicName(Int32 Id)
        {
            String PublicName = String.Empty;

            PublicName += Convert.ToString(Id);

            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 50; i++)
                PublicName += (Char)rnd.Next(65, 90);

            return PublicName;
        }

        #region UpdateForwardSmarterMail
        public static Boolean CallWebServiceForward(String email, String emailForward)
        {
            var url = "http://66.85.130.178:9998/Services/svcUserAdmin.asmx";
            var action = "http://tempuri.org/UpdateUserForwardingInfo2";
            Boolean success = false;
            try
            {
                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(email, emailForward);
                HttpWebRequest webRequest = CreateWebRequest(url, action);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                XDocument xmlResponse = null;
                XNamespace nspace = "http://tempuri.org/";
                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    xmlResponse = XDocument.Load(webResponse.GetResponseStream());
                }

                if (xmlResponse != null)
                {
                    clsResult res = (clsResult)from result in xmlResponse.Descendants(nspace + "UpdateUserForwardingInfo2Response").Descendants(nspace + "UpdateUserForwardingInfo2Result")
                                               select new clsResult
                                               {
                                                   Result = Convert.ToBoolean(xmlResponse.Element(nspace + "Result").Value),
                                                   ResultCode = Convert.ToInt32(xmlResponse.Element(nspace + "ResultCode").Value),
                                                   Message = xmlResponse.Element(nspace + "Message").Value
                                               };
                    success = res.Result;

                }
            }
            catch (Exception)
            {
            }
            return success;
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope(String email, String emailForward)
        {
            XmlDocument soapEnvelop = new XmlDocument();
            soapEnvelop.LoadXml(@"<?xml version='1.0' encoding='utf-8'?>
                    <soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                      <soap:Body>
                        <UpdateUserForwardingInfo2 xmlns='http://tempuri.org/'>
                          <AuthUserName>administrator</AuthUserName>
                          <AuthPassword>@dmin20+13</AuthPassword>
                          <EmailAddress>" + email + "</EmailAddress>" +
                          "<DeleteOnForward>false</DeleteOnForward>" +
                          "<ForwardingAddresses>" +
                            "<string>" + emailForward + "</string>" +
                          "</ForwardingAddresses>" +
                        "</UpdateUserForwardingInfo2>" +
                      "</soap:Body>" +
                    "</soap:Envelope>");
            return soapEnvelop;
        }
        #region Send Email
        public static String EmailFormat2(String Template, String DistName = "", String NameHostess = "", String PhoneHostess = "", String partyName = "", String Date = "", String Hour = "", String Address = "", String Address2 = "", String City = "", String State = "", String ZipCode = "", String Country = "", String Email = "")
        {
            String srEmail = "";
            try
            {
                if (!String.IsNullOrEmpty(Template))
                {
                    srEmail = Template;
                    srEmail = srEmail.Replace("{Party_NameHostess}", !String.IsNullOrEmpty(NameHostess) ? NameHostess : "");
                    srEmail = srEmail.Replace("{Party_Phone}", !String.IsNullOrEmpty(PhoneHostess) ? PhoneHostess : "");
                    srEmail = srEmail.Replace("{Email}", !String.IsNullOrEmpty(Email) ? Email : "");
                    srEmail = srEmail.Replace("{PARTY_DATE}", !String.IsNullOrEmpty(Date) ? Date : "");
                    srEmail = srEmail.Replace("{PARTY_TIME}", !String.IsNullOrEmpty(Hour) ? Hour : "");
                    srEmail = srEmail.Replace("{Party_ADDRESS}", !String.IsNullOrEmpty(Address) ? Address : "");
                    srEmail = srEmail.Replace("{Party_ADDRESS2}", !String.IsNullOrEmpty(Address2) ? Address2 : "");
                    srEmail = srEmail.Replace("{Party_CITY}", !String.IsNullOrEmpty(City) ? City : "");
                    srEmail = srEmail.Replace("{Party_STATE}", !String.IsNullOrEmpty(State) ? State : "");
                    srEmail = srEmail.Replace("{Party_POSTALCODE}", !String.IsNullOrEmpty(ZipCode) ? ZipCode : "");
                    srEmail = srEmail.Replace("{Party_COUNTRY}", !String.IsNullOrEmpty(Country) ? Country : "");
                    srEmail = srEmail.Replace("{PARTY_NAME}", !String.IsNullOrEmpty(partyName) ? partyName : "");
                    srEmail = srEmail.Replace("{DISTRIBUTOR_NAME}", !String.IsNullOrEmpty(DistName) ? DistName : "");
                    srEmail = srEmail.Replace("{GoogleMap directions}", /*!String.IsNullOrEmpty(partyName) ? partyName :*/ "");

                }
            }
            catch (Exception) { }
            return srEmail;

        }
        public static String EmailFormat(String Template
           , String FullName = ""
           //, String Url = ""
           //, String EmailSetting = ""
           , String Code = ""
           , String Number=""
           , String Domain = ""
           , String Address1 = ""
           , String Address2 = ""
           , String Email = ""
           , String Phone = ""
           , String Date = ""
           , String Representative = ""
           , String Detail = ""
           , String Card = ""
           , String Balance = ""
           , String Subtotal = ""
           , String Shipping = ""
           , String Total = ""
           , String Type=""
            , String Tax = ""
            , String Discounts = ""
            , String Auth = ""

            ,  String aNumber=""
            , String aDetail = ""
             , String aSubtotal = ""
           , String aShipping = ""
           , String aTotal = ""
            , String aTax = ""
            , String aDiscounts = ""

           )
        {
            String srEmail = "";
            try
            {
                if (!String.IsNullOrEmpty(Template))
                {
                    srEmail = Template;
                    srEmail = srEmail.Replace("{FullName}", !String.IsNullOrEmpty(FullName) ? FullName : "");
                    //srEmail = srEmail.Replace("{Url}", !String.IsNullOrEmpty(Url) ? Url : "");
                    //srEmail = srEmail.Replace("{EmailSetting}", !String.IsNullOrEmpty(EmailSetting) ? EmailSetting : "support@tru-friends.com");
                    srEmail = srEmail.Replace("{Code}", !String.IsNullOrEmpty(Code) ? Code : "");
                    srEmail = srEmail.Replace("{Number}", !String.IsNullOrEmpty(Number) ? Number : "");
                    if(Type=="join")
                    srEmail = srEmail.Replace("{Domain}", !String.IsNullOrEmpty(Domain) ? Domain : "");

                    srEmail = srEmail.Replace("{Address1}", !String.IsNullOrEmpty(Address1) ? Address1 : "-");
                    srEmail = srEmail.Replace("{Address2}", !String.IsNullOrEmpty(Address2) ? Address2 : "-");
                    srEmail = srEmail.Replace("{Email}", !String.IsNullOrEmpty(Email) ? Email : "");
                    srEmail = srEmail.Replace("{Phone}", !String.IsNullOrEmpty(Phone) ? Phone : "");
                    srEmail = srEmail.Replace("{Date}", !String.IsNullOrEmpty(Date) ? Date : "");
                    //srEmail = srEmail.Replace("{Representative}", !String.IsNullOrEmpty(Representative) ? Representative : "");

                    srEmail = srEmail.Replace("<tr><td colspan=\"4\">{Detail}</td></tr>", !String.IsNullOrEmpty(Detail) ? Detail : "");
                    srEmail = srEmail.Replace("{Card}", !String.IsNullOrEmpty(Card) ? Card : "");
                    srEmail = srEmail.Replace("{Balance}", !String.IsNullOrEmpty(Balance) ? Balance : "0.00");
                    srEmail = srEmail.Replace("{Subtotal}", !String.IsNullOrEmpty(Subtotal) ? Subtotal : "0.00");
                    srEmail = srEmail.Replace("{Shipping}", !String.IsNullOrEmpty(Shipping) ? Shipping : "0.00");
                    srEmail = srEmail.Replace("{Total}", !String.IsNullOrEmpty(Total) ? Total : "0.00");
                    srEmail = srEmail.Replace("{Taxes}", !String.IsNullOrEmpty(Tax) ? Tax : "0.00");
                    srEmail = srEmail.Replace("{Credit}", !String.IsNullOrEmpty(Discounts) ? Discounts : "0.00");
                    srEmail = srEmail.Replace("{Auth}", !String.IsNullOrEmpty(Auth) ? Auth : "--");
                    //srEmail = srEmail.Replace("{Note}", !String.IsNullOrEmpty(Note) ? Note : "");


                    if (!String.IsNullOrEmpty(aNumber))
                    {
                        srEmail = srEmail.Replace("{aNumber}", !String.IsNullOrEmpty(aNumber) ? aNumber : "");
                        srEmail = srEmail.Replace("<tr><td colspan=\"4\">{aDetail}</td></tr>", !String.IsNullOrEmpty(aDetail) ? aDetail : "");
                        srEmail = srEmail.Replace("{aSubtotal}", !String.IsNullOrEmpty(aSubtotal) ? aSubtotal : "0.00");
                        srEmail = srEmail.Replace("{aShipping}", !String.IsNullOrEmpty(aShipping) ? aShipping : "0.00");
                        srEmail = srEmail.Replace("{aTotal}", !String.IsNullOrEmpty(aTotal) ? aTotal : "0.00");
                        srEmail = srEmail.Replace("{aTaxes}", !String.IsNullOrEmpty(aTax) ? aTax : "0.00");
                        srEmail = srEmail.Replace("{aCredit}", !String.IsNullOrEmpty(aDiscounts) ? aDiscounts : "0.00");
                    }

                }
            }
            catch (Exception) { }
            return srEmail;

        }

        public static String EmailFormat(String Template, String body)
        {
            String srEmail = "";
            try
            {
                if (!String.IsNullOrEmpty(Template))
                {
                    srEmail = Template;
                    srEmail = srEmail.Replace("{BODY}", !String.IsNullOrEmpty(body) ? body : "");


                }
            }
            catch (Exception) { }
            return srEmail;

        }
#endregion 




        #region Send EmailA

        public static String EmailFormatA(String Template
           , String Legacy = ""
           , String Username = ""
           , String FullName = ""
           , String Code = ""
           , String Number = ""
           , String Domain = ""
           , String Address1 = ""
           , String Address2 = ""
           , String City = ""
           , String State = ""
           , String Zip = ""
           , String Country = ""
           , String Email = ""
           , String Phone = ""
           , String Date = ""
           , String Representative = ""
           , String OrderCv = ""
           , String PaymentType = ""
           , String ShippingOption = ""
           , String Detail = ""
           , String EnrollerName = ""
           , String EnrollerId = ""
           , String Card = ""
           , String Balance = ""
           , String Subtotal = ""
           , String Shipping = ""
           , String Total = ""
           , String Type = ""
            , String Sponsor_name = ""
            , String Sponsor_lastname = ""
            , String Sponsor_phone = ""
            , String Taxes = ""
            , String SAddress1 = ""
           , String SAddress2 = ""
           , String SCity = ""
           , String SState = ""
           , String SZip = ""
           , String SCountry = ""
           , String Notes = ""
           )
        {
            String srEmail = "";
            try
            {
                if (!String.IsNullOrEmpty(Template))
                {
                    srEmail = Template;
                    srEmail = srEmail.Replace("{FullName}", !String.IsNullOrEmpty(FullName) ? FullName : "");
                    srEmail = srEmail.Replace("{Username}", !String.IsNullOrEmpty(Username) ? Username : "");
                    srEmail = srEmail.Replace("{LegacyNo}", !String.IsNullOrEmpty(Legacy) ? Legacy : "");
                    srEmail = srEmail.Replace("{Code}", !String.IsNullOrEmpty(Code) ? Code : "");
                    srEmail = srEmail.Replace("{OrderNumber}", !String.IsNullOrEmpty(Number) ? Number : "");
                    if (Type == "join")
                        srEmail = srEmail.Replace("{Domain}", !String.IsNullOrEmpty(Domain) ? Domain : "");
                    else
                    {

                        srEmail = srEmail.Replace("D_S_PHONE", !String.IsNullOrEmpty(Sponsor_phone) ? Sponsor_phone : "");
                    }

                    srEmail = srEmail.Replace("{Address1}", !String.IsNullOrEmpty(Address1) ? Address1 : "-");
                    srEmail = srEmail.Replace("{Address2}", !String.IsNullOrEmpty(Address2) ? Address2 : "-");
                    srEmail = srEmail.Replace("{City}", !String.IsNullOrEmpty(City) ? City : "-");
                    srEmail = srEmail.Replace("{State}", !String.IsNullOrEmpty(State) ? State : "-");
                    srEmail = srEmail.Replace("{Zip}", !String.IsNullOrEmpty(Zip) ? Zip : "-");
                    srEmail = srEmail.Replace("{Country}", !String.IsNullOrEmpty(Country) ? Country : "-");

                    srEmail = srEmail.Replace("{SAddress1}", !String.IsNullOrEmpty(SAddress1) ? SAddress1 : "-");
                    srEmail = srEmail.Replace("{SAddress2}", !String.IsNullOrEmpty(SAddress2) ? SAddress2 : "-");
                    srEmail = srEmail.Replace("{SCity}", !String.IsNullOrEmpty(SCity) ? SCity : "-");
                    srEmail = srEmail.Replace("{SState}", !String.IsNullOrEmpty(SState) ? SState : "-");
                    srEmail = srEmail.Replace("{SZip}", !String.IsNullOrEmpty(SZip) ? SZip : "-");
                    srEmail = srEmail.Replace("{SCountry}", !String.IsNullOrEmpty(SCountry) ? SCountry : "-");

                    srEmail = srEmail.Replace("{Email}", !String.IsNullOrEmpty(Email) ? Email : "");
                    srEmail = srEmail.Replace("{Phone}", !String.IsNullOrEmpty(Phone) ? Phone : "");
                    srEmail = srEmail.Replace("{Date}", !String.IsNullOrEmpty(Date) ? Date : "");
                    srEmail = srEmail.Replace("{Contact}", !String.IsNullOrEmpty(Representative) ? Representative : "");
                    srEmail = srEmail.Replace("{OrderCv}", !String.IsNullOrEmpty(OrderCv) ? OrderCv : "");
                    srEmail = srEmail.Replace("{PaymentType}", !String.IsNullOrEmpty(PaymentType) ? PaymentType : "");
                    srEmail = srEmail.Replace("{ShippingOption}", !String.IsNullOrEmpty(ShippingOption) ? ShippingOption : "");
                    srEmail = srEmail.Replace("{AssociateID}", !String.IsNullOrEmpty(EnrollerId) ? EnrollerId : "");
                    srEmail = srEmail.Replace("{SponsorName}", !String.IsNullOrEmpty(EnrollerName) ? EnrollerName : "");

                    srEmail = srEmail.Replace("<tr><td colspan=\"8\">{Detail}</td></tr>", !String.IsNullOrEmpty(Detail) ? Detail : "");
                    srEmail = srEmail.Replace("{Card}", !String.IsNullOrEmpty(Card) ? Card : "");
                    srEmail = srEmail.Replace("{Balance}", !String.IsNullOrEmpty(Balance) ? Balance : "0.00");
                    srEmail = srEmail.Replace("{Taxes}", !String.IsNullOrEmpty(Taxes) ? Taxes : "0.00");
                    srEmail = srEmail.Replace("{Subtotal}", !String.IsNullOrEmpty(Subtotal) ? Subtotal : "0.00");
                    srEmail = srEmail.Replace("{Shipping}", !String.IsNullOrEmpty(Shipping) ? Shipping : "0.00");
                    srEmail = srEmail.Replace("{Total}", !String.IsNullOrEmpty(Total) ? Total : "0.00");
                    srEmail = srEmail.Replace("D_S_NAME", !String.IsNullOrEmpty(Sponsor_name) ? Sponsor_name : "");
                    srEmail = srEmail.Replace("D_S_LASTNAME", !String.IsNullOrEmpty(Sponsor_lastname) ? Sponsor_lastname : "");
                    srEmail = srEmail.Replace("{Notes}", !String.IsNullOrEmpty(Notes) ? Notes : "");

                }
            }
            catch (Exception) { }
            return srEmail;

        }

        public static String EmailFormatA(String Template, String Email = "", String Name = "", String Subject = "", String Message = "")
        {
            String srEmail = "";
            try
            {
                if (!String.IsNullOrEmpty(Template))
                {
                    srEmail = Template;
                    srEmail = srEmail.Replace("{CK_EMAIL}", !String.IsNullOrEmpty(Email) ? Email : "");
                    srEmail = srEmail.Replace("{CK_NAME}", !String.IsNullOrEmpty(Name) ? Name : "");
                    srEmail = srEmail.Replace("{CK_SUBJECT}", !String.IsNullOrEmpty(Subject) ? Subject : "");
                    srEmail = srEmail.Replace("{CK_MESSAGE}", !String.IsNullOrEmpty(Message) ? Message : "");
                }
            }
            catch (Exception) { }
            return srEmail;

        }
        public static String EmailFormatA(String Template, String DistName = "", String NameHostess = "", String PhoneHostess = "", String partyName = "", String Date = "", String Hour = "", String Address = "", String Address2 = "", String City = "", String State = "", String ZipCode = "", String Country = "")
        {
            String srEmail = "";
            try
            {
                if (!String.IsNullOrEmpty(Template))
                {
                    srEmail = Template;
                    srEmail = srEmail.Replace("{Party_NameHostess}", !String.IsNullOrEmpty(NameHostess) ? NameHostess : "");
                    srEmail = srEmail.Replace("{Party_Phone}", !String.IsNullOrEmpty(PhoneHostess) ? PhoneHostess : "");
                    srEmail = srEmail.Replace("{PARTY_DATE}", !String.IsNullOrEmpty(Date) ? Date : "");
                    srEmail = srEmail.Replace("{PARTY_TIME}", !String.IsNullOrEmpty(Hour) ? Hour : "");
                    srEmail = srEmail.Replace("{Party_ADDRESS}", !String.IsNullOrEmpty(Address) ? Address : "");
                    srEmail = srEmail.Replace("{Party_ADDRESS2}", !String.IsNullOrEmpty(Address2) ? Address2 : "");
                    srEmail = srEmail.Replace("{Party_CITY}", !String.IsNullOrEmpty(City) ? City : "");
                    srEmail = srEmail.Replace("{Party_STATE}", !String.IsNullOrEmpty(State) ? State : "");
                    srEmail = srEmail.Replace("{Party_POSTALCODE}", !String.IsNullOrEmpty(ZipCode) ? ZipCode : "");
                    srEmail = srEmail.Replace("{Party_COUNTRY}", !String.IsNullOrEmpty(Country) ? Country : "");
                    srEmail = srEmail.Replace("{PARTY_NAME}", !String.IsNullOrEmpty(partyName) ? partyName : "");
                    srEmail = srEmail.Replace("{DISTRIBUTOR_NAME}", !String.IsNullOrEmpty(DistName) ? DistName : "");
                    srEmail = srEmail.Replace("{GoogleMap directions}", /*!String.IsNullOrEmpty(partyName) ? partyName :*/ "");

                }
            }
            catch (Exception) { }
            return srEmail;

        }
        public static String EmailFormatSubjectA(String Template, String partyName = "")
        {
            String srEmail = "";
            try
            {
                if (!String.IsNullOrEmpty(Template))
                {
                    srEmail = Template;

                    srEmail = srEmail.Replace("{PARTY_NAME}", !String.IsNullOrEmpty(partyName) ? partyName : "");


                }
            }
            catch (Exception) { }
            return srEmail;

        }
        #endregion
        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        private class clsResult
        {
            public Boolean Result { get; set; }
            public String Message { get; set; }
            public Int32 ResultCode { get; set; }
        }
        #endregion
        #region genealogy
        public static String Genealogy_FormatName(String name)
        {
            String Name_Com = "";
            try
            {

           
            String[] sname = name.Split(',');
            if (sname.Length >= 2)
            {
                String fname_tmp = sname[1].Trim();
                String fname = "";
                if (fname_tmp.Length > 13)
                {
                    String[] sname2 = fname_tmp.Split(' ');
                    if (sname2.Length >= 2)
                        fname = String.Format("{0}", sname2[0].Trim());
                    else
                        fname = fname_tmp;
                }
                else fname = fname_tmp;




                Name_Com = String.Format("{0} {1}.", fname, sname[0].Trim()[0]);
            }
            else
            {
                if (name.Length > 13)
                {
                    sname = name.Split(' ');
                    if (sname.Length >= 2)
                        Name_Com = String.Format("{0} {1}.", sname[0].Trim(), sname[1].Trim()[0]);
                    else
                        Name_Com = name;
                }
                else Name_Com = name;
            }
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(name) && name.Length > 7)
                    Name_Com = name.Substring(0, 7) + ".";
            }
            try
            {
                Name_Com = Name_Com.Replace(" and ", " & ");
            }
            catch (Exception ex)
            {
              
            }

            return Name_Com;
        }

         public static String Genealogy_FormatNumber(Int32 number)
        {
             String format="";
             if (number == 1 || number == 21 || number == 31)
                 format=String.Format("{0}st",number);
             else if (number == 2 || number == 22)
                 format=String.Format("{0}nd",number);
             else if (number == 3 || number == 23)
                 format=String.Format("{0}rd",number);
             else 
                 format=String.Format("{0}th",number);
             return format;
         }

          public static String Genealogy_FormatNumberweb(Int32 number)
        {
             String format="";
             if (number == 1 || number == 21 || number == 31)
                 format=String.Format("{0}<small>st</small>",number);
             else if (number == 2 || number == 22)
                 format=String.Format("{0}<small>nd</small>",number);
             else if (number == 3 || number == 23)
                 format=String.Format("{0}<small>rd</small>",number);
             else 
                 format=String.Format("{0}<small>th</small>",number);
             return format;
         }

      
          public static String FormatIcon(DataRow row, String value)
          {
              String ico = EnumGenealogyIco.circle.GetStringValue();
              try
              {
                  if (Convert.ToInt32(row["month" + value + "_auto"].ToString().Substring(1)) == 1)
                      ico = EnumGenealogyIco.star.GetStringValue();
                  else if (Convert.ToInt32(row["month" + value + "_nonauto"].ToString().Substring(1)) == 1)
                      ico = EnumGenealogyIco.point.GetStringValue();
                  else
                      ico = EnumGenealogyIco.circle.GetStringValue();
              }
              catch (Exception)
              {
                  ico = EnumGenealogyIco.circle.GetStringValue();
              }
              

              return ico;
          }
        #endregion

        public static String ValidateAndReplace(String text, String replace)
          {
              String value = "";
              if (String.IsNullOrEmpty(text))
              {
                  value = replace;

              }
              else
                  value = text;
              return value;
          }

        public static String ValidateAndReplace(String type, String key, String replace)
        {
            String value = "";
            if (String.IsNullOrEmpty((String)HttpContext.GetGlobalResourceObject(type, key)))
            {
                CultureInfo ciAux = Thread.CurrentThread.CurrentCulture;
                CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
                value = ValidateAndReplace((String)HttpContext.GetGlobalResourceObject(type, key), replace);
                Thread.CurrentThread.CurrentCulture = ciAux;
                Thread.CurrentThread.CurrentUICulture = ciAux;
            }
            else
                value = (String)HttpContext.GetGlobalResourceObject(type, key);
            return value;
        }

        #region Image
          /// <summary>
          /// Method to resize, convert and save the image.
          /// </summary>   
          public static void ResizeImage(string filePath, int maxWidth, int maxHeight)
          {
              try
              {
                  if (!File.Exists(filePath))
                  {
                      return;
                  
                  }

                  int quality = byte.MaxValue;
                  Bitmap image = new Bitmap(filePath, true);
                  // Get the image's original width and height
                  int originalWidth = image.Width;
                  int originalHeight = image.Height;
                  if (image.Width <= maxWidth && image.Height <= maxHeight)
                  {
                      return;
                  }

                  // To preserve the aspect ratio
                  float ratioX = (float)maxWidth / (float)originalWidth;
                  float ratioY = (float)maxHeight / (float)originalHeight;
                  float ratio = Math.Min(ratioX, ratioY);

                  // New width and height based on aspect ratio
                  int newWidth = (int)(originalWidth * ratio);
                  int newHeight = (int)(originalHeight * ratio);

                  // Convert other formats (including CMYK) to RGB.
                  Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

                  // Draws the image in the specified size with quality mode set to HighQuality
                  using (Graphics graphics = Graphics.FromImage(newImage))
                  {
                      graphics.CompositingQuality = CompositingQuality.HighQuality;
                      graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                      graphics.SmoothingMode = SmoothingMode.HighQuality;
                      graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                  }
                  ImageFormat format = ImageFormat.Jpeg;

                  if (filePath.Contains(".png"))
                      format = ImageFormat.Png;
                  else if (filePath.Contains(".gif"))
                      format = ImageFormat.Gif;
                  else if (filePath.Contains(".bmp"))
                      format = ImageFormat.Bmp;
                  // Get an ImageCodecInfo object that represents the JPEG codec.
                  ImageCodecInfo imageCodecInfo = GetEncoderInfo(format);

                  // Create an Encoder object for the Quality parameter.
                  System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;

                  // Create an EncoderParameters object. 
                  EncoderParameters encoderParameters = new EncoderParameters(1);

                  // Save the image as a JPEG file with quality level.
                  EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
                  encoderParameters.Param[0] = encoderParameter;

                  byte[] imageBytes = null;// newImage.ToString(ImageFormat.Bmp);

                  using (MemoryStream ms = new MemoryStream())
                  {
                      newImage.Save(ms, ImageFormat.Jpeg);
                      imageBytes = ms.ToArray();
                  }

                  // Create the full path to the file.
                  // Path.Combine(photoFilePath, strFilename);
                  image.Dispose();
                  string fullPath = filePath;
                  // Write the file.
                  File.WriteAllBytes(fullPath, imageBytes);

                  //newImage.Save(filePath2, imageCodecInfo, encoderParameters);
              }
              catch (Exception)
              {

              }
          }

          /// <summary>
          /// Method to get encoder infor for given image format.
          /// </summary>
          /// <param name="format">Image format</param>
          /// <returns>image codec info.</returns>
          private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
          {
              return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
          }
#endregion

        public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
        {
              return new DateTime(
                  dateTime.Year,
                  dateTime.Month,
                  dateTime.Day,
                  hours,
                  minutes,
                  seconds,
                  milliseconds,
                  dateTime.Kind);
        }
        public static string StripHTML(string source)
          {
              try
              {
                  string result;

                  // Remove HTML Development formatting
                  // Replace line breaks with space
                  // because browsers inserts space
                  result = source.Replace("\r", " ");
                  // Replace line breaks with space
                  // because browsers inserts space
                  result = result.Replace("\n", " ");
                  // Remove step-formatting
                  result = result.Replace("\t", "");
                  // Remove repeating spaces because browsers ignore them
                  result = System.Text.RegularExpressions.Regex.Replace(result, @"( )+", " ");

                  // Remove the header (prepare first by clearing attributes)
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*head([^>])*>", "<head>",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"(<( )*(/)( )*head( )*>)", "</head>",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(<head>).*(</head>)", string.Empty,
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // remove all scripts (prepare first by clearing attributes)
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*script([^>])*>", "<script>",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"(<( )*(/)( )*script( )*>)", "</script>",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  //result = System.Text.RegularExpressions.Regex.Replace(result,
                  //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
                  //         string.Empty,
                  //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"(<script>).*(</script>)", string.Empty,
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // remove all styles (prepare first by clearing attributes)
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*style([^>])*>", "<style>",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"(<( )*(/)( )*style( )*>)", "</style>",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(<style>).*(</style>)", string.Empty,
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // insert tabs in spaces of <td> tags
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*td([^>])*>", "\t",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // insert line breaks in places of <BR> and <LI> tags
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*br( )*>", "\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*li( )*>", "\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // insert line paragraphs (double line breaks) in place
                  // if <P>, <DIV> and <TR> tags
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*div([^>])*>", "\r\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*tr([^>])*>", "\r\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*p([^>])*>", "\r\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // Remove remaining tags like <a>, links, images,
                  // comments etc - anything that's enclosed inside < >
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<[^>]*>", string.Empty,
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // replace special characters:
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @" ", " ",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&bull;", " * ",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&lsaquo;", "<",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&rsaquo;", ">",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&trade;", "(tm)",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&frasl;", "/",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&lt;", "<",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&gt;", ">",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&copy;", "(c)",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&reg;", "(r)",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  // Remove all others. More can be added, see
                  // http://hotwired.lycos.com/webmonkey/reference/special_characters/
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&(.{2,6});", string.Empty,
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // for testing
                  //System.Text.RegularExpressions.Regex.Replace(result,
                  //       this.txtRegex.Text,string.Empty,
                  //       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // make line breaking consistent
                  result = result.Replace("\n", "\r");

                  // Remove extra line breaks and tabs:
                  // replace over 2 breaks with 2 and over 4 tabs with 4.
                  // Prepare first to remove any whitespaces in between
                  // the escaped characters and remove redundant tabs in between line breaks
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(\r)( )+(\r)", "\r\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(\t)( )+(\t)", "\t\t",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(\t)( )+(\r)", "\t\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(\r)( )+(\t)", "\r\t",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  // Remove redundant tabs
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(\r)(\t)+(\r)", "\r\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  // Remove multiple tabs following a line break with just one tab
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(\r)(\t)+", "\r\t",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  // Initial replacement target string for line breaks
                  string breaks = "\r\r\r";
                  // Initial replacement target string for tabs
                  string tabs = "\t\t\t\t\t";
                  for (int index = 0; index < result.Length; index++)
                  {
                      result = result.Replace(tabs, "\t\t\t\t");
                      breaks = breaks + "\r";
                      tabs = tabs + "\t";
                  }

                  // That's it.
                  return result;
              }
              catch
              {
                  return null;
              }
          }

        #region WebRequest Calls

        public static string WebResquestGet(string url, string contentType = "application/json; charset=utf-8")
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url/*Config.UrlApiAutoship.Replace("{0}", "Autoship_GetProductByMarket?marketId=") + marketId*/);
                request.Method = "GET";
                request.ContentType = contentType/*"application/json; charset=utf-8"*/;
                var response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                //strRespSession = ex.Message;
                //isCorrect = false;
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    return reader.ReadToEnd();
                    // log errorText
                }
                //
            }

          //  return null;
        }

        public static string WebResquestPost(String url, String s)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                byte[] data = Encoding.UTF8.GetBytes(s);
                request.ContentLength = data.Length;
                request.Timeout = 500000;//20 SECONDS
                request.ContentType = "application/json";
                request.Method = "POST";
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return result;
                //txtResponse.Text = result;
            }
            catch (WebException ex)
            {
                //strRespSession = ex.Message;
                //isCorrect = false;
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    return reader.ReadToEnd();
                    // log errorText
                }
                //
            }
        }

        #endregion


        public static String Call_ws_xPos_POST(string _uri, string _parmjson)
          {
                String _resultjson = "";
                try
                {
                    WebRequest wr = WebRequest.Create(_uri);
                    wr.Method = "POST";
                    wr.ContentType = "application/json";
                    wr.ContentLength = _parmjson.Length;
                    var requestStream = wr.GetRequestStream();

                    byte[] postBytes = Encoding.UTF8.GetBytes(_parmjson);
                    requestStream.Write(postBytes, 0, postBytes.Length);
                    requestStream.Close();

                    // grab te response and print it out to the console along with the status code
                    var response = (HttpWebResponse)wr.GetResponse();
             
                    using (var rdr = new StreamReader(response.GetResponseStream()))
                    {
                        _resultjson = rdr.ReadToEnd();
                    }


                }
                catch (Exception ex)
                {

                    throw ex;
                }        

              return _resultjson;
          }


        public static String Call_ws_xPos_GET(string _uri){

              String _resultjson = "";
              
                  try
                  {
                      WebRequest wr = WebRequest.Create(_uri);
                      //wr.Method = "POST";
                      //wr.ContentType = "application/json";
                      //wr.ContentLength = _parmjson.Length;
                      //var requestStream = wr.GetRequestStream();

                      //byte[] postBytes = Encoding.UTF8.GetBytes(_parmjson);
                      //requestStream.Write(postBytes, 0, postBytes.Length);
                      //requestStream.Close();

                      // grab te response and print it out to the console along with the status code
                      var response = (HttpWebResponse)wr.GetResponse();  
                      using (var rdr = new StreamReader(response.GetResponseStream()))
                      {
                          _resultjson = rdr.ReadToEnd();
                      }


                  }
                  catch (Exception)
                  {

                  }

                  return _resultjson;
          }

        public static String EmailFormatReport(String Template
            , String FullName = ""
            //, String Url = ""
            //, String EmailSetting = ""
            , String Code = ""
            , String Number = ""
            , String Domain = ""
            , String Address1 = ""
            , String Address2 = ""
            , String City = ""
            , String Country = ""
            , String Email = ""
            , String Phone = ""
            , String Date = ""
            , String Representative = ""
            , String Detail = ""
            , String Card = ""
            , String Balance = ""
            , String Subtotal = ""
            , String Shipping = ""
            , String Total = ""
            , String Type = ""
            , String Sponsor_name = ""
            , String Sponsor_lastname = ""
            , String Sponsor_phone = ""
            , String Taxes = ""
                , String SAddress1 = ""
            , String SAddress2 = ""
            , String SCity = ""
            , String SCountry = ""
            )
        {
            String srEmail = "";
            try
            {
                if (!String.IsNullOrEmpty(Template))
                {
                    srEmail = Template;
                    srEmail = srEmail.Replace("{FullName}", !String.IsNullOrEmpty(FullName) ? FullName : "");
                    //srEmail = srEmail.Replace("{Url}", !String.IsNullOrEmpty(Url) ? Url : "");
                    //srEmail = srEmail.Replace("{EmailSetting}", !String.IsNullOrEmpty(EmailSetting) ? EmailSetting : "support@tru-friends.com");
                    srEmail = srEmail.Replace("{Code}", !String.IsNullOrEmpty(Code) ? Code : "");
                    srEmail = srEmail.Replace("{Number}", !String.IsNullOrEmpty(Number) ? Number : "");
                    if (Type == "join")
                        srEmail = srEmail.Replace("{Domain}", !String.IsNullOrEmpty(Domain) ? Domain : "");
                    else
                    {

                        srEmail = srEmail.Replace("D_S_PHONE", !String.IsNullOrEmpty(Sponsor_phone) ? Sponsor_phone : "");
                    }

                    srEmail = srEmail.Replace("{Address1}", !String.IsNullOrEmpty(Address1) ? Address1 : "-");
                    srEmail = srEmail.Replace("{Address2}", !String.IsNullOrEmpty(Address2) ? Address2 : "-");
                    srEmail = srEmail.Replace("{City}", !String.IsNullOrEmpty(City) ? City : "-");
                    srEmail = srEmail.Replace("{Country}", !String.IsNullOrEmpty(Country) ? Country : "-");

                    srEmail = srEmail.Replace("{SAddress1}", !String.IsNullOrEmpty(SAddress1) ? SAddress1 : "-");
                    srEmail = srEmail.Replace("{SAddress2}", !String.IsNullOrEmpty(SAddress2) ? SAddress2 : "-");
                    srEmail = srEmail.Replace("{SCity}", !String.IsNullOrEmpty(SCity) ? SCity : "-");
                    srEmail = srEmail.Replace("{SCountry}", !String.IsNullOrEmpty(SCountry) ? SCountry : "-");

                    srEmail = srEmail.Replace("{Email}", !String.IsNullOrEmpty(Email) ? Email : "");
                    srEmail = srEmail.Replace("{Phone}", !String.IsNullOrEmpty(Phone) ? Phone : "");
                    srEmail = srEmail.Replace("{Date}", !String.IsNullOrEmpty(Date) ? Date : "");
                    //srEmail = srEmail.Replace("{Representative}", !String.IsNullOrEmpty(Representative) ? Representative : "");

                    srEmail = srEmail.Replace("<tr><td></td><td colspan=\"9\">{Detail}</td></tr>", !String.IsNullOrEmpty(Detail) ? Detail : "");
                    srEmail = srEmail.Replace("{Card}", !String.IsNullOrEmpty(Card) ? Card : "");
                    srEmail = srEmail.Replace("{Balance}", !String.IsNullOrEmpty(Balance) ? Balance : "0.00");
                    srEmail = srEmail.Replace("{Taxes}", !String.IsNullOrEmpty(Taxes) ? Taxes : "0.00");
                    srEmail = srEmail.Replace("{Subtotal}", !String.IsNullOrEmpty(Subtotal) ? Subtotal : "0.00");
                    srEmail = srEmail.Replace("{Shipping}", !String.IsNullOrEmpty(Shipping) ? Shipping : "0.00");
                    srEmail = srEmail.Replace("{Total}", !String.IsNullOrEmpty(Total) ? Total : "0.00");
                    srEmail = srEmail.Replace("D_S_NAME", !String.IsNullOrEmpty(Sponsor_name) ? Sponsor_name : "");
                    srEmail = srEmail.Replace("D_S_LASTNAME", !String.IsNullOrEmpty(Sponsor_lastname) ? Sponsor_lastname : "");


                }
            }
            catch (Exception) { }
            return srEmail;

        }


        #region Credit Card Validator


           private const string cardRegex = "^(?:(?<Visa>4\\d{3})|(?<MasterCard>5[1-5]\\d{2})|(?<Discover>6011)|(?<DinersClub>(?:3[68]\\d{2})|(?:30[0-5]\\d))|(?<Amex>3[47]\\d{2}))([ -]?)(?(DinersClub)(?:\\d{6}\\1\\d{4})|(?(Amex)(?:\\d{6}\\1\\d{5})|(?:\\d{4}\\1\\d{4}\\1\\d{4})))$";

           public static string IsValidNumber(string cardNum)
           {
               Regex cardTest = new Regex(cardRegex);

               //Determine the card type based on the number
               CreditCardTypeType? cardType = GetCardTypeFromNumber(cardNum);
               //Call the base version of IsValidNumber and pass the 
               //number and card type
               if (IsValidNumber(cardNum, cardType))
                   return cardType.ToString();
               else
                   return null;
           }

           public static bool IsValidNumber(string cardNum, CreditCardTypeType? cardType)
           {
               //Create new instance of Regex comparer with our 
               //credit card regex pattern
               Regex cardTest = new Regex(cardRegex);

               //Make sure the supplied number matches the supplied
               //card type
               if (cardTest.Match(cardNum).Groups[cardType.ToString()].Success)
               {
                   //If the card type matches the number, then run it
                   //through Luhn's test to make sure the number appears correct
                   if (PassesLuhnTest(cardNum))
                       return true;
                   else
                       //The card fails Luhn's test
                       return false;
               }
               else
                   //The card number does not match the card type
                   return false;
           }

           public static bool PassesLuhnTest(string cardNumber)
           {
               //Clean the card number- remove dashes and spaces
               cardNumber = cardNumber.Replace("-", "").Replace(" ", "");

               //Convert card number into digits array
               int[] digits = new int[cardNumber.Length];
               for (int len = 0; len < cardNumber.Length; len++)
               {
                   digits[len] = Int32.Parse(cardNumber.Substring(len, 1));
               }

               //Luhn Algorithm
               //Adapted from code availabe on Wikipedia at
               //http://en.wikipedia.org/wiki/Luhn_algorithm
               int sum = 0;
               bool alt = false;
               for (int i = digits.Length - 1; i >= 0; i--)
               {
                   int curDigit = digits[i];
                   if (alt)
                   {
                       curDigit *= 2;
                       if (curDigit > 9)
                       {
                           curDigit -= 9;
                       }
                   }
                   sum += curDigit;
                   alt = !alt;
               }

               //If Mod 10 equals 0, the number is good and this will return true
               return sum % 10 == 0;
           }

           public static bool IsGovernmetID(int marketid, string ssn)
           {
               Regex regSsn = null;
               if (Convert.ToInt16(EnumMarkets.Switzerland) == marketid)
                   regSsn = new Regex("^([A-Z]{1}[0-9]{7})|(756[0-9]{10})$");
               else if (Convert.ToInt16(EnumMarkets.Italia) == marketid)
                   regSsn = new Regex("^[A-Z]{6}[0-9]{2}[A-Z]{1}[0-9]{2}[A-Z]{1}[0-9]{3}[A-Z]{1}$");
               else if (Convert.ToInt16(EnumMarkets.UnitedStates) == marketid || Convert.ToInt16(EnumMarkets.Canada) == marketid)
                   regSsn = new Regex("^[0-9]{9}$");
               else
                   return true;
               if (regSsn != null)
               {
                   if (regSsn.IsMatch(ssn))
                       return true;
               }
               return false;
           }

           public static string GetIDCreditCardType_toGC(string CreditCardNumber)
           {
               Regex regVisa = new Regex("^4[0-9]{12}(?:[0-9]{3})?$");
               Regex regMaster = new Regex("^5[1-5][0-9]{14}$");
               Regex regExpress = new Regex("^3[47][0-9]{13}$");
               Regex regDiners = new Regex("^3(?:0[0-5]|[68][0-9])[0-9]{11}$");
               Regex regDiscover = new Regex("^6(?:011|5[0-9]{2})[0-9]{12}$");
               Regex regJSB = new Regex("^(?:2131|1800|35\\d{3})\\d{11}$");


               if (regVisa.IsMatch(CreditCardNumber))
                   return "1";//VISA
               if (regMaster.IsMatch(CreditCardNumber))
                   return "3";//MASTER
               if (regExpress.IsMatch(CreditCardNumber))
                   return "2";//AEXPRESS
               if (regDiners.IsMatch(CreditCardNumber))
                   return "132";//DINERS
               if (regDiscover.IsMatch(CreditCardNumber))
                   return "128";//DISCOVERS
               if (regJSB.IsMatch(CreditCardNumber))
                   return "125";//JSB
               return "";//return "invalid";
           }
           public static string GetCreditCardType(string CreditCardNumber)
           {
               Regex regVisa = new Regex("^4[0-9]{12}(?:[0-9]{3})?$");
               Regex regMaster = new Regex("^5[1-5][0-9]{14}$");
               Regex regExpress = new Regex("^3[47][0-9]{13}$");
               Regex regDiners = new Regex("^3(?:0[0-5]|[68][0-9])[0-9]{11}$");
               Regex regDiscover = new Regex("^6(?:011|5[0-9]{2})[0-9]{12}$");
               Regex regJSB = new Regex("^(?:2131|1800|35\\d{3})\\d{11}$");


               if (regVisa.IsMatch(CreditCardNumber))
                   return "VISA";
               if (regMaster.IsMatch(CreditCardNumber))
                   return "MASTER";
               if (regExpress.IsMatch(CreditCardNumber))
                   return "AEXPRESS";
               if (regDiners.IsMatch(CreditCardNumber))
                   return "DINERS";
               if (regDiscover.IsMatch(CreditCardNumber))
                   return "DISCOVERS";
               if (regJSB.IsMatch(CreditCardNumber))
                   return "JSB";
               return "";//return "invalid";
           }

           public static CreditCardTypeType? GetCardTypeFromNumber(string cardNum)
           {
               //Create new instance of Regex comparer with our
               //credit card regex patter
               Regex cardTest = new Regex(cardRegex);

               //Compare the supplied card number with the regex
               //pattern and get reference regex named groups
               GroupCollection gc = cardTest.Match(cardNum).Groups;

               //Compare each card type to the named groups to 
               //determine which card type the number matches
               if (gc[CreditCardTypeType.Amex.ToString()].Success)
               {
                   return CreditCardTypeType.Amex;
               }
               else if (gc[CreditCardTypeType.MasterCard.ToString()].Success)
               {
                   return CreditCardTypeType.MasterCard;
               }
               else if (gc[CreditCardTypeType.Visa.ToString()].Success)
               {
                   return CreditCardTypeType.Visa;
               }
               else if (gc[CreditCardTypeType.Discover.ToString()].Success)
               {
                   return CreditCardTypeType.Discover;
               }
               else
               {
                   //Card type is not supported by our system, return null
                   //(You can modify this code to support more (or less)
                   // card types as it pertains to your application)
                   return null;
               }
           }

           public enum CreditCardTypeType
           {
               Visa,
               MasterCard,
               Discover,
               Amex,
               Switch,
               Solo
           }


           #endregion

        public static String GetCulturebyCultureBrowser(List<String> List, String ci)
        {
            String ci_db = "";

            if (List != null)
            {
                foreach (String item in List)
                {

                    if (item.ToLower().Contains(ci.ToLower()))
                    {
                        ci_db = item;
                        break;
                    }

                }
                if (String.IsNullOrEmpty(ci_db))
                {
                    String[] array_ci = ci.Split('-');
                    foreach (String item in List)
                    {
                        for (int i = 0; i < array_ci.Length; i++)
                        {
                            if (item.ToLower().Contains(array_ci[i].ToLower()))
                            {
                                ci_db = item;
                                break;
                            }

                        }
                        if (!String.IsNullOrEmpty(ci_db))
                        {
                            break;
                        }

                    }
                }
            }
            if (String.IsNullOrEmpty(ci_db))
            {
                ci_db = ci;
            }
            return ci_db;
        }
        public static String GetInfotraxLanguageCodebyCulture(String lancode)
        {
            lancode = lancode.ToLower();
            String Culture = "en-US";
            switch (lancode)
            {
                case "hr-hr":
                    Culture = "hv";
                    break;
                case "cs-cz":
                    Culture = "cs";
                    break;
                case "da":
                    Culture = "da";
                    break;
                case "nl":
                    Culture = "nl";
                    break;
                case "nl-be":
                    Culture = "nl_bel";
                    break;
                case "en-us":
                    Culture = "en_asea";
                    break;
                case "fi-fi":
                    Culture = "fi";
                    break;
                case "fr-be":
                    Culture = "fr_bel";
                    break;
                case "fr-ca":
                    Culture = "fr_can";
                    break;
                case "fr-fr":
                    Culture = "fr";
                    break;
                case "de-de":
                    Culture = "de";
                    break;
                case "hu-hu":
                    Culture = "hu";
                    break;
                case "it-it":
                    Culture = "it";
                    break;
                case "nn-no":
                    Culture = "no";
                    break;
                case "pt-pt":
                case "pt-br":
                    Culture = "pt";
                    break;
                case "ro":
                    Culture = "ro";
                    break;
                case "sk-sk":
                    Culture = "sk";
                    break;
                case "sl-si":
                    Culture = "sl";
                    break;
                case "es-mx":
                    Culture = "es";
                    break;
                case "es-es":
                    Culture = "es_esp";
                    break;
                case "sv-se":
                    Culture = "sv";
                    break;
                default:
                    Culture = "en_asea";
                    break;

            }

            return Culture;


        }
        public static String GenerateAuthKey_Jixiti(String apikey, String method)
        {
            String signature = "";

            DateTime date = DateTime.Now;
            List<String> lstTime = new List<String>();

            String concat = apikey + Base64Encode(method) + date.ToUniversalTime().ToString("yyyyMMddmm");
            signature = clsEncryption.Encrypt_Sha1(concat, false);

            return signature;
        }
        public static string Base64Encode(string plainText)
           {
               var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
               return System.Convert.ToBase64String(plainTextBytes);
           }




        public static string ValidateDataRowKey(DataRow dr, string key) 
        {
            try
            {
                return dr[key] is DBNull ? String.Empty : Convert.ToString(dr[key], CultureInfo.InvariantCulture);
            }
            catch (Exception) { return String.Empty; }
        }
        public static string ValidateDataRowKeyDefault(DataRow dr, string key, string vdefault)
        {
            try
            {
                return dr[key] is DBNull ? vdefault : Convert.ToString(dr[key], CultureInfo.InvariantCulture);
            }
            catch (Exception) { return vdefault; }
        }
        public static String EmailFormatSubject(String Template, String partyName = "")
        {
            String srEmail = "";
            try
            {
                if (!String.IsNullOrEmpty(Template))
                {
                    srEmail = Template;

                    srEmail = srEmail.Replace("{PARTY_NAME}", !String.IsNullOrEmpty(partyName) ? partyName : "");


                }
            }
            catch (Exception) { }
            return srEmail;

        }

        public static String EmailSendFormatParty(String Template, String PersonalMessage = "", String G_FirstName = "", String G_LastName = "",
        String G_Country = "", String G_Phone = "", String G_Address = "", String PTY_Name = "", String PTY_Country = "", String PTY_City = "", String PTY_State = "",
            String PTY_Address = "", String PTY_Start = "", String PTY_End = "", String Hostes_Email = "", String Distributor_RSite = "", String PTY_ID = "", String G_ID = "", String ANS_Y = "", String ANS_M = "", String ANS_N = "", String Password = "", String Siteid = ""
        )
        {
            String srEmail = "";
            try
            {
                if (!String.IsNullOrEmpty(Template))
                {
                    srEmail = Template;

                    srEmail = srEmail.Replace("{PERSONALMESSAGE}", !String.IsNullOrEmpty(PersonalMessage) ? PersonalMessage : "");
                    srEmail = srEmail.Replace("{G_FIRSTNAME}", !String.IsNullOrEmpty(G_FirstName) ? G_FirstName : "");
                    srEmail = srEmail.Replace("{G_LASTNAME}", !String.IsNullOrEmpty(G_LastName) ? G_LastName : "");
                    srEmail = srEmail.Replace("{G_COUNTRY}", !String.IsNullOrEmpty(G_Country) ? G_Country : "");
                    srEmail = srEmail.Replace("{G_PHONE}", !String.IsNullOrEmpty(G_Phone) ? G_Phone : "");
                    srEmail = srEmail.Replace("{G_ADDRESS}", !String.IsNullOrEmpty(G_Address) ? G_Address : "");
                    srEmail = srEmail.Replace("{PTY_NAME}", !String.IsNullOrEmpty(PTY_Name) ? PTY_Name : "");
                    srEmail = srEmail.Replace("{PTY_COUNTRY}", !String.IsNullOrEmpty(PTY_Country) ? PTY_Country : "");
                    srEmail = srEmail.Replace("{PTY_CITY}", !String.IsNullOrEmpty(PTY_City) ? PTY_City : "");
                    srEmail = srEmail.Replace("{PTY_STATE}", !String.IsNullOrEmpty(PTY_State) ? PTY_State : "");
                    srEmail = srEmail.Replace("{PTY_ADDRESS}", !String.IsNullOrEmpty(PTY_Address) ? PTY_Address : "");
                    srEmail = srEmail.Replace("{PTY_START}", !String.IsNullOrEmpty(PTY_Start) ? PTY_Start : "");
                    srEmail = srEmail.Replace("{PTY_END}", !String.IsNullOrEmpty(PTY_End) ? PTY_End : "");
                    srEmail = srEmail.Replace("{HOSTES_EMAIL}", !String.IsNullOrEmpty(Hostes_Email) ? Hostes_Email : "");
                    srEmail = srEmail.Replace("{DISTRIBUTOR_REPLICATEDSITE}", !String.IsNullOrEmpty(Distributor_RSite) ? Distributor_RSite : "");
                    srEmail = srEmail.Replace("{PTY_ID}", !String.IsNullOrEmpty(PTY_ID) ? PTY_ID : String.Empty);
                    srEmail = srEmail.Replace("{G_ID}", !String.IsNullOrEmpty(G_ID) ? G_ID : String.Empty);
                    srEmail = srEmail.Replace("{ANS_Y}", !String.IsNullOrEmpty(ANS_Y) ? ANS_Y : String.Empty);
                    srEmail = srEmail.Replace("{ANS_M}", !String.IsNullOrEmpty(ANS_M) ? ANS_M : String.Empty);
                    srEmail = srEmail.Replace("{ANS_N}", !String.IsNullOrEmpty(ANS_N) ? ANS_N : String.Empty);
                    srEmail = srEmail.Replace("{HOSTES_PASSWORD}", !String.IsNullOrEmpty(Password) ? Password : String.Empty);
                    srEmail = srEmail.Replace("{SITE_ID}", !String.IsNullOrEmpty(Siteid) ? Siteid : String.Empty);
                }
            }
            catch (Exception) { }
            return srEmail;

        }

        public static String EmailFormatParty(String Template, String PersonalMessage = "", String G_FirstName = "", String G_LastName = "",
         String G_Country = "", String G_Phone = "", String G_Address = "", String PTY_Name = "", String PTY_Country = "", String PTY_City = "", String PTY_State = "",
             String PTY_Address = "", String PTY_Start = "", String PTY_End = "", String Hostes_Email = "", String Distributor_RSite = "", String PTY_ID = "", String G_ID = "", String ANS_Y = "", String ANS_M = "", String ANS_N = "", String Hostess_fname = "", String Hostess_lname = "", String Hostess_phone = "", String PTY_Eventdate = "", String Hostess_id = ""
         )
        {
            String srEmail = "";
            try
            {
                if (!String.IsNullOrEmpty(Template))
                {
                    srEmail = Template;

                    srEmail = srEmail.Replace("{PERSONALMESSAGE}", !String.IsNullOrEmpty(PersonalMessage) ? PersonalMessage : "");
                    srEmail = srEmail.Replace("{G_FIRSTNAME}", !String.IsNullOrEmpty(G_FirstName) ? G_FirstName : "");
                    srEmail = srEmail.Replace("{G_LASTNAME}", !String.IsNullOrEmpty(G_LastName) ? G_LastName : "");
                    srEmail = srEmail.Replace("{G_COUNTRY}", !String.IsNullOrEmpty(G_Country) ? G_Country : "");
                    srEmail = srEmail.Replace("{G_PHONE}", !String.IsNullOrEmpty(G_Phone) ? G_Phone : "");
                    srEmail = srEmail.Replace("{G_ADDRESS}", !String.IsNullOrEmpty(G_Address) ? G_Address : "");
                    srEmail = srEmail.Replace("{PTY_NAME}", !String.IsNullOrEmpty(PTY_Name) ? PTY_Name : "");
                    srEmail = srEmail.Replace("{PTY_COUNTRY}", !String.IsNullOrEmpty(PTY_Country) ? PTY_Country : "");
                    srEmail = srEmail.Replace("{PTY_CITY}", !String.IsNullOrEmpty(PTY_City) ? PTY_City : "");
                    srEmail = srEmail.Replace("{PTY_STATE}", !String.IsNullOrEmpty(PTY_State) ? PTY_State : "");
                    srEmail = srEmail.Replace("{PTY_ADDRESS}", !String.IsNullOrEmpty(PTY_Address) ? PTY_Address : "");
                    srEmail = srEmail.Replace("{PTY_START}", !String.IsNullOrEmpty(PTY_Start) ? PTY_Start : "");
                    srEmail = srEmail.Replace("{PTY_END}", !String.IsNullOrEmpty(PTY_End) ? PTY_End : "");
                    srEmail = srEmail.Replace("{HOSTES_EMAIL}", !String.IsNullOrEmpty(Hostes_Email) ? Hostes_Email : "");
                    srEmail = srEmail.Replace("{DISTRIBUTOR_REPLICATEDSITE}", !String.IsNullOrEmpty(Distributor_RSite) ? Distributor_RSite : "");
                    srEmail = srEmail.Replace("{PTY_ID}", !String.IsNullOrEmpty(PTY_ID) ? PTY_ID : String.Empty);
                    srEmail = srEmail.Replace("{G_ID}", !String.IsNullOrEmpty(G_ID) ? G_ID : String.Empty);
                    srEmail = srEmail.Replace("{ANS_Y}", !String.IsNullOrEmpty(ANS_Y) ? ANS_Y : String.Empty);
                    srEmail = srEmail.Replace("{ANS_M}", !String.IsNullOrEmpty(ANS_M) ? ANS_M : String.Empty);
                    srEmail = srEmail.Replace("{ANS_N}", !String.IsNullOrEmpty(ANS_N) ? ANS_N : String.Empty);
                    srEmail = srEmail.Replace("{HOSTES_FIRSTNAME}", !String.IsNullOrEmpty(Hostess_fname) ? Hostess_fname : "");
                    srEmail = srEmail.Replace("{HOSTES_LASTNAME}", !String.IsNullOrEmpty(Hostess_lname) ? Hostess_lname : "");
                    srEmail = srEmail.Replace("{HOSTES_PHONE}", !String.IsNullOrEmpty(Hostess_phone) ? Hostess_phone : "");
                    srEmail = srEmail.Replace("{ID_INFOTRAX}", !String.IsNullOrEmpty(Hostess_id) ? Hostess_id : "");
                    srEmail = srEmail.Replace("{PTY_EVENTDATE}", !String.IsNullOrEmpty(PTY_Eventdate) ? PTY_Eventdate : "");
                }
            }
            catch (Exception) { }
            return srEmail;

        }






        /// <summary>
        /// TO VALIDATE IF EXITS RESOURCE KEY
        /// </summary>
        /// <param name="text"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static String ValidateResourceEntry(string text, string replace)
        {
            return String.IsNullOrEmpty(text) ? replace : text;
        }


        public static string GetCardFormatToShow(string cardNumber)
        {
            int cardNumberLength = cardNumber.Length;
            //NO VALID CREDIT CARD LENGTH
            if (cardNumberLength < 13 || cardNumberLength > 16) return null;

            //COMPLETE CHARACTERS WITH *
            string chars = String.Empty;
            for (int i = 4; i < cardNumberLength; i++) chars += "*";

            return chars + cardNumber.Substring(cardNumberLength - 4);
        }


        public static string HTMLToText(string HTMLCode)
        {
            // Remove new lines since they are not visible in HTML
            HTMLCode = HTMLCode.Replace("\n", " ");

            // Remove tab spaces
            HTMLCode = HTMLCode.Replace("\t", " ");

            // Remove multiple white spaces from HTML
            HTMLCode = Regex.Replace(HTMLCode, "\\s+", " ");

            // Remove HEAD tag
            HTMLCode = Regex.Replace(HTMLCode, "<head.*?</head>", ""
                                , RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Remove any JavaScript
            HTMLCode = Regex.Replace(HTMLCode, "<script.*?</script>", ""
              , RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Replace special characters like &, <, >, " etc.
            StringBuilder sbHTML = new StringBuilder(HTMLCode);
            // Note: There are many more special characters, these are just
            // most common. You can add new characters in this arrays if needed
            string[] OldWords = {"&nbsp;", "&amp;", "&quot;", "&lt;", 
       "&gt;", "&reg;", "&copy;", "&bull;", "&trade;"};
            string[] NewWords = { " ", "&", "\"", "<", ">", "Â®", "Â©", "â€¢", "â„¢" };
            for (int i = 0; i < OldWords.Length; i++)
            {
                sbHTML.Replace(OldWords[i], NewWords[i]);
            }

            // Check if there are line breaks (<br>) or paragraph (<p>)
            sbHTML.Replace("<br>", "\n<br>");
            sbHTML.Replace("<br ", "\n<br ");
            sbHTML.Replace("<p ", "\n<p ");

            // Finally, remove all HTML tags and return plain text
            return System.Text.RegularExpressions.Regex.Replace(
              sbHTML.ToString(), "<[^>]*>", "");
        }


        public static Boolean IsValidEmail(String email)
        {
            return regularExpressionEmail.IsMatch(email.ToLower().Trim());
        }

        /// <summary>
        /// Get object value by reflection
        /// </summary>
        /// <param name="src"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }


        public static T GetPropValue<T>(object src, string propName)
        {
            return (T)src.GetType().GetProperty(propName).GetValue(src, null);
        }



        public static String GenerateMasterKey()
        {

            var Key = System.Web.Configuration.WebConfigurationManager.AppSettings["KeyMaster"];
            var dm = DateTime.Now.ToString("yyyyMMddHHmm");

            var mkey = clsEncryption.Encrypt(Key + "." + dm);
            return mkey;
        }

        public static Boolean ValidateMasterKey(String xKey) 
        {
            String[] arrayConcated = clsEncryption.Decrypt(xKey).Split('.');
           DateTime date = DateTime.ParseExact(arrayConcated[1], "yyyyMMddHHmm",System.Globalization.CultureInfo.InvariantCulture);
            DateTime date2 = date.AddMinutes(10);

            DateTime ActualDate = DateTime.ParseExact(DateTime.Now.ToString("yyyyMMddHHmm"), "yyyyMMddHHmm",System.Globalization.CultureInfo.InvariantCulture) ;
            Boolean success = false;

            if (System.Web.Configuration.WebConfigurationManager.AppSettings["KeyMaster"] == arrayConcated[0])
            {
                if (ActualDate >= date && ActualDate <= date2)
                {
                    success = true;
                }
            }
            return success;
        }

        /// <summary>
        /// Gets an attribute value contained on an xml string
        /// </summary>
        /// <param name="xml">String containing xml</param>
        /// <param name="AttributeName"></param>
        /// <param name="AttributeValueToFind"></param>
        /// <returns></returns>
        public static String GetValueFromXmlString(String xml, String AttributeName, String AttributeValueToFind)
        {
            String idhost = "0";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            // XmlNode node = xmlDoc.DocumentElement.FirstChild;
            XmlNodeList elemList = xmlDoc.GetElementsByTagName("data");
            XmlNode node = elemList[0].FirstChild;
            XmlNodeList lststruct = node.ChildNodes;
            for (int i = 0; i < lststruct.Count; i++)
            {

                if (lststruct[i].Attributes[AttributeName].Value == AttributeValueToFind)
                {
                    XmlNodeList lstId = lststruct[i].ChildNodes;
                    for (int j = 0; j < lstId.Count; j++)
                    {
                        idhost = lstId[j].InnerText;
                    }

                }
                //String node = lststruct[i].InneAttributeValueToFindrXml;
            }
            return idhost;
        }

        public static String ToDynamicString(dynamic attr)
        {
            try
            {
                if (attr is Int32) { return ((Int32)attr).ToString(); }
                else if (attr is Int64) { return ((Int64)(attr)).ToString(); }
                else if (attr is Decimal) { return ((Int64)attr).ToString(); }
                else { return ((string)attr).ToString(); }

            }
            catch (Exception)
            {
                return "";
            }
        }

        public static DataTable FormatDataToDataDll(DataTable dt)
        {
            DataTable dtDll = null;
            
            try
            {

                dtDll = new DataTable();
               
                dtDll.Columns.Add("ID");
                dtDll.Columns.Add("PERIODNAME");
                
                foreach (DataRow item in dt.Rows) {
                    DataRow rowDll = dtDll.NewRow();
                    rowDll[0] = item["ID"];
                    rowDll[1] = ValidateResourceEntry((String)HttpContext.GetGlobalResourceObject("Strings", "PERIOD_DDL"), "Period") +" "+ item["PERIOD"] + " "+ValidateResourceEntry((String)HttpContext.GetGlobalResourceObject("Strings", "PERIOD_OF_DDL"), "of")+ " " + item["TOTALWEEK"] + ", " + item["YEAR"];
                    dtDll.Rows.Add(rowDll);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return dtDll;
        }

        public static DataTable FormatDataToDataDll2(DataTable dt)
        {
            DataTable dtDll = null;

            try
            {

                dtDll = new DataTable();

                dtDll.Columns.Add("ID");
                dtDll.Columns.Add("NAME");

                foreach (DataRow item in dt.Rows)
                {
                    DataRow rowDll = dtDll.NewRow();
                    rowDll[0] = item["ID"];
                    rowDll[1] = item["NAME"];
                    dtDll.Rows.Add(rowDll);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return dtDll;
        }

    }
}
