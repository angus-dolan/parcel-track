using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_2.business
{
    class CourierFactory
    {
        /*
         * A Factory that creates couriers.
         * Each courier has a unique courier id allocated by the factory
         */
        public Courier NewCourier(String type, String[] assigned)
        {
            int id = UniqueID.Create(); // create unique courier id

            if (type == "Van")
            {
                return new VanCourier(id, assigned); // return new van courier
            }
            else if (type == "Cycle")
            {
                return new CycleCourier(id, assigned); // return new cycle courier
            }
            else if (type == "Walking")
            {
                return new WalkingCourier(id, assigned); // return new walking courier
            }
            else
            {
                return null; // default, can only create VanCourier, CycleCourier or WalkingCourier. If anything else is requested return null
            }
        }
        /*
            * A Factory that restores existing couriers from the courier csv file.
            * Unlike NewCourier(), id is also given to avoid giving a new random id.
        */
        public Courier RestoreCourier(int id, String type, String[] assigned, ArrayList parcels)
        {
            if (type == "Van")
            {
                return new VanCourier(id, assigned, parcels); // return new van courier
            }
            else if (type == "Cycle")
            {
                return new CycleCourier(id, assigned, parcels); // return new cycle courier
            }
            else if (type == "Walking")
            {
                return new WalkingCourier(id, assigned, parcels); // return new walking courier
            }
            else
            {
                return null; // default, can only create VanCourier, CycleCourier or WalkingCourier. If anything else is requested return null
            }
        }
    }
}
