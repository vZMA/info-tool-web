using ZoaReference.Features.DialCodes.Models;

namespace ZoaReference.Features.DialCodes.Repositories;

public class DialCodeRepository
{
    private IReadOnlyList<DialCodeEntry> _entries = [];

    public IReadOnlyList<DialCodeEntry> AllEntries => _entries;

    public void ReplaceAll(IReadOnlyList<DialCodeEntry> entries) => _entries = entries;
}
