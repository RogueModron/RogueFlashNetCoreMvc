using NUnit.Framework;
using RogueFlashNetCoreMvc.Model.Planner;
using System;

namespace RogueFlashNetCoreMvcTest
{
    public class TestPlanner
    {
        [Test]
        public void TestCalculateNextPlanDates()
        {
            DateTimeOffset d1 = DateTimeOffset.Now;

            PlannerResult result = Planner.PlanNext(ReviewValues.Values.VALUE_1, d1, null);
            Assert.AreEqual(1, result.DaysNext);
            Assert.AreEqual(d1.AddDays(1), result.NextDate);

            result = Planner.PlanNext(ReviewValues.Values.VALUE_4, d1, null);
            Assert.AreEqual(12, result.DaysNext);
            Assert.AreEqual(d1.AddDays(12), result.NextDate);

            result = Planner.PlanNext(ReviewValues.Values.VALUE_2, d1, d1.AddDays(-122));
            Assert.AreEqual(137, result.DaysNext);
            Assert.AreEqual(d1.AddDays(137), result.NextDate);

            result = Planner.PlanNext(ReviewValues.Values.VALUE_0, d1, null);
            Assert.AreEqual(0, result.DaysNext);
            Assert.AreEqual(d1, result.NextDate);
        }

        [Test]
        public void TestCalculateNextPlanDatesException()
        {
            DateTimeOffset d1 = DateTimeOffset.Now;
            DateTimeOffset d2;

		    Exception exception = null;
		    try
		    {
			    d2 = d1.AddDays(-(-Planner.PASSED_DAYS_MIN - 1));
			    Planner.PlanNext(ReviewValues.Values.VALUE_1, d1, d2);
		    }
		    catch (Exception e)
		    {
			    exception = e;
		    }
            Assert.IsInstanceOf(typeof(ArgumentException), exception);

		    exception = null;
		    try
		    {
			    d2 = d1.AddDays(-(Planner.PASSED_DAYS_MAX + 1));
			    Planner.PlanNext(ReviewValues.Values.VALUE_1, d1, d2);
		    }
		    catch (Exception e)
		    {
			    exception = e;
		    }
		    Assert.IsInstanceOf(typeof(ArgumentException), exception);
	    }
    }
}
