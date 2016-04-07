using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;


namespace OPSI_Security
{

    /// <summary>
    /// This class cares to write/read the calls to the webapis
    /// </summary>
    public class CallersLogger
    {

        #region "CONSTS"

        private string settingPathLogFile = ConfigurationManager.AppSettings["OPSI_Security_FilepathCallersLog"];
        private double settingMinutesOfInterval = double.Parse(ConfigurationManager.AppSettings["OPSI_Security_MinutesOfIntervalCallersLog"]);
        private double settingAllowedCallsInInterval = double.Parse(ConfigurationManager.AppSettings["OPSI_Security_AllowedCallsInInterval"]);          
           
        #endregion


        #region "METHODS"

        /// <summary>
        /// This method will write an update to the text file
        /// </summary>
        public bool Write_Caller_Info( CallersLoggerEntry callersLoggerEntry )
        {

            bool result = true;

            try
            { 

                //checking existence of the file first
                if (File.Exists(settingPathLogFile) == false)
                {
                    //must be created
                    StreamWriter swCreateFile = new StreamWriter(File.Open(settingPathLogFile, System.IO.FileMode.Append));
                    swCreateFile.Close();
                    swCreateFile.Dispose();
                }
            
                //Reading existing content
                List<CallersLoggerEntry> ListEntries = Read_Log_File();

                //Clearing out expired entries
                Clear_Expired_Entries( ref ListEntries );

                //Writing down the array of entries
                StreamWriter swWriteFile = new StreamWriter(settingPathLogFile, false);
                
                //Add a new element or updating the counter of an existing one?
                bool existingFound = false;
                foreach (CallersLoggerEntry callerEntryExisting in ListEntries)
                {
                    if (callerEntryExisting.IPAddress == callersLoggerEntry.IPAddress && callerEntryExisting.WebApiName == callersLoggerEntry.WebApiName && callerEntryExisting.Agent == callersLoggerEntry.Agent)
                    {
                        
                        //Checks if the number of times this API has been called, exceed the allowed limit
                        if (callerEntryExisting.Counter >= this.settingAllowedCallsInInterval)
                        {
                            //block execution
                            result = false;
                        }

                        callerEntryExisting.Counter = callerEntryExisting.Counter + 1;
                        existingFound = true;

                    }
                }

                //Was an existing item found? if not, we just add it
                if (existingFound == false)
                {
                    callersLoggerEntry.Counter = 0;
                    ListEntries.Add(callersLoggerEntry);
                }


                //One element equals to one new line
                foreach( CallersLoggerEntry callerEntry in ListEntries  )
                {
                    swWriteFile.WriteLine(callerEntry.Return_TextLine());
                }

                //Closes up
                swWriteFile.Close();
                swWriteFile.Dispose();

            }
            catch (Exception ex)
            {

                OPSI_Logger.ErrorLog logger = new OPSI_Logger.ErrorLog();
                logger.Error("Errore Scrittura su file", ex, "Write_Caller_Info" );

            }

            return result;

        }


        /// <summary>
        /// This method reads the whole file and builds an array of entries objects
        /// </summary>
        public List<CallersLoggerEntry> Read_Log_File()
        {

            List<CallersLoggerEntry> result = new List<CallersLoggerEntry>();

            try
            {

                foreach (string line in File.ReadLines(settingPathLogFile))
                {

                    if (line != "")
                    { 

                        string[] arrayValues = line.Split("#".ToCharArray());

                        CallersLoggerEntry newEntry = new CallersLoggerEntry();
                        newEntry.WebApiName = arrayValues[0];
                        newEntry.IPAddress = arrayValues[1];
                        newEntry.Agent = arrayValues[2];
                        newEntry.Time = DateTime.Parse(arrayValues[3]);
                        newEntry.Counter = Int32.Parse(arrayValues[4]);
                        result.Add(newEntry);

                    }

                }

            }
            catch (Exception ex)
            {

                OPSI_Logger.ErrorLog logger = new OPSI_Logger.ErrorLog();
                logger.Error("Errore Lettura da file", ex, "Read_Log_File");

            }

            return result;

        }


        /// <summary>
        /// This method clears out the expired entries from a read list
        /// </summary>
        public void Clear_Expired_Entries(ref List<CallersLoggerEntry> ListEntries)
        {

            ListEntries.RemoveAll(item => ( item.Time.AddMinutes(this.settingMinutesOfInterval) < DateTime.Now) );

        }

        #endregion



    }

}
