namespace KafkaTest.Api
{
    public class BackgroundTaskManager
    {
        private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public static CancellationToken GetToken() => _cancellationTokenSource.Token;

        public static void CancelAllTasks()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
}
