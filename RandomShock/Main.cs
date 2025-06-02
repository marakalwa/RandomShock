using NLog;
using OpenShock.SDK.CSharp;
using System.Diagnostics;

namespace RandomShock
{
    /// <summary>
    /// Provides an easy way to configure and send random shocks using the OpenShock API.
    /// </summary>
    public class RandomShock
    {
        private bool detection = false;

        /// <summary>
        /// Starts the Random Shock service, which continuously checks for detection and sends control requests.
        /// </summary>
        public async Task Start()
        {
            var logger = LogManager.GetCurrentClassLogger();

            var config = Config.Load("config.toml");

            var client = new OpenShockApiClient(new ApiClientOptions
            {
                Token = config.ApiToken
            });

            const int period = 500;
            var lastDetection = false;

            logger.Info("Random Shock initialized");
            while (true)
            {
                var stopwatch = Stopwatch.StartNew();

                if (detection)
                {
                    logger.Info("Positive detected, sending control request");
                    var controlRequest = config.CreateRandomControlRequest();
                    await client.ControlShocker(controlRequest)
                        .ContinueWith(task => logger.Info($"Control request status: {task.Result}"));
                }

                lastDetection = detection;

                stopwatch.Stop();
                var elapsed = stopwatch.ElapsedMilliseconds;
                var delay = Math.Max(0, period - elapsed);
                Thread.Sleep((int)delay);
            }
        }
        /// <summary>
        /// Sets the detection state for the Random Shock service.
        /// </summary>
        /// <param name="value">Wether a detection was made</param>
        public void SetDetection(bool value)
        {
            detection = value;
        }
    }
}
