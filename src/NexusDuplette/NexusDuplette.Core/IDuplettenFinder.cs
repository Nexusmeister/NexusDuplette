namespace NexusDuplette.Core;

public interface IDuplettenFinder
{
    bool StartDuplettenMatching(IEnumerable<string> pathsToAnalyze);
}