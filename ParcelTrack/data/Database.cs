using Coursework_2.business;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Collections;

namespace Coursework_2.data
{
    class Database
    {
        /*
            * Database Class
            * Provides singleton instance with database functionalites.
        */

        public Dictionary<int, Courier> courierDB = new Dictionary<int, Courier>();
        public Dictionary<int, Parcel> parcelDB = new Dictionary<int, Parcel>();
        public Dictionary<int, Parcel> unassignedParcels = new Dictionary<int, Parcel>();
        private String courierDBFile = "courier_database.csv";
        private String parcelDBFile = "parcel_database.csv";

        public void restoreDB()
        {
            // Courier DB
            if (File.Exists(courierDBFile))
            {
                string[] lines = File.ReadAllLines(courierDBFile);

                // Foreach record in db
                foreach (string line in lines)
                {
                    string[] lineData = line.Split(',');

                    // Extract data
                    bool success = int.TryParse(lineData[0], out int id);
                    String type = lineData[1];
                    String[] areasAssigned = lineData[5].Split(' ');

                    ArrayList parcels = new ArrayList();
                    String[] stringParcels = lineData[6].Split(' ');
                    foreach (String parcel in stringParcels)
                    {
                        int.TryParse(parcel, out int p);
                        parcels.Add(p);
                    }

                    // Restore data into Courier object
                    CourierFactory courierFactory = new CourierFactory();
                    Courier courier = courierFactory.RestoreCourier(id, type, areasAssigned, parcels);

                    // Add to data structure
                    courierDB.Add(id, courier);
                }
            }

            // Parcels DB
            if (File.Exists(parcelDBFile))
            {
                string[] lines = File.ReadAllLines(parcelDBFile);

                // Foreach record in db
                foreach (string line in lines)
                {
                    string[] lineData = line.Split(',');

                    // Extract data
                    int id = int.Parse(lineData[0]);
                    String address = lineData[1];
                    String postcode = lineData[2];
                    int courierID = int.Parse(lineData[3]);

                    // Restore data into Parcel object
                    Parcel parcel = new Parcel(id, address, postcode, courierID);

                    // Add to data structure
                    parcelDB.Add(id, parcel); 
                }
            }
        }
        public void addCourier(Courier c)
        {
            // Add to data structure
            courierDB.Add(c.ID, c);
        }

        public Courier getCourier(int id)
        {
            Courier result = courierDB[id]; 
            return result;
        }

        public void addParcel(Parcel p)
        {
            // Add to data structure
            parcelDB.Add(p.ID, p);
            // Assign to courier
            Courier courier = courierDB[p.CourierID];
            courier.addParcel(p.ID);
        }

        public Parcel getParcel(int id)
        {
            Parcel result = parcelDB[id];
            return result;
        }

        public void addUnParcel(Parcel p)
        {
            unassignedParcels.Add(p.ID, p);
        }

        public bool transferParcel(int parcelID, int courierID, bool isUnassigned)
        {
            // Check rider delivers to area
            bool assignedToCorrectArea = false;
            foreach (string deliveryArea in courierDB[courierID].AreasAssigned)
            {
                if (isUnassigned)
                {
                    if (deliveryArea == unassignedParcels[parcelID].getAreaCode()) assignedToCorrectArea = true;
                }
                else
                {
                    if (deliveryArea == parcelDB[parcelID].getAreaCode()) assignedToCorrectArea = true;
                }
            }
            if (!assignedToCorrectArea) return false;

            // Check rider has space
            if (courierDB[courierID].Parcels.Count < courierDB[courierID].MaxParcels)
            {
                // Update current courier
                if (!isUnassigned) courierDB[parcelDB[parcelID].CourierID].Parcels.Remove(parcelID);

                // Update new courier
                courierDB[courierID].Parcels.Add(parcelID);

                // Update parcel
                if (isUnassigned)
                {
                    parcelDB.Add(unassignedParcels[parcelID].ID, unassignedParcels[parcelID]);
                    unassignedParcels.Remove(parcelID);
                }
                parcelDB[parcelID].CourierID = courierID;

                return true;
            } else
            {
                // Courier doesn't have space
                return false;
            }
        }

        public void saveToCSV()
        {
            //
            // Courier CSV
            //
            ArrayList courierLines = new ArrayList();

            foreach (var record in courierDB)
            {
                Courier c = record.Value;

                StringBuilder builder = new StringBuilder();
                var id = c.ID;
                var type = c.Type;
                var areaLimit = c.AreaLimit;
                var maxParcels = c.MaxParcels;
                var maxNumAreas = c.MaxNumAreas;
                var areasAssigned = string.Join(" ", c.AreasAssigned);
                var parcels = string.Join(" ", c.Parcels.ToArray());
                builder.Append(string.Format("{0},{1},{2},{3},{4},{5},{6}", id, type, areaLimit, maxParcels, maxNumAreas, areasAssigned, parcels));

                courierLines.Add(builder.ToString());
                builder.Clear();
            }

            String[] courierLinesArr = (String[]) courierLines.ToArray(typeof(string));
            File.WriteAllLines(courierDBFile, courierLinesArr);

            //
            // Parcel CSV
            //
            ArrayList parcelLines = new ArrayList();

            foreach (var record in parcelDB)
            {
                Parcel p = record.Value;

                StringBuilder builder = new StringBuilder();
                var parcelId = p.ID;
                var destination = p.Destination;
                var postcode = p.Postcode;
                var courierId = p.CourierID;

                builder.Append(string.Format("{0},{1},{2},{3}", parcelId, destination, postcode, courierId));

                parcelLines.Add(builder.ToString());
                builder.Clear();
            }

            String[] parcelLinesArr = (String[]) parcelLines.ToArray(typeof(string));
            File.WriteAllLines(parcelDBFile, parcelLinesArr);
            
        }

        public void newLog(string data)
        {
            String dt = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            File.AppendAllText("log.txt", dt + " " + data + "\n");
        }
    }
}
