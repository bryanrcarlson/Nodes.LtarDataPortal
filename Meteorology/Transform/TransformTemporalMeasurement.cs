using Nsar.Nodes.LtarDataPortal.Meteorology.Core;
using System;
using System.Collections.Generic;
using Nsar.Nodes.LtarDataPortal.Meteorology.Dto;
using System.Linq;
using Nsar.Common.UnitOfMeasure;

namespace Nsar.Nodes.LtarDataPortal.Meteorology.Transform
{
    public class TransformTemporalMeasurement : ITransformToCOReDataRecord<ITemporalMeasurement>
    {
        /// <summary>
        /// Converts a list of ITemperalMeasurements into a DTO formated for CORe ingest and grouped by datetime
        /// <note>This function is not finished and is hardcoded</note>
        /// <todo>Accept an object that defines the DataRecord properties and maps Measurements to those properties</todo>
        /// </summary>
        /// <param name="ltarSiteAcronym">Three character acronym for the LTAR site.  Use the LtarSiteAcronymCodes class</param>
        /// <param name="stationId">Number label of the station (000, 001, etc)</param>
        /// <param name="recordType">Flag for identifying sampling methods of station.  Use the RecordTypeCodes class</param>
        /// <param name="utcHourOffset">Timezone offset from UTC for the data (accepts negatives)</param>
        /// <param name="data">List of measurements with datetime associated</param>
        /// <returns>List of created DataRecords</returns>
        public List<COReDataRecord> Transform(
            string ltarSiteAcronym,
            string stationId,
            string recordType,
            int utcHourOffset,
            List<ITemporalMeasurement> data)
        {
            List<COReDataRecord> dataRecords = new List<COReDataRecord>();

            IEnumerable<IGrouping<DateTime, ITemporalMeasurement>> groups = data.GroupBy(d => d.DateTime);

            foreach (IGrouping<DateTime, ITemporalMeasurement> group in groups)
            {
                // TODO: Don't hardcode!  When you have time...
                var airTemp = group.Single(d => d.Phenomenon.GetType() == typeof(AirTemperature)).NumericalValue;
                var windSp = group.Single(d => d.Phenomenon.GetType() == typeof(WindSpeed)).NumericalValue;
                var windDir = group.Single(d => d.Phenomenon.GetType() == typeof(WindDirection)).NumericalValue;
                var rH = group.Single(d => d.Phenomenon.GetType() == typeof(RelativeHumididty)).NumericalValue;
                var precip = group.Single(d => d.Phenomenon.GetType() == typeof(Precipitation)).NumericalValue;
                var dto = new DateTimeOffset(group.Key, new TimeSpan(utcHourOffset, 0, 0));

                COReDataRecord dr = new COReDataRecord()
                {
                    AirPressure = "",
                    AirTemperature = airTemp.ToString("0.00"),
                    BatteryVoltage = "",
                    DateTime = dto.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    LoggerTemperature = "",
                    LongWaveIn = "",
                    LTARSiteAcronym = ltarSiteAcronym,
                    PAR = "",
                    Precipitation = precip.ToString("0.00"),
                    RecordType = recordType,
                    RelativeHumidity = rH.ToString(),
                    ShortWaveIn = "",
                    StationID = stationId,
                    WindDirection = windDir.ToString(),
                    WindSpeed = windSp.ToString("0.00")
                };

                dataRecords.Add(dr);
            }

            return dataRecords;
        }
    }
}
