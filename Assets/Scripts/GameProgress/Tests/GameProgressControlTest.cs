using System;
using NUnit.Framework;

namespace Agate.WaskitaInfra1.GameProgress
{
    public class GameProgressControlTest
    {
        private GameProgressControl _gameProgressCntrl;

        private readonly IGameProgressData _testData = new TestGameProgressData()
        {
            CompletionCount = 1,
            PlayTime = 100000,
            MaxCompletedLevelIndex = -1,
        };

        [SetUp]
        public void SetUp()
        {
            _gameProgressCntrl = new GameProgressControl();
        }

        [Test]
        public void Constructed_Controller_Have_Null_Data()
        {
            Assert.That(_gameProgressCntrl.Data, Is.Null);
        }

        [Test]
        public void SetData_Assign_Data_In_Controller()
        {
            _gameProgressCntrl.SetData(_testData);
            Assert.That(_gameProgressCntrl.Data.PlayTime, Is.EqualTo(_testData.PlayTime));
            Assert.That(_gameProgressCntrl.Data.CompletionCount, Is.EqualTo(_testData.CompletionCount));
            Assert.That(_gameProgressCntrl.Data.MaxCompletedLevelIndex, Is.EqualTo(_testData.MaxCompletedLevelIndex));
        }

        [Test]
        public void SetData_Invoke_OnDataChange_Event()
        {
            bool eventInvoked = false;
            IGameProgressData invokedData = new TestGameProgressData();
            _gameProgressCntrl.OnDataChange += data =>
            {
                eventInvoked = true;
                invokedData = data;
            };
            _gameProgressCntrl.SetData(_testData);
            Assert.That(eventInvoked);
            Assert.That(invokedData.PlayTime, Is.EqualTo(_testData.PlayTime));
            Assert.That(invokedData.CompletionCount, Is.EqualTo(_testData.CompletionCount));
            Assert.That(invokedData.MaxCompletedLevelIndex, Is.EqualTo(_testData.MaxCompletedLevelIndex));
        }

        [Test]
        public void DataEquality_Compare_Data_Value()
        {
            _gameProgressCntrl.SetData(_testData);
            Assert.That(_gameProgressCntrl.CurrentDataEquality(_testData));
        }

        [Test]
        public void NewGame_SetData_With_EmptyProgress()
        {
            _gameProgressCntrl.NewGame();
            Assert.That(_gameProgressCntrl.Data.PlayTime, Is.EqualTo(0));
            Assert.That(_gameProgressCntrl.Data.CompletionCount, Is.EqualTo(0));
            Assert.That(_gameProgressCntrl.Data.MaxCompletedLevelIndex, Is.EqualTo(-1));
        }

        [Test]
        public void Operating_Controller_Without_Data_Throws_Exception()
        {
            Assert.Throws(typeof(NullReferenceException), () => _gameProgressCntrl.FinishGame());
        }

        [Test]
        public void AddPlayTime_Increase_PlayTime(
            [Random(0, 10, 3)] double initial,
            [Random(0, 50, 3)] double delta)
        {
            _gameProgressCntrl.SetData(new TestGameProgressData() {PlayTime = initial});
            _gameProgressCntrl.AddPlayTime(delta);
            Assert.That(_gameProgressCntrl.Data.PlayTime, Is.EqualTo(initial + delta));
        }

        [Test]
        public void UpdateCompletedLevelIndex_With_Number_Lower_Or_Equal_than_MaxCompletedIndex_Do_Nothing(
            [Range(0, 3)] short index)
        {
            _gameProgressCntrl.SetData(new TestGameProgressData() {MaxCompletedLevelIndex = 3});
            _gameProgressCntrl.UpdateCompletedLevelIndex(index);
            Assert.That(_gameProgressCntrl.Data.MaxCompletedLevelIndex, Is.EqualTo(3));
        }

        [Test]
        public void UpdateCompletedLevelIndex_With_Higher_Than_MaxCompletedIndex_Update_Value(
            [Range(1, 3)] short index)
        {
            _gameProgressCntrl.SetData(new TestGameProgressData() {MaxCompletedLevelIndex = 0});
            _gameProgressCntrl.UpdateCompletedLevelIndex(index);
            Assert.That(_gameProgressCntrl.Data.MaxCompletedLevelIndex, Is.Not.EqualTo(0));
        }
    }

    public struct TestGameProgressData : IGameProgressData
    {
        public short MaxCompletedLevelIndex { get; set; }
        public uint CompletionCount { get; set; }
        public double PlayTime { get; set; }
    }
}