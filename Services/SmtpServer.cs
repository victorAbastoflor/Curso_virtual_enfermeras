using System.Net;
using System.Net.Mail;

namespace NurseCourse.Services;

public class SmtpServer
{
    public bool SendEmail(string toEmail, MemoryStream memoryStream)
    {
        try
        {
            using MailMessage mail = new();
            mail.From = new MailAddress("fernandezteddy04@gmail.com");
            mail.To.Add(toEmail);
            mail.Subject = "Entrega de certificado";
            mail.Body = "Felicidades por completar el curso";
            var attachment = new Attachment(memoryStream, "certificado.pdf", "application/pdf");
            mail.Attachments.Add(attachment);

            using SmtpClient smtpClient = new();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("fernandezteddy04@gmail.com", "fvdm foxa cwxq rtyl");
            smtpClient.Send(mail);
            return true;
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
}