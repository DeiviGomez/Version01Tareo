using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using xAPI.Library.General;

namespace xAPI.Library.Base
{
    public class BaseUserControl : System.Web.UI.UserControl 
    {
        public void Alert(string message)
        {
            String script = "jAlert('" + message + "');";
            ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Alert", script, true);
        }
        public void Message(EnumAlertType type)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "message", @"<script type='text/javascript'>fn_message('" + type.GetStringValue() + "');</script>", false);
        }

        public void Message(EnumAlertType type, String message)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "message", @"<script type='text/javascript'>fn_message('" + type.GetStringValue() + "', '" + message + "');</script>", false);
        }
    }
}
