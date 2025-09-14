using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;
using VertexHRMS.DAL.Repo.Abstraction;

namespace VertexHRMS.DAL.Repo.Implementation
{
    public class AIRepo : IAIRepo
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey;
        public AIRepo(IConfiguration config, HttpClient httpClient)
        {
            this.httpClient = httpClient;
            apiKey = config["Gemini:ApiKey"];
        }
        public async Task<string> AskAsync(string prompt)
        {
            var context = @"
            You are a friendly AI assistant that helps explain VERTEX's HRMS (Human Resource Management System) project. 
            Your job is to answer questions about the project in a clear but friendly way. 
            Keep your tone professional but approachable (like a helpful colleague), use alittle emojis.  

            The HRMS project includes the following key features:
            - Employee management (update, remove employee profiles, search employee name)
            - Attendance tracking (with late, overtime, leave management)
            - Payroll management (salary calculation, allowances, deductions)
            - Face recognition login (AI-powered attendance system)
            - Role-based access control (HR, Employee dashboards)
            - Multi-language support (English and Arabic)
            - Performance evaluation and reporting
            - Integration with AI assistant (for Q&A and support, Gemini Key)
            - Login and sign-up (Authentication, Authorization, signup if employee require 2 verifications(employee, hr)gmails, if HR require just a known key input fpr HRs and gmail verfiy)
            - Recruitment and onboarding modules
            - User-friendly UI with responsive design
            - About Us page
            - My Profile page(view and edit personal info)
            - Home page(Departments cards, search department and filter)
            - Calendar integration (view interveiws date, holidays)

            Rules:
            - Always format your answers as:
                Q: <user’s question>
                A: <your answer in a friendly tone>
            - If the user asks about something not related to the HRMS project, respond:
                'Sorry 😊 I can only answer questions about the HRMS project.'
            ";

            var requestBody = new
            {
                contents = new[]
                {
                    new {
                        parts = new[] { new { text = context + "\n\nUser Question: " + prompt } }
                    }
                }
            };

            using var request = new HttpRequestMessage(
        HttpMethod.Post,
        $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}"
            )
            {
                Content = JsonContent.Create(requestBody)
            };

            var response = await httpClient.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Gemini Response: " + responseContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini API Error ({response.StatusCode}): {responseContent}");
            }

            var result = JsonDocument.Parse(responseContent);

            return result.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();
        }

    }
}
