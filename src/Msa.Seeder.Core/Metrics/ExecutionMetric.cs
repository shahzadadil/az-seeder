namespace Msa.Seeder.Core.Metrics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Msa.Core.Extensions;
    using Msa.Core.Infra;

    public class ExecutionMetric : IList<StepMetric>
    {
        private IList<StepMetric> _StepMetrics = new List<StepMetric>();

        public ExecutionMetric()
        {}

        public Boolean IsExecutionSuccessful { get; private set; }

        public bool IsFixedSize => false;

        public bool IsReadOnly => false;

        public int Count => this._StepMetrics.Count;

        public bool IsSynchronized => false;

        public object SyncRoot => null;

        public StepMetric this[Int32 index] 
        { 
            get => this._StepMetrics.ElementAtOrDefault(0); 
            set => this._StepMetrics.Add(value); 
        }

        public IEnumerator GetEnumerator()
        {
            return this._StepMetrics.GetEnumerator();
        }

        public ExecutionMetric Success()
        {
            this.IsExecutionSuccessful = true;
            return this;
        }

        public ExecutionMetric Failure()
        {
            this.IsExecutionSuccessful = false;
            return this;
        }

        public int Add(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }            

            this._StepMetrics.Add((StepMetric)value);
            return 1;
        }

        public void Clear()
        {
            this._StepMetrics.Clear();
        }

        public bool Contains(object value)
        {
            return this._StepMetrics.Contains((StepMetric)value);
        }

        public int IndexOf(object value)
        {
            return this._StepMetrics.IndexOf((StepMetric)value);
        }

        public void Insert(Int32 index, object value)
        {
            this._StepMetrics.Insert(index, (StepMetric)value);
        }

        public void Remove(object value)
        {
            this._StepMetrics.Remove((StepMetric)value);
        }

        public void RemoveAt(Int32 index)
        {
            this._StepMetrics.RemoveAt(index);
        }

        public void CopyTo(Array array, Int32 index)
        {
            this._StepMetrics.CopyTo((StepMetric[])array, index);
        }

        public Int32 IndexOf(StepMetric item)
        {
            return this._StepMetrics.IndexOf(item);
        }

        public void Insert(Int32 index, StepMetric item)
        {
            this._StepMetrics.Insert(index, item);
        }

        public void Add(StepMetric item)
        {
            this._StepMetrics.Add(item);
        }

        public bool Contains(StepMetric item)
        {
            return this._StepMetrics.Contains(item);
        }

        public void CopyTo(StepMetric[] array, Int32 arrayIndex)
        {
            this._StepMetrics.CopyTo(array, arrayIndex);
        }

        public bool Remove(StepMetric item)
        {
            return this._StepMetrics.Remove(item);
        }

        IEnumerator<StepMetric> IEnumerable<StepMetric>.GetEnumerator()
        {
            return this._StepMetrics.GetEnumerator();
        }
        

        public override String ToString()
        {
            var logBuilder = new StringBuilder();

            if (!this._StepMetrics.Any())
            {
                return "No Steps executed";
            }

            var executionStart = this._StepMetrics.First().Duration.Start;
            var executionEnd = this._StepMetrics.Last().Duration.End;

            var executionSpan = new DateRange(executionStart, executionEnd);

            logBuilder.Append($"Total Execution time: {executionSpan.Span.TotalSeconds}s{Environment.NewLine}");

            logBuilder.Append($"Execution started at {executionStart.UtcTimestampString()}{Environment.NewLine}");

            foreach (var step in this._StepMetrics)
            {
                logBuilder.Append($"{step}{Environment.NewLine}");
            }

            logBuilder.Append($"Execution ended at {executionEnd.UtcTimestampString()}{Environment.NewLine}");

            return logBuilder.ToString();
        }
    }
}