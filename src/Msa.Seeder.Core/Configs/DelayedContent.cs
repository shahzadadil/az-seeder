namespace Msa.Seeder.Core.Configs
{
    using System;

    public class DelayedContent<TContent>
    {
        public DelayedContent(TContent content, TimeSpan? delay = null)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            this.Delay = delay ?? TimeSpan.FromSeconds(0);  // ?? doesnt work
            this.Content = content;
        }

        public TimeSpan Delay { get; private set; }
        public TContent Content { get; private set; }
    }
}