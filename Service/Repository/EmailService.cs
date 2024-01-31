using Data;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Model;
using Model.DTO;
using Service.Repository.IRepository;

namespace Service.Repository
{
	public class EmailService : IEmail
    {
        private readonly PostDBContext _dbContext;
        public EmailService(PostDBContext context) 
        { 
            _dbContext = context;
        }
        public async Task<string> SendEmailAsync(UserRequestDto? request, EmailToUser? emailToUser)
        {
            string createdTemplate = await GenerateEmailTemplate(request,emailToUser,null);
            if (createdTemplate == "")
            {
                return "template values was not replaced";
            }

            var message = CreateMessage(emailToUser);
            var bodyBuilder = new BodyBuilder();
            if(emailToUser is null && request is not null) 
            {
			     bodyBuilder = await GetBodyBuilder(createdTemplate, request.CvFile);

				if (bodyBuilder == null)
				{
					return "cv must be in pdf format";
				}
				
			}
            else
            {
                bodyBuilder.HtmlBody = createdTemplate;
            }
			string sendMessage = await SendMail(message, bodyBuilder);
			

            return sendMessage;
        }
		private async Task<string> SendMail(MimeMessage message,BodyBuilder bodyBuilder)
		{
			message.Body = bodyBuilder.ToMessageBody();
			var smtp = new SmtpClient();
			await smtp.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
			await smtp.AuthenticateAsync("hirushop123@gmail.com", "cmilbqwvudunnmbw");
			await smtp.SendAsync(message);
			await smtp.DisconnectAsync(true);
			return "Email is Send";
		}
		public async Task<string> GetSubscribersEmailAsync(PostNotificationDto? data, List<string?> subscriberEmail)
		{
			string createdTemplate = await GenerateEmailTemplate(null, null, data);
			if (createdTemplate == "")
			{
				return "template values was not replaced";
			}
			var messages = CreateMessageToSubscribers(subscriberEmail);
			var bodyBuilder = new BodyBuilder();
			bodyBuilder.HtmlBody = createdTemplate;
			var notificationMessage =  await SendMailToSub(messages, bodyBuilder);
			return notificationMessage;
		}
		private async Task<string?> GenerateEmailTemplate(UserRequestDto? request, EmailToUser? emailToUser, PostNotificationDto? subscriberData)
        {
            string emailTemplate;
			if (subscriberData is not null)
			{
				try
				{
					string filePath = Directory.GetCurrentDirectory() + "\\Template\\SubscriberEmail.html";
					emailTemplate = await File.ReadAllTextAsync(filePath);
				}
				catch (Exception ex)
				{
					return "";
				}
				emailTemplate = emailTemplate.Replace("{thumbnail}", subscriberData.Thumbnail);
				emailTemplate = emailTemplate.Replace("{title}", subscriberData.Title);
				return emailTemplate;
			}
			if (request is not null)
            {
				try
				{
					string filePath = Directory.GetCurrentDirectory() + "\\Template\\EmailTemplate.html";
					emailTemplate = await File.ReadAllTextAsync(filePath);
				}
				catch (Exception ex)
				{
					return "";
				}
				emailTemplate = emailTemplate.Replace("{Date}", DateTime.Now.ToString("d"));
				emailTemplate = emailTemplate.Replace("{bio}", request.Bio);
				emailTemplate = emailTemplate.Replace("{email}", request.Email);
				emailTemplate = emailTemplate.Replace("{firstName}", request.FirstName);
				emailTemplate = emailTemplate.Replace("{lastName}", request.LastName);
				return emailTemplate;
			}
             if(emailToUser is not null) 
            {
				try
				{
					string filePath = Directory.GetCurrentDirectory() + "\\Template\\UserWelcome.html";
                    emailTemplate = await File.ReadAllTextAsync(filePath);
				}
				catch (Exception ex)
				{
					return "";
				}
				emailTemplate = emailTemplate.Replace("{firstName}", emailToUser.FirstName);
                emailTemplate = emailTemplate.Replace("{lastName}",emailToUser.LastName);
				emailTemplate = emailTemplate.Replace("{email}", emailToUser.Email);
				emailTemplate = emailTemplate.Replace("{password}", emailToUser.Password);

				return emailTemplate;
			}
           
            return "";
          
        }
        private async Task<BodyBuilder?> GetBodyBuilder(string createdTemplate, IFormFile cvFile)
        {
            var extention = Path.GetExtension(cvFile.FileName);
            if (extention == ".pdf")
            {
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = createdTemplate;
                using (var stream = new MemoryStream())
                {
                    await cvFile.CopyToAsync(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    await bodyBuilder.Attachments.AddAsync("CV.pdf", stream);
                }
                return bodyBuilder;
            }
            return null;
        }
        private MimeMessage CreateMessage(EmailToUser? emailToUser)
        {
			var message = new MimeMessage();
			if (emailToUser == null)
            {
				message.From.Add(new MailboxAddress("", "hirushop123@gmail.com"));
				message.To.Add(new MailboxAddress("Admin", "hirunprabodhya@gmail.com"));
				message.Subject = "Writer Request Application";
				return message;
			}
			else
			{
                message.From.Add(new MailboxAddress("", "hirushop123@gmail.com"));
                message.To.Add(new MailboxAddress("user",emailToUser.Email));
                message.Subject = "Welcome to Born to Blog - Your Journey as a Writer Begins!";
                return message;

			}
            
        }
		private List<MimeMessage> CreateMessageToSubscribers(List<string?> subscriberEmail)
		{
			var messageList = new List<MimeMessage>();
			foreach (var subscriberEmailItem in subscriberEmail)
			{
				var message = new MimeMessage();
				message.From.Add(new MailboxAddress("", "hirushop123@gmail.com"));
				message.To.Add(new MailboxAddress("user", subscriberEmailItem));
				message.Subject = "Check this out";
				messageList.Add(message);
			}
			return messageList;
		}

		private async Task<string> SendMailToSub(List<MimeMessage> messages,BodyBuilder bodyBuilder)
		{
			string notificationConfirm = "";
			foreach (var message in messages)
			{
				notificationConfirm = await SendMail(message, bodyBuilder);
			}
			return "send notification";
		}

	}
}
