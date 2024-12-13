namespace Recsite_Ats.Infrastructure.Services.BackGroundService;
//public class EmailBackgroundService : BackgroundService, IBackgroundEmailService
//{
//    private readonly IEmailSender _emailService;
//    private readonly ConcurrentQueue<(string ToEmail, string Subject, string Body)> _emailQueue = new();

//    public EmailBackgroundService(IEmailSender emailService)
//    {
//        _emailService = emailService;
//    }

//    public void QueueEmail(string toEmail, string subject, string body)
//    {
//        _emailQueue.Enqueue((toEmail, subject, body));
//    }

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        while (!stoppingToken.IsCancellationRequested)
//        {
//            if (_emailQueue.TryDequeue(out var email))
//            {
//                await _emailService.SendMessage(email.ToEmail, email.Subject, email.Body);
//            }
//            await Task.Delay(1000, stoppingToken); // Avoid tight loop
//        }
//    }
//}
