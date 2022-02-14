using Coursework_2.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_2.business
{
    class FindAvailableCourier
    {
        /*
            * Class for finding an available Courier.
            * Returns nullable int.
            * int = successfully found.
            * null = no rider available.
            * 
            * Checks rider has capacity and if they delivery to area code.
        */

        public static int? Run(String postcode)
        {
            DataFacadeSingleton df = DataFacadeSingleton.getInstance();

            // Get area code from postcode
            String[] explodedPostcode = postcode.Split(" ");
            String areacode = explodedPostcode[0];

            // Search for available courier
            bool success = false;
            int selectedRiderID = 0;
            foreach (var record in df.getCourierDB())
            {
                Courier courier = record.Value;

                foreach (String deliveryArea in courier.AreasAssigned)
                {
                    // Find first courier assigned to that area code with adequate parcel capacity left
                    if (
                        deliveryArea == areacode && // areacode matches
                        courier.Parcels.Count < courier.MaxParcels && // courier still has space for parcel
                        success != true // match not found yet
                    )
                    {
                        selectedRiderID = courier.ID;
                        success = true;
                    }
                }
            }

            // Return results
            if (success)
            {
                return selectedRiderID;
            } 
            else
            {
                return null;
            }   
        }
    }
}
