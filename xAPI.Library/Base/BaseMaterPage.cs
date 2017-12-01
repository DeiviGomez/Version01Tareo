using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace xAPI.Library.Base
{
    public class BaseMaterPage : System.Web.UI.MasterPage
    {
        public void Alert(string message)
        {
            String script = "<script language='javascript'>jAlert('" + message + "');</script>";
            ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Alert", script, true);
        }

        public void Dc_Menu(Int32 distributorid)
        {
            String script, ele;
            if (distributorid > 0)
            {
                ele = "$('#mega-menu').dcMegaMenu(); $('#singin-li, #spacer-li').hide();";
                script = "$(document).ready(function () {" + ele + "});";
                ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Menu", script, true);
            }
            else if (distributorid == -1)
            {
                ele = "$('.mega-menu-li-ul').css('display', 'none'); + $('#image-row').css('display','none')";
                script = "$(document).ready(function () {" + ele + "});";
                ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Menu", script, true);
            }
        }
    }
}
