namespace Msa.Seeder.Azure.Steps
{
    using System;
    using System.Threading.Tasks;

    public abstract class Step<TConfig> where TConfig: StepConfig
    {
        protected TConfig _StepConfig;
        public Int32 Order { get; private set; }
        public String StepName { get; private set; }

        public abstract Task Execute();

        public Step<TConfig> WithName(String stepName)
        {
            if (String.IsNullOrWhiteSpace(stepName))
            {
                throw new ArgumentNullException(nameof(stepName));
            }

            this.StepName = stepName;

            return this;
        }

        public Step<TConfig> WithOrder(Int32 order)
        {
            if (order <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(order));
            }

            this.Order = order;

            return this;
        }

        public Step<TConfig> WithConfig(TConfig stepConfig)
        {
            if (stepConfig == null)
            {
                throw new ArgumentNullException(nameof(stepConfig));
            }

            this._StepConfig = stepConfig;

            return this;
        }

        public new String ToString()
        {
            return $"Step name: {this.StepName}, Order: {this.Order}";
        }
    }
}