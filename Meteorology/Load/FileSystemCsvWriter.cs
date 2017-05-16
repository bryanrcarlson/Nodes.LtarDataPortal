using CsvHelper;
using Nsar.Nodes.LtarDataPortal.Meteorology.Core;
using Nsar.Nodes.LtarDataPortal.Meteorology.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nsar.Nodes.LtarDataPortal.Meteorology.Load
{
    public class FileSystemCsvWriter : ILoadMeteorology
    {
        private readonly string folderPath;
        private ICsvWriter writer;

        public FileSystemCsvWriter(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new ArgumentException("Folder path was not found");
            }

            this.folderPath = folderPath;
        }

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
