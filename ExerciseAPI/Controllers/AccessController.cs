using ExerciseAPI.Models;
using ExerciseAPI.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ExerciseAPI.Controllers
{
    [Route("api/Access")]
    public class AccessController : ApiController
    {
        [Route("Get")]
        public List<FlattenedRecord> Get()
        {
            //Exception ex = null;
            //throw ex;
            string CSVFilePath = "~/resources/exercise01.csv";
            string FilePointer = System.Web.Hosting.HostingEnvironment.MapPath(CSVFilePath);
            List<FlattenedRecord> recordSet = File.ReadAllLines(FilePointer)
                    .Skip(1)
                    .Select(v => FlattenedRecord.FromCsv(v))
                    .ToList();
            return recordSet;
        }
        [Route("GetRecords")]
        [HttpGet]
        public List<FlattenedRecord> Get([FromUri]PagingParameterModel pagingparametermodel)
        {

            string CSVFilePath = "~/resources/exercise01.csv";
            string FilePointer = System.Web.Hosting.HostingEnvironment.MapPath(CSVFilePath);
            List<FlattenedRecord> recordSet = File.ReadAllLines(FilePointer)
                    .Skip(1)
                    .Select(v => FlattenedRecord.FromCsv(v))
                    .ToList();

            int TotalCount = recordSet.Count;

            int CurrentPage = pagingparametermodel.pageNumber;

            int PageSize = pagingparametermodel.pageSize;

            int TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            var items = recordSet.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage
            };

            HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

            return items;
        }

        [Route("PrepareCSV")]
        [HttpGet]
        public string PrepareCSV()
        {
            string BasePath = System.Web.Hosting.HostingEnvironment.MapPath("~/resources/");
            string DBFilePath = "exercise01.sqlite";
            DataTable dt = new DataTable();
            string query = File.ReadAllText(BasePath + "query.txt");
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={BasePath + DBFilePath}"))
            {
                try
                {
                    connection.Open();
                    SQLiteCommand cmd = connection.CreateCommand();
                    SQLiteDataAdapter adapter;
                    cmd.CommandText = query;
                    adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                catch (SQLiteException)
                {
                    //Add your exception code here.
                }
                connection.Close();
            }

            var fileContent = dt.ToCSV(',');
            //Exception ex = null;
            //throw ex;
            string CSVFilePath = "exercise01.csv";
            File.WriteAllText(BasePath + CSVFilePath, fileContent);
            string FilePointer = BasePath + CSVFilePath;
            return "All Good!";
        }
        
    }

}
