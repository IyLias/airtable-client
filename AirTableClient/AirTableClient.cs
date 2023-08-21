using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirtableApiClient;

namespace AirTable
{
    public class AirTableClient
    {
        // baseID for target base
        private const string baseID = "";

        // accessToken for my account
        private const string AccessToken = "";


        public static string getBaseID() { return baseID; }

        public static string GetAccessToken() { return AccessToken; }



        // Insert multiple records into target table
        // maximum: 10 records at one moment
        public static async void InsertTupleIntoTable(string tableName, Fields[] fields)
        {
            string offset = null;
            string errorMessage = null;
            var records = new List<AirtableRecord>();

            using (AirtableBase airtableBase = new AirtableBase(AccessToken, baseID))
            {
                //
                // Use 'offset' and 'pageSize' to specify the records that you want
                // to retrieve.
                // Only use a 'do while' loop if you want to get multiple pages
                // of records.
                //

                do
                {
                    Task<AirtableCreateUpdateReplaceMultipleRecordsResponse> task = airtableBase.CreateMultipleRecords(tableName, fields, true);

                    var response = await task;

                    if (response.Success)
                    {


                    }
                    else if (response.AirtableApiError is AirtableApiException)
                    {
                        errorMessage = response.AirtableApiError.ErrorMessage;
                        if (response.AirtableApiError is AirtableInvalidRequestException)
                        {
                            errorMessage += "\nDetailed error message: ";
                            errorMessage += response.AirtableApiError.DetailedErrorMessage;
                        }
                        Console.WriteLine(errorMessage);
                        break;
                    }
                    else
                    {
                        errorMessage = "Unknown error";
                        Console.WriteLine(errorMessage);
                        break;
                    }
                } while (offset != null);
            }

        }





        // read records from target table
        public static async void getRecords(string tableName, Fields[] fields)
        {
            string offset = null;
            string errorMessage = null;
            List<AirtableRecord> records = new List<AirtableRecord>();

            using (AirtableBase airtableBase = new AirtableBase(AccessToken, baseID))
            {
                //Use 'offset' and 'pageSize' to specify the records that you want
                // to retrieve.
                // Only use a 'do while' loop if you want to get multiple pages
                // of records.
                //

                do
                {
                    Task<AirtableListRecordsResponse> task = airtableBase.ListRecords(
                           tableName,
                           offset,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           false,
                           null);

                    AirtableListRecordsResponse response = await task;

                    if (response.Success)
                    {
                        records.AddRange(response.Records.ToList());
                        offset = response.Offset;
                    }
                    else if (response.AirtableApiError is AirtableApiException)
                    {
                        errorMessage = response.AirtableApiError.ErrorMessage;
                        if (response.AirtableApiError is AirtableInvalidRequestException)
                        {
                            errorMessage += "\nDetailed error message: ";
                            errorMessage += response.AirtableApiError.DetailedErrorMessage;
                        }
                        break;
                    }
                    else
                    {
                        errorMessage = "Unknown error";
                        break;
                    }
                } while (offset != null);
            }


        }



    }
}
