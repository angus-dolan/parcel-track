using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_2.business
{
    class VanCourier : Courier
    {
        /*
         * Class for Van Couriers
         * Inherits Courier base class
         * 2 parameter constructor: creating new courier
         * 3 parameter constructor: restoring couriers from csv
         */

        public VanCourier(int id, String[] assigned) : base(id, assigned)
        {
            base.Type = "Van";
            base.AreaLimit = "EH22"; // may cover any postcode
            base.MaxParcels = 100; // can carry up to 100 parcels
            base.MaxNumAreas = 22; // may cover any number of areas
        }

        // Parcels are passed when restoring db
        public VanCourier(int id, String[] assigned, ArrayList parcels) : base(id, assigned, parcels)
        {
            base.Type = "Van";
            base.AreaLimit = "EH22"; // may cover any postcode
            base.MaxParcels = 100; // can carry up to 100 parcels
            base.MaxNumAreas = 22; // may cover any number of areas
        }
    }
}