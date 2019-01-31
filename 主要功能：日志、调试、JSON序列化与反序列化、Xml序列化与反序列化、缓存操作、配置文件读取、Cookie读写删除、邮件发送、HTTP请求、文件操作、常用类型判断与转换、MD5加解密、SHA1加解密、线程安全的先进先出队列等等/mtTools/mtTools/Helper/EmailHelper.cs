using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using mtTools.Models;

namespace mtTools
{
    /// <summary>
    /// 邮件发送工具类
    /// </summary>
    public static class EmailHelper
    {

        /// <summary>
        /// 获取自定义toolEmail配置
        /// </summary>
        public static CustomNodeSection EmailConfig = ConfigHelper.GetCustomNodeSetting("toolEmail");

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toEmail">要发送的邮件地址</param>
        /// <param name="Title">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="ccEmail">抄送地址</param>
        /// <param name="AttPath">附件地址(如：F:\\dir.txt)</param>
        /// <returns></returns>
        public static bool SendEmail(string toEmail, string Title, string body, string ccEmail = null, string AttPath = null)
        {
            return SendEmail(toEmail.ToList(), Title, body, ccEmail.ToList(), AttPath.ToList());
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toEmailList">要发送的邮件地址列表</param>
        /// <param name="Title">邮件标题，UTF8格式</param>
        /// <param name="body">邮件内容，UTF8格式</param>
        /// <param name="ccEmailList">抄送地址列表</param>
        /// <param name="AttPathList">附件地址列表(如：F:\\dir.txt)</param>
        /// <returns></returns>
        public static bool SendEmail(List<string> toEmailList, string Title, string body, List<string> ccEmailList = null, List<string> AttPathList = null)
        {
            try
            {
                //以下stmp服务器及用户名密码保证长期有效
                string mtEmailName = EmailConfig["EmailName"] ?? "文昌";
                string mtEmailSmtp = EmailConfig["EmailSmtp"] ?? "smtp.126.com";
                string mtEmailAddress = EmailConfig["EmailAddress"] ?? "mkwuji@126.com";
                string mtEmailPassword = EmailConfig["EmailPassword"] ?? "mtTools2017";


                //声明一个Mail对象
                MailMessage mymail = new MailMessage();
                //发件人地址：如是自己，在此输入自己的邮箱
                mymail.From = new MailAddress(mtEmailAddress, mtEmailName);
                //收件人地址
                if (toEmailList != null && toEmailList.Count > 0)
                {
                    foreach (string toEmail in toEmailList)
                    {
                        mymail.To.Add(new MailAddress(toEmail));
                    }
                }
                //邮件主题
                mymail.Subject = Title;
                //邮件标题编码
                mymail.SubjectEncoding = System.Text.Encoding.UTF8;
                //发送邮件的内容
                mymail.Body = body;
                //邮件内容编码
                mymail.BodyEncoding = System.Text.Encoding.UTF8;
                //添加附件
                if (AttPathList != null && AttPathList.Count > 0)
                {
                    foreach (string AttPath in AttPathList)
                    {
                        Attachment myfiles = new Attachment(AttPath);
                        mymail.Attachments.Add(myfiles);
                    }
                }
                //抄送到其他邮箱
                if (ccEmailList != null && ccEmailList.Count > 0)
                {
                    foreach (string ccEmail in ccEmailList)
                    {
                        mymail.CC.Add(new MailAddress(ccEmail));
                    }
                }
                //是否是HTML邮件
                mymail.IsBodyHtml = true;
                //邮件优先级
                mymail.Priority = MailPriority.High;
                //创建一个邮件服务器类
                SmtpClient myclient = new SmtpClient();
                myclient.Host = mtEmailSmtp;
                //SMTP服务端口
                myclient.Port = 25;
                //验证登录
                myclient.Credentials = new NetworkCredential(mtEmailAddress, mtEmailPassword);
                myclient.Send(mymail);

                return true;
            }
            catch (Exception ex)
            {
                ex.WriteFile("mtTools.EmailHelper");
                return false;
            }
        }

    }
}
