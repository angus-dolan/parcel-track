using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Coursework_2.business
{
    class Courier
    {
        /*
         * The base class for couriers
         * couriers can be of type Van, Cycle or Walking
         * unique ID's are created using the UniqueID class in CourierFactory
         */

        // Fields
        private int id;
        private String type; // van, cycle, walking
        private String areaLimit; // what areas they can cover, EH(1) - EH(22)
        private int maxParcels; // max num of parcels they can carry
        private int maxNumAreas; // max num of areas they can cover
        private String[] areasAssigned = new String[22]; // area codes they have been assigned, e.g. EH1 etc
        private ArrayList parcels = new ArrayList(); // ID's of parcels assign to courier

        // Constructor
        public Courier(int i, String[] assigned)
        {
            id = i;
            areasAssigned = assigned;
        }
        public Courier(int i, String[] assigned, ArrayList p)
        {
            // When db.restore() is called, id is passed to restore 
            // correct id saved in csv
            id = i;
            areasAssigned = assigned;
            parcels = p;
        }

        // Getters and Setters
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public String Type
        {
            get { return type; }
            set { type = value; }
        }
        public String AreaLimit
        {
            get { return areaLimit; }
            set { areaLimit = value; }
        }
        public int MaxParcels
        {
            get { return maxParcels; }
            set { maxParcels = value; }
        }
        public int MaxNumAreas
        {
            get { return maxNumAreas; }
            set { maxNumAreas = value; }
        }
        public String[] AreasAssigned
        {
            get { return areasAssigned; }
            set { areasAssigned = value; }
        }
        public ArrayList Parcels
        {
            get { return parcels; }
            set { parcels = value; }
        }

        // Methods
        public void addParcel(int parcelID)
        {
            Parcels.Add(parcelID);
        }
        public bool areasAssignedValid()
        {
            // Get area limit num from stored property
            // e.g. EH22 returns => 22
            string areaLimitString = Regex.Match(AreaLimit, @"\d+").Value;
            int.TryParse(areaLimitString, out int areaLimitNum);

            foreach (string assigned in AreasAssigned)
            {
                string[] exploded = assigned.Split(' ');
                string areaCode = exploded[0];
                string numString = Regex.Match(areaCode, @"\d+").Value;
                int.TryParse(numString, out int num);

                // Compare with area limit
                if (num > areaLimitNum || num < 1) return false;
            }

            return true;
        }
        public String logData()
        {
            String data = "New Courier Added - id={0}, type={1}";
            String log = String.Format(data, ID, Type);

            return log;
        }
    }
}
