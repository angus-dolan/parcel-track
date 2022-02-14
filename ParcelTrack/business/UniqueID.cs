using Coursework_2.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_2.business
{
    class UniqueID
    {
        /*
         * Class for creating Parcel and Courier ID's that are unique.
         * Randomly generated using 3 wildcards.
         * Will never return an ID stored in Parcel or Courier db.
        */

        public static int Create()
        {
            DataFacadeSingleton df = DataFacadeSingleton.getInstance();
            Random rnd = new Random();
            bool unique = false;
            int id = 0;

            while (unique == false)
            {
                int month = rnd.Next(1, 13); // creates a number between 1 and 12
                int dice = rnd.Next(1, 7); // creates a number between 1 and 6
                int card = rnd.Next(52); // creates a number between 0 and 51
                id = int.Parse(month.ToString() + dice.ToString() + card.ToString());

                // Continue until unique id is generated
                if (df.getCourierDB().ContainsKey(id) || df.getParcelDB().ContainsKey(id))
                {
                    unique = false;
                }
                else
                {
                    unique = true;
                }
            }

            return id;
        }
    }
}
