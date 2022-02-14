using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_2.business
{
    class WalkingCourier : Courier
    {
        /*
         * Class for Walking Couriers
         * Inherits Courier base class
         * 2 parameter constructor: creating new courier
         * 3 parameter constructor: restoring couriers from csv
        */

        public WalkingCourier(int id, String[] assigned) : base(id, assigned)
        {
            base.Type = "Walking";
            base.AreaLimit = "EH4"; // may only cover postcodes EH1-EH4
            base.MaxParcels = 5; // can carry up to 5 parcels
            base.MaxNumAreas = 1; // may cover 1 area at a time
        }

        // Parcels are passed when restoring db
        public WalkingCourier(int id, String[] assigned, ArrayList parcels) : base(id, assigned, parcels)
        {
            base.Type = "Walking";
            base.AreaLimit = "EH4"; // may only cover postcodes EH1-EH4
            base.MaxParcels = 5; // can carry up to 5 parcels
            base.MaxNumAreas = 1; // may cover 1 area at a time
        }
    }
}