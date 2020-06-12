using A3.Quiz;
using Agate.WaskitaInfra1.Animations;
using Agate.WaskitaInfra1.Level;
using NUnit.Framework;
using System.Collections.Generic;

namespace Agate.WaskitaInfra1.LevelProgress.Test
{
    public class LevelProgressControlTest
    {
        private static readonly TestQuestion TestQuestion = new TestQuestion()
        {
            Quiz = new TestQuiz()
        };

        private LevelProgressControl levelProgressCntrl;

        private readonly LevelData testLevel = new LevelData()
        {
            DayDuration = 50,
            Questions = new List<IQuestion>() { TestQuestion, TestQuestion, TestQuestion }
        };


        [SetUp]
        public void SetUp()
        {
            levelProgressCntrl = new LevelProgressControl();
        }

        [Test]
        public void ConstructedControl_Have_Null_Data()
        {
            Assert.That(levelProgressCntrl.Data, Is.Null);
        }

        [Test]
        public void LoadData_Load_Value_Into_New_Data()
        {
            ILevelProgressData testData = new TestLevelProgressData(1, 0, 1, testLevel);
            levelProgressCntrl.LoadData(testData);
            Assert.That(levelProgressCntrl.Data != testData);
        }

        [Test]
        public void DataEquality_Is_Based_On_Value()
        {
            ILevelProgressData testData = new TestLevelProgressData(1, 0, 1, testLevel);
            levelProgressCntrl.LoadData(testData);
            Assert.That(levelProgressCntrl.CurrentDataEquality(testData));
        }

        [Test]
        public void LoadData_Invoke_OnDataChange_Event()
        {
            ILevelProgressData testData = new TestLevelProgressData(1, 0, 1, testLevel);
            bool eventInvoked = false;
            ILevelProgressData invokedData = null;
            levelProgressCntrl.OnDataChange += data =>
            {
                eventInvoked = true;
                invokedData = data;
            };
            levelProgressCntrl.LoadData(testData);
            Assert.That(eventInvoked);
            Assert.That(levelProgressCntrl.CurrentDataEquality(invokedData));
        }

        [Test]
        public void StartLevel_Create_Base_Level_State()
        {
            levelProgressCntrl.StartLevel(testLevel);
            Assert.That(levelProgressCntrl.Data.Answers.Capacity, Is.EqualTo(testLevel.Questions.Count));
            Assert.That(levelProgressCntrl.Data.Level, Is.EqualTo(testLevel));
            Assert.That(levelProgressCntrl.Data.CurrentDay, Is.EqualTo(1));
            Assert.That(levelProgressCntrl.Data.TryCount, Is.EqualTo(1));
        }

        [Test]
        public void NextDay_Add_CurrentDay()
        {
            const uint deltaNum = 12;
            levelProgressCntrl.StartLevel(testLevel);
            levelProgressCntrl.NextDay(deltaNum);
            Assert.That(levelProgressCntrl.Data.CurrentDay, Is.EqualTo(1 + deltaNum));
        }

        [Test]
        public void Proceeding_Pass_Last_Day_Finishes_Level()
        {
            const uint deltaNum = 52;
            bool eventInvoked = false;
            levelProgressCntrl.StartLevel(testLevel);
            levelProgressCntrl.OnFinishLevel += data => eventInvoked = true;
            levelProgressCntrl.NextDay(deltaNum);
            Assert.That(eventInvoked);
        }

        [Test]
        public void AnswerQuestion_Store_Answer_At_Specified_Index(
            [Range(0, 2)] int index)
        {
            string testAnswer = "answer";
            levelProgressCntrl.StartLevel(testLevel);
            levelProgressCntrl.AnswerQuestion(index, testAnswer);
            Assert.That(levelProgressCntrl.Data.Answers[index], Is.EqualTo(testAnswer));
        }

        [Test]
        public void UpdateCheckPoint_Set_LastCheckpoint_To_CurrentDay(
            [Random(2)] uint currentDay)
        {
            ILevelProgressData testData = new TestLevelProgressData(currentDay, 0, 1, testLevel);
            levelProgressCntrl.LoadData(testData);
            levelProgressCntrl.UpdateCheckPoint();
            Assert.That(levelProgressCntrl.Data.LastCheckpoint, Is.EqualTo(currentDay));
        }

        [Test]
        public void RestartFromCheckPoint_Set_Day_To_LastCheckPoint(
            [Random(2)] uint initialDay, [Random(2)] uint checkPoint)
        {
            ILevelProgressData testData = new TestLevelProgressData(initialDay, checkPoint, 0, testLevel);
            levelProgressCntrl.LoadData(testData);
            levelProgressCntrl.RetryFromCheckPoint();
            Assert.That(levelProgressCntrl.Data.CurrentDay, Is.EqualTo(checkPoint));
        }

        [Test]
        public void RestartFromCheckPoint_Increase_RetryCount(
            [Random(2)] uint initialTryCount)
        {
            ILevelProgressData testData = new TestLevelProgressData(30, 0, initialTryCount, testLevel);
            levelProgressCntrl.LoadData(testData);
            levelProgressCntrl.RetryFromCheckPoint();
            Assert.That(levelProgressCntrl.Data.TryCount, Is.EqualTo(initialTryCount + 1));
        }
    }

    public class TestLevelProgressData : ILevelProgressData
    {
        public uint LastCheckpoint { get; }
        public uint CurrentDay { get; }
        public uint TryCount { get; }
        public List<object> Answers { get; }
        public DayCondition Condition { get; }
        public LevelData Level { get; }

        public TestLevelProgressData(uint day, uint checkPoint, uint tryCount, LevelData level)
        {
            CurrentDay = day;
            TryCount = tryCount;
            LastCheckpoint = checkPoint;
            Answers = new List<object>(level.Questions.Count);
            Condition = new DayCondition();
            Level = level;
        }
    }

    public class TestQuiz : IQuiz
    {
        public bool IsCorrect(object answer)
        {
            return true;
        }

        public object Question => null;
    }

    public class TestQuestion : IQuestion
    {
        public string WrongExplanation => null;

        public string Category => null;

        public string DisplayName => null;

        public IQuiz Quiz { get; set; }

        public AnimationSceneControl WrongAnimation { get; set; }
    }
}