namespace Structurizr
{

   internal class SequenceCounter
    {

        internal readonly SequenceCounter Parent;
        internal int Sequence { get; set; }

        internal SequenceCounter()
        {
        }

        internal SequenceCounter(SequenceCounter parent)
        {
            Parent = parent;
        }

        internal virtual void Increment()
        {
            Sequence++;
        }

        public virtual string AsString()
        {
            return "" + Sequence;
        }

    }

}