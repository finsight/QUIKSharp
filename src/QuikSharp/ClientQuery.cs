using System;

namespace QuikSharp
{
    // TODO this should be a data contract
    public class ClientQuery
    {
        public ClientQuery()
        {
        }

        public ClientQuery(string expression,
                DateTime[] dates,
                bool isDataWithDates,
                bool isRange,
                bool isDataHorizontal = false)
        {
            Expression = expression;
            Dates = dates;
            IsDataWithDates = isDataWithDates;
            IsRange = isRange;
            IsDataHorizontal = isDataHorizontal;
        }

        public string Expression { get; set; }

        public DateTime[] Dates { get; set; }

        public bool IsDataWithDates { get; set; }

        /// <summary>
        /// If true, Dates must contain only two values that are Start and End dates, 
        /// Data is returned for all available dates between the start and end dates
        /// </summary>
        public bool IsRange { get; set; }


        /// <summary>
        /// Indicates that data will be returned by rows
        /// </summary>
        public bool IsDataHorizontal { get; set; }

        public string Data { get; set; }

    }
}
