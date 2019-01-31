using System;
using System.Text;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using CF;
using System.Web.Hosting;
using System.Collections;

namespace Common
{
    public class MyEmail
    {
        /// <summary>
        /// 邮件发送方法
        /// </summary>
        /// <param name="List">发件人地址列表</param>
        /// <param name="MailTitle">邮件标题</param>
        /// <param name="MailContent">邮件正文</param>
        /// <param name="FromAddress">发件人邮箱地址</param>
        /// <param name="MailHost">邮件服务器如mail.qq.com</param>
        /// <param name="MailPossword">发送方的邮件地址密码</param>
        public static void SendMail(List<string> List, string MailTitle, string MailContent,string FromAddress,string MailHost,string MailPossword)
        {
            MailMessage objMailMessage = new MailMessage();

            objMailMessage.From = new MailAddress(FromAddress);//发送方地址
            foreach (var EmailAddress in List)
            {
                objMailMessage.To.Add(new MailAddress(EmailAddress));//收信人地址
            }
            objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;//邮件编码
            objMailMessage.Subject = MailTitle;//邮件标题
            objMailMessage.Body = MailContent;//邮件内容
            objMailMessage.IsBodyHtml = false;//邮件正文是否为html格式

            SmtpClient objSmtpClient = new SmtpClient();
            objSmtpClient.Host = MailHost;//邮件服务器地址
            objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//通过网络发送到stmp邮件服务器
            objSmtpClient.Credentials = new System.Net.NetworkCredential(FromAddress, MailPossword);//发送方的邮件地址，密码
            //objSmtpClient.EnableSsl = true;//SMTP 服务器要求安全连接需要设置此属性

            try
            {
                objSmtpClient.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                cls_Common.writeLog(
                      HostingEnvironment.MapPath("/log/" + DateTime.Now.ToString("MMdd") + "/" + DateTime.Now.ToString("HH") + "_Error.txt"),
                       DateTime.Now.ToString("HH:mm:ss") + "\t\t" + ex.Message + "\t\t发送邮件失败");
            }
        }
    }
}
