namespace Msa.Seeder.Core.Metrics
{
    using System;
    using Msa.Core.Infra;

    public class StepMetric
    {
        public StepMetric(
            IStep step,            
            DateRange duration,
            Outcome outcome = null)
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            if (!duration.HasStartAndEnd())
            {
                throw new ArgumentNullException(nameof(duration));
            }

            this.StepName = step.StepName;
            this.Order = step.Order;
            this.Duration = duration;
            this.Outcome = outcome ?? Outcome.Succcess();
        }

        public String StepName { get; private set; }
        public Int32 Order { get; private set; }
        public DateRange Duration { get; private set; }
        public Outcome Outcome { get; private set; }

        public override String ToString()
        {
            return $"Step '{this.Order}:{this.StepName}' ran '{this.Duration}' with '{this.Outcome}'";
        } 
    }
}