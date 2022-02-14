using Coursework_2.business;
using Coursework_2.data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Coursework_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Restore Courier and Parcels DB
            DataFacadeSingleton df = DataFacadeSingleton.getInstance();
            df.restore();

            // Add courier types to dropdown
            NewCourier_Type.Items.Add("Van");
            NewCourier_Type.Items.Add("Cycle");
            NewCourier_Type.Items.Add("Walking");
        }

        // Add Courier
        private void NewCourier_Add_Click(object sender, RoutedEventArgs e)
        {
            DataFacadeSingleton df = DataFacadeSingleton.getInstance();
            CourierFactory courierFactory = new CourierFactory();

            // Get form data
            String type = NewCourier_Type.Text;
            String[] deliveryAreas = NewCourier_DeliveryAreas.Text.Split(" ");
            int typeLength = type.Length;

            if (typeLength > 0 && deliveryAreas[0] != "")
            {
                Courier courier = courierFactory.NewCourier(type, deliveryAreas);

                bool validDeliveryAreas = courier.areasAssignedValid();
                if (validDeliveryAreas)
                {
                    // Create courier
                    df.addCourier(courier);             
                    df.newLog(courier.logData()); // new log entry
                    NewCourier_Type.Text = NewCourier_DeliveryAreas.Text = ""; // clear form
                    Success_Message("New courier added 🚚");
                } else
                {
                    Error_Message("Delivery area not possible for courier type");
                }
                
            } else
            {
                Error_Message("Please provide a type and atleast 1 Postcode for Courier.");
            }
        }
        // Add Parcel
        private void NewParcel_Add_Click(object sender, RoutedEventArgs e)
        {
            DataFacadeSingleton df = DataFacadeSingleton.getInstance();

            // Get form data
            String address = NewParcel_Address.Text;
            String postcode = NewParcel_Postcode.Text;
            int addressLength = address.Length;
            int postcodeLength = postcode.Length;

            // Validate 
            if (addressLength > 0 && postcodeLength > 0)
            {
                // Attempt to find available courier
                int? courierID = FindAvailableCourier.Run(postcode);
                if (courierID == null)
                {
                    // Courier not availablle, move to unassigned
                    Error_Message("Available courier could not be found, Parcel moved to unassigned.");
                    Parcel parcel = new Parcel(address, postcode);
                    df.addUnParcel(parcel);
                    NewParcel_Address.Text = NewParcel_Postcode.Text = "";
                    df.newLog(parcel.unassignedLogData());
                }
                else
                {
                    // Rider successfully found
                    Parcel parcel = new Parcel(address, postcode, courierID.Value);
                    df.addParcel(parcel);

                    
                    df.newLog(parcel.logData()); // new log entry
                    NewParcel_Address.Text = NewParcel_Postcode.Text = ""; // clear form
                    Success_Message("New parcel added 📦");
                }
            } else
            {
                Error_Message("Please provide an Address and Postcode");
            }
        }
        // Transfer Parcel
        private void TransferParcel_Btn_Click(object sender, RoutedEventArgs e)
        {
            DataFacadeSingleton df = DataFacadeSingleton.getInstance();

            // Get form data
            int.TryParse(TransferParcel_Parcel_ID.Text, out int parcelID);
            int.TryParse(TransferParcel_Courier_ID.Text, out int courierID);
            int pLength = TransferParcel_Parcel_ID.Text.Length;
            int cLength = TransferParcel_Courier_ID.Text.Length;

            // Validate
            if (pLength > 0 && cLength > 0)
            {
                // Handle unassigned parcel
                bool isUnassigned = df.getUnassigned().ContainsKey(parcelID);
                
                bool parcelExists = df.getParcelDB().ContainsKey(parcelID);
                bool courierExists = df.getCourierDB().ContainsKey(courierID);
                if ((parcelExists && courierExists) || (isUnassigned && courierExists))
                {
                    // Check courier has space
                    if (df.transferParcel(parcelID, courierID, isUnassigned))
                    {
                        // Success
                        TransferParcel_Parcel_ID.Text = TransferParcel_Courier_ID.Text = "";
                        df.newLog("Parcel " + parcelID + " has been tranfered to courier " + courierID);
                        Success_Message("Parcel " + parcelID + " has been tranfered to courier " + courierID + " ✅");
                    }
                    else
                    {
                        Error_Message("Either courier doesn't deliver to that area or they don't have space.");
                    }
                }
                else
                {
                    // Parcel or Courier not found
                    Error_Message("Parcel or Courier can't be found.");
                }
            }
            else
            {
                // Error, form incomplete
                Error_Message("Please provide a Courier and Parcel ID.");
            }
        }
        // Generate and display delivery list for single courier
        private void DeliveryList_Btn_Click(object sender, RoutedEventArgs e)
        {
            DataFacadeSingleton df = DataFacadeSingleton.getInstance();
            int inputLength = DeliveryList_CourierID.Text.Length;
            if (inputLength > 0)
            {
                int.TryParse(DeliveryList_CourierID.Text, out int courierID);

                // Validate
                bool courierExists = df.getCourierDB().ContainsKey(courierID);
                if (courierExists)
                {
                    Courier courier = df.getCourier(courierID);
                    Summary.Text = "Delivery list for courier " + courierID + ":";

                    foreach (int parcelID in courier.Parcels)
                    {
                        Parcel p = df.getParcel(parcelID);
                        String data = "Parcel {0} - {1} - {2}";
                        String output = String.Format(data, p.ID, p.Destination, p.Postcode);
                        Summary.Text = Summary.Text + "\n" + output;
                    }

                    df.newLog("Delivery list generated for courier " + courierID);
                }
                else
                {
                    // Error, courier not found
                    Error_Message("Courier not found.");
                }
            } else
            {
                // Error, form incomplete
                Error_Message("Please provide an existing courier ID.");
            }
        }
        // Display all unassigned Parcels
        private void ShowUnassigned_Click(object sender, RoutedEventArgs e)
        {
            DataFacadeSingleton df = DataFacadeSingleton.getInstance();

            Summary.Text = "";
            foreach (KeyValuePair<int, Parcel> parcel in df.getUnassigned())
            {
                Parcel p = parcel.Value;

                String data = "Parcel {0} - {1} - {2} - Courier: UNASSIGNED";
                String output = String.Format(data, p.ID, p.Destination, p.Postcode);

                Summary.Text = Summary.Text + "\n" + output;
            }
        }
        // Display summary
        private void ShowSummary_Click(object sender, RoutedEventArgs e)
        {
            DataFacadeSingleton df = DataFacadeSingleton.getInstance();

            Summary.Text = "";
            foreach (KeyValuePair<int, Courier> courier in df.getCourierDB())
            {
                Courier c = courier.Value;

                String data = "{0} - Courier {1} ({2}) : {3}/{4}";
                String output = String.Format(data, string.Join(",", c.AreasAssigned), c.ID, c.Type, c.Parcels.Count, c.MaxParcels);

                Summary.Text = Summary.Text + "\n" + output;
            }

            // New log
            df.newLog("Retrieved current list of areas and couriers");
        }
        // Trigger db save to courier and parcel csv
        private void SaveToCSV_Click(object sender, RoutedEventArgs e)
        {
            DataFacadeSingleton df = DataFacadeSingleton.getInstance();
            df.saveToCSV();
            df.newLog("Saving to CSV...");
            Success_Message("Successfully saved to CSV 💾");
        }
        // Display success message on GUI
        private void Success_Message(String data)
        {
            Success.Text = data;
            Success.Visibility = Visibility.Visible;
            Error.Visibility = Visibility.Hidden;
        }
        // Display error message on GUI
        private void Error_Message(String data)
        {
            Error.Text = data;
            Success.Visibility = Visibility.Hidden;
            Error.Visibility = Visibility.Visible;
        }

    }
}
