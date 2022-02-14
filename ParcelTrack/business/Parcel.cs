using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_2.business
{
    public class Parcel
    {
        /*
         * Class for Parcels
         * Unit tests implimented
         * 2 parameter constructor: New Unassigned Parcel (Courier ID missing)
         * 3 parameter constructor: New Parcel
         * 4 parameter constructor: Restore from csv
        */

        // Properties
        private int id;
        private String destination;
        private String postcode;
        private int courierId;

        // Unit test error messages
        public const string PostcodeInvalidMessage = "Postcode provided is invalid";

        // Constructors
        public Parcel(String d, String p)
        {
            // New Unassigned Parcel
            id = UniqueID.Create();
            destination = d;
            postcode = p;
        }
        public Parcel(String d, String p, int c)
        {
            // New Parcel
            id = UniqueID.Create();
            destination = d;
            postcode = p;
            courierId = c;
        }
        public Parcel(int i, String d, String p, int c)
        {
            // Restore Parcel from csv
            id = i;
            destination = d;
            postcode = p;
            courierId = c;
        }

        // Getters and Setters
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public String Destination
        {
            get { return destination; }
            set { destination = value; }
        }
        public String Postcode
        {
            get { return postcode; }
            set { postcode = value; }
        }
        public int CourierID
        {
            get { return courierId; }
            set { courierId = value; }
        }

        // Methods
        public String getAreaCode()
        {
            string[] exploded = Postcode.Split(' ');
            string area = exploded[0];

            // Throw error when postcode invalid
            if (exploded.Length != 2)
            {
                throw new System.ArgumentException(PostcodeInvalidMessage);
            }
            
            return area;
        }
        public String logData()
        {
            // Log data for Parcel
            String data = "New Parcel Added ({0}, '{1}') Allocated to Courier {2}";
            String log = String.Format(data, Postcode, Destination, CourierID);

            return log;
        }
        public String unassignedLogData()
        {
            // Log data for unassigned Parcel
            String data = "New Parcel Added ({0}, '{1}') Allocated to Courier UNASSIGNED";
            String log = String.Format(data, Postcode, Destination);

            return log;
        }
    }
}
