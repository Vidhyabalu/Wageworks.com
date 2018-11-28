using Sitecore.Configuration;
using System;
using System.Configuration;
using System.Net.Mail;

namespace Wageworks.Foundation.SitecoreExtensions.Helpers
{
    public class SendGridEmailHelper
    {

        public static void SendEmail(string to, string from, string subject, string message)
        {
            string SendGridApiKey = Settings.GetSetting("sendgrid.ApiKey");
            if (string.IsNullOrEmpty(SendGridApiKey))
                throw new ConfigurationErrorsException("'sendgrid.ApiKey' setting value must be configured. Check configuration.");

            using (var mail = new MailMessage(from, to))
            {
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("apikey", SendGridApiKey);
                smtpClient.Credentials = credentials;

                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    // TODO: Log error
                    //Log.Error(ex.ToString(), ex);
                    var i = ex;
                }
            }
        }
    }
}