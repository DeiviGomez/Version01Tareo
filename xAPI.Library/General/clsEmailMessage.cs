using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Library.Security;
using Microsoft.SqlServer.Server;
using System.Web;

namespace xAPI.Library.General
{
    public class clsEmailMessage
    {
        public static StringBuilder MailBody(String Username, String Eventname, String TypeTeam, String TeamContactId, Boolean Exist)
        {
            StringBuilder mailBody = new StringBuilder();

            String urlImage = "http://www.tru-friends.com/contests/Resources/images/headerTF.jpg";

            mailBody.AppendFormat("<div style='width: 100%; height: 100%; background-color: #FFFFFF; margin: 0 auto;'>");
            mailBody.AppendFormat("<table style='margin: 0 auto; width: 700px; overflow: hidden;'  border='0' cellspacing='0' cellpadding='0'>");
            mailBody.AppendFormat("<thead><tr><td>");
            mailBody.AppendFormat("<div style='overflow:hidden; position:relative; width: 700px; height: 120px; background-color: #FFFFFF ; border-radius: 8px 8px 0px 0px; -moz-border-radius: 8px 8px 0px 0px; -webkit-border-radius: 8px 8px 0px 0px;'>");
            mailBody.AppendFormat("<img src='{0}' alt='TruFriends' title='TruFriends' style='width: 700px; height: 120; position: absolute; border-radius: 8px 8px 0px 0px; -moz-border-radius: 8px 8px 0px 0px; -webkit-border-radius: 8px 8px 0px 0px;' />", urlImage);
            mailBody.AppendFormat("<div style='z-index: 2; position:absolute; margin: 90px 5px;'><h1 style=' color: white; font-family:Lucida Calligraphy; font-size:medium;'><strong> ... a mail delivered </strong></h1></div>");
            mailBody.AppendFormat("</div>");
            mailBody.AppendFormat("</td></tr></thead>");
            mailBody.AppendFormat("<tbody style='background: #ececec repeat;'>");
            mailBody.AppendFormat("<tr><td><hr/></td></tr>");
            mailBody.AppendFormat("<tr><td style='width: 700px; text-align: justify;'>");
            mailBody.AppendFormat("<h2 align='center' style='font-family: Aparajita; font-size: xx-large; color: #348BB4;'>True-Friends invite you to join to a team event</h2>");
            mailBody.AppendFormat("</td></tr>");
            mailBody.AppendFormat("<tr><td>");
            mailBody.AppendFormat("<div style='position:relative;  width: 530px; height: 160px; margin: 0 auto; margin-bottom: 20px; text-align: justify; color: #676767; -moz-box-shadow: 2px 2px 4px #CCC; -webkit-box-shadow: 2px 2px 4px #CCC; box-shadow: 2px 2px 4px #CCC;'>");
            mailBody.AppendFormat("<h4>Dear,</h4>");
            mailBody.AppendFormat("<p style='text-indent: 3em; font-family: Aparajita; font-size: medium;'>You have been selected to take part as a <strong style='color: #348BB4 '>{0}</strong> for <strong style='color: #348BB4'>{1}</strong> event.</p>", TypeTeam, Eventname);
            mailBody.AppendFormat("<p  style='display: inline; font-family: Aparajita; font-size: medium;'>If you are interested or you want to get more information about, just click on the next link in order to get - </p>");

            if (Exist == false)
                mailBody.AppendFormat("<a href='http://www.tru-friends.com/Register.aspx?q={0}' target='_blank' style='text-decoration:none; font-family: Aparajita; font-size: medium; color: #348BB4'> Enrolled </a>", TeamContactId);
            else
                mailBody.AppendFormat("<a href='http://www.tru-friends.com/contests/Private/Login.aspx?q={0}' target='_blank' style='text-decoration:none; font-family: Aparajita; font-size: medium; color: #348BB4'>Logged</a>", TeamContactId);


            //mailBody.AppendFormat("<a href='http://www.tru-friends.com/contests/Private/Checking.aspx?q={0}' target='_blank' style='text-decoration:none; font-family: Aparajita; font-size: medium; color: #348BB4'>Join us</a>",TeamContactId);
            
            
            mailBody.AppendFormat("</div>");
            mailBody.AppendFormat("</td></tr>");
            mailBody.AppendFormat("<tr><td><hr/></td></tr>");
            mailBody.AppendFormat("</tbody>");
            mailBody.AppendFormat("<tfoot style='border-radius: 0px 0px 8px 8px; -moz-border-radius: 0px 0px 8px 8px; -webkit-border-radius: 0px 0px 8px 8px;'>");
            mailBody.AppendFormat("<tr><td>");
            mailBody.AppendFormat("<div style='overflow:hidden; position:relative; background: #ececec repeat; width: 700px; height: 50px; border-radius: 0px 0px 8px 8px; -moz-border-radius: 0px 0px 8px 8px; -webkit-border-radius: 0px 0px 8px 8px;'>");
            mailBody.AppendFormat("<p align='center'; style='color: #616161; font-size: medium; '>Copyright © - Tru-Friends</p>");
            mailBody.AppendFormat("</div>");
            mailBody.AppendFormat("</td></tr>");
            mailBody.AppendFormat("</tfoot>");
            mailBody.AppendFormat("</table>");
            mailBody.AppendFormat("</div>");
   
            return mailBody;
        }

        public static StringBuilder ManagementTeam(String Username, String Eventname, String TypeTeam, String TeamContactId, Boolean Exist)
        {
            String encrypt = clsEncryption.Encrypt(TeamContactId);
            encrypt = "?a=" + encrypt;
            encrypt = clsEncryption.Encrypt(encrypt);
            encrypt = System.Web.HttpUtility.UrlEncode(encrypt);

            return MailBody(Username, Eventname, TypeTeam, encrypt, Exist);
        }
    }
}
