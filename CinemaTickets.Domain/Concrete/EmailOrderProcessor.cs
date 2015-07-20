using CinemaTickets.Domain.Abstract;
using CinemaTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Aspose.BarCode;
using System.IO;

namespace CinemaTickets.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "cinematicketstaket@gmail.com";//"sportsstore@example.com";
        public bool UseSsl = true;
        public string Username = "cinematicketstaket";
        public string Password = "cinematickets_nantaket";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"c:\sports_store_emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Order order, CustomerDetails shippingInfo)
        {

            using (var smtpClient = new SmtpClient())
            {

                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                    = new NetworkCredential(emailSettings.Username,
                          emailSettings.Password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                //My edit
                emailSettings.WriteAsFile = false;
                emailSettings.MailToAddress = shippingInfo.Email;

                //if (emailSettings.WriteAsFile)
                //{
                //    smtpClient.DeliveryMethod
                //        = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                //    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                //    smtpClient.EnableSsl = false;
                //}

                StringBuilder body = new StringBuilder()
                    .AppendLine("Ваше замовлення оброблено")
                    .AppendLine("---")
                    .AppendLine("В прикріпленому файлі знаходиться квиток з усією необхідною інформацією");

                BarCodeBuilder bb = new BarCodeBuilder();

                //Set the Code text for the barcode
                DateTime date = order.OrderedTickets.FirstOrDefault().Seance.Date;
                TimeSpan time = order.OrderedTickets.FirstOrDefault().Seance.Time;
                string seance_date = String.Format("{0}{1}{2}{3}", date.Day, date.Month, time.Hours, time.Minutes);//= "1234567";
                string movie_id = order.OrderedTickets.FirstOrDefault().Seance.MovieId.ToString();
                string row = order.OrderedTickets.FirstOrDefault().Row.ToString();
                string seat = order.OrderedTickets.FirstOrDefault().Seat.ToString();
                string movie = order.OrderedTickets.FirstOrDefault().Movie;
                string hall = order.OrderedTickets.FirstOrDefault().Hall;
                string cinema = order.OrderedTickets.FirstOrDefault().Cinema;
                bb.CodeText = seance_date + movie_id + row + seat;
                //Set the symbology type to Code39Standard
                bb.SymbologyType = Symbology.Code128;

                //Set the width of the bars to 1 millimeter
                bb.xDimension = 1f;

                //Save the image to your system and set its image format to Jpeg
                bb.Save(shippingInfo.SavePath + "barcode.png", BarCodeImageFormat.Png);


                var pdf_doc = new Document(new Rectangle(0, 0, 500, 300));

                PdfWriter.GetInstance(pdf_doc, new FileStream(shippingInfo.SavePath + "ticket.pdf", FileMode.Create));
                pdf_doc.Open();
                //BaseFont baseFont = BaseFont.CreateFont(/*"C:\\Windows\\Fonts\\arial.ttf"*/BaseFont.TIMES_ROMAN, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                string ARIALUNI_TFF = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");

                //Create a base font object making sure to specify IDENTITY-H
                BaseFont bf = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                //Create a specific font object
                Font f = new Font(bf, 12, Font.NORMAL);

                pdf_doc.Add(new Paragraph("Дата: " + date.ToShortDateString() + ",x` Час: " + time.Hours + ":" + time.Minutes, f));
                pdf_doc.Add(new Paragraph("Фільм: " + movie + ", Кінотеатр: " + cinema + ", Зал: " + hall, f));
                pdf_doc.Add(new Paragraph("Ряд: " + row + " Місце: " + seat, f));
                 Image gif = Image.GetInstance(shippingInfo.SavePath + "barcode.png");
                gif.ScalePercent(24f);
                pdf_doc.Add(gif);
                pdf_doc.Close();
                //foreach (var line in order.OrderedTickets)
                //{
                  
                //    body.AppendFormat("Time: {0}\nDate: {1}\nMovie: {2}\n Price: {3:c}\nRow: {4} Seat:{5}", 
                //        line.Seance.Time, line.Seance.Date, line.Seance.Movie.Name, line.Seance.Price, line.Row, line.Seat);
                //}
                

                //body.AppendFormat("Total order value: {0:c}", order.ComputeTotalValue())
                //    .AppendLine("---")
                //    .AppendLine("Ship to:")
                //    .AppendLine(shippingInfo.Email)
                //    .AppendLine("---");

                MailMessage mailMessage = new MailMessage(
                                       emailSettings.MailFromAddress,   // From
                                       emailSettings.MailToAddress,     // To
                                       "Замовлення здійснено!",          // Subject
                                       body.ToString());                // Body

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(shippingInfo.SavePath + "ticket.pdf");
                //if (emailSettings.WriteAsFile)
                //{
                //    mailMessage.BodyEncoding = Encoding.ASCII;
                //}
                mailMessage.Attachments.Add(attachment);
                smtpClient.Send(mailMessage);
            }
        }
    }
}
