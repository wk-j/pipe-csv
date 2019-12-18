using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace Pipe {

    public static class DictionaryCsvExtentions {
        public static dynamic BuildCsvObject(this Dictionary<string, object> document) {
            dynamic csvObj = new ExpandoObject();

            foreach (var p in document) {
                AddProperty(csvObj, p.Key, p.Value);
            }

            return csvObj;
        }

        private static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue) {
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName)) {
                expandoDict[propertyName] = propertyValue;
            } else {
                expandoDict.Add(propertyName, propertyValue);
            }
        }
    }

    class Student {
        public int Id { set; get; }
        public string Name { set; get; }
    }

    class Program {

        static void Records() {
            var records = new Student[] {
                new Student {
                    Id = 1,
                    Name = "wk1"
                },
                new Student {
                    Id = 2,
                    Name = "wk2"
                }
            };

            var config = new Configuration {
                Delimiter = "|"
            };

            using (var writer = new StreamWriter(".temp/students.csv"))
            using (var csv = new CsvWriter(writer, config)) {
                csv.WriteRecords(records);
            }
        }

        static void Dict() {
            var dict = new Dictionary<string, object> {
                {"A", "1'xy"},
                {"B", @"2"""},
                {"C", "4"}
            };

            var records = new List<dynamic> {
                dict.BuildCsvObject()
            };

            var config = new Configuration {
                Delimiter = "|"
            };

            using (var writer = new StreamWriter(".temp/dict.csv"))
            using (var csv = new CsvWriter(writer, config)) {
                csv.WriteRecords(records);
            }
        }

        static void Main(string[] args) {
            Dict();
        }
    }
}
