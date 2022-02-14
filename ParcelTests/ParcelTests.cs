using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coursework_2.business;

namespace ParcelTests
{
    [TestClass]
    public class ParcelTests
    {
        [TestMethod]
        public void Test_Constructor_With_2_Params()
        {
            // Arrange 
            string destination = "5230 Newell Road Palo Alto";
            string postcode = "CA 91364";
            Parcel parcel = new Parcel(destination, postcode);

            // Assert
            Assert.AreEqual(destination, parcel.Destination, "Parcel destination not set correctly in constructor with 2 parameters.");
            Assert.AreEqual(postcode, parcel.Postcode, "Parcel postcode not set correctly in constructor with 2 parameters.");
        }
        [TestMethod]
        public void Test_Constructor_With_3_Params()
        {
            // Arrange 
            string destination = "5230 Newell Road Palo Alto";
            string postcode = "CA 91364";
            int courierID = 1234;
            Parcel parcel = new Parcel(destination, postcode, courierID);

            // Assert
            Assert.AreEqual(destination, parcel.Destination, "Parcel destination not set correctly in constructor with 3 parameters.");
            Assert.AreEqual(postcode, parcel.Postcode, "Parcel postcode not set correctly in constructor with 3 parameters.");
            Assert.AreEqual(courierID, parcel.CourierID, "Parcel CourierID not set correctly in constructor with 3 parameters.");
        }
        [TestMethod]
        public void Test_Constructor_With_4_Params()
        {
            // Arrange 
            string destination = "5230 Newell Road Palo Alto";
            string postcode = "CA 91364";
            int courierID = 0987;
            int id = 1234;
            Parcel parcel = new Parcel(id, destination, postcode, courierID);

            // Assert
            Assert.AreEqual(destination, parcel.Destination, "Parcel destination not set correctly in constructor with 4 parameters.");
            Assert.AreEqual(postcode, parcel.Postcode, "Parcel postcode not set correctly in constructor with 4 parameters.");
            Assert.AreEqual(courierID, parcel.CourierID, "Parcel CourierID not set correctly in constructor with 4 parameters.");
            Assert.AreEqual(id, parcel.ID, "Parcel ID not set correctly in constructor with 4 parameters.");
        }
        [TestMethod]
        public void GetAreaCode_WhenPostcodeInvalid_ShouldThrowArgumentException()
        {
            // Arrange 
            Parcel parcel = new Parcel(1234, "5230 Newell Road Palo Alto", "CA91364", 9876);

            // Act
            try
            {
                parcel.getAreaCode();
            }
            catch (System.ArgumentException e)
            {
                // Assert
                StringAssert.Contains(e.Message, Parcel.PostcodeInvalidMessage);
                return;
            }

            // Assert
            Assert.Fail("The expected exception was not thrown.");
        }
        [TestMethod]
        public void GetAreaCode_WithValidPostcode_ReturnsAreaCode()
        {
            // Arrange
            string expected = "CA";
            Parcel parcel = new Parcel(1234, "5230 Newell Road Palo Alto", "CA 91364", 9876);

            // Act
            string actual = parcel.getAreaCode();

            // Assert
            Assert.AreEqual(expected, actual, "Areacode Invalid.");
        }
        [TestMethod]
        public void LogData_ShouldReturnCorrectData()
        {
            // Arrange
            string expected = "New Parcel Added (CA 91364, '5230 Newell Road Palo Alto') Allocated to Courier 9876";
            Parcel parcel = new Parcel(1234, "5230 Newell Road Palo Alto", "CA 91364", 9876);

            // Act 
            string actual = parcel.logData();

            // Assert
            Assert.AreEqual(expected, actual, "Log data method not producing correct data.");
        }
        [TestMethod]
        public void UnassignedLogData_ShouldReturnCorrectData()
        {
            // Arrange
            string expected = "New Parcel Added (CA 91364, '5230 Newell Road Palo Alto') Allocated to Courier UNASSIGNED";
            Parcel parcel = new Parcel("5230 Newell Road Palo Alto", "CA 91364");

            // Act 
            string actual = parcel.unassignedLogData();

            // Assert
            Assert.AreEqual(expected, actual, "Unassigned Log data method not producing correct data.");
        }
    }
}