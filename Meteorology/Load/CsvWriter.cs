using CsvHelper;
using Nsar.Common.UnitOfMeasure;
using Nsar.Nodes.LtarDataPortal.Meteorology.Core;
using Nsar.Nodes.LtarDataPortal.Meteorology.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nsar.Nodes.LtarDataPortal.Meteorology.Load
{
    public class CsvWriter : ILoadMeteorology
    {
        //private List<COReDataRecord> data;
        private readonly string folderPath;
        private ICsvWriter writer;

        public CsvWriter(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new ArgumentException("Folder path was not found");
            }

            this.folderPath = folderPath;

            //this.writer = writer;
        }

        /// <summary>
        /// Converts a list of ITemperalMeasurements into a data table grouped by datetime
        /// <note>This function is not finished and is hardcoded</note>
        /// <todo>Accept an object that defines the DataRecord properties and maps Measurements to those properties</todo>
        /// </summary>
        /// <param name="ltarSiteAcronym">Three character acronym for the LTAR site.  Use the LtarSiteAcronymCodes class</param>
        /// <param name="stationId">Number label of the station (000, 001, etc)</param>
        /// <param name="recordType">Flag for identifying sampling methods of station.  Use the RecordTypeCodes class</param>
        /// <param name="utcHourOffset">Timezone offset from UTC for the data (accepts negatives)</param>
        /// <param name="data">List of measurements with datetime associated</param>
        /// <returns>List of created DataRecords</returns>
        //public List<COReDataRecord> CreateDataRecord(
        //    string ltarSiteAcronym,
        //    string stationId,
        //    string recordType,
        //    int utcHourOffset,
        //    List<ITemporalMeasurement> data)
        //{
        //    List<COReDataRecord> dataRecords = new List<COReDataRecord>();
        //
        //    var tests = data.GroupBy(d => d.DateTime).ToList();
        //    IEnumerable<IGrouping<DateTime, ITemporalMeasurement>> groups = data.GroupBy(d => d.DateTime);
        //
        //    foreach (IGrouping<DateTime, ITemporalMeasurement> group in groups)
        //    {
        //        // TODO: Don't hardcode!  When you have time...
        //        var airTemp = group.Single(d => d.Phenomenon.GetType() == typeof(AirTemperature)).NumericalValue;
        //        var windSp = group.Single(d => d.Phenomenon.GetType() == typeof(WindSpeed)).NumericalValue;
        //        var windDir = group.Single(d => d.Phenomenon.GetType() == typeof(WindDirection)).NumericalValue;
        //        var rH = group.Single(d => d.Phenomenon.GetType() == typeof(RelativeHumididty)).NumericalValue;
        //        var precip = group.Single(d => d.Phenomenon.GetType() == typeof(Precipitation)).NumericalValue;
        //        var dto = new DateTimeOffset(group.Key, new TimeSpan(utcHourOffset, 0, 0));
        //
        //        // TODO: Move this to a function of DataRecord so it can be overloaded in derived classes
        //        COReDataRecord dr = new COReDataRecord()
        //        {
        //            AirPressure = "",
        //            AirTemperature = airTemp.ToString("0.00"),
        //            BatteryVoltage = "",
        //            DateTime = dto.ToString("yyyy-MM-ddTHH:mm:sszzz"),
        //            LoggerTemperature = "",
        //            LongWaveIn = "",
        //            LTARSiteAcronym = ltarSiteAcronym,
        //            PAR = "",
        //            Precipitation = precip.ToString("0.00"),
        //            RecordType = recordType,
        //            RelativeHumidity = rH.ToString(),
        //            ShortWaveIn = "",
        //            StationID = stationId,
        //            WindDirection = windDir.ToString(),
        //            WindSpeed = windSp.ToString("0.00")
        //        };
        //
        //        dataRecords.Add(dr);
        //    }
        //
        //    this.data = dataRecords;
        //
        //    return this.data;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void Load(List<COReDataRecord> data)
        {
            if (!Directory.Exists(this.folderPath))
            {
                throw new ArgumentException("Dir path was not found");
            }

            if (data == null)
            {
                throw new Exception("DataRecord was not found, did you call CreateDataRecord()?");
            }

            string filePath = Path.Combine(folderPath, constructFileName(data.Last()));

            using (var file = File.CreateText(filePath))
            {
                writer = new CsvHelper.CsvWriter(file);
                writer.WriteRecords(data);
            }
        }

        private string constructFileName(COReDataRecord data)
        {
            //if (data == null)
            //{
            //    throw new Exception("DataRecord was not found, did you call CreateDataRecord()?");
            //}

            //COReDataRecord d = data.Last();

            DateTime date = DateTime.Parse(data.DateTime);

            string fileName = data.LTARSiteAcronym.ToLower() +
                "MET" +
                data.StationID +
                data.RecordType +
                "_01_" +
                date.ToString("yyyyMMdd") +
                "_00" +
                ".csv";

            return fileName;
        }
    }
}
