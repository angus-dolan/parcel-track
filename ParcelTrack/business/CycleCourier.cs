using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_2.business
{
    class CycleCourier : Courier
    {
        /*
         * Class for Cycle Couriers
         * Inherits Courier base class
         * 2 parameter constructor: creating new courier
         * 3 parameter constructor: restoring couriers from csv
        */

        public CycleCourier(int id, String[] assigned) : base(id, assigned)
        {
            base.Type = "Cycle";
            base.AreaLimit = "EH22"; // may cover any postcode
            base.MaxParcels = 10; // can carry up to 10 parcels
            base.MaxNumAreas = 1; // may cover 1 area at a time
        }

        // Parcels are passed when restoring db
        public CycleCourier(int id, String[] assigned, ArrayList parcels) : base(id, assigned, parcels)
        {
            base.Type = "Cycle";
            base.AreaLimit = "EH22"; // may cover any postcode
            base.MaxParcels = 10; // can carry up to 10 parcels
            base.MaxNumAreas = 1; // may cover 1 area at a time
        }
    }
}
