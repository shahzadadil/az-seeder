namespace Msa.Core.Infra
{
    using System;
    using Msa.Core.Extensions;

    public class DateRange
    {
        public DateRange(
            DateTime? start,
            DateTime? end)
        {
            if (start.HasValue && !start.Value.IsInUtc())
            {
                throw new InvalidTimeZoneException(nameof(start));
            }

            if (end.HasValue && !end.Value.IsInUtc())
            {
                throw new InvalidTimeZoneException(nameof(end));
            }

            this.Start = start;
            this.End = end;
        }

        public DateTime? Start { get; private set; }
        public DateTime? End { get; private set; }

        public TimeSpan Span => this.HasStartAndEnd() ? this.End.Value.Subtract(this.Start.Value) : TimeSpan.MaxValue;

        public Boolean HasStartAndEnd()
        {
            return this.Start.HasValue && End.HasValue;
        }

        public Double TotalSeconds()
        {
            if (!this.HasStartAndEnd())
            {
                throw new InvalidOperationException("Either start or end date time is missing");
            }

            return this.Span.TotalSeconds;
        }

        public override String ToString()
        {
            return $"{this.Start.UtcTimestampString()} to {this.End.UtcTimestampString()} for {this.TotalSeconds()}s";
        }
    }
}