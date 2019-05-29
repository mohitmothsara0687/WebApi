using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExerciseAPI.Models
{
    public class FlattenedRecord
    {
        public int id { get; set; }
        public int? age { get; set; }
        public string work_class { get; set; }
        public string education_level { get; set; }
        public int? education_num { get; set; }
        public string marital_status { get; set; }
        public string occupation { get; set; }
        public string relationship { get; set; }
        public string race { get; set; }
        public string sex { get; set; }
        public int capital_gain { get; set; }
        public int capital_loss { get; set; }
        public int hours_week { get; set; }
        public string country_name { get; set; }
        public int over_50k { get; set; }

        public static FlattenedRecord FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            FlattenedRecord record = new FlattenedRecord();
            record.id = Convert.ToInt32(values[0]);
            record.age = validateValue(values[1]);
            record.work_class = values[2];
            record.education_level = (values[3]);
            record.education_num = validateValue(values[4]);
            record.marital_status = values[5];
            record.occupation = values[6];
            record.relationship = values[7];
            record.race = values[8];
            record.sex = values[9];
            record.capital_gain = Convert.ToInt32(values[10]);
            record.capital_loss = Convert.ToInt32(values[11]);
            record.hours_week = Convert.ToInt32(values[12]);
            record.country_name = values[13];
            record.over_50k = Convert.ToInt32(Convert.ToBoolean(values[14]));
            return record;
        }

        public static int? validateValue(string value)
        {
            if (value == "?")
                return null;
            return Convert.ToInt32(value);
        }
    }
}