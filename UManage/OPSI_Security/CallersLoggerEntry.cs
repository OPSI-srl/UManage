using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace OPSI_Security
{

    /// <summary>
    /// This class represents a single line in the callerslog text file
    /// </summary>
    public class CallersLoggerEntry
    {

        #region PROPERTIES

        public string IPAddress { get; set; }
        public string Agent { get; set; }
        public DateTime Time { get; set; }
        public string WebApiName { get; set; }
        public int Counter { get; set; }

        #endregion

        #region METHODS

        public string Return_TextLine()
        {

            string result = string.Empty;

            try
            {

                result += this.WebApiName + "#";
                result += this.IPAddress + "#";
                result += this.Agent + "#";
                result += this.Time + "#";
                result += this.Counter.ToString();

            }
            catch (Exception ex)
            {

                OPSI_Logger.ErrorLog logger = new OPSI_Logger.ErrorLog();
                logger.Error("CallersLoggerEntry, errore nel ritorno della riga informazioni", ex, "Return_TextLine");

            }

            return result;

        }


        #endregion


    }

}
