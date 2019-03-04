using System.Collections.Generic;
using Web.Dto;

namespace Web.Log
{
    public interface ILogViewRepository
    {
        double? Total();
        int CountOnLevel(string value);  

        LogAggregatedValueDto AggregatedValuesForAll();

        Dictionary<string, int> GetErrorSet();
    }
}