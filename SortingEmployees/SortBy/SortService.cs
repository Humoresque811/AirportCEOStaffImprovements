using AirportCEOStaffImprovements.SortingEmployees.Models;
using System.Collections.Generic;

namespace AirportCEOStaffImprovements.SortingEmployees.SortBy;

internal static class SortService
{

    private static readonly Dictionary<SortByEnum, ISortBy> _sorters = new Dictionary<SortByEnum, ISortBy>();

    public static void Initialize()
    {
        // Manually register each class that implements ISortBy
        Register(new SortBySkill());
        Register(new SortBySalary());
    }

    private static void Register(ISortBy sorter)
    {
        if (sorter.Type == SortByEnum.Default)
        {
            AirportCEOStaffImprovements.SILogger.LogWarning("Don't register SortByEnum.Name this is the default behaviour");
            return;
        }

        if (!_sorters.ContainsKey(sorter.Type))
        {
            _sorters.Add(sorter.Type, sorter);
            return;
        }

        AirportCEOStaffImprovements.SILogger.LogWarning($"Can't register type: {sorter.Type}, because already exist");
    }

    public static ISortBy GetSortingStrategy(SortByEnum type)
    {
        if (_sorters.TryGetValue(type, out var sorter))
        {
            return sorter;
        }
        // Handle the case where the requested type is not found.
        throw new KeyNotFoundException($"No sorter found for type: {type}");
    }

}
