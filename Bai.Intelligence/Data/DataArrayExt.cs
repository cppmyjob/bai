using System;
using System.Collections.Generic;
using System.Text;

namespace Bai.Intelligence.Data
{
    public static class DataArrayExt
    {
        public static DataArray ToCategorical(this DataArray valueArray, int numClasses)
        {
            var dim = valueArray.GetDimension();
            if (dim.Length > 1)
                throw new Exception("You can convert 1D array only");

            var result = new DataArray(new[] {numClasses});
            foreach (var item in valueArray)
            {
                var categorical = new float[numClasses];
                var value = (int)item[0];
                if (value >= numClasses)
                {
                    throw new Exception($"value {item[0]} >= {numClasses}");
                }
                categorical[value] = 1;
                result.Add(categorical);
            }

            return result;
        }
    }
}
