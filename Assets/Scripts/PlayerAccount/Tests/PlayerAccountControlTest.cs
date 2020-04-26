using NUnit.Framework;

namespace Agate.WaskitaInfra1.PlayerAccount.Test
{
    public class PlayerAccountControlTest
    {
        private PlayerAccountControl accountControl;
        private PlayerAccountData testAccountData = new PlayerAccountData()
        {
            Username = "TestAccount",
            AuthenticationToken = "testToken"
        };

        [SetUp]
        public void SetUp()
        {
            accountControl = new PlayerAccountControl();
        }
        [Test]
        public void Constructed_Control_Have_Empty_Data()
        {
            Assert.That(accountControl.Data.IsEmpty);
        }
        [Test]
        public void SetData_Change_Account_Data()
        {
            Assert.That(accountControl.Data.Username, Is.Not.EqualTo(testAccountData.Username));
            Assert.That(accountControl.Data.AuthenticationToken, Is.Not.EqualTo(testAccountData.AuthenticationToken));
            accountControl.SetData(testAccountData);
            Assert.That(accountControl.Data.Username, Is.EqualTo(testAccountData.Username));
            Assert.That(accountControl.Data.AuthenticationToken, Is.EqualTo(testAccountData.AuthenticationToken));
        }

        [Test]
        public void SetData_Invoke_OnChangeData_Event()
        {
            bool eventInvoked = false;
            PlayerAccountData invokedData;
            accountControl.OnDataChange += data =>
            {
                eventInvoked = true;
                invokedData = data;
            };
            accountControl.SetData(testAccountData);
            Assert.That(eventInvoked);
            Assert.That(invokedData, Is.EqualTo(accountControl.Data));
        }
    }
}