using NLog;

namespace NexusDuplette.Core;

public class DuplettenFinder : IDuplettenFinder
{
    private readonly ILogger _logger;
    public DuplettenFinder()
    {
        _logger = LogManager.GetCurrentClassLogger();
    }

    public bool StartDuplettenMatching(IEnumerable<string> pathsToAnalyze)
    {
        var gesamtDateiliste = new List<string>();
        _logger.Debug("Sammel alle Filenamen in Gesamtliste");
        foreach (var pfad in pathsToAnalyze)
        {
            _logger.Debug("Gehe in Pfad {path}", pfad);
            gesamtDateiliste.AddRange(GetAllFilesWithoutHierarchy(pfad));
        }

        return true;
    }

    private IEnumerable<string> GetAllFilesWithoutHierarchy(string path)
    {
        FileInfo i = new FileInfo(path);
        return null;
    }
}