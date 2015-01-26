//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
////using Google.GData.Client;
////using Google.GData.Spreadsheets;


//namespace Duty_Tjssm
//{
//    class GspConnector
//    {
//        private string USERNAME = "tjssmduty@gmail.com";
//        private string PASSWORD = "eowjsapaqjtlq";
//        private SpreadsheetsService gService;
//        SpreadsheetEntry entrySelect;
//        WorksheetEntry entryWorkSheet;
//        AtomLink listFeedLink;
//        ListQuery listQuery;

//        public GspConnector()
//        {
//            createService();
//            selectSheet();
//            createLinkQuery();
//        }


//        private void updateData(ListFeed data, string _best, string _worst)
//        {
//            ListEntry row = (ListEntry)data.Entries[0];

//            // Update the row's data.
//            foreach (ListEntry.Custom element in row.Elements)
//            {

//                if (element.LocalName == "best")
//                {
//                    element.Value = _best;
//                }
//                if (element.LocalName == "worst")
//                {
//                    element.Value = _worst;
//                }
//            }
//            row.Update();

//        }

//        private string timeToGString(DateTime _time)
//        {
//            string gStringTime = _time.ToShortDateString();
//            string yy;
//            int mm, dd;

//            yy = gStringTime.Substring(0, 4);
//            mm = int.Parse( gStringTime.Substring(5, 2));
//            dd = int.Parse(gStringTime.Substring(8, 2));

//            gStringTime = yy  + intToGString(mm)  + intToGString(dd);

//            return gStringTime;

//        }

//        private string intToGString(int _data)
//        {
//            if (_data >= 10)
//            {
//                return _data.ToString();
//            }
//            else
//            {
//                return "0" + _data.ToString();
//            }


//        }

//        public string[] loadData(DateTime dt, bool night)
//        {
//            string[] returnArray = { "", "" }; 
//            listQuery.SpreadsheetQuery = createQuery(dt, night);
//            ListFeed chkFeed = gService.Query(listQuery);
//            if (chkFeed.Entries.Count != 0)
//            {
//                ListEntry row = (ListEntry)chkFeed.Entries[0];

//                foreach (ListEntry.Custom element in row.Elements)
//                {

//                    if (element.LocalName == "best")
//                    {
//                        returnArray[0] = element.Value;
//                    }
//                    if (element.LocalName == "worst")
//                    {
//                        returnArray[1] = element.Value;
//                    }
//                }
//                return returnArray;
//            }
//            return null;
//        }


//        private string createQuery(DateTime dt, bool night)
//        {

//            return "date = " + timeToGString(dt) + " and time = " + night.ToString();
//        }

//        public void writeData(DateTime dt,bool night, string best, string worst){

//            listQuery.SpreadsheetQuery = null;
//            ListFeed listFeed = gService.Query(listQuery);

//            // TODO: Choose a row more intelligently based on your app's needs.

//            ListEntry row = new ListEntry();
//            row.Elements.Add(new ListEntry.Custom() { LocalName = "date", Value = timeToGString(dt)});
//            row.Elements.Add(new ListEntry.Custom() { LocalName = "time", Value = night.ToString()});
//            row.Elements.Add(new ListEntry.Custom() { LocalName = "best", Value = best });
//            row.Elements.Add(new ListEntry.Custom() { LocalName = "worst", Value = worst });

//            listQuery.SpreadsheetQuery = createQuery(dt, night);

//            ListFeed chkFeed = gService.Query(listQuery);

//            if (chkFeed.Entries.Count == 0)
//            {
//                //중복이 없을 때
//                gService.Insert(listFeed, row);
//            }
//            else
//            {
//                //중복이 있을 때
//                updateData(chkFeed, best, worst);

//            }



//        }
//        void createService()
//        {
//            gService = new SpreadsheetsService("test");
//            gService.setUserCredentials(USERNAME, PASSWORD);

//        }
//        private void createLinkQuery()
//        {

//            // Fetch the cell feed of the worksheet.
//            // Define the URL to request the list feed of the worksheet.
//            listFeedLink = entryWorkSheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

//            // Fetch the list feed of the worksheet.
//            listQuery = new ListQuery(listFeedLink.HRef.ToString());
//        }
//        void selectSheet()
//        {
//            SpreadsheetQuery query = new SpreadsheetQuery();
//            SpreadsheetFeed feed = gService.Query(query);

//            AtomLink link;


//            foreach ( SpreadsheetEntry entry in feed.Entries)
//            {
//                if (entry.Title.Text.Equals("tjssmduty"))
//                {
//                    entrySelect = entry;
//                    link = entrySelect.Links.FindService(GDataSpreadsheetsNameTable.WorksheetRel, null);
//                    WorksheetQuery queryWS = new WorksheetQuery(link.HRef.ToString());
//                    WorksheetFeed feedWS = gService.Query(queryWS);
//                    foreach (WorksheetEntry worksheet in feedWS.Entries)
//                    {
//                        if (worksheet.Title.Text.Equals("Worst"))
//                        {
//                            entryWorkSheet = worksheet;


//                            break;
//                        }

//                    }
//                    break;

//                }
//            }
//            Console.WriteLine("select complete");


//        }


//    }
//}
