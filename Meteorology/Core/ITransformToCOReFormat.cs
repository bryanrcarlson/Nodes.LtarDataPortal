using Nsar.Nodes.LtarDataPortal.Meteorology.Dto;
using System.Collections.Generic;

namespace Nsar.Nodes.LtarDataPortal.Meteorology.Core
{
    interface ITransformToCOReDataRecord<T> where T : class
    {
        List<COReDataRecord> Transform(
            string ltarSiteAcronym,
            string stationId,
            string recordType,
            int utcHourOffset,
            List<T> data);
    }
}