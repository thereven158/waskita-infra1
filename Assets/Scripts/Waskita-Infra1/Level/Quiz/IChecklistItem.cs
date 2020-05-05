using System;
using A3.Quiz;

namespace Agate.WaskitaInfra1.Level
{
    public interface IChecklistItem
    {
        string Category { get; }
        IQuiz Quiz { get; }
    }
}