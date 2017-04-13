using System;
using System.Collections.Generic;

namespace RogueFlashNetCoreMvc.Model.Planner
{
    public class ReviewValues
    {
        public class ReviewValue
        {
            public double DaysBase          { get; }
            public double DaysMultiplier    { get; }

            protected internal ReviewValue(
                    double daysBase,
                    double daysMultiplier)
            {
                this.DaysBase = daysBase;
                this.DaysMultiplier = daysMultiplier;
            }
        }


        public enum Values: int
        {
            VALUE_0,
            VALUE_1,
            VALUE_2,
            VALUE_3,
            VALUE_4
        }


        private static IDictionary<Values, ReviewValue> values = new Dictionary<Values, ReviewValue>() {
            { Values.VALUE_0, new ReviewValue(  0.00, 0.00 ) },
            { Values.VALUE_1, new ReviewValue(  1.00, 1.00 ) },
            { Values.VALUE_2, new ReviewValue(  3.00, 1.10 ) },
            { Values.VALUE_3, new ReviewValue(  7.00, 1.20 ) },
            { Values.VALUE_4, new ReviewValue( 12.00, 1.30 ) }
        };


        public static Values GetValueFromOrdinal(int ordinal)
        {
            if (Enum.IsDefined(typeof(Values), ordinal))
            {
                return (Values) ordinal;
            }

            throw new ArgumentException();
        }

        public static ReviewValue GetReviewValueFromValue(Values value)
        {
            ReviewValue reviewValue = null;
            if (values.TryGetValue(value, out reviewValue))
            {
                return reviewValue;
            }

            throw new ArgumentException();
        }
    }
}
