using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace OPSI_Security
{


    public class Token
    {


        #region "CONSTS"

        private string settingDateFormatToken = ConfigurationManager.AppSettings["OPSI_Security_DateFormatToken"];
        private int settingMinutesTokensValid = Int32.Parse(ConfigurationManager.AppSettings["OPSI_Security_MinutesTokensValid"]);

        #endregion


        #region "PROPERTIES"

        public string Username { get; set; }
        public string Token_EncryptedValue { get; set; }
        public DateTime DateCreated { get; set; }
        public string Token_ReadableValue { get; set; }

        #endregion


        #region "CONSTRUCTORS"

        /// <summary>
        /// Default constructor, use it on first load to create the "base" token
        /// </summary>
        /// <param name="userName">The username of current logged user</param>
        public Token(string userName)
        {

            //stores the username
            this.Username = userName; 

            //creates the token
            this.UpdateTokensValues();

        }

        /// <summary>
        /// Use it for consecutive calls, passing the encrypted key to get the information needed
        /// </summary>
        /// <param name="userName">The username of current logged user</param>
        /// <param name="encryptedValue">The value that is sent in the header of the API</param>
        public Token(string userName, string encryptedValue)
        {

            //saves the encrypted passed value
            this.Token_EncryptedValue = encryptedValue;

            //decrypts the passed value
            GetValuesFromEncryptedValue();

        }

        #endregion


        #region "METHODS"

        /// <summary>
        /// Updates the encrypted token, according to the stored username and current time.
        /// </summary>
        public void UpdateTokensValues()
        {

            StringBuilder sbToken = new StringBuilder();

            //adding username
            sbToken.Append(this.Username);

            //adding separator
            sbToken.Append("|");

            //calculating and storing current time
            this.DateCreated = DateTime.Now;

            //adding the time
            sbToken.Append(this.DateCreated.ToString(settingDateFormatToken));

            //saves the non encrypted value
            this.Token_ReadableValue = sbToken.ToString();

            //saves the encrypted value
            this.Token_EncryptedValue = Crypt.Encrypt(sbToken.ToString());
            
        }


        /// <summary>
        /// Call only after encrypted value has been set (eg constructor). It will fill username and date with the found values in the encrypted string
        /// </summary>
        public void GetValuesFromEncryptedValue()
        {

            if (String.IsNullOrEmpty(this.Token_EncryptedValue) == false)
            {

                //decrypts value
                this.Token_ReadableValue = Crypt.Decrypt(this.Token_EncryptedValue);

                //stores username
                this.Username = this.Token_ReadableValue.Split("|".ToCharArray())[0];

                //gets the date 
                string decryptedDate = this.Token_ReadableValue.Split("|".ToCharArray())[1];

                //calculates the original date that the token was created on
                this.DateCreated = DateTime.ParseExact(decryptedDate, settingDateFormatToken, System.Globalization.CultureInfo.InvariantCulture);
                
            }
            else
            {

                //no encrypted value

            }



        }


        /// <summary>
        /// Checks wheter the token is within the specified time interval
        /// </summary>
        public bool Validate() 
        {

            bool result = false;

            //stating time is when the token was first created
            DateTime startTime = this.DateCreated;

            //end time is now
            DateTime endTime = DateTime.Now;

            //calculating difference
            TimeSpan timePassed = endTime.Subtract(startTime);

            //Check that the minutes passed are lower than our setting
            if ( timePassed.Minutes < settingMinutesTokensValid)
            {
                result = true;
            }

            return result;

        }

        #endregion



    }


}
