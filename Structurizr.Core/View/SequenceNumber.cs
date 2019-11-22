namespace Structurizr
{
    internal class SequenceNumber
    {

        private SequenceCounter _counter = new SequenceCounter();

        internal SequenceNumber()
        {
        }

        internal string GetNext()
        {
            _counter.Increment();
            return _counter.AsString();
        }

        internal void StartParallelSequence()
        {
            _counter = new ParallelSequenceCounter(_counter);
        }

        internal void EndParallelSequence(bool endAllParallelSequencesAndContinueNumbering)
        {
            if (endAllParallelSequencesAndContinueNumbering)
            {
                int sequence = _counter.Sequence;
                _counter = _counter.Parent;
                _counter.Sequence = sequence;
            }
            else
            {
                _counter = _counter.Parent;
            }
        }

    }
}