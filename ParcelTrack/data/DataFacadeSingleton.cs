using Coursework_2.business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_2.data
{
    class DataFacadeSingleton
    {
        /*
            * DataFacadeSingleton Class
            * Provides abstracted interface with database class
        */

        // Start Singleton code
        private static DataFacadeSingleton reference;
        private DataFacadeSingleton() { }
        public static DataFacadeSingleton getInstance()
        {
            if (reference == null)
                reference = new DataFacadeSingleton();

            return reference;
        }
        // End Singleton code

        private Database db = new Database(); // The CSV database
        
        /*
            [--- Courier ---]
        */       
        public Dictionary<int, Courier> getCourierDB()
        {
            // Get entire Courier data structure
            return db.courierDB;
        }
        public Courier getCourier(int id)
        {
            // Get single courier
            return db.getCourier(id);
        }
        public void addCourier(Courier c)
        {
            // Save Courier to DB
            db.addCourier(c);
        }

        /*
            [--- Parcels ---]
        */
        public Dictionary<int, Parcel> getParcelDB()
        {
            // Get entire Parcels data structure
            return db.parcelDB;
        }
        public void addParcel(Parcel parcel)
        {
            // Save Parcel to DB
            db.addParcel(parcel);
        }
        public Parcel getParcel(int id)
        {
            // Get single parcel
            return db.getParcel(id);
        }
        public bool transferParcel(int parcelID, int courierID, bool isUnassigned)
        {
            // Tranfer Parcel to different Courier
            return db.transferParcel(parcelID, courierID, isUnassigned);
        }

        /*
            [--- Unassigned Parcels ---]
        */
        public Dictionary<int, Parcel> getUnassigned()
        {
            // Get all currently unassigned parcels
            return db.unassignedParcels;
        }
        public void addUnParcel(Parcel parcel)
        {
            // Save unassigned parcel
            db.addUnParcel(parcel);
        }

        /*
            [--- MISC ---]
        */
        public void restore()
        {
            // Restore Courier and Parcels DB from csv
            db.restoreDB();
        }

        public void saveToCSV()
        {
            // Save current Parcel and Courier data structures to csv files
            db.saveToCSV();
        }
        public void newLog(string data)
        {
            // Add new entry to log.txt
            db.newLog(data);
        }
    }
}
