using OpenShock.SDK.CSharp.Models;
using Tomlyn;

namespace RandomShock
{
    internal class Config
    {
        private static readonly Random _random = new();
        public string ApiToken { get; set; }

        public int CheckInterval { get; set; }

        public Dictionary<string, ShockerConfig> Shocker { get; set; }
        public Dictionary<string, ActionDetails> Action { get; set; }

        public class ShockerConfig
        {
            public string Uuid { get; set; }
        }

        public class ActionDetails
        {
            public ControlType Type { get; set; }
            public byte Intensity { get; set; }
            public float Duration { get; set; }
            public double Weight { get; set; }

            public string[]? Shockers { get; set; }
        }

        private double totalWeight;

        public static Config Load(string filePath)
        {
            var toml = File.ReadAllText(filePath);
            var model = Toml.ToModel<Config>(toml);

            model.ApiToken = Environment.GetEnvironmentVariable("OPENSHOCK_TOKEN") ?? model.ApiToken;

            if (model.Shocker.Count == 0) throw new Exception("No shockers provided!");

            if (model.Action.Count == 0) throw new Exception("No actions provided!");

            model.totalWeight = model.Action.Values.Sum(a => a.Weight);

            return model;
        }

        public ControlRequest CreateRandomControlRequest()
        {
            var randomWeight = _random.NextDouble() * totalWeight;

            ActionDetails? selectedAction = null;
            var cumulativeWeight = 0.0;
            foreach (var action in Action.Values)
            {
                cumulativeWeight += action.Weight;
                if (randomWeight <= cumulativeWeight)
                {
                    selectedAction = action;
                    break;
                }
            }

            var shockers = selectedAction.Shockers?
                .Select(shocker => Shocker.GetValueOrDefault(shocker))
                .Where(shocker => shocker != null)
                .ToArray();

            if (shockers == null || !shockers.Any())
            {
                var shockerIndex = _random.Next(Shocker.Count);
                shockers = [Shocker.Values.ElementAt(shockerIndex)];
            }

            return new ControlRequest
            {
                Shocks = shockers.Select(shocker =>
                    new Control
                    {
                        Id = Guid.Parse(shocker.Uuid),
                        Type = selectedAction.Type,
                        Intensity = selectedAction.Intensity,
                        Duration = (ushort)(selectedAction.Duration * 1000)
                    }),
                CustomName = $"DeathDetect - Shock"
            };
        }
    }
}
