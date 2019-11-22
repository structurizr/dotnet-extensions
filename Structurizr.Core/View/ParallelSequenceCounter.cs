namespace Structurizr
{

    internal class ParallelSequenceCounter : SequenceCounter
    {

        internal ParallelSequenceCounter(SequenceCounter parent) : base(parent)
        {
            Sequence = Parent.Sequence;
        }

    }

}