using MimeKit;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Net.Mail;

using System.Text;

using System.Threading.Tasks;

using VertexHRMS.BLL.Service.Abstraction;

using VertexHRMS.DAL.Entities;

using MailKit.Net.Smtp;

using MimeKit;

namespace VertexHRMS.BLL.Service.Implementation

{

    public class PayrollEmailService : IPayrollEmailService

    {

        private readonly string _email = "vertexhrms@gmail.com";

        private readonly string _password = "szluvvxqsywatvst";

        public async Task SendPayrollEmailAsync(Employee employee, Payroll payroll)

        {

            if (string.IsNullOrEmpty(employee.Email))

                return;

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("HR System", _email));

            message.To.Add(new MailboxAddress(employee.FirstName + " " + employee.LastName, employee.Email));

            message.Subject = $"Payroll for period {payroll.PaymentDate:MMMM yyyy}";

            string body = $@"

                Hi {employee.FirstName + " " + employee.LastName},
 
                Your payroll has been processed:
 
                Base Salary: {payroll.BaseSalary:C}

                Gross Earnings: {payroll.GrossEarnings:C}

                Net Salary: {payroll.NetSalary:C}

                Payment Date: {payroll.PaymentDate:dd/MM/yyyy}
 
                Regards,

                HR Department

            ";

            message.Body = new TextPart("plain") { Text = body };

            using var client = new MailKit.Net.Smtp.SmtpClient();

            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(_email, _password);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);

        }

        public async Task SendPayrollRunEmailsAsync(PayrollRun run)

        {

            foreach (var payroll in run.Payrolls)

            {

                await SendPayrollEmailAsync(payroll.Employee, payroll);

            }

        }

    }

}

